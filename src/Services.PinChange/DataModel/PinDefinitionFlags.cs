using System;

namespace PinPlatform.Services.PinChange.DataModel
{
    [Flags]
    public enum PinDefinitionFlags : uint
    {
        NoDefaultCombinations,
        NoNumberRepetition
    }
}
