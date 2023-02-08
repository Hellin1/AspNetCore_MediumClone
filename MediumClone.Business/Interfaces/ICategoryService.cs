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
    public interface ICategoryService : IService<CategoryCreateDto, CategoryUpdateDto, CategoryListDto, Category>
    {
        IResponse<List<CategoryListDto>> GetSearchResult(string searchWord);

        IResponse<List<BlogListDto>> GetBlogsByCategory(int categoryId);
    }
}
