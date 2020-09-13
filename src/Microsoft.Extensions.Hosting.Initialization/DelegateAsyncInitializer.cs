using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting.Initialization
{
    public static partial class InitializationServiceCollectionExtensions
    {
        private class DelegateAsyncInitializer : IAsyncInitializer
        {
            private readonly Func<Task> _initializer;

            public DelegateAsyncInitializer(Func<Task> initializer)
            {
                _initializer = initializer;
            }

            public Task InitializeAsync()
            {
                return _initializer();
            }
        }
    }
}