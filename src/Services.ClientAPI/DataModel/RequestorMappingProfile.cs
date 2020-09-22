using AutoMapper;
using PinPlatform.Common.DataModels;

namespace PinPlatform.Services.ClientApi.DataModel
{
    public class RequestorMappingProfile : Profile
    {
        public RequestorMappingProfile()
        {
            CreateMap<RequestorModel, RequestorInfo>();
        }
    }

}
