using System;

namespace PinPlatform.Common
{
    public enum ErrorCodes : ushort
    {
        NoError = 0,
        ParameterMissing = 1,
        UnknownOpCo = 1000,
        OpCoWithoutPins = 2000,
        NoPinHashFound = 2001,
        PinHashesNotMatching = 2002,
        WithinGracePeriod = 2003,
        Unknown = ushort.MaxValue
    }
}
