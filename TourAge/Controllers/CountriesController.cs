#region < Usings >

using System;
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
			if (iCountryId > 0)
			{
				cCountries vCountries = cCountries.Fill();
				string sError = vCountries.RemoveById(iCountryId, true);

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
		/// <param name="vCountry">������ ������</param>
		/// <param name="bAddNewCountry">�������� ����� ������</param>
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
						ViewData["EditError"] = "������� ������ � � ���!";
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
				ViewData["vCountry"] = vCountry;
		}

		#endregion < ������ >
	}
}