#region < Usings >

using System;
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
				cCities vCities = cCities.Fill();
				string sError = vCities.RemoveById(iCityId, true);

				if(string.IsNullOrWhiteSpace(sError))
					ViewData["EditError"] = sError;
			}
			else
			{
				ViewData["EditError"] = "��� Id ������!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "��� �������� ������ �������� ������:\n" + ViewData["EditError"]});
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
						ViewData["EditError"] = "������� ����� � ��� ������!";
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
				ViewData["vCity"] = vCity;
		}

		#endregion < ������ >
	}
}