#region < Usings >

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Web.Mvc;
using DevExpress.XtraReports.UI;


#endregion <Using>

namespace TourAge.Controllers
{
	/// <summary>
	/// Контроллер печати
	/// </summary>

	public class PrintController : Controller
	{
		#region < Print >

		/// <summary>
		/// Вызов печатной формы
		/// </summary>
		/// <param name="iReportNumber">Id формы</param>
		/// <param name="dDateBegin">Дата начала</param>
		/// <param name="dDateEnd">Дата окончания</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult PrintForm(int iReportNumber, DateTime dDateBegin, DateTime dDateEnd)
		{
			XtraReport vReport = new XtraReport();

			ViewData["iReportNumber"] = iReportNumber;


			if (iReportNumber != 0)
			{

				//// Загрузим представление
				//using (MemoryStream vStream = new MemoryStream())
				//{
				//	StreamWriter vStreamWriter = new StreamWriter(vStream);
				//	vStreamWriter.Write(vPrintViewForm.Layout);
				//	vStreamWriter.Flush();

				//	vStream.Seek(0, SeekOrigin.Begin);

				//	vReport = new XtraReport();
				//	vReport.LoadLayoutFromXml(vStream);
				//	vStreamWriter.Close();
				//}
				string sReportPath = "", sReportName= "Отчёт";
				object vDataSource = null;

				switch (iReportNumber)
				{
					case 1:
						sReportPath = Server.MapPath("/Reports/Routes_report.repx");
						List<SqlParameter> vParams = new List<SqlParameter> {
							new SqlParameter("@dDateStart", SqlDbType.DateTime) { Value = dDateBegin },
							new SqlParameter("@dDateEnd", SqlDbType.DateTime) { Value = dDateEnd },
						};

						vDataSource = DataProvider.GetDataTable("SELECT R.Id, R.Name AS RouteName, COUNT(O.Id) AS OrderCount FROM ROUTES AS R" +
						                                        " INNER JOIN ROUTESINTOUR AS RIT ON R.Id = RIT.RouteId INNER JOIN TOURS AS T ON T.Id = RIT.TourId" +
						                                        " INNER JOIN ORDERS AS O ON O.TourId = T.Id WHERE O.Date >= @dDateStart AND O.Date < @dDateEnd" +
						                                        " GROUP BY R.Id, R.Name ORDER BY R.Name", vParams);
						sReportName = "Отчёт 1";
						break;
					case 2:
						sReportPath = Server.MapPath("/Reports/Routes_report2.repx");
						vDataSource = DataProvider.GetDataTable("SELECT C.Id, C.LastName AS SurName, C.Name AS Name" +
						                                        ", C.Patronymic AS Patronymic, SUM(GR.ClientsCount) AS QtyManInOrder" +
						                                        ", SUM(GR.RQty) AS QtyRoute, SUM(GR.RSum) AS SumRoute" +
						                                        ", SUM(GR.LSum) AS SumHome, SUM(GR.FSum) AS SumOrder" +
						                                        " FROM (SELECT O.ClientId AS ClientId, O.ClientsCount AS ClientsCount, COUNT(R.Id) AS RQty" +
						                                        ", SUM(R.RCost) AS RSum, SUM(T.CostOfLiving) AS LSum, (SUM(R.RCost) + SUM(T.CostOfLiving))" +
						                                        " * O.ClientsCount AS FSum FROM ROUTES AS R INNER JOIN ROUTESINTOUR AS RIT ON R.Id = RIT.RouteId" +
						                                        " INNER JOIN TOURS AS T ON T.Id = RIT.TourId INNER JOIN ORDERS AS O ON O.TourId = T.Id" +
						                                        " GROUP BY O.Id, O.ClientsCount, O.ClientId) AS GR INNER JOIN CLIENTS AS C ON GR.ClientId = C.Id" +
						                                        "  GROUP BY C.Id, C.LastName, C.Name, C.Patronymic ORDER BY C.LastName, C.Name, C.Patronymic");
						break;
					case 3:
						sReportPath = Server.MapPath("/Reports/Routes_report3.repx");
						vDataSource = DataProvider.GetDataTable("SELECT DATE_FORMAT('%Y-%M', GR.odate) AS Months, SUM(GR.ClientsCount) AS QtyMenInOrder" +
						                                        ", SUM(GR.RQty) AS QtyRoute , SUM(GR.RSum) AS SumRoute , SUM(GR.LSum) AS SumHome" +
						                                        ", SUM(GR.FSum) AS SumOrder FROM (SELECT O.Date AS Odate , O.ClientsCount AS ClientsCount , COUNT(R.Id) AS RQty" +
						                                        ", SUM(R.RCost) AS RSum , SUM(T.CostOfLiving) AS LSum , (SUM(R.RCost) + SUM(T.CostOfLiving)) * O.ClientsCount AS FSum" +
						                                        " FROM ROUTES AS R INNER JOIN ROUTESINTOUR AS RIT ON R.Id = RIT.RouteId INNER JOIN TOURS AS T ON T.Id = RIT.TourId" +
						                                        " INNER JOIN ORDERS AS O ON O.TourId = T.Id GROUP BY O.Id, O.ClientsCount, O.ClientId) AS GR" +
						                                        " GROUP BY DATE_FORMAT('%Y-%M', GR.odate) ORDER BY DATE_FORMAT('%Y-%M', GR.odate)");
						break;
				}



				vReport.LoadLayout(sReportPath);
				vReport.DisplayName = sReportName;
				vReport.DataSource = new ArrayList {vDataSource};
				vReport.RequestParameters = false;
				vReport.CreateDocument(false);
			}

			return View(vReport);
		}

		/// <summary>
		/// Событие перед печатью подотчёта
		/// </summary>
		/// <param name="sender">Источник</param>
		/// <param name="e">Аргументы</param>
		private void Subreport_BeforePrint(object sender, PrintEventArgs e)
		{
			if (sender is XRSubreport vSubreport)
			{
				vSubreport.ReportSource.DataSource = new ArrayList {vSubreport.RootReport.GetCurrentRow()};
			}
		}

		#endregion < Print >
	}
}