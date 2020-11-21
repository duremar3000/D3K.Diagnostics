using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Ninject.Extensions.Interception;

namespace D3K.Diagnostics.Ninject
{
    public abstract class AsyncInterceptor : IInterceptor
    {
        #region Fields

        readonly ConcurrentDictionary<Type, Func<IInvocation, BlockingCollection<Task>, Task>> _wrapperCreators;
        readonly ConcurrentDictionary<Type, Func<IInvocation, Task>> _interceptAsyncCreators;

        #endregion

        #region Constructors

        public AsyncInterceptor()
        {
            _wrapperCreators = new ConcurrentDictionary<Type, Func<IInvocation, BlockingCollection<Task>, Task>>();
            _interceptAsyncCreators = new ConcurrentDictionary<Type, Func<IInvocation, Task>>();
        }

        #endregion

        #region IInterceptor

        void IInterceptor.Intercept(IInvocation invocation)
        {
            var method = invocation.Request.Method;

            if (!typeof(Task).IsAssignableFrom(method.ReturnType))
                throw new ArgumentException();

            var interceptAsyncCreator = GetInterceptAsyncCreator(method.ReturnType);

            var task = interceptAsyncCreator(invocation);

            invocation.ReturnValue = task;
        }

        #endregion

        #region Protected Methods

        protected abstract Task InterceptAsync(IInvocation invocation, Task proceed);

        protected abstract Task<T> InterceptAsync<T>(IInvocation invocation, Task<T> proceed);

        #endregion

        #region Private Methods

        private Func<IInvocation, Task> GetInterceptAsyncCreator(Type taskType)
        {
            return _interceptAsyncCreators.GetOrAdd(taskType, CreateInterceptAsyncCreator);
        }

        private Func<IInvocation, Task> CreateInterceptAsyncCreator(Type taskType)
        {
            if (taskType == typeof(Task))
            {
                return InterceptAsync;
            }

            if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var gm = GetType().BaseType
                  .GetMethod("GenericInterceptAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                  .MakeGenericMethod(new Type[] { taskType.GenericTypeArguments[0] });

                var func = (Func<IInvocation, Task>)gm.CreateDelegate(typeof(Func<IInvocation, Task>), this);

                return func;
            }

            throw new ArgumentException();
        }

        private async Task InterceptAsync(IInvocation invocation)
        {
            var wrapperTask = Intercept(invocation);

            await wrapperTask;
        }

        private async Task<T> GenericInterceptAsync<T>(IInvocation invocation)
        {
            var wrapperTask = Intercept(invocation);

            return await (Task<T>)wrapperTask;
        }

        private Task Intercept(IInvocation invocation)
        {
            var method = invocation.Request.Method;

            var wrapperCreator = GetWrapperCreator(method.ReturnType);

            var taskBlockingCollection = new BlockingCollection<Task>();

            var wrapperTask = wrapperCreator(invocation, taskBlockingCollection);

            invocation.Proceed();

            if (invocation.ReturnValue != null)
            {
                var returnTask = (Task)invocation.ReturnValue;

                taskBlockingCollection.Add(returnTask);
            }

            return wrapperTask;
        }

        private Func<IInvocation, BlockingCollection<Task>, Task> GetWrapperCreator(Type taskType)
        {
            return _wrapperCreators.GetOrAdd(taskType, CreateWrapperCreator);
        }

        private Func<IInvocation, BlockingCollection<Task>, Task> CreateWrapperCreator(Type taskType)
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

                var func = (Func<IInvocation, BlockingCollection<Task>, Task>)gm.CreateDelegate(typeof(Func<IInvocation, BlockingCollection<Task>, Task>), this);

                return func;
            }

            throw new ArgumentException();
        }

        private Task CreateWrapperTask(IInvocation invocation, BlockingCollection<Task> taskBlockingCollection)
        {
            var task = Task.Run(() => CreateWaitHandleWrapperTask(taskBlockingCollection));

            return InterceptAsync(invocation, task);
        }

        private Task CreateWaitHandleWrapperTask(BlockingCollection<Task> taskBlockingCollection)
        {
            var task = taskBlockingCollection.Take();

            return task;
        }

        private Task CreateGenericWrapperTask<T>(IInvocation invocation, BlockingCollection<Task> taskBlockingCollection)
        {
            var task = Task.Run(() => CreateWaitHandleWrapperTask<T>(taskBlockingCollection));

            return InterceptAsync(invocation, task);
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
