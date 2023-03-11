using AutoMapper;
using MediumClone.Dtos.NlogDtos;
using MediumClone.UI.Models;

namespace MediumClone.UI.Mappings.AutoMapper
{
	public class BlogModelProfile : Profile
	{
		public BlogModelProfile()
		{
            //MediumClone.Dtos.NlogDtos.BlogListDto->MediumClone.UI.Models.BlogAdminUpdateModel
            // BlogListDto -> BlogAdminUpdateModel
            CreateMap<BlogAdminUpdateModel, BlogListDto>().ReverseMap();

        }
	}
}
