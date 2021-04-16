using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TourAge.Models
{
	public class cTour : IBase
	{
		[DisplayName("Id")] public int Id { get; set; }

		[DisplayName("Название тура")] public string Name { get; set; }

		[DisplayName("Стоимость проживания")] public decimal CostOfLiving { get; set; }

		[DisplayName("Дата начала")] public DateTime TDateBegin { get; set; }

		[DisplayName("Дата окончания")] public DateTime TDateEnd { get; set; }

		[DisplayName("Маршруты")] public cRouts Routs { get; set; }

		public int[] RoutsIDs { get; set; }

		[DisplayName("URL")]
		public string ImageURL{ get; set; }

		[DisplayName("Описание")]
		public string Descriptions{ get; set; }

		[DisplayName("Стоимость тура")]
		public decimal CostTour { get; set; }

		public string RoutsIDsString { get; set; }
		//{
		//	get
		//	{
		//		int[] RoutsIDs = this.RoutsIDs;

		//		return cRouts.Fill().Where(t => RoutsIDs.Contains(t.Id)).Select(t => t.Name).DefaultIfEmpty()
		//			.Aggregate((a, b) => a + ", " + b);
		//	}
		//}

		public string TourCities { get; set; }


		public cTour()
		{
			this.Routs = new cRouts();
		}

		/// <summary>
		/// Загрузка объекта
		/// </summary>
		/// <param name="iTourId"></param>
		/// <returns></returns>
		public bool Load(int iTourId)
		{
			List<SqlParameter> vParams = new List<SqlParameter>
			{
				new SqlParameter("@Id", SqlDbType.Int) {Value = iTourId},
			};

			DataTable vRes = DataProvider.GetDataTable("Select Id, Name, CostOfLiving, TDateBegin, TDateEnd, ImageURL, Descriptions From Tours Where Id = @Id", vParams);

			if (vRes?.Rows.Count > 0)
			{
				DataRow vRow = vRes.Rows[0];
				if (vRow != null)
				{
					this.Id = iTourId;
					this.Name = vRow["Name"].ToString();
					this.CostOfLiving = Convert.ToDecimal(vRow["CostOfLiving"]);
					this.TDateBegin = Convert.ToDateTime(vRow["TDateBegin"]);
					this.TDateEnd = Convert.ToDateTime(vRow["TDateEnd"]);
					this.Routs = new cRouts();
					this.ImageURL = Convert.ToString(vRow["ImageURL"]);
					this.Descriptions = Convert.ToString(vRow["Descriptions"]);

					List<SqlParameter> vRParams = new List<SqlParameter>
					{
						new SqlParameter("@Id", SqlDbType.Int) {Value = iTourId},
					};
					DataTable vRouts = DataProvider.GetDataTable("Select RouteId From RoutesInTour Where TourId = @Id", vRParams);

					this.RoutsIDs = new int[] {};
					this.TourCities = "";
					this.CostTour = this.CostOfLiving;
					this.RoutsIDsString = "";

					if (vRouts.Rows.Count > 0)
					{
						foreach (DataRow vRRow in vRouts.Rows)
						{
							cRout vcRout = new cRout();
							vcRout.Load(Convert.ToInt32(vRRow["RouteId"]));
							this.Routs.Add(vcRout);
							int[] vArr = this.RoutsIDs;

							Array.Resize(ref vArr, vArr.Length + 1);
							vArr[vArr.GetUpperBound(0)] = vcRout.Id;
							this.RoutsIDs = vArr;

							if (!string.IsNullOrWhiteSpace(this.TourCities))
								this.TourCities += "<br/>";
							if (!string.IsNullOrWhiteSpace(this.RoutsIDsString))
								this.RoutsIDsString += ", ";

							this.RoutsIDsString += vcRout.Id.ToString();
							this.TourCities += vcRout.CityStart.Name;
							this.CostTour += vcRout.RCost;
						}
					}

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Сохранение клиента
		/// </summary>
		/// <returns></returns>
		public bool Save()
		{
			List<SqlParameter> vParams = new List<SqlParameter>
			{
				new SqlParameter("@Id", SqlDbType.Int) {Value = this.Id},
				new SqlParameter("@Name", SqlDbType.NVarChar) {Value = this.Name},
				new SqlParameter("@CostOfLiving", SqlDbType.Decimal) {Value = this.CostOfLiving},
				new SqlParameter("@TDateBegin", SqlDbType.DateTime) {Value = this.TDateBegin},
				new SqlParameter("@TDateEnd", SqlDbType.DateTime) {Value = this.TDateEnd},
				new SqlParameter("@ImageURL", SqlDbType.NVarChar) { Value = this.ImageURL },
				new SqlParameter("@Descriptions", SqlDbType.NVarChar) { Value = this.Descriptions },
			};

			DataProvider.ExecuteQuery("UPDATE Tours SET Name = @Name, CostOfLiving = @CostOfLiving, TDateBegin = @TDateBegin, TDateEnd = @TDateEnd, ImageURL = @ImageURL, Descriptions = @Descriptions WHERE id = @Id", vParams);
			return true;
		}

		/// <summary>
		/// Удаление маршрутов из базы данных
		/// </summary>
		public void RemoveRoutes()
		{
			List<SqlParameter> vParams = new List<SqlParameter>
			{
				new SqlParameter("@Id", SqlDbType.Int) {Value = this.Id},
			};

			DataProvider.ExecuteQuery("DELETE FROM RoutesInTour WHERE TourId = @Id", vParams);


			this.Routs.Clear();
		}

		/// <summary>
		/// Запись маршрутов в базу данных
		/// </summary>
		public void SaveRoutes()
		{
			if (this.Routs.Count > 0)
			{
				this.RoutsIDs = new int[] {};

				foreach (cRout vRout in this.Routs)
				{
					List<SqlParameter> vParams = new List<SqlParameter>
					{
						new SqlParameter("@RouteId", SqlDbType.Int) {Value = vRout.Id},
						new SqlParameter("@TourId", SqlDbType.Int) {Value = this.Id},
					};

					DataProvider.ExecuteQuery("INSERT INTO RoutesInTour (RouteId, TourId) VALUES(@RouteId, @TourId)", vParams);
					int[] vArr = this.RoutsIDs;

					Array.Resize(ref vArr, vArr.Length + 1);
					vArr[vArr.GetUpperBound(0)] = vRout.Id;
					this.RoutsIDs = vArr;
				}
			}
		}
	}
}
