using AutoMapper;
using MediumClone.Business.Interfaces;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using MediumClone.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediumClone.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public AdminController(ICommentService commentService, ICategoryService categoryService, IBlogService blogService, IMapper mapper)
        {
            _commentService = commentService;
            _categoryService = categoryService;
            _blogService = blogService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Blogs(string searchWord)
        {
            // son oluşturulan bloglar || en beğenilen limit 6
            if (searchWord == null)
            {
                var latestBlogs = await _blogService.GetLatestBlogs();
                var blogsOrderedById = await _blogService.GetBlogsOrderById();
                var model = new BlogAdminListModel
                {
                    BlogsOrderedById = blogsOrderedById,
                    LatestBlogs = latestBlogs

                };
                return View(model);
            }
            //var result = _blogService.Filter(searchWord);
            //ViewBag.searchWord = searchWord;
            //return View(result.Data);
            return View();

        }


        public IActionResult JustView()
        {
            return View();
        }

        public async Task<IActionResult> BlogUpdate2(int blogId)
        {
            var blog = await _blogService.GetByIdWithCategory(blogId);
            var dto = _mapper.Map<BlogUpdateDto>(blog);

            return View(dto);
        }
        [HttpPost]

        public IActionResult BlogUpdate2(BlogUpdateDto dto)
        {
            _blogService.UpdateAsync(dto);

            return RedirectToAction("UpdateBlog", new { blogId = dto.Id });
        }


        public async Task<IActionResult> RemoveBlog(int blogId)
        {
            await _blogService.RemoveAsync(blogId);
            return RedirectToAction("Blogs");
        }


        // categories

        public async Task<IActionResult> Categories(string searchWord)
        {
            if (searchWord == null)
            {
                var result2 = await _categoryService.GetAllAsync();
                ViewBag.searchWord = null;
                return View(result2.Data);
            }
            var result = _categoryService.GetSearchResult(searchWord);
            ViewBag.searchWord = searchWord;
            return View(result.Data);
        }
        public IActionResult CreateCategory()
        {
            return View(new CategoryCreateDto());
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            await _categoryService.CreateAsync(dto);

            return RedirectToAction("Categories");

        }

        public async Task<IActionResult> UpdateCategory(int categoryId)
        {
            var category = await _categoryService.GetByIdAsync<CategoryUpdateDto>(categoryId);

            return View(category.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDto dto)
        {
            var result = await _categoryService.UpdateAsync(dto);

            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            await _categoryService.RemoveAsync(categoryId);
            return RedirectToAction("Categories");
        }
    }
}
