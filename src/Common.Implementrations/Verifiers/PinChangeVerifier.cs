using PinPlatform.Common.Repositories;

namespace PinPlatform.Common.Verifiers
{
    public class PinChangeVerifier : IPinChangeVerifier
    {
        private readonly IRulesConfiguratonStore _rulesConfiguratonStore;

        public PinChangeVerifier(IRulesConfiguratonStore rulesConfiguratonStore)
        {
            _rulesConfiguratonStore = rulesConfiguratonStore;
        }

        public (bool Success, ErrorCodes Error) CheckNewPinAgainstRules(string opcoId, uint pinType, string newPin)
        {
            return (true, ErrorCodes.NoError);
        }
    }
}
