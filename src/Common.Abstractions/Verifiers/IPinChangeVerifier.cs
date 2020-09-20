using PinPlatform.Common;

namespace PinPlatform.Common.Verifiers
{
    public interface IPinChangeVerifier
    {
        (bool Success, ErrorCodes Error) CheckNewPinAgainstRules(string opcoId, uint pinType, string newPin);
    }
}
