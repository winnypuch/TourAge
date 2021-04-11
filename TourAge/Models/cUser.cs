using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
	public class cUser : IBase
    {
        public int Id { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Имя")]
        public string Name { get; set; }

        /// <summary>
        /// Загрузка объекта
        /// </summary>
        /// <param name="iClientId"></param>
        /// <returns></returns>
        public bool Load(int iClientId)
        {
            List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = iClientId },
                    };

            DataTable vRes = DataProvider.GetDataTable("Select Id, Login, Name, Password From Users Where Id = @Id", vParams);

            if (vRes != null && vRes.Rows.Count > 0)
            {
                DataRow vRow = vRes.Rows[0];
                if (vRow != null) {
                    this.Id = iClientId;
                    this.Login = vRow["Login"].ToString();
                    this.Password = vRow["Password"].ToString();
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
                       new SqlParameter("@Login", SqlDbType.NVarChar) { Value = this.Login },
                       new SqlParameter("@Password", SqlDbType.NVarChar) { Value = this.Password },
                      new SqlParameter("@Name", SqlDbType.NVarChar) { Value = this.Name },
                    };

            DataProvider.ExecuteQuery("UPDATE Users SET Login = @Login, Password = @Password, Name = @Name WHERE id = @Id", vParams);
            return true;
        }
    }
}
