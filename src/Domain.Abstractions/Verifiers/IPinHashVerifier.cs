using PinPlatform.Domain.Models;
using System;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Verifiers
{
    public interface IPinHashVerifier
    {
        /// <summary>
        /// Checks if a provided hash is identical to the one stored in the repository
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref=""
        /// <returns></returns>
        Task<PinModel> VerifyPinHashAsync(Domain.Processors.VerifyPinParameters data);
    }
}