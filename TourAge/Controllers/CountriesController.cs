#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер стран
	/// </summary>
	public class CountriesController : Controller
	{
		#region < Страны >

		/// <summary>
		/// Таблица детализации стран
		/// </summary>
		/// <returns></returns>
		public ActionResult CountriesGridPartial()
		{
			return PartialView("CountriesGridPartial", cCountries.Fill());
		}

		/// <summary>
		/// Добавление новой страны
		/// </summary>
		/// <param name="vCountry">Модель страны</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CountriesAddNewPartial(cCountry vCountry)
		{
			UpdateCountry(vCountry, true);

			return CountriesGridPartial();
		}

		/// <summary>
		/// Обновление страны
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CountriesUpdatePartial(cCountry vCountry)
		{
			UpdateCountry(vCountry);

			return CountriesGridPartial();
		}

		/// <summary>
		/// Удаление страны
		/// </summary>
		/// <param name="iCountryId">Id страны</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CountriesDeletePartial(int iCountryId)
		{
			if (iCountryId > 0)
			{
				cCountries vCountries = cCountries.Fill();
				string sError = vCountries.RemoveById(iCountryId, true);

				if(string.IsNullOrWhiteSpace(sError))
					ViewData["EditError"] = sError;
			}
			else
			{
				ViewData["EditError"] = "Нет Id страны!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении страны возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление страны
		/// </summary>
		/// <param name="vCountry">Модель страны</param>
		/// <param name="bAddNewCountry">Добавить новую страну</param>
		private void UpdateCountry(cCountry vCountry, bool bAddNewCountry = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (vCountry != null && !string.IsNullOrWhiteSpace(vCountry.Name))
					{
						if (!bAddNewCountry)
						{
							vCountry.Save();
						}
						else
						{
							cCountries vCountries = cCountries.Fill();
							vCountries.Add(vCountry, true);
						}
					}
					else
					{
						ViewData["EditError"] = "Укажите страну и её имя!";
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
				ViewData["vCountry"] = vCountry;
		}

		#endregion < Страны >
	}
}