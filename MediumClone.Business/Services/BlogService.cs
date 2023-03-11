using AutoMapper;
using FluentValidation;
using MediumClone.Business.Interfaces;
using MediumClone.Common.ResponseObjects;
using MediumClone.DataAccess.UnitOfWork;
using MediumClone.Dtos.Interfaces;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.Business.Services
{
    public class BlogService : Service<BlogCreateDto, BlogUpdateDto, BlogListDto, Blog>, IBlogService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IValidator<BlogCreateDto> _createDtoValidator;
        private readonly IValidator<BlogUpdateDto> _updateDtoValidator;

        public BlogService(IUow uow, IValidator<BlogCreateDto> createDtoValidator, IValidator<BlogUpdateDto> updateDtoValidator, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) : base(uow, createDtoValidator, updateDtoValidator, mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
        }


        public async Task<List<BlogListDto>> GetLatestBlogs(string searchWord = "")
        {
            var latest = String.IsNullOrEmpty(searchWord) ? await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).OrderByDescending(x => x.CreatedTime).Take(6).ToListAsync() : await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Where(x => x.Title.Contains(searchWord)).OrderByDescending(x => x.CreatedTime).Take(6).ToListAsync();

            var mapped = _mapper.Map<List<BlogListDto>>(latest);
            return mapped;
        }

        public async Task<List<BlogListDto>> GetBlogsOrdered<Tkey>(Expression<Func<Blog, Tkey>> selector, string searchword, bool ad = false)
        {
            List<Blog> blogOrderedById;

            blogOrderedById = ad == true ? await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Take(6).Where(x => EF.Functions.Like(x.Title, $"%{searchword}%")).OrderBy(selector).ToListAsync() : await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Where(x => EF.Functions.Like(x.Title, $"%{searchword}%")).OrderBy(selector).ToListAsync();

            await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Where(x => x.Title.Contains(searchword)).OrderBy(selector).ToListAsync();
            var mapped = _mapper.Map<List<BlogListDto>>(blogOrderedById);
            return mapped;
        }


        public async Task<List<BlogListDto>> GetAllWithCategory()
        {
            var blog = _uow.GetRepository<Blog>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Category).ToList();
            var dto = _mapper.Map<List<BlogListDto>>(blog);
            return dto;
        }

        public async Task<BlogListDto> GetByIdWithCategory(int blogId)
        {
            var blog = await _uow.GetRepository<Blog>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Category).SingleOrDefaultAsync(x => x.Id == blogId);

            var dto = _mapper.Map<BlogListDto>(blog);
            return dto;

        }

        public async Task<BlogHomePageDto> GetAllHomePage()
        {
            var blogHomePageDto = new BlogHomePageDto();
            var latest = _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).OrderByDescending(x => x.CreatedTime).ToList();
            var orderedById = _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Take(6).OrderBy(x => x.Id).ToList();
            var categories = await _uow.GetRepository<Category>().GetAllAsync();

            var categoriesDto = _mapper.Map<List<CategoryListDto>>(categories);

            var latestDto = _mapper.Map<List<BlogListDto>>(latest);

            var orderedByIdDto = _mapper.Map<List<BlogListDto>>(orderedById);

            return new BlogHomePageDto
            {
                Categories = categoriesDto,
                HomePageBlogs = latestDto,
                TrendingBlogs = orderedByIdDto,
            };
        }

        public async Task<List<BlogListDto>> GetAll()
        {
            //var list = await _uow.GetRepository<Blog>().GetAllAsync();

            var blogList = new List<BlogListDto>();


            return blogList;
        }



        public async Task<BlogListDto> GetById(int id)
        {
            var blog = await _uow.GetRepository<Blog>().GetByFilter(x => x.Id == id);
            return new()
            {
                AppUser = blog.AppUser,
                AppUserId = blog.AppUserId,
                BlogCategories = blog.BlogCategories,
                Comments = blog.Comments,
                Content = blog.Content,
                CreatedTime = blog.CreatedTime,
                Id = blog.Id,
                Title = blog.Title,
                UpdatedTime = blog.UpdatedTime
            };
        }

        public async Task<BlogListDto> GetRelationalDataById(int id)
        {
            var blog = await _uow.GetRepository<Blog>().GetRelationalDataById(x => x.Id == id);
            return new()
            {
                AppUser = blog.AppUser,
                AppUserId = blog.AppUserId,
                BlogCategories = blog.BlogCategories,
                Comments = blog.Comments,
                Content = blog.Content,
                CreatedTime = blog.CreatedTime,
                Id = blog.Id,
                Title = blog.Title,
                UpdatedTime = blog.UpdatedTime

            };
        }

        public async Task<List<BlogListDto>> GetRelationalBlog()
        {
            var blogs = await _uow.GetRepository<Blog>().GetRelationalData();
            List<BlogListDto> list = new();
            foreach (var blog in blogs)
            {
                list.Add(new()
                {
                    AppUser = blog.AppUser,
                    AppUserId = blog.AppUserId,
                    BlogCategories = blog.BlogCategories,
                    Comments = blog.Comments,
                    Content = blog.Content,
                    Title = blog.Title,
                    CreatedTime = blog.CreatedTime,
                    Id = blog.Id,
                    UpdatedTime = blog.UpdatedTime,

                });
            }
            return list;
        }

        public async Task Create(BlogCreateDto dto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == dto.AppUserId);
            var convertedBlogCategories = new List<BlogCategory>();

            foreach (var item in dto.SelectedCategories)
            {
                convertedBlogCategories.Add(new BlogCategory() { CategoryId = int.Parse(item) });
            }

            var createdEntity = _mapper.Map<Blog>(dto);
            createdEntity.AppUserId = user.Id;
            createdEntity.AppUser = user;

            await _uow.GetRepository<Blog>().CreateAsync(createdEntity);

            await _uow.SaveChanges();
        }
    }
}
