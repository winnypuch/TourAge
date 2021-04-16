#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер Заказов
	/// </summary>
	public class OrdersController : Controller
	{
		#region < Заказы >

		/// <summary>
		/// Таблица детализации заказов
		/// </summary>
		/// <returns></returns>
		public ActionResult OrdersGridPartial()
		{
			return PartialView("OrdersGridPartial", cOrders.Fill());
		}

		/// <summary>
		/// Добавление нового заказа
		/// </summary>
		/// <param name="vOrder">Модель заказа</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OrdersAddNewPartial(cOrder vOrder)
		{
			UpdateOrder(vOrder, true);

			return OrdersGridPartial();
		}

		/// <summary>
		/// Обновление заказа
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult OrdersUpdatePartial(cOrder vOrder)
		{
			UpdateOrder(vOrder);

			return OrdersGridPartial();
		}

		/// <summary>
		/// Удаление заказа
		/// </summary>
		/// <param name="iOrderId">Id заказа</param>
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
				ViewData["EditError"] = "Нет Id заказа!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении заказа возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление заказа
		/// </summary>
		/// <param name="vOrder">Модель заказа</param>
		/// <param name="bAddNewOrder">Добавить новый заказ</param>
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
						ViewData["EditError"] = "Укажите данные заказа!";
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
				ViewData["vOrder"] = vOrder;
		}

		#endregion < Заказы >
	}
}