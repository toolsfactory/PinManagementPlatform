using PinPlatform.Common;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Verifiers
{
    public interface IOpCoVerifier
    {
        /// <summary>
        /// Checks if an opcoId provided as string is known by the services and if it supports management of pins 
        /// </summary>
        /// <param name="opcoId">The OpCo Identifier as string</param>
        /// <exception cref="PinPlatform.Domain.Exceptions.OpCoUnknownException">This exception is thrown when OpCo is not known to the service</exception>
        /// <exception cref="PinPlatform.Domain.Exceptions.OpCoNotSupportingPinsException">This exception is thrown when the OpCo is not supporting Pins</exception>
        /// <example>CheckIfOpCoHasPinService Async("vfde")</example>
        Task CheckIfOpCoHasPinServiceAsync(string opcoId);
    }
}
