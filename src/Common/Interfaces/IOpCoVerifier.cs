using PinPlatform.Common;

namespace PinPlatform.Common.Interfaces
{
    public interface IOpCoVerifier
    {
        (bool Success, ErrorCodes Error) CheckIfOpCoHasPinService(string opcoId);
    }
}
