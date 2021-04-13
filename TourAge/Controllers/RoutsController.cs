#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// ���������� ���������
	/// </summary>
	public class RoutsController : Controller
	{
		#region < �������� >

		/// <summary>
		/// ������� ����������� ���������
		/// </summary>
		/// <returns></returns>
		public ActionResult RoutsGridPartial()
		{
			return PartialView("RoutsGridPartial", cRouts.Fill());
		}

		/// <summary>
		/// ���������� ������ ��������
		/// </summary>
		/// <param name="vRout">������ ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoutsAddNewPartial(cRout vRout)
		{
			UpdateRout(vRout, true);

			return RoutsGridPartial();
		}

		/// <summary>
		/// ���������� ��������
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoutsUpdatePartial(cRout vRout)
		{
			UpdateRout(vRout);

			return RoutsGridPartial();
		}

		/// <summary>
		/// �������� ��������
		/// </summary>
		/// <param name="iRoutId">Id ��������</param>
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
				ViewData["EditError"] = "��� Id ��������!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "��� �������� �������� �������� ������:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// ����������/���������� ��������
		/// </summary>
		/// <param name="vRout">������ ��������</param>
		/// <param name="bAddNewRout">�������� ����� �������</param>
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
				ViewData["vRout"] = vRout;
		}

		#endregion < �������� >
	}
}