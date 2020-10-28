using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface IDeletePinProcessor
    {
        Task ProcessRequestAsync(Models.RequestorModel requestor);
    }
}
