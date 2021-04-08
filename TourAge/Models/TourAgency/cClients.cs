using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TourAgency
{
    public class cClients : BindingList<cClient>, IExtCollections<cClient>
    {
        private Dictionary<int, cClient> _vDictIdObject;

        public cClients()
        {
            _vDictIdObject = new Dictionary<int, cClient>();
        }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cClient>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Clients");

            if (vRes != null && vRes.Rows.Count > 0)
            {
                foreach (DataRow vRow in vRes.Rows)
                {
                    cClient vcClient = new cClient();
                    vcClient.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcClient);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vClient"></param>
        public new void Add(cClient vClient)
        {
            this.Add(vClient, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vClient"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cClient vClient, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vClient, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vClient">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cClient vClient)
        {
            this.Insert(iIndex, vClient, false, true);
        }

        /// <summary>
        /// Вставка элемента в коллекцию по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс</param>
        /// <param name="vClient">Клиент</param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cClient vClient, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       //new SqlParameter("@Login", SqlDbType.NVarChar) { Value = vClient.Login },
                       //new SqlParameter("@Password", SqlDbType.NVarChar) { Value = vClient.Password },
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vClient.Name },
                       new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = vClient.LastName },
                       new SqlParameter("@Patronymic", SqlDbType.NVarChar) { Value = vClient.Patronymic },
                       new SqlParameter("@Phones", SqlDbType.NVarChar) { Value = vClient.Phones },
                    };

                vClient.Id = DataProvider.ExecuteQuery("INSERT INTO Clients(Name, LastName, Patronymic, Phones) values(@Name, @LastName, @Patronymic, @Phones)", vParams, true);
            }
            _vDictIdObject.Add(vClient.Id, vClient);

            if (bAddToList)
                base.Insert(iIndex, vClient);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cClient GetObjectById(int Id)
        {
            return _vDictIdObject.ContainsKey(Id) ? _vDictIdObject[Id] : null;
        }

        /// <summary>
        /// Удаляет объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="bRemoveToDatabase">Удалить из базы данных</param>
        public void RemoveById(int Id, bool bRemoveToDatabase = false)
        {
            cClient vClient = this.GetObjectById(Id);

            if (vClient != null)
                this.Remove(vClient, bRemoveToDatabase);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vClient">Удаляемый объект</param>
        public new void Remove(cClient vClient)
        {
            this.Remove(vClient, false);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vClient">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public void Remove(cClient vClient, bool bRemoveToDatabase = false)
        {
	        bool bIsDel = true;

            if (bRemoveToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vClient.Id },
                    };

                DataTable vRes = DataProvider.GetDataTable("SELECT id FROM Orders WHERE ClientId = @Id", vParams);

                if (vRes != null && vRes.Rows.Count > 0)
                {
	                //MessageBox.Show("Клиента удалить нельзя он указан в заказах!","Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
	                bIsDel = false;
                }
                else
                {
	                DataProvider.ExecuteQuery("DELETE FROM Clients WHERE Id = @Id", vParams);
                }
            }

            if (bIsDel)
            {
	            if (_vDictIdObject.ContainsKey(vClient.Id))
		            _vDictIdObject.Remove(vClient.Id);

	            base.Remove(vClient);
            }
        }
    }
}
