using System;

namespace PinPlatform.Common.Configuration
{
    [Flags]
    public enum PinDefinitionFlags : uint
    {
        NoDefaultCombinations = 1,
        NoNumberRepetition = 2
    }
}
