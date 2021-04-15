#region < Usings >

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Web.Mvc;
using DevExpress.XtraReports.UI;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер печати
	/// </summary>

	public class ReportsController : Controller
	{
		#region < Reports >

		/// <summary>
		/// Вызов печатной формы
		/// </summary>
		/// <returns></returns>
		public ActionResult ReportsPartial()
		{
			SearchModel vSearchModel = new SearchModel();
			vSearchModel.DateBegin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			vSearchModel.DateEnd = vSearchModel.DateBegin.AddMonths(1);

			return PartialView("ReportsPartial", vSearchModel);
		}

		#endregion < Reports >
	}
}