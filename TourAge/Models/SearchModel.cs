#region < Usings >

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DevExpress.Web.Mvc;

#endregion < Usings >

namespace TourAge.Models
{
	/// <summary>
	/// Модель поиска
	/// </summary>
	public class SearchModel
	{
		/// <summary>
		/// Модель поиска
		/// </summary>
		public SearchModel()
		{
		}

		/// <summary>
		/// Id города
		/// </summary>
		public int CityId { get; set; }

		/// <summary>
		/// Дата начала льготного удостоверения
		/// </summary>
		[Display(Name = "Дата начала")]
		public DateTime DateBegin{ get; set; }

		/// <summary>
		/// Дата окончания льготного удостоверения
		/// </summary>
		[Display(Name = "Дата окончания")]
		[DateRange(StartDateEditFieldName = "DateBegin", MinDayCount = 1, MaxDayCount = 30)]
		public DateTime DateEnd{ get; set; }

		/// <summary>
		/// Заполнение информации по поиску
		/// </summary>
		/// <returns></returns>
		/// <param name="iSubscriberId">Id абонента</param>
		/// <returns></returns>
		public static ICollection<SearchModel> FillSearch(int iSubscriberId)
		{
			List<SearchModel> vSearchModel = new List<SearchModel>();

			return vSearchModel;
		}
	}
}