using AutoMapper;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Mappings.AutoMapper
{
    public class AppRoleProfile : Profile
    {
        public AppRoleProfile()
        { 
            CreateMap<AppUser, AppUserCreateDto>().ReverseMap();
            CreateMap<AppUser, AppUserSignInDto>().ReverseMap();
        }
    }
}
