namespace PinPlatform.Services.PinChange.DataModel
{
    public class PinValidationRule
    {
        public PinValidationRule(ushort minErrors, ushort maxErrors, PinValidationMode validationMode, ushort gracePeriodSec)
        {
            MinErrors = minErrors;
            MaxErrors = maxErrors;
            ValidationMode = validationMode;
            GracePeriodSec = gracePeriodSec;
        }

        public ushort MinErrors { get; private set; }
        public ushort MaxErrors { get; private set; }
        public PinValidationMode ValidationMode { get; private set; }
        public ushort GracePeriodSec { get; private set; }
    }
}
