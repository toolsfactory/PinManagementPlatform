using PinPlatform.Common;
using PinPlatform.Domain.Models;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Verifiers
{
    public interface IPinChangeVerifier
    {

        /// <summary>
        /// Checks if a new pin is according to the rules defined by a certain OpCo
        /// </summary>
        /// <param name="data">The data model describing pintype, opco and potential new pin <see cref="PinChangeVerificationModel"/></param>
        /// <exception cref="PinPlatform.Domain.Exceptions.PinNotCompliantToRulesException">This exception is thrown when the provided pin is not in accordance with the rules the OpCo has specified</exception>
        Task CheckNewPinAgainstRulesAsync(PinChangeVerificationModel data);
    }
}
