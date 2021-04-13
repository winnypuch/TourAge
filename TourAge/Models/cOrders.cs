using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
	public class cOrders : BindingList<cOrder>, IExtCollections<cOrder>
    {
	    private Dictionary<int, cOrder> _vDictIdObject;

	    public cOrders()
	    {
		    _vDictIdObject = new Dictionary<int, cOrder>();
	    }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cOrder>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Orders");

            if (vRes?.Rows.Count > 0)
            {
                foreach (DataRow vRow in vRes.Rows)
                {
                    cOrder vcOrder = new cOrder();
                    vcOrder.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcOrder);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vOrder"></param>
        public new void Add(cOrder vOrder)
        {
            this.Add(vOrder, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vOrder"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cOrder vOrder, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vOrder, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vOrder">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cOrder vOrder)
        {
            this.Insert(iIndex, vOrder, false, true);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="vOrder"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cOrder vOrder, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Date", SqlDbType.DateTime) { Value = vOrder.Date },
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vOrder.Name },
                       new SqlParameter("@ClientsCount", SqlDbType.Int) { Value = vOrder.ClientsCount },
                       new SqlParameter("@iTourId", SqlDbType.Int) { Value = vOrder.Tour.Id },
                       new SqlParameter("@iClientId", SqlDbType.Int) { Value = vOrder.Client.Id },
                    };

                vOrder.Id = DataProvider.ExecuteQuery("INSERT INTO Orders(Date, Name, ClientsCount, TourId, ClientId) values(@Date, @Name, @ClientsCount, @iTourId, @iClientId)", vParams, true);
            }
            _vDictIdObject.Add(vOrder.Id, vOrder);

            if (bAddToList)
                base.Insert(iIndex, vOrder);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cOrder GetObjectById(int Id)
        {
            return _vDictIdObject.ContainsKey(Id) ? _vDictIdObject[Id] : null;
        }

        /// <summary>
        /// Удаляет объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="bRemoveToDatabase"></param>
        public string RemoveById(int Id, bool bRemoveToDatabase = false)
        {
            cOrder vOrder = this.GetObjectById(Id);

            if (vOrder != null)
	            return this.Remove(vOrder, bRemoveToDatabase);
            return null;
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vOrder">Удаляемый объект</param>
        public new string Remove(cOrder vOrder)
        {
           return this.Remove(vOrder, true);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vOrder">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public string Remove(cOrder vOrder, bool bRemoveToDatabase = false)
        {
            if (bRemoveToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vOrder.Id },
                    };

                DataProvider.ExecuteQuery("DELETE FROM Orders WHERE Id = @Id", vParams);
            }

            if (_vDictIdObject.ContainsKey(vOrder.Id))
                _vDictIdObject.Remove(vOrder.Id);

            base.Remove(vOrder);
            return null;
        }
    }
}
