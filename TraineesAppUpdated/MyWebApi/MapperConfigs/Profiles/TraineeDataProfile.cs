using AutoMapper;
using CommonLib.Helpers;
using Trainees.Models.Models;
using Trainees.Models.ModelsDTO;

namespace MyWebApi.MapperConfigs.Profiles
{
    public class TraineeDataProfile : Profile
    {
        public TraineeDataProfile()
        {
            CreateMap<CFMUser, CFMUserDTO>();
            CreateMap<CFMUserDTO, CFMUser>();

            CreateMap<CFMSurvey, CFMSurveyReadDTO>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser.Username))
            .ForMember(dest => dest.ModifiedByUserName, opt => opt.MapFrom(src => src.ModifiedByUser.Username));

            CreateMap<CFMSurveyReadDTO, CFMSurvey>();

            CreateMap<CFMSurvey, CFMSurveyCreateDTO>();
            CreateMap<CFMSurveyCreateDTO, CFMSurvey>();
        }
    }
}