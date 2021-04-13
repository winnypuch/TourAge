#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер маршрутов
	/// </summary>
	public class RoutsController : Controller
	{
		#region < Маршруты >

		/// <summary>
		/// Таблица детализации маршрутов
		/// </summary>
		/// <returns></returns>
		public ActionResult RoutsGridPartial()
		{
			return PartialView("RoutsGridPartial", cRouts.Fill());
		}

		/// <summary>
		/// Добавление нового маршрута
		/// </summary>
		/// <param name="vRout">Модель города</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoutsAddNewPartial(cRout vRout)
		{
			UpdateRout(vRout, true);

			return RoutsGridPartial();
		}

		/// <summary>
		/// Обновление маршрута
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoutsUpdatePartial(cRout vRout)
		{
			UpdateRout(vRout);

			return RoutsGridPartial();
		}

		/// <summary>
		/// Удаление маршрута
		/// </summary>
		/// <param name="iRoutId">Id маршрута</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoutsDeletePartial(int iRoutId)
		{
			if (iRoutId > 0)
			{
				cRouts vRouts = cRouts.Fill();
				string sError = vRouts.RemoveById(iRoutId, true);

				if(string.IsNullOrWhiteSpace(sError))
					ViewData["EditError"] = sError;
			}
			else
			{
				ViewData["EditError"] = "Нет Id маршрута!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении маршрута возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление маршрута
		/// </summary>
		/// <param name="vRout">Модель маршрута</param>
		/// <param name="bAddNewRout">Добавить новый маршрут</param>
		private void UpdateRout(cRout vRout, bool bAddNewRout = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (vRout != null && !string.IsNullOrWhiteSpace(vRout.Name) && vRout.CityStart.Id != 0 && vRout.CityEnd.Id != 0)
					{
						if (!bAddNewRout)
						{
							vRout.Save();
						}
						else
						{
							cRouts vRouts = cRouts.Fill();
							vRouts.Add(vRout, true);
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
				ViewData["vRout"] = vRout;
		}

		#endregion < Маршруты >
	}
}