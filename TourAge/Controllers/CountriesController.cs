#region < Usings >

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// ���������� �����
	/// </summary>
	public class CountriesController : Controller
	{
		#region < ������ >

		/// <summary>
		/// ������� ����������� �����
		/// </summary>
		/// <returns></returns>
		public ActionResult CountriesGridPartial()
		{
			return PartialView("CountriesGridPartial", cCountries.Fill());
		}

		/// <summary>
		/// ���������� ����� ������
		/// </summary>
		/// <param name="vCountry">������ ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CountriesAddNewPartial(cCountry vCountry)
		{
			UpdateCountry(vCountry, true);

			return CountriesGridPartial();
		}

		/// <summary>
		/// ���������� ������
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CountriesUpdatePartial(cCountry vCountry)
		{
			UpdateCountry(vCountry);

			return CountriesGridPartial();
		}

		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="iCountryId">Id ������</param>
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
		/// <param name="vCountry">������ ������</param>
		/// <param name="bAddNewCountry">�������� ����� ������</param>
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