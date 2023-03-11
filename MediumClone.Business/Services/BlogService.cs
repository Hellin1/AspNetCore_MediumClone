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
        //public BlogService(IUow uow, IValidator<BlogCreateDto> createDtoValidator, IValidator<BlogUpdateDto> updateDtoValidator, IMapper mapper) : base(uow, createDtoValidator, updateDtoValidator, mapper)
        //{

        //}



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

        //public BlogService(IUow uow, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        //{
        //    _uow = uow;
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //}
        // uow, userManager, signInManager, roleManager






        public async Task<List<BlogListDto>> GetLatestBlogs(string searchWord = "")
        {
            var latest = String.IsNullOrEmpty(searchWord) ? await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).OrderByDescending(x => x.CreatedTime).Take(6).ToListAsync() : await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Where(x => x.Title.Contains(searchWord)).OrderByDescending(x => x.CreatedTime).Take(6).ToListAsync();


            ;
            var mapped = _mapper.Map<List<BlogListDto>>(latest);
            return mapped;
        }

        // blogları arama metnine göre getirme
        // get latest blog ve get blogs ordered




        public async Task<List<BlogListDto>> GetBlogsOrdered<Tkey>(Expression<Func<Blog, Tkey>> selector, string searchword, bool ad = false)
        {
            List<Blog> blogOrderedById;
            // x => EF.Functions.Like(x.Title, $"%{searchWord}%")


            blogOrderedById = ad == true ? await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Take(6).Where(x => EF.Functions.Like(x.Title, $"%{searchword}%")).OrderBy(selector).ToListAsync():await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Where(x => EF.Functions.Like(x.Title, $"%{searchword}%")).OrderBy(selector).ToListAsync(); 

           

            //blogsOrderedById = ad == true ? await _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Take(6).Where(x => x.Title.Contains(searchword)).OrderBy(selector).ToListAsync() :
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
            //    var blog = context.Blogs
            //.Include(b => b.BlogCategories)
            //    .ThenInclude(bc => bc.Category)
            //.Where(b => b.Id == blogId)
            //.FirstOrDefault();

            //var blog =await _uow.GetRepository<Blog>().GetQuery().Where(x => x.Id == blogId).SelectMany(x => x.BlogCategories).Select(x => x.Category).ToListAsync();
            // return await _context.Blogs.Include(x => x.Comments).ThenInclude(x => x.AppUser).AsNoTracking().SingleOrDefaultAsync(filter);
            var blog = await _uow.GetRepository<Blog>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Category).SingleOrDefaultAsync(x => x.Id == blogId);

            var dto = _mapper.Map<BlogListDto>(blog);
            return dto;

        }


        public async Task UpdateBlog()
        {

            var blog = _uow.GetRepository<Blog>().GetQuery().Include(x => x.BlogCategories).ThenInclude(x => x.Category).ToList();

            _uow.SaveChanges();
        }


        public async Task<BlogHomePageDto> GetAllHomePage()
        { // en yeni, en beğenilen, en çok etkileşim alan,   || şimdilik => en yeni ve rastgele
            //_uow.GetRepository<Blog>().GetByFilter(x => x.BegenilmeMax);
            var blogHomePageDto = new BlogHomePageDto();
            var latest = _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).OrderByDescending(x => x.CreatedTime).ToList();
            //var latestDto = new List<BlogListDto>();
            var orderedById = _uow.GetRepository<Blog>().GetQuery().Include(x => x.AppUser).Take(6).OrderBy(x => x.Id).ToList();
            //var orderedByIdDto = new List<BlogListDto>();
            var categories = await _uow.GetRepository<Category>().GetAllAsync();
            //var categoriesDto = new List<CategoryListDto>();

            // mapping -- AutoMapping
            //foreach (var category in categories)
            //{
            //    categoriesDto.Add(new()
            //    {
            //        Id = category.Id,
            //        Title = category.Title,
            //    });
            //}

            var categoriesDto = _mapper.Map<List<CategoryListDto>>(categories);

            //foreach (var blog in latest)
            //{
            //    latestDto.Add(new()
            //    {
            //        AppUser = blog.AppUser,
            //        AppUserId = blog.AppUserId,
            //        BlogCategories = blog.BlogCategories,
            //        Comments = blog.Comments,
            //        Content = blog.Content,
            //        CreatedTime = blog.CreatedTime,
            //        Id = blog.Id,
            //        Title = blog.Title,
            //        UpdatedTime = blog.UpdatedTime
            //    });
            //}
            var latestDto = _mapper.Map<List<BlogListDto>>(latest);

            //foreach (var blog in orderedById)
            //{
            //    orderedByIdDto.Add(new()
            //    {
            //        AppUser = blog.AppUser,
            //        AppUserId = blog.AppUserId,
            //        BlogCategories = blog.BlogCategories,
            //        Comments = blog.Comments,
            //        Content = blog.Content,
            //        CreatedTime = blog.CreatedTime,
            //        Id = blog.Id,
            //        Title = blog.Title,
            //        UpdatedTime = blog.UpdatedTime
            //    });
            //}

            var orderedByIdDto = _mapper.Map<List<BlogListDto>>(orderedById);


            //blogHomePageDto.Categories = categoriesDto;
            //blogHomePageDto.HomePageBlogs = latestDto;
            //blogHomePageDto.TrendingBlogs = orderedByIdDto;

            //return blogHomePageDto;
            return new BlogHomePageDto
            {
                Categories = categoriesDto,
                HomePageBlogs = latestDto,
                TrendingBlogs = orderedByIdDto,
            };
        }

        public async Task<List<BlogListDto>> GetAll()
        { // gonna change
            var list = await _uow.GetRepository<Blog>().GetAllAsync();

            var blogList = new List<BlogListDto>();

            if (list != null && list.Count > 0)
            {
                foreach (var work in list)
                {
                    blogList.Add(new()
                    {
                        // ekleme yapılacak
                    });
                }
            }
            return blogList;
        }


        // gonna change
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
        { // gonna change
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == dto.AppUserId);
            //var blogCategories = dto.Categories;

            var convertedBlogCategories = new List<BlogCategory>();

            //foreach (var category in dto.Categories)
            //{
            //    convertedBlogCategories.Add(new() { CategoryId =  category.Id });
            //}

            foreach (var item in dto.SelectedCategories)
            {
                convertedBlogCategories.Add(new BlogCategory() { CategoryId = int.Parse(item) });
            }
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //var appUser = _userManager.Users.SingleOrDefault(x => x.Id == dto.AppUserId);

            var createdEntity = _mapper.Map<Blog>(dto);
            //createdEntity.Id = 0;
            createdEntity.AppUserId = user.Id;
            createdEntity.AppUser = user;
            //createdEntity.BlogCategories = convertedBlogCategories;

            await _uow.GetRepository<Blog>().CreateAsync(createdEntity);


            //await _uow.GetRepository<Blog>().CreateAsync(new()
            //{// ekleme yapılacak
            //    AppUser = user,
            //    AppUserId = user.Id,
            //    Title = dto.Title,
            //    Content = dto.Content,
            //    BlogCategories = convertedBlogCategories
            //});


            //await _blogService.Create(dto);


            //await _uow.GetRepository<Work>().Create(new()
            //{
            //    IsCompleted = dto.IsCompleted,
            //    Definition = dto.Definition
            //});

            //var validator = new WorkCreateDtoValidator();
            //var validationResult = validator.Validate(dto);

            await _uow.SaveChanges();
        }

        public async Task CategoryCreate(CategoryCreateDto dto)
        {
            await _uow.GetRepository<Category>().CreateAsync(new Category()
            {
                Title = dto.Title,

            });

            await _uow.SaveChanges();
        }



        public void Update(BlogUpdateDto dto)
        { // gonna change
            //_uow.GetRepository<Blog>().Update(new()
            //{

            //});



        }


        // remove eklenecek


    }
}
