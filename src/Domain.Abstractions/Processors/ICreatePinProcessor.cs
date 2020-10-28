using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface ICreatePinProcessor
    {
        Task ProcessRequestAsync(Models.RequestorModel requestor, string newPin);
    }
}
