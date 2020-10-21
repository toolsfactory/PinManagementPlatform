using System;

namespace PinPlatform.Domain.Exceptions
{
    public enum ErrorCodes : int
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

    public static class ErrorTexts
    {
        private static readonly System.Collections.Generic.Dictionary<ErrorCodes, string> _mappingTable = new System.Collections.Generic.Dictionary<ErrorCodes, string>();

        static ErrorTexts()
        {
            _mappingTable.Add(ErrorCodes.NoError, "No Error");
            _mappingTable.Add(ErrorCodes.ParameterMissing, "Required parameter missing");
            _mappingTable.Add(ErrorCodes.UnknownOpCo, "Unknown OpCo");
            _mappingTable.Add(ErrorCodes.OpCoWithoutPins, "Specified OpCo doesn't support the feature 'PinManagement'");
            _mappingTable.Add(ErrorCodes.NoPinHashFound, "No matching pin found");
            _mappingTable.Add(ErrorCodes.PinHashesNotMatching, "Pin hash not matching. Pin invalid.");
            _mappingTable.Add(ErrorCodes.WithinGracePeriod, "Still within grace period. Retry after the specified time.");
        }

        public static string GetTextForErrorCode(ErrorCodes code)
        {
            return _mappingTable.TryGetValue(code, out var text) ? text : "Unknown error";
        }
    }
}
