using System;
using System.Web.Mvc;
using TourAge.Models;

namespace TourAge.Controllers
{
    public partial class DetailsController : Controller
    {
        public ActionResult Details(SearchModel vSearchModel)
        {
	        //SearchModel vSearchModel = new SearchModel();
	        //vSearchModel.DateBegin = DateTime.Today;
	        //vSearchModel.DateEnd = vSearchModel.DateBegin.AddDays(1);
            return PartialView("DetailsPartial", vSearchModel);
        }
    }
}