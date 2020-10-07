using PinPlatform.Domain.Models;
using PinPlatform.Domain.Repositories;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Verifiers
{
    public class PinChangeVerifier : IPinChangeVerifier
    {
        private readonly IRulesConfiguratonStore _rulesConfiguratonStore;

        public PinChangeVerifier(IRulesConfiguratonStore rulesConfiguratonStore)
        {
            _rulesConfiguratonStore = rulesConfiguratonStore;
        }

        public Task CheckNewPinAgainstRulesAsync(PinChangeVerificationModel data)
        {
            return Task.CompletedTask;
        }
    }
}
