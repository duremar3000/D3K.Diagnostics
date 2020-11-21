using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    public abstract class AsyncInterceptionBehavior : IInterceptionBehavior
    {
        #region Fields

        readonly ConcurrentDictionary<Type, Func<IMethodInvocation, BlockingCollection<Task>, Task>> _wrapperCreators;
        readonly ConcurrentDictionary<Type, Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, Task>> _invokeAsyncCreators;

        #endregion

        #region Constructors

        public AsyncInterceptionBehavior()
        {
            _wrapperCreators = new ConcurrentDictionary<Type, Func<IMethodInvocation, BlockingCollection<Task>, Task>>();
            _invokeAsyncCreators = new ConcurrentDictionary<Type, Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, Task>>();
        }

        #endregion

        #region IInterceptionBehavior

        IEnumerable<Type> IInterceptionBehavior.GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        bool IInterceptionBehavior.WillExecute
        {
            get { return true; }
        }

        IMethodReturn IInterceptionBehavior.Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var method = input.MethodBase as MethodInfo;

            if (!typeof(Task).IsAssignableFrom(method.ReturnType))
                throw new ArgumentException();

            var invokeAsyncCreator = GetInvokeAsyncCreator(method.ReturnType);

            var task = invokeAsyncCreator(input, getNext);

            return input.CreateMethodReturn(task);
        }

        #endregion

        #region Protected Methods

        protected abstract Task InvokeAsync(IMethodInvocation input, Task proceed);

        protected abstract Task<T> InvokeAsync<T>(IMethodInvocation input, Task<T> proceed);

        #endregion

        #region Private Methods

        private Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, Task> GetInvokeAsyncCreator(Type taskType)
        {
            return _invokeAsyncCreators.GetOrAdd(taskType, CreateInvokeAsyncCreator);
        }

        private Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, Task> CreateInvokeAsyncCreator(Type taskType)
        {
            if (taskType == typeof(Task))
            {
                return InvokeAsync;
            }

            if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var gm = GetType().BaseType
                  .GetMethod("GenericInvokeAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                  .MakeGenericMethod(new Type[] { taskType.GenericTypeArguments[0] });

                var func = (Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, Task>)gm.CreateDelegate(typeof(Func<IMethodInvocation, GetNextInterceptionBehaviorDelegate, Task>), this);

                return func;
            }

            throw new ArgumentException();
        }

        private async Task InvokeAsync(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var wrapperTask = Invoke(input, getNext);

            await wrapperTask;
        }

        private async Task<T> GenericInvokeAsync<T>(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var wrapperTask = Invoke(input, getNext);

            return await (Task<T>)wrapperTask;
        }

        private Task Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var method = input.MethodBase as MethodInfo;

            var wrapperCreator = GetWrapperCreator(method.ReturnType);

            var taskBlockingCollection = new BlockingCollection<Task>();

            var wrapperTask = wrapperCreator(input, taskBlockingCollection);

            var methodReturn = getNext().Invoke(input, getNext);

            if (methodReturn.ReturnValue != null)
            {
                var returnTask = (Task)methodReturn.ReturnValue;

                taskBlockingCollection.Add(returnTask);
            }

            return wrapperTask;
        }

        private Func<IMethodInvocation, BlockingCollection<Task>, Task> GetWrapperCreator(Type taskType)
        {
            return _wrapperCreators.GetOrAdd(taskType, CreateWrapperCreator);
        }

        private Func<IMethodInvocation, BlockingCollection<Task>, Task> CreateWrapperCreator(Type taskType)
        {
            if (taskType == typeof(Task))
            {
                return CreateWrapperTask;
            }

            if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var gm = GetType().BaseType
                  .GetMethod("CreateGenericWrapperTask", BindingFlags.Instance | BindingFlags.NonPublic)
                  .MakeGenericMethod(new Type[] { taskType.GenericTypeArguments[0] });

                var func = (Func<IMethodInvocation, BlockingCollection<Task>, Task>)gm.CreateDelegate(typeof(Func<IMethodInvocation, BlockingCollection<Task>, Task>), this);

                return func;
            }

            throw new ArgumentException();
        }

        private Task CreateWrapperTask(IMethodInvocation input, BlockingCollection<Task> taskBlockingCollection)
        {
            var task = Task.Run(() => CreateWaitHandleWrapperTask(taskBlockingCollection));

            return InvokeAsync(input, task);
        }

        private Task CreateWaitHandleWrapperTask(BlockingCollection<Task> taskBlockingCollection)
        {
            var task = taskBlockingCollection.Take();

            return task;
        }

        private Task CreateGenericWrapperTask<T>(IMethodInvocation input, BlockingCollection<Task> taskBlockingCollection)
        {
            var task = Task.Run(() => CreateWaitHandleWrapperTask<T>(taskBlockingCollection));

            return InvokeAsync(input, task);
        }

        private async Task<T> CreateWaitHandleWrapperTask<T>(BlockingCollection<Task> taskBlockingCollection)
        {
            var task = taskBlockingCollection.Take();

            var res = await (Task<T>)task;

            return res;
        }

        #endregion
    }
}