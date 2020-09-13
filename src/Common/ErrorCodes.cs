using System;

namespace PinPlatform.Common
{
    public enum ErrorCodes : ushort
    {
        NoError = 0,
        NoPinHashFound = 1001,
        PinHashesNotMatching = 1002,
        WithinGracePeriod = 1003,
        Unknown = ushort.MaxValue
    }
}
