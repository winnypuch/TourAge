using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
    public class cOrder: IBase
    {
        public int Id { get; set; }

        [DisplayName("Наименование заказа")]
        public string Name { get; set; }

        [DisplayName("Дата")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Кол-во человек")]
        public int ClientsCount { get; set; }

        [DisplayName("Тур")]
        public cTour Tour { get; set; }

        [DisplayName("Клиент")]
        public cClient Client { get; set; }


        public cOrder()
        {
	        this.Tour = new cTour();
	        this.Client = new cClient();
        }

        /// <summary>
        /// Загрузка объекта
        /// </summary>
        /// <param name="iOrderId"></param>
        /// <returns></returns>
        public bool Load(int iOrderId)
        {
            List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = iOrderId },
                    };

            DataTable vRes = DataProvider.GetDataTable("Select Id, Name, Date, ClientsCount, TourId, ClientId From Orders Where Id = @Id", vParams);

            if (vRes?.Rows.Count > 0)
            {
                DataRow vRow = vRes.Rows[0];
                if (vRow != null)
                {
                    this.Id = iOrderId;
                    this.Name = Convert.ToString(vRow["Name"]);
                    this.OrderDate = Convert.ToDateTime(vRow["Date"]);
                    this.ClientsCount = Convert.ToInt32(vRow["ClientsCount"]);
                    this.Tour = new cTour();
                    this.Tour.Load(Convert.ToInt32(vRow["TourId"]));
                    this.Client = new cClient();
                    this.Client.Load(Convert.ToInt32(vRow["ClientId"]));

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
                       new SqlParameter("@Date", SqlDbType.DateTime) { Value = this.OrderDate },
                       new SqlParameter("@ClientsCount", SqlDbType.Int) { Value = this.ClientsCount },
                       new SqlParameter("@iTourId", SqlDbType.Int) { Value = this.Tour.Id },
                       new SqlParameter("@iClientId", SqlDbType.Int) { Value = this.Client.Id },
            };

            DataProvider.ExecuteQuery("UPDATE Orders SET Date = @Date, Name = @Name, ClientsCount = @ClientsCount, TourId = @iTourId, ClientId = @iClientId WHERE id = @Id", vParams);
            return true;
        }
    }
}
