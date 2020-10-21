using System;
using System.Collections.Generic;
using System.Linq;

namespace PinPlatform.Domain.Models
{
    public class PinTypeModel
    {
        public PinTypeModel(IList<PinValidationRuleModel> validationRules, ushort id, string name, ushort minLength, ushort maxLength)
        {
            ValidationRules = validationRules ?? new List<PinValidationRuleModel>();
            Id = id;
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public ushort Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ushort MinLength { get; set; }
        public ushort MaxLength { get; set; }
        public IList<PinValidationRuleModel> ValidationRules { get; set; } = new List<PinValidationRuleModel>();

        public static PinTypeModel FromPinTypeConfig(Common.Configuration.PinTypeConfig config)
        {
            var rules = new List<PinValidationRuleModel>();
            if (config.ValidationRules != null && config.ValidationRules.Count > 0)
            {
                foreach (var rule in config.ValidationRules.OrderBy(x => x.MinErrors))
                {
                    rules.Add(PinValidationRuleModel.FromPinValidationRuleConfig(rule));
                }
            }
            return new PinTypeModel(rules, config.Id, config.Name, config.MinLength, config.MaxLength);
        }
    }
}
