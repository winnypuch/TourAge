﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
    public class cRout : IBase
    {
        public int Id { get; set; }

        [DisplayName("Название маршрута")]
        public string Name { get; set; }

        [DisplayName("Стоимость маршрута")]
        public decimal RCost { get; set; }

        [DisplayName("Начальный город")]
        public cCity CityStart { get; set; }

        [DisplayName("Конечный город")]
        public cCity CityEnd { get; set; }

        [DisplayName("URL")]
        public string ImageURL{ get; set; }

        [DisplayName("Описание")]
        public string Descriptions{ get; set; }

        public cRout()
        {
	        this.CityStart = new cCity();
	        this.CityEnd = new cCity();
        }

        /// <summary>
        /// Загрузка объекта
        /// </summary>
        /// <param name="iRouteId">Идентификатор маршрута</param>
        /// <returns></returns>
        public bool Load(int iRouteId)
        {
            List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = iRouteId },
                    };

            DataTable vRes = DataProvider.GetDataTable("Select Id, Name, RCost, CityStartId, CityEndId, ImageURL, Descriptions From Routes Where Id = @Id", vParams);

            if (vRes?.Rows.Count > 0)
            {
                DataRow vRow = vRes.Rows[0];
                if (vRow != null)
                {
                    this.Id = iRouteId;
                    this.Name = vRow["Name"].ToString();
                    this.RCost = Convert.ToDecimal(vRow["RCost"].ToString());
                    this.CityStart = new cCity();
                    this.CityStart.Load(Convert.ToInt32(vRow["CityStartId"]));
                    this.CityEnd = new cCity();
                    this.CityEnd.Load(Convert.ToInt32(vRow["CityEndId"]));
                    this.ImageURL = Convert.ToString(vRow["ImageURL"]);
                    this.Descriptions = Convert.ToString(vRow["Descriptions"]);

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
            List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = this.Id },
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = this.Name },
                       new SqlParameter("@RCost", SqlDbType.NVarChar) { Value = this.RCost },
                       new SqlParameter("@CityStartId", SqlDbType.NVarChar) { Value = this.CityStart.Id },
                       new SqlParameter("@CityEndId", SqlDbType.NVarChar) { Value = this.CityEnd.Id },
                       new SqlParameter("@ImageURL", SqlDbType.NVarChar) { Value = this.ImageURL },
                       new SqlParameter("@Descriptions", SqlDbType.NVarChar) { Value = this.Descriptions },
            };

            DataProvider.ExecuteQuery("UPDATE Routes SET Name = @Name, RCost = @RCost, CityStartId = @CityStartId, CityEndId = @CityEndId, ImageURL = @ImageURL, Descriptions = @Descriptions  WHERE id = @Id", vParams);
            return true;
        }
    }
}
