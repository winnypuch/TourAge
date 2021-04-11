using System.Web.Mvc;

namespace TourAge.Controllers
{
    public class ToolsController : Controller
    {
        public ActionResult PCToolsPartial()
        {
            return PartialView();
        }
    }
}