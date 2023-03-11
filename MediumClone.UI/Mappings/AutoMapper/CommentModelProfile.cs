using AutoMapper;
using MediumClone.Dtos.NlogDtos;
using MediumClone.UI.Models;

namespace MediumClone.UI.Mappings.AutoMapper
{
    public class CommentModelProfile : Profile
    {
        public CommentModelProfile()
        {// commentListDto <==> CommnetAdminListModel 
            CreateMap<CommentModelProfile, CommentListDto>().ReverseMap();
            CreateMap<CommentModelProfile, CommentAdminListModel>().ReverseMap();
            CreateMap<CommentListDto, CommentAdminListModel>().ReverseMap();
            
        }
    }
}
