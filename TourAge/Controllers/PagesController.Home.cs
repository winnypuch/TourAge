using System;
using System.Web.Mvc;
using TourAge.Models;

namespace TourAge.Controllers
{
    public partial class PagesController : Controller
    {
        public ActionResult Home()
        {
	        SearchModel vSearchModel = new SearchModel();
	        vSearchModel.DateBegin = DateTime.Today;
	        vSearchModel.DateEnd = vSearchModel.DateBegin.AddDays(1);
            return View("Home", vSearchModel);
        }
    }
}