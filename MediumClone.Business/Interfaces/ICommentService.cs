using MediumClone.Common.ResponseObjects;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Interfaces
{
    public interface ICommentService : IService<CommentCreateDto, CommentUpdateDto, CommentListDto, Comment>
    {
        Task CreateComment(CommentCreateDto dto);

        Task<List<CommentListDto>> GetComments();

        Task<String> GetCount();

        Task<IResponse<CommentUpdateDto>> UpdateComment(CommentUpdateDto dto);
    }
}
