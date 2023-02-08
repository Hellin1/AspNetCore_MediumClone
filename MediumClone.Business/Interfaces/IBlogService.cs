using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Interfaces
{
    public interface IBlogService : IService<BlogCreateDto, BlogUpdateDto, BlogListDto, Blog>
    {
        // gonna change

        Task<List<BlogListDto>> GetLatestBlogs();

        Task<List<BlogListDto>> GetBlogsOrderById();

        Task<List<BlogListDto>> GetAll();


        Task<List<Category>> GetAllCategory();

        Task<BlogListDto> GetById(int id);

        Task<BlogListDto> GetByIdWithCategory(int blogId);

        Task<List<BlogListDto>> GetRelationalBlog();

        Task<BlogListDto> GetRelationalDataById(int id);

        Task Create(BlogCreateDto dto);

        Task CreateCategory(CategoryCreateDto dto);

        Task<BlogHomePageDto> GetAllHomePage();
    }
}
