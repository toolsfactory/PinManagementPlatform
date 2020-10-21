using PinPlatform.Domain.Models;
using System.Collections.Generic;

namespace PinPlatform.Domain.Repositories
{
    public interface IRulesConfiguratonStore 
    {
        IDictionary<string, OpCoModel> OpCoConfigurations { get; }
    }
}