#region < Usings >

using System;
using System.Web.Mvc;
using TourAge.Models;

#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер клиентов
	/// </summary>
	public class ClientsController : Controller
	{
		#region < Клиенты >

		/// <summary>
		/// Таблица детализации стран
		/// </summary>
		/// <returns></returns>
		public ActionResult ClientsGridPartial()
		{
			return PartialView("ClientsGridPartial", cClients.Fill());
		}

		/// <summary>
		/// Добавление новой страны
		/// </summary>
		/// <param name="vClient">Модель страны</param>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ClientsAddNewPartial(cClient vClient)
		{
			UpdateClient(vClient, true);

			return ClientsGridPartial();
		}

		/// <summary>
		/// Обновление страны
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ClientsUpdatePartial(cClient vClient)
		{
			UpdateClient(vClient);

			return ClientsGridPartial();
		}

		/// <summary>
		/// Удаление страны
		/// </summary>
		/// <param name="iClientId">Id страны</param>
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
				ViewData["EditError"] = "Нет Id страны!";
			}

			return ViewData["EditError"] == null ? Json(new {Result = "OK"}) : Json(new {Result = "Error", Error = "При удалении данных клиента возникла ошибка:\n" + ViewData["EditError"]});
		}

		/// <summary>
		/// Добавление/обновление страны
		/// </summary>
		/// <param name="vClient">Модель страны</param>
		/// <param name="bAddNewClient">Добавить новую страну</param>
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
						ViewData["EditError"] = "Укажите ФИО и телефоны!";
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
				ViewData["vClient"] = vClient;
		}

		#endregion < Клиенты >
	}
}