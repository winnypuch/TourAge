using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
    public class cClient : IBase
    {
        public int Id { get; set; }

        //[DisplayName("Логин")]
        //public string Login { get; set; }

        //[DisplayName("Пароль")]
        //public string Password { get; set; }

        [DisplayName("Имя")]
        public string Name { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        public string Patronymic { get; set; }

        [DisplayName("Телефоны")]
        public string Phones { get; set; }

        [DisplayName("ФИО")]
        public string FIO
        {
            get
            {
                return string.Format("{0} {1}. {2}.", string.IsNullOrWhiteSpace(this.LastName) ? "" : this.LastName, string.IsNullOrWhiteSpace(this.Name) ? "" : this.Name.Substring(0, 1), string.IsNullOrWhiteSpace(this.Patronymic) ? "" : this.Patronymic.Substring(0, 1));
            }
        }
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

            DataTable vRes = DataProvider.GetDataTable("Select Id, Name, LastName, Patronymic, Phones From Clients Where Id = @Id", vParams);

            if (vRes?.Rows.Count > 0)
            {
                DataRow vRow = vRes.Rows[0];

                if (vRow != null)
                {
                    this.Id = iClientId;
                    //this.Login = vRow["Login"].ToString();
                    //this.Password = vRow["Password"].ToString();
                    this.Name = vRow["Name"].ToString();
                    this.LastName = vRow["LastName"].ToString();
                    this.Patronymic = vRow["Patronymic"].ToString();
                    this.Phones = vRow["Phones"].ToString();
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
                       //new SqlParameter("@Login", SqlDbType.NVarChar) { Value = this.Login },
                       //new SqlParameter("@Password", SqlDbType.NVarChar) { Value = this.Password },
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = this.Name },
                       new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = this.LastName },
                       new SqlParameter("@Patronymic", SqlDbType.NVarChar) { Value = this.Patronymic },
                       new SqlParameter("@Phones", SqlDbType.NVarChar) { Value = this.Phones },
                    };

            DataProvider.ExecuteQuery("UPDATE Clients SET Name = @Name, LastName = @LastName, Phones = @Phones, Patronymic = @Patronymic WHERE id = @Id", vParams);
            return true;
        }
    }
}
