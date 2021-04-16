#region < Usings >

using System;
using System.Linq;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер туров
	/// </summary>
	public class ToursController : Controller
	{
		#region < Туры >

		/// <summary>
		/// Таблица детализации туров
		/// </summary>
		/// <returns></returns>
		public ActionResult ToursGridPartial()
		{
			return PartialView("ToursGridPartial", cTours.Fill());
		}

		public ActionResult RoutSelectPartial(int? CurrentID)
		{

			cTour model = new cTour() { Id = -1, RoutsIDs = new int[0] };
			if (CurrentID > -1)
			{
				model = cTours.Fill().Where(item => item.Id == CurrentID).FirstOrDefault();
			}

			return PartialView("RoutSelectPartial", model);
		}

		/// <summary>
		/// Добавление нового тура
		/// </summary>
		/// <param name="vTour">Модель города</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ToursAddNewPartial(cTour vTour)
		{
			UpdateTour(vTour, true);

			return ToursGridPartial();
		}

		/// <summary>
		/// Обновление тура
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ToursUpdatePartial(cTour vTour)
		{
			UpdateTour(vTour);

			return ToursGridPartial();
		}

		/// <summary>
		/// Удаление тура
		/// </summary>
		/// <param name="iTourId">Id тура</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ToursDeletePartial(int iTourId)
		{
			if (iTourId > 0)
			{
				cTours vTours = cTours.Fill();
				string sError = vTours.RemoveById(iTourId, true);

				if(string.IsNullOrWhiteSpace(sError))
					ViewData["EditError"] = sError;
			}
			else
			{
				ViewData["EditError"] = "Нет Id тура!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении тура возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление тура
		/// </summary>
		/// <param name="vTour">Модель тура</param>
		/// <param name="bAddNewTour">Добавить новый маршрут</param>
		private void UpdateTour(cTour vTour, bool bAddNewTour = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (vTour != null && !string.IsNullOrWhiteSpace(vTour.Name) && vTour.TDateBegin != DateTime.MinValue && vTour.TDateBegin != DateTime.MinValue)
					{
						if (!bAddNewTour)
						{
							vTour.Save();
						}
						else
						{
							cTours vTours = cTours.Fill();
							vTours.Add(vTour, true);
						}

						vTour.RemoveRoutes();
						foreach (int iTourRoutsID in vTour.RoutsIDs)
						{
							cRout vRout = new cRout();
							vRout.Load(iTourRoutsID);
							vTour.Routs.Add(vRout);
						}
						vTour.SaveRoutes();

					}
					else
					{
						ViewData["EditError"] = "Укажите наименование тура, стоимость проживания, дату начала и окончания тура !";
					}
				}
				catch (Exception e)
				{
					ViewData["EditError"] = "Критическая ошибка " + e.Message;
				}
			}
			else
			{
				ViewData["EditError"] = "Исправьте все ошибки.";
			}

			if (ViewData["EditError"] != null)
				ViewData["vTour"] = vTour;
		}

		#endregion < Туры >
	}
}