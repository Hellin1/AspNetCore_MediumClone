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

   
        public async Task<IActionResult> Index()
        {
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
