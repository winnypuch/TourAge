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
	public class OrdersController : Controller
	{
		#region < ������ >

		/// <summary>
		/// ������� ����������� �������
		/// </summary>
		/// <returns></returns>
		public ActionResult OrdersGridPartial()
		{
			return PartialView("OrdersGridPartial", cOrders.Fill());
		}

		/// <summary>
		/// ���������� ������ ������
		/// </summary>
		/// <param name="vOrder">������ ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OrdersAddNewPartial(cOrder vOrder)
		{
			UpdateOrder(vOrder, true);

			return OrdersGridPartial();
		}

		/// <summary>
		/// ���������� ������
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OrdersUpdatePartial(cOrder vOrder)
		{
			UpdateOrder(vOrder);

			return OrdersGridPartial();
		}

		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="iOrderId">Id ������</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OrdersDeletePartial(int iOrderId)
		{
			if (iOrderId > 0)
			{
				cOrders vOrders = cOrders.Fill();
				string sError = vOrders.RemoveById(iOrderId, true);

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
		/// <param name="vOrder">������ ������</param>
		/// <param name="bAddNewOrder">�������� ����� �����</param>
		private void UpdateOrder(cOrder vOrder, bool bAddNewOrder = false)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (vOrder != null && !string.IsNullOrWhiteSpace(vOrder.Name) && vOrder.Client.Id != 0 && vOrder.Tour.Id != 0 && vOrder.ClientsCount > 0)
					{
						if (!bAddNewOrder)
						{
							vOrder.Save();
						}
						else
						{
							cOrders vOrders = cOrders.Fill();
							vOrders.Add(vOrder, true);
						}
					}
					else
					{
						ViewData["EditError"] = "������� ������ ������!";
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
				ViewData["vOrder"] = vOrder;
		}

		#endregion < ������ >
	}
}