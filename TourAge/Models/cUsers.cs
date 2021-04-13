using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
	public class cUsers : BindingList<cUser>, IExtCollections<cUser>
    {
        private Dictionary<int, cUser> _vDictIdObject;

        public cUsers()
        {
	        _vDictIdObject = new Dictionary<int, cUser>();
        }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cUser>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Users");

            if (vRes?.Rows.Count > 0)
            {
                foreach(DataRow vRow in vRes.Rows)
                {
                    cUser vcUser = new cUser();
                    vcUser.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcUser);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vUser"></param>
        public new void Add(cUser vUser)
        {
            this.Add(vUser, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vUser"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cUser vUser, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vUser, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vUser">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cUser vUser)
        {
            this.Insert(iIndex, vUser, false, true);
        }

        /// <summary>
        /// Вставка элемента в коллекцию по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс</param>
        /// <param name="vUser">Клиент</param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cUser vUser, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Login", SqlDbType.NVarChar) { Value = vUser.Login },
                       new SqlParameter("@Password", SqlDbType.NVarChar) { Value = vUser.Password },
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vUser.Name },
                       //new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = vUser.LastName },
                       //new SqlParameter("@Patronymic", SqlDbType.NVarChar) { Value = vUser.Patronymic },
                       //new SqlParameter("@Phones", SqlDbType.NVarChar) { Value = vUser.Phones },
                    };

                //vUser.Id = DataProvider.ExecuteQuery("INSERT INTO Users(Login, Password, Name, LastName, Patronymic, Phones) values(@Login , @Password , @Name, @LastName, @Patronymic, @Phones)", vParams, true);
                vUser.Id = DataProvider.ExecuteQuery("INSERT INTO Users(Login, Password, Name) values(@Login , @Password , @Name)", vParams, true);
            }
            _vDictIdObject.Add(vUser.Id, vUser);

            if(bAddToList)
                base.Insert(iIndex, vUser);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cUser GetObjectById(int Id)
        {
            return _vDictIdObject.ContainsKey(Id) ? _vDictIdObject[Id] : null;
        }

        /// <summary>
        /// Удаляет объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="bRemoveToDatabase">Удалить из базы данных</param>
        public string RemoveById(int Id, bool bRemoveToDatabase = false)
        {
            cUser vUser = this.GetObjectById(Id);

            if(vUser != null)
                return this.Remove(vUser, bRemoveToDatabase);
            return null;
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vUser">Удаляемый объект</param>
        public new string Remove(cUser vUser)
        {
            return this.Remove(vUser, true);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vUser">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public string Remove(cUser vUser, bool bRemoveToDatabase = false)
        {
            if (bRemoveToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vUser.Id },
                    };

                DataProvider.ExecuteQuery("DELETE FROM Users WHERE Id = @Id", vParams);
            }

            if (_vDictIdObject.ContainsKey(vUser.Id))
                _vDictIdObject.Remove(vUser.Id);

            base.Remove(vUser);
            return null;
        }
    }
}
