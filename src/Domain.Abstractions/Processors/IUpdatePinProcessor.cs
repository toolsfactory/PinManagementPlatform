using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface IUpdatePinProcessor
    {
        Task ProcessRequestAsync(Models.RequestorModel requestor, string newPin);
    }
}
