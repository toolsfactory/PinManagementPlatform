using PinPlatform.Common;

namespace PinPlatform.Common.Verifiers
{
    public interface IOpCoVerifier
    {
        (bool Success, ErrorCodes Error) CheckIfOpCoHasPinService(string opcoId);
    }
}
