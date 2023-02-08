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
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogCreateDto>().ReverseMap();
            CreateMap<Blog, BlogHomePageDto>().ReverseMap();
            CreateMap<Blog, BlogListDto>().ReverseMap();
            CreateMap<Blog, BlogUpdateDto>().ReverseMap();
        }
    }
}
