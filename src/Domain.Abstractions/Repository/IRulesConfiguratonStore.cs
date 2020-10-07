using PinPlatform.Common.DataModels;
using PinPlatform.Domain.Models;
using System.Collections.Generic;

namespace PinPlatform.Domain.Repositories
{
    public interface IRulesConfiguratonStore 
    {
        IDictionary<string, OpCoConfiguration> OpCoConfigurations { get; }
    }
}