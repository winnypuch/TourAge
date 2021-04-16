using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using TourAge.Models;

namespace TourAge.Controllers
{
    public partial class ResultsController : Controller
    {
	    public ActionResult Results(SearchModel vSearchModel)
	    {
		    ViewData["vSearchModel"] = vSearchModel;
		    cCity vCity = new cCity();
		    vCity.Load(vSearchModel.CityId);

		    ViewData["CityName"] = vCity.Name;
		    ViewData["CountryName"] = vCity.Country.Name;
		    ViewData["Period"] = "c " + vSearchModel.DateBegin.ToString("dd.MM.yyyy") + " ОН " +
		                         vSearchModel.DateEnd.ToString("dd.MM.yyyy");



		    List<SqlParameter> vParams = new List<SqlParameter>
		    {
			    new SqlParameter("@CityId", SqlDbType.Int) {Value = vSearchModel.CityId},
			    new SqlParameter("@TDateBegin", SqlDbType.DateTime) {Value = vSearchModel.DateBegin},
			    new SqlParameter("@TDateEnd", SqlDbType.DateTime) {Value = vSearchModel.DateEnd},
		    };

		    List<cTour> vTours = new List<cTour>();

		    DataTable vRes = DataProvider.GetDataTable("SELECT DISTINCT T.Id AS Id " +
		                                               "FROM [dbo].[Routes] AS R INNER JOIN dbo.RoutesInTour AS RT ON R.Id = RT.RouteId " +
		                                               "INNER JOIN dbo.Tours AS T ON T.Id = RT.TourId INNER JOIN dbo.Cities AS C ON R.[CityStartId] = C.Id " +
		                                               "INNER JOIN dbo.Countries AS CO ON C.CountryId = CO.Id WHERE R.[CityStartId] = @CityId AND T.TDateBegin >= @TDateBegin AND T.TDateBegin <= @TDateEnd",
			    vParams);

		    if (vRes?.Rows.Count > 0)
		    {
			    foreach (DataRow vRow in vRes.Rows)
			    {
				    cTour vcTour = new cTour();
				    vcTour.Load(Convert.ToInt32(vRow["Id"]));
				    vTours.Add(vcTour);
			    }
		    }

		   string sToursCount = vTours.Count + " ";

		    if (vTours.Count == 1)
			    sToursCount += "рсп";
		    else if (vTours.Count > 2 && vTours.Count < 5)
			    sToursCount += "рспю";
		    else
			    sToursCount += "рспнб";

		    ViewData["TourCount"] = sToursCount;
		    //SearchModel vSearchModel = new SearchModel();
		    //vSearchModel.DateBegin = DateTime.Today;
		    //vSearchModel.DateEnd = vSearchModel.DateBegin.AddDays(1);
		    return View("ResultsPartial", vTours);
	    }
    }
}