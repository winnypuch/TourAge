#region < Usings >

using System;
using System.Collections.Generic;
using System.Web.Mvc;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер городов
	/// </summary>
	public class CitiesController : Controller
	{
		#region < Категории >

		/// <summary>
		/// Таблица детализации городов
		/// </summary>
		/// <returns></returns>
		public ActionResult CitiesGridPartial()
		{
			return PartialView("CitiesGridPartial", AgentCategoryModel.FillAgentCategories(iSubscriberId));
		}

		/// <summary>
		/// Добавление нового категории
		/// </summary>
		/// <param name="iContractId">Id договора</param>
		/// <param name="iLocationId">Id месторасположения</param>
		/// <param name="iSubscriberId">Id абонента</param>
		/// <param name="vAgentCategoryModel">Модель категории абонента</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, CategoryAdd")]
		public ActionResult CategoriesAddNewPartial(int? iContractId, int? iLocationId, int iSubscriberId, AgentCategoryModel vAgentCategoryModel)
		{
			UpdateCategory(iSubscriberId, vAgentCategoryModel, true);

			return CategoriesDetailGridPartial(iContractId, iLocationId, iSubscriberId);
		}

		[HttpPost]
		public ActionResult CategoriesAddPartial(string s)
		{
			return PartialView("CategoriesAddPartial", new AgentCategoryModel());
		}

		/// <summary>
		/// Обновление категории
		/// </summary>
		/// <param name="iContractId">Id договора</param>
		/// <param name="iLocationId">Id месторасположения</param>
		/// <param name="iSubscriberId">Id абонента</param>
		/// <param name="vAgentCategoryModel">Модель категории абонента</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, CategoryUpdate")]
		public ActionResult CategoriesUpdatePartial(int? iContractId, int? iLocationId, int iSubscriberId, AgentCategoryModel vAgentCategoryModel)
		{
			UpdateCategory(iSubscriberId, vAgentCategoryModel);

			return CategoriesDetailGridPartial(iContractId, iLocationId, iSubscriberId);
		}

		/// <summary>
		/// Удаление категории
		/// </summary>
		/// <param name="iContractId">Id договора</param>
		/// <param name="iLocationId">Id месторасположения</param>
		/// <param name="iSubscriberId">Id абонента</param>
		/// <param name="dCategoryDate">Дата категории</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, CategoryDelete")]
		public ActionResult CategoriesDeletePartial(int? iContractId, int? iLocationId, int iSubscriberId, DateTime? dCategoryDate)
		{
			if (dCategoryDate.HasValue)
			{
				SessionCache vSessionCache;

				if ((vSessionCache = ServiceConnectionAdapter.Instance.CheckConnect()) != null && vSessionCache.DataBaseProvider != null)
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
					ViewData["EditError"] = "Ошибка соединения с сервером!";
				}
			}
			else
			{
				ViewData["EditError"] = "Нет даты категории!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении категории возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление категории
		/// </summary>
		/// <param name="iSubscriberId">Id абонента</param>
		/// <param name="vAgentCategoryModel">Модель категории абонента</param>
		/// <param name="bAddNewCategory">Добавить новую категорию абонента</param>
		private void UpdateCategory(int iSubscriberId, AgentCategoryModel vAgentCategoryModel, bool bAddNewCategory = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					SessionCache vSessionCache;

					if ((vSessionCache = ServiceConnectionAdapter.Instance.CheckConnect()) != null && vSessionCache.DataBaseProvider != null)
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
					else
					{
						ViewData["EditError"] = "Ошибка соединения с сервером!";
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

		#endregion < Категории >
	}
}