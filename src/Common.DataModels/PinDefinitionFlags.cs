using System;

namespace PinPlatform.Common.DataModels
{
    [Flags]
    public enum PinDefinitionFlags : uint
    {
        NoDefaultCombinations,
        NoNumberRepetition
    }
}
