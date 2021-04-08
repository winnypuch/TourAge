using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAgency
{
	public class cCountry : IBase
	{
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Название страны")]
        public string Name { get; set; }

        /// <summary>
        /// Загрузка объекта
        /// </summary>
        /// <param name="iCountryId"></param>
        /// <returns></returns>
        public bool Load(int iCountryId)
        {
            List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = iCountryId },
                    };

            DataTable vRes = DataProvider.GetDataTable("Select Id, Name From Countries Where Id = @Id", vParams);

            if (vRes != null && vRes.Rows.Count > 0)
            {
                DataRow vRow = vRes.Rows[0];
                if (vRow != null)
                {
                    this.Id = iCountryId;
                    this.Name = vRow["Name"].ToString();

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
            };

            DataProvider.ExecuteQuery("UPDATE Countries SET Name = @Name WHERE id = @Id", vParams);
            return true;
        }
    }
}
