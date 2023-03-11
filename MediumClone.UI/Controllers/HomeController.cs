using MediumClone.Business.Interfaces;
using MediumClone.Business.Services;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediumClone.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeController(IBlogService blogService, ICommentService commentService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            //var blogList = await _blogService.GetAll();
            //var blogs = await _blogService.GetRelationalBlog();

            var homePage = await _blogService.GetAllHomePage();


            return View(homePage);
        }
        [Authorize]
        public async Task<IActionResult> Create()
        { // gonna change2

            var blogCreateDto = new BlogCreateDto();
            var blogCategories = new List<BlogCategory>();
            var categories = await _categoryService.GetAllAsync();
            var categories2 = new List<CategoryListDto>()
            {

            };

            foreach (var category in categories.Data)
            {
                categories2.Add(new()
                {
                    Id = category.Id,
                    Title = category.Title
                });
            }

            //foreach (var category in categories)
            //{
            //    blogCategories.Add(new BlogCategory()
            //    {
            //        CategoryId = category.Id
            //    });
            //}
            //blogCreateDto.BlogCategories = blogCategories;

            blogCreateDto.Categories = categories2;
            //blogCreateDto.AppUserId = ;

            return View(blogCreateDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateDto dto)
        { // giriş yapma olmadığı için user a ulaşamayıp patlıyor
            //var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == dto.AppUserId); 
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            dto.AppUserId = userId;
            await _blogService.Create(dto);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Detail(int id)
        {
            //var blog = await _blogService.GetById(id);
            var blog = await _blogService.GetRelationalDataById(id);
            return View(blog);
        }
        [Authorize]
        public IActionResult CategoryCreate()
        {
            return View(new CategoryCreateDto());
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CommentCreate(CommentCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _commentService.CreateComment(new CommentCreateDto
            {
                BlogId = dto.BlogId,
                UserId = userId,
                Content = dto.Content
            }); ;


            return RedirectToAction("Detail", "Home", new { id = dto.BlogId });
        }

        public async Task<IActionResult> BlogsThatIncludesCategory(int categoryId)
        {
            // gelen id ye göre ilgili kategoriden tüm blogların getirilmesi
            //var categories = await _categoryService.GetByIdAsync<Category>(categoryId);
            var categories = _categoryService.GetBlogsByCategory(categoryId);

            if (categories != null)
            {
                var dadta = categories.Data;

                var category = await _categoryService.GetByIdAsync<CategoryListDto>(categoryId);
                ViewBag.Category = category.Data;

                return View(categories.Data);
            }
            return RedirectToAction("Index");
        }



    }
}
