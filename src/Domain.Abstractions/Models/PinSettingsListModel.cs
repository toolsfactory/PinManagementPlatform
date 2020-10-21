using System;
using System.Collections.Generic;
using PinPlatform.Common.Configuration;

namespace PinPlatform.Domain.Models
{
    public class PinSettingsListModel
    {
        public PinSettingsListModel()
        { }
        public PinSettingsListModel(IDictionary<string, PinDetailsModel> pinTypes)
        {
            PinTypes = pinTypes ?? throw new ArgumentNullException(nameof(pinTypes));
        }

        public IDictionary<string, PinDetailsModel> PinTypes { get; set; } = new Dictionary<string, PinDetailsModel>() ;
    }

}