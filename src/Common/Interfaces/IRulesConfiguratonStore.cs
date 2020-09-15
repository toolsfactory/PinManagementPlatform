using PinPlatform.Common.DataModels;
using System.Collections.Generic;

namespace PinPlatform.Common.Interfaces
{
    public interface IRulesConfiguratonStore 
    {
        IDictionary<string, OpCoConfiguration> OpCoConfigurations { get; }
    }
}