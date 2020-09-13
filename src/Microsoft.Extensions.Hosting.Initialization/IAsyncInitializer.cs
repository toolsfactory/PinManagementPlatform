using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting.Initialization
{
    /// <summary>
    /// Represents a type that performs async initialization.
    /// </summary>
    public interface IAsyncInitializer
    {
        /// <summary>
        /// Performs async initialization.
        /// </summary>
        /// <returns>A task that represents the initialization completion.</returns>
        Task InitializeAsync();
    }
}