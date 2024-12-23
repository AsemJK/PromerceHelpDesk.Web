using AutoMapper;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.DTOs;
using PromerceCRM.API.Models.ERP;
using PromerceCRM.API.Models.ModelsViews;

namespace PromerceCRM.API.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountGroup, AccountGroupDto>().ReverseMap();
            CreateMap<Tenant, TenantDto>().ReverseMap();
            CreateMap<Incident, IncidentDto>().ReverseMap();
            CreateMap<Attachment, AttachmentDto>().ReverseMap();
            CreateMap<Resolution, ResolutionDto>().ReverseMap();
            CreateMap<SystemModule, SystemModuleDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
