using Microsoft.AspNetCore.Mvc;

namespace Hello.Api.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
