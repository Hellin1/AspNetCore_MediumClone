using AutoMapper;
using MediumClone.Business.Interfaces;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using MediumClone.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MediumClone.UI.Controllers
{
    [Authorize(Roles = "Admin")]
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
            return RedirectToAction("Blogs");
        }
        // blogs
        public async Task<IActionResult> Blogs(string searchWord)
        {
            

            var latestBlogs = await _blogService.GetLatestBlogs(searchWord);
            var blogsOrderedById = await _blogService.GetBlogsOrdered(x => x.Id, searchWord, true);
            var model = new BlogAdminListModel
            {
                BlogsOrderedById = blogsOrderedById,
                LatestBlogs = latestBlogs

            };
            return View(model);

        }


        public async Task<IActionResult> BlogUpdate(int blogId)
        {
            var blog = await _blogService.GetByIdWithCategory(blogId);
            var categories = await _categoryService.GetAllAsync();
            var dto = _mapper.Map<BlogAdminUpdateModel>(blog);

            dto.Categories = new SelectList(categories.Data, "Categories.Id", "Categories.Title");

            return View(dto);
        }


        [HttpPost]
        public IActionResult BlogUpdate(BlogUpdateDto dto)
        {
            _blogService.UpdateAsync(dto);
            return RedirectToAction("Blogs");
        }


        public async Task<IActionResult> BlogRemove(int blogId)
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
        public IActionResult CategoryCreate()
        {
            return View(new CategoryCreateDto());
        }


        [HttpPost]
        public async Task<IActionResult> CategoryCreate(CategoryCreateDto dto)
        {
            await _categoryService.CreateAsync(dto);

            return RedirectToAction("Categories");

        }

        public async Task<IActionResult> CategoryUpdate(int categoryId)
        {
            var category = await _categoryService.GetByIdAsync<CategoryUpdateDto>(categoryId);

            return View(category.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUpdate(CategoryUpdateDto dto)
        {
            var result = await _categoryService.UpdateAsync(dto);

            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> CategoryRemove(int categoryId)
        {
            await _categoryService.RemoveAsync(categoryId);
            return RedirectToAction("Categories");
        }

        // comments
        public async Task<IActionResult> Comments()
        {
            var comments = await _commentService.GetComments();
            var model = _mapper.Map<List<CommentAdminListModel>>(comments);
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> CommentUpdate(int commentId)
        {
            var comment = await _commentService.GetByIdAsync<CommentUpdateDto>(commentId);
            return View(comment.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CommentUpdate(CommentUpdateDto dto)
        {
            dto.UpdatedTime = DateTime.UtcNow;
            var result = await _commentService.UpdateComment(dto);
            return RedirectToAction("Comments");
            
        }

        public async Task<IActionResult> CommentRemove(int commentId)
        {
            await _commentService.RemoveAsync(commentId);
            return RedirectToAction("Comments");
        }
    }
}
