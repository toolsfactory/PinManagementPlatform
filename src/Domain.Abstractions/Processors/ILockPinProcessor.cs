using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface ILockPinProcessor
    {
        Task ProcessRequestAsync(Models.RequestorModel requestor, bool lockpin = true, string reason = default(string));
    }
}
