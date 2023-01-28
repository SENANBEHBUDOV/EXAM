using Microsoft.AspNetCore.Mvc;

namespace Bootstrap_3.Areas.Manage.Controllers
{
    public class DashBoardController : Controller
    {
        [Area("Manage")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
