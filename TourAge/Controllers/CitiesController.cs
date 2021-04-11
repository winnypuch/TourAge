#region < Usings >

using System;
using System.Collections.Generic;
using System.Web.Mvc;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// ���������� �������
	/// </summary>
	public class CitiesController : Controller
	{
		#region < ��������� >

		/// <summary>
		/// ������� ����������� �������
		/// </summary>
		/// <returns></returns>
		public ActionResult CitiesGridPartial()
		{
			return PartialView("CitiesGridPartial", AgentCategoryModel.FillAgentCategories(iSubscriberId));
		}

		/// <summary>
		/// ���������� ������ ���������
		/// </summary>
		/// <param name="iContractId">Id ��������</param>
		/// <param name="iLocationId">Id �����������������</param>
		/// <param name="iSubscriberId">Id ��������</param>
		/// <param name="vAgentCategoryModel">������ ��������� ��������</param>
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
		/// ���������� ���������
		/// </summary>
		/// <param name="iContractId">Id ��������</param>
		/// <param name="iLocationId">Id �����������������</param>
		/// <param name="iSubscriberId">Id ��������</param>
		/// <param name="vAgentCategoryModel">������ ��������� ��������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, CategoryUpdate")]
		public ActionResult CategoriesUpdatePartial(int? iContractId, int? iLocationId, int iSubscriberId, AgentCategoryModel vAgentCategoryModel)
		{
			UpdateCategory(iSubscriberId, vAgentCategoryModel);

			return CategoriesDetailGridPartial(iContractId, iLocationId, iSubscriberId);
		}

		/// <summary>
		/// �������� ���������
		/// </summary>
		/// <param name="iContractId">Id ��������</param>
		/// <param name="iLocationId">Id �����������������</param>
		/// <param name="iSubscriberId">Id ��������</param>
		/// <param name="dCategoryDate">���� ���������</param>
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
						ViewData["EditError"] = "������ ��������� ������ ��������!";
					}
				}
				else
				{
					ViewData["EditError"] = "������ ���������� � ��������!";
				}
			}
			else
			{
				ViewData["EditError"] = "��� ���� ���������!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "��� �������� ��������� �������� ������:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// ����������/���������� ���������
		/// </summary>
		/// <param name="iSubscriberId">Id ��������</param>
		/// <param name="vAgentCategoryModel">������ ��������� ��������</param>
		/// <param name="bAddNewCategory">�������� ����� ��������� ��������</param>
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
							ViewData["EditError"] = "������� ��������� � � ����!";
						}
					}
					else
					{
						ViewData["EditError"] = "������ ���������� � ��������!";
					}
				}
				catch (Exception e)
				{
					ViewData["EditError"] = "����������� ������ " + e.Message;
				}
			}
			else
			{
				ViewData["EditError"] = "��������� ��� ������.";
			}

			if (ViewData["EditError"] != null)
				ViewData["vCategoryModel"] = vAgentCategoryModel;
		}

		#endregion < ��������� >
	}
}