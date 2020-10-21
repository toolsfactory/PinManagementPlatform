namespace PinPlatform.Domain.Models
{
    public class PinDetailsModel : PinTypeModel
    {
        public PinDetailsModel(PinTypeModel inner, string salt, string hash, bool locked) :
            base (inner.ValidationRules, inner.Id, inner.Name, inner.MinLength, inner.MaxLength)
        {
            PinSalt = salt;
            PinHash = hash;
            PinLocked = locked;
        }
        public string PinSalt { get; private set; }
        public string PinHash { get; private set; }
        public bool PinLocked { get; private set; }

    }
}
