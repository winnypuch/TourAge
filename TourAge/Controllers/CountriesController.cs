#region < Usings >

using System;
using System.Collections.Generic;
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
			if (iCityId > 0)
			{
					AgentBase vSubscriber = DBObject.Load<AgentBase>(iSubscriberId);

					if (vSubscriber != null)
					{
						vSubscriber.AgentCategories.RemoveAt(dCategoryDate.Value);
						vSubscriber.Save();
					}
					else
					{
						ViewData["EditError"] = "Ошибка получения данных абонента!";
					}
			}
			else
			{
				ViewData["EditError"] = "Нет даты категории!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении категории возникла ошибка:\n" + ViewData["EditError"]});
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

					AgentCategory vAgentCategory = DBObject.Load<AgentCategory>(vAgentCategoryModel.CategoryId);
					AgentBase vAgent = DBObject.Load<AgentBase>(iSubscriberId);

					if (vAgentCategory != null && vAgent != null && vAgentCategoryModel.CategoryId != 0 && vAgentCategoryModel.CategoryDate != null)
					{
						Dictionary<DateTime?, AgentCategory> vDates = vAgent.AgentCategories.GetItemsDates();

						if (!bAddNewCategory || (vDates != null && vDates.ContainsKey(vAgentCategoryModel.CategoryDate)))
						{
							if (vDates != null && !vDates[vAgentCategoryModel.CategoryDate].Equals(vAgentCategory))
							{
								vAgent.AgentCategories.RemoveAt(vAgentCategoryModel.CategoryDate.Value);
								vAgent.AgentCategories.Add(vAgentCategory, vAgentCategoryModel.CategoryDate);
								vAgent.Save();
							}
						}
						else
						{
							vAgent.AgentCategories.Add(vAgentCategory, vAgentCategoryModel.CategoryDate);
							vAgent.Save();
						}
					}
					else
					{
						ViewData["EditError"] = "Укажите категорию и её дату!";
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
				ViewData["vCategoryModel"] = vAgentCategoryModel;
		}

		#endregion < Страны >
	}
}