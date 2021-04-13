using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
    public class cCity : IBase
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Название города")]
        //[Required(ErrorMessage = "Необходимо ввести название города")]
        public string Name { get; set; }

        [DisplayName("Страна")]
        public cCountry Country { get; set; }

        public cCity()
        {
	        this.Country = new cCountry();
        }

        /// <summary>
        /// Загрузка объекта
        /// </summary>
        /// <param name="iCityId"></param>
        /// <returns></returns>
        public bool Load(int iCityId)
        {
            List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = iCityId },
                    };

            DataTable vRes = DataProvider.GetDataTable("Select Id, Name, CountryId From Cities Where Id = @Id", vParams);

            if (vRes?.Rows.Count > 0)
            {
                DataRow vRow = vRes.Rows[0];

                if (vRow != null)
                {
                    this.Id = iCityId;
                    this.Name = vRow["Name"].ToString();
                    this.Country = new cCountry();
                    this.Country.Load(Convert.ToInt32(vRow["CountryId"]));
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
                       new SqlParameter("@iCountryId", SqlDbType.Int) { Value = this.Country.Id },
                    };

            DataProvider.ExecuteQuery("UPDATE Cities SET Name = @Name, CountryId = @iCountryId WHERE id = @Id", vParams);
            return true;
        }
    }
}
