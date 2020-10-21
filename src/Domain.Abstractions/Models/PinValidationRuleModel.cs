using PinPlatform.Common.Configuration;

namespace PinPlatform.Domain.Models
{
    public class PinValidationRuleModel
    {
        public PinValidationRuleModel(ushort minErrors, ushort maxErrors, PinValidationMode validationMode, ushort gracePeriodSec)
        {
            MinErrors = minErrors;
            MaxErrors = maxErrors;
            ValidationMode = validationMode;
            GracePeriodSec = gracePeriodSec;
        }

        public ushort MinErrors { get; set; }
        public ushort MaxErrors { get; set; }
        public PinValidationMode ValidationMode { get; set; }
        public ushort GracePeriodSec { get; set; }

        public static PinValidationRuleModel FromPinValidationRuleConfig(Common.Configuration.PinValidationRuleConfig config)
        {
            return new PinValidationRuleModel(config.MinErrors, config.MaxErrors, config.ValidationMode, config.GracePeriodSec);
        }
    }
}
