using PinPlatform.Domain.Models;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface IPinSettingsProcessor
    {
        Task<PinSettingsListModel> ProcessRequestAsync(RequestorModel requestor);
    }
}
