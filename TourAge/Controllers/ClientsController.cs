#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// ���������� ��������
	/// </summary>
	public class ClientsController : Controller
	{
		#region < ������� >

		/// <summary>
		/// ������� ����������� �����
		/// </summary>
		/// <returns></returns>
		public ActionResult ClientsGridPartial()
		{
			return PartialView("ClientsGridPartial", cClients.Fill());
		}

		/// <summary>
		/// ���������� ����� ������
		/// </summary>
		/// <param name="vClient">������ ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ClientsAddNewPartial(cClient vClient)
		{
			UpdateClient(vClient, true);

			return ClientsGridPartial();
		}

		/// <summary>
		/// ���������� ������
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ClientsUpdatePartial(cClient vClient)
		{
			UpdateClient(vClient);

			return ClientsGridPartial();
		}

		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="iClientId">Id ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ClientsDeletePartial(int iClientId)
		{
			if (iClientId > 0)
			{
				cClients vClients = cClients.Fill();
				string sError = vClients.RemoveById(iClientId, true);

				if(string.IsNullOrWhiteSpace(sError))
					ViewData["EditError"] = sError;
			}
			else
			{
				ViewData["EditError"] = "��� Id ������!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "��� �������� ������ ������� �������� ������:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// ����������/���������� ������
		/// </summary>
		/// <param name="vClient">������ ������</param>
		/// <param name="bAddNewClient">�������� ����� ������</param>
		private void UpdateClient(cClient vClient, bool bAddNewClient = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (vClient != null && !string.IsNullOrWhiteSpace(vClient.LastName) && !string.IsNullOrWhiteSpace(vClient.Patronymic) && !string.IsNullOrWhiteSpace(vClient.Name) && !string.IsNullOrWhiteSpace(vClient.Phones))
					{
						if (!bAddNewClient)
						{
							vClient.Save();
						}
						else
						{
							cClients vClients = cClients.Fill();
							vClients.Add(vClient, true);
						}
					}
					else
					{
						ViewData["EditError"] = "������� ��� � ��������!";
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
				ViewData["vClient"] = vClient;
		}

		#endregion < ������� >
	}
}