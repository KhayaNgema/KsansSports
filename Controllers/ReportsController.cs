using Microsoft.AspNetCore.Mvc;

namespace MyField.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
