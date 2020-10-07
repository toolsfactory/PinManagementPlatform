using PinPlatform.Domain.Models;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface IChangePinProcessor
    {
        Task ProcessRequestAsync(ChangePinParameters data);
    }
}
