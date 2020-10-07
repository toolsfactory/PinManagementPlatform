using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public interface IVerifyPinProcessor
    {
        Task ProcessRequestAsync(VerifyPinParameters data);
    }
}
