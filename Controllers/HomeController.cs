namespace IfCovid.Controllers
{
    using IfCovid.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index(PageSettings pageSettings)
        {
            return this.View(pageSettings);
        }
    }
}