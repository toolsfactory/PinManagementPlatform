using PinPlatform.Common.DataModels;
using System.Collections.Generic;

namespace PinPlatform.Common.Repositories
{
    public interface IRulesConfiguratonStore 
    {
        IDictionary<string, OpCoConfiguration> OpCoConfigurations { get; }
    }
}