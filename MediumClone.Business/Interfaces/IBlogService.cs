using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Interfaces
{
    public interface IBlogService : IService<BlogCreateDto, BlogUpdateDto, BlogListDto, Blog>
    {
        // gonna change


        Task<List<BlogListDto>> GetBlogsOrdered<Tkey>(Expression<Func<Blog, Tkey>> selector, string searchword, bool ad = false);

        Task<List<BlogListDto>> GetAll();

        Task<List<BlogListDto>> GetLatestBlogs(string searchWord = "");

        Task<BlogListDto> GetById(int id);

        Task<BlogListDto> GetByIdWithCategory(int blogId);

        Task<List<BlogListDto>> GetRelationalBlog();

        Task<BlogListDto> GetRelationalDataById(int id);

        Task Create(BlogCreateDto dto);

        Task CategoryCreate(CategoryCreateDto dto);

        Task<BlogHomePageDto> GetAllHomePage();
    }
}
