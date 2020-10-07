using System;

namespace PinPlatform.Domain.Models
{
    [Flags]
    public enum PinDefinitionFlags : uint
    {
        NoDefaultCombinations,
        NoNumberRepetition
    }
}
