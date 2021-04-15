#region < Usings >

using System;
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
					}
					else
					{
						ViewData["EditError"] = "Укажите город и его страну!";
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