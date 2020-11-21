using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using LightInject.Interception;

namespace D3K.Diagnostics.LightInject
{
    public abstract class AsyncInterceptor : IInterceptor
    {
        #region Fields

        readonly ConcurrentDictionary<Type, Func<IInvocationInfo, BlockingCollection<Task>, Task>> _wrapperCreators;
        readonly ConcurrentDictionary<Type, Func<IInvocationInfo, Task>> _invokeAsyncCreators;

        #endregion

        #region Constructors

        public AsyncInterceptor()
        {
            _wrapperCreators = new ConcurrentDictionary<Type, Func<IInvocationInfo, BlockingCollection<Task>, Task>>();
            _invokeAsyncCreators = new ConcurrentDictionary<Type, Func<IInvocationInfo, Task>>();
        }

        #endregion

        #region IInterceptor

        object IInterceptor.Invoke(IInvocationInfo invocationInfo)
        {
            var method = invocationInfo.Method;

            if (!typeof(Task).IsAssignableFrom(method.ReturnType))
                throw new ArgumentException();

            var invokeAsyncCreator = GetInvokeAsyncCreator(method.ReturnType);

            var task = invokeAsyncCreator(invocationInfo);

            return task;
        }

        #endregion

        #region Protected Methods

        protected abstract Task InvokeAsync(IInvocationInfo invocation, Task proceed);

        protected abstract Task<T> InvokeAsync<T>(IInvocationInfo invocation, Task<T> proceed);

        #endregion

        #region Private Methods

        private Func<IInvocationInfo, Task> GetInvokeAsyncCreator(Type taskType)
        {
            return _invokeAsyncCreators.GetOrAdd(taskType, CreateInvokeAsyncCreator);
        }

        private Func<IInvocationInfo, Task> CreateInvokeAsyncCreator(Type taskType)
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

                var func = (Func<IInvocationInfo, Task>)gm.CreateDelegate(typeof(Func<IInvocationInfo, Task>), this);

                return func;
            }

            throw new ArgumentException();
        }

        private async Task InvokeAsync(IInvocationInfo invocation)
        {
            var wrapperTask = Invoke(invocation);

            await wrapperTask;
        }

        private async Task<T> GenericInvokeAsync<T>(IInvocationInfo invocation)
        {
            var wrapperTask = Invoke(invocation);

            return await (Task<T>)wrapperTask;
        }

        private Task Invoke(IInvocationInfo invocation)
        {
            var method = invocation.Method;

            var wrapperCreator = GetWrapperCreator(method.ReturnType);

            var taskBlockingCollection = new BlockingCollection<Task>();

            var wrapperTask = wrapperCreator(invocation, taskBlockingCollection);

            var returnValue = invocation.Proceed();

            if (returnValue != null)
            {
                var returnTask = (Task)returnValue;

                taskBlockingCollection.Add(returnTask);
            }

            return wrapperTask;
        }

        private Func<IInvocationInfo, BlockingCollection<Task>, Task> GetWrapperCreator(Type taskType)
        {
            return _wrapperCreators.GetOrAdd(taskType, CreateWrapperCreator);
        }

        private Func<IInvocationInfo, BlockingCollection<Task>, Task> CreateWrapperCreator(Type taskType)
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

                var func = (Func<IInvocationInfo, BlockingCollection<Task>, Task>)gm.CreateDelegate(typeof(Func<IInvocationInfo, BlockingCollection<Task>, Task>), this);

                return func;
            }

            throw new ArgumentException();
        }

        private Task CreateWrapperTask(IInvocationInfo invocation, BlockingCollection<Task> taskBlockingCollection)
        {
            var task = Task.Run(() => CreateWaitHandleWrapperTask(taskBlockingCollection));

            return InvokeAsync(invocation, task);
        }

        private Task CreateWaitHandleWrapperTask(BlockingCollection<Task> taskBlockingCollection)
        {
            var task = taskBlockingCollection.Take();

            return task;
        }

        private Task CreateGenericWrapperTask<T>(IInvocationInfo invocation, BlockingCollection<Task> taskBlockingCollection)
        {
            var task = Task.Run(() => CreateWaitHandleWrapperTask<T>(taskBlockingCollection));

            return InvokeAsync(invocation, task);
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
