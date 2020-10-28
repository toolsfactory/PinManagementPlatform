using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;
using System.Threading.Tasks;

namespace PinPatform.Domain.Processors
{
    public class PinProvisioningProcessor
    {
        public async Task<bool> ProcessCreatePinAsync()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> ProcessDeletePinAsync()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> ProcessLockPinAsync()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> ProcessUnlockPinAsync()
        {
            return await Task.FromResult(true);
        }
    }
}
