using AutoMapper;
using PinPlatform.Common.DataModels;
using System.ComponentModel.DataAnnotations;

namespace PinPlatform.Services.ClientApi.DataModel
{
    public class RequestorModel
    {
        [Required]
        public string HouseholdId { get; set; } = string.Empty;
        public uint ProfileId { get; set; }
    }

    public class RequestorMappingProfile : Profile
    {
        public RequestorMappingProfile()
        {
            CreateMap<RequestorModel, RequestorInfo>();
        }
    }

}
