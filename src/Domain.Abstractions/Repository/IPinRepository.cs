using PinPlatform.Domain.Models;
using System;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Repositories
{
    public interface IPinRepository
    {
        Task<PinModel?> TryGetPinDetailsAsync(RequestorModel requestor);
        Task CreateOrUpdatePinAsync(RequestorModel requestor, PinModel pin);
        Task DeletePinAsync(RequestorModel requestor);
        Task UpdatePinFailureInfoAsync(RequestorModel requestor, PinModel pin);
    }
}