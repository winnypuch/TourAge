using System.Web.Mvc;

namespace TourAge.Controllers
{
    public partial class PagesController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }
    }
}