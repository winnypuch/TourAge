#region < Usings >

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// ���������� �������
	/// </summary>
	public class CitiesController : Controller
	{
		#region < ������ >

		/// <summary>
		/// ������� ����������� �������
		/// </summary>
		/// <returns></returns>
		public ActionResult CitiesGridPartial()
		{
			return PartialView("CitiesGridPartial", cCities.Fill());
		}

		/// <summary>
		/// ���������� ������ ������
		/// </summary>
		/// <param name="vCity">������ ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CitiesAddNewPartial(cCity vCity)
		{
			UpdateCity(vCity, true);

			return CitiesGridPartial();
		}

		/// <summary>
		/// ���������� ������
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CitiesUpdatePartial(cCity vCity)
		{
			UpdateCity(vCity);

			return CitiesGridPartial();
		}

		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="iCityId">Id ��������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CitiesDeletePartial(int iCityId)
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
						ViewData["EditError"] = "������ ��������� ������ ��������!";
					}
			}
			else
			{
				ViewData["EditError"] = "��� ���� ���������!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "��� �������� ��������� �������� ������:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// ����������/���������� ������
		/// </summary>
		/// <param name="vCity">������ ������</param>
		/// <param name="bAddNewCity">�������� ����� �����</param>
		private void UpdateCity(cCity vCity, bool bAddNewCity = false)
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
						ViewData["EditError"] = "������� ��������� � � ����!";
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

		#endregion < ������ >
	}
}