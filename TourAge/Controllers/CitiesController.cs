#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер городов
	/// </summary>
	public class CitiesController : Controller
	{
		#region < Города >

		/// <summary>
		/// Таблица детализации городов
		/// </summary>
		/// <returns></returns>
		public ActionResult CitiesGridPartial()
		{
			return PartialView("CitiesGridPartial", cCities.Fill());
		}

		/// <summary>
		/// Добавление нового города
		/// </summary>
		/// <param name="vCity">Модель города</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CitiesAddNewPartial(cCity vCity)
		{
			UpdateCity(vCity, true);

			return CitiesGridPartial();
		}

		/// <summary>
		/// Обновление города
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CitiesUpdatePartial(cCity vCity)
		{
			UpdateCity(vCity);

			return CitiesGridPartial();
		}

		/// <summary>
		/// Удаление города
		/// </summary>
		/// <param name="iCityId">Id договора</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CitiesDeletePartial(int iCityId)
		{
			if (iCityId > 0)
			{
				cCities vCities = cCities.Fill();
				string sError = vCities.RemoveById(iCityId, true);

				if(string.IsNullOrWhiteSpace(sError))
					ViewData["EditError"] = sError;
			}
			else
			{
				ViewData["EditError"] = "Нет Id города!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении города возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление города
		/// </summary>
		/// <param name="vCity">Модель города</param>
		/// <param name="bAddNewCity">Добавить новый город</param>
		private void UpdateCity(cCity vCity, bool bAddNewCity = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (vCity != null && !string.IsNullOrWhiteSpace(vCity.Name) && vCity.Country.Id != 0)
					{
						if (!bAddNewCity)
						{
							vCity.Save();
						}
						else
						{
							cCities vCities = cCities.Fill();
							vCities.Add(vCity, true);
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
				ViewData["vCity"] = vCity;
		}

		#endregion < Города >
	}
}