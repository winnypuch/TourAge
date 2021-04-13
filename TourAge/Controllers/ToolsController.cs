using System.Web.Mvc;

namespace TourAge.Controllers
{
    public class ToolsController : Controller
    {
	    public ActionResult Admin()
	    {
		    return View();
	    }
        public ActionResult PCToolsPartial()
        {
            return PartialView();
        }
    }
}