using MediumClone.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MediumClone.UI.Controllers
{
    public class NonUserController : Controller
    {
        private readonly IBlogService _blogService;

        public NonUserController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // need lazy loading
        public async Task<IActionResult> Index()
        {
            //var blogList = await _blogService.GetAll();
            //var blogs = await _blogService.GetRelationalBlog();

            var homePage = await _blogService.GetAllHomePage();

            return View(homePage);
        }

        public IActionResult OurStory()
        {

            return View();
        }

        public IActionResult MemberShip()
        {
            return View();
        }

        public IActionResult Write()
        {
            return View();
        }

    }
}
