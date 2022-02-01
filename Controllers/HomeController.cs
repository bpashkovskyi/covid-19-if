namespace Covid19.Controllers
{
    using Covid19.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index(PageSettings pageSettings)
        {
            return this.View(pageSettings);
        }

        public IActionResult Old(PageSettings pageSettings)
        {
            return this.View(pageSettings);
        }
    }
}