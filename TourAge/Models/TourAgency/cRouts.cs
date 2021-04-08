using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TourAgency
{
    public class cRouts : BindingList<cRout>, IExtCollections<cRout>
    {
        private Dictionary<int, cRout> _vDictIdObject;

        public cRouts()
        {
	        _vDictIdObject = new Dictionary<int, cRout>();
        }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cRout>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Routes");

            if (vRes.Rows.Count > 0)
            {
                foreach (DataRow vRow in vRes.Rows)
                {
                    cRout vcRout = new cRout();
                    vcRout.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcRout);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vRout"></param>
        public new void Add(cRout vRout)
        {
            this.Add(vRout, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vRout"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cRout vRout, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vRout, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vRout">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cRout vRout)
        {
            this.Insert(iIndex, vRout, false, true);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="vRout"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cRout vRout, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vRout.Id },
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vRout.Name },
                       new SqlParameter("@CityStartId", SqlDbType.NVarChar) { Value = vRout.CityStart.Id },
                       new SqlParameter("@CityEndId", SqlDbType.NVarChar) { Value = vRout.CityEnd.Id },
                       new SqlParameter("@RCost", SqlDbType.NVarChar) { Value = vRout.RCost },
                    };

                vRout.Id = DataProvider.ExecuteQuery("INSERT INTO Routes(Name, CityStartId, CityEndId, RCost) values(@Name , @CityStartId , @CityEndId, @RCost)", vParams, true);
            }
            _vDictIdObject.Add(vRout.Id, vRout);

            if (bAddToList)
                base.Insert(iIndex, vRout);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cRout GetObjectById(int Id)
        {
            return _vDictIdObject.ContainsKey(Id) ? _vDictIdObject[Id] : null;
        }

        /// <summary>
        /// Удаляет объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="bRemoveToDatabase">Удалить объект из базы</param>
        public void RemoveById(int Id, bool bRemoveToDatabase = false)
        {
            cRout vRout = this.GetObjectById(Id);

            if (vRout != null)
                this.Remove(vRout, bRemoveToDatabase);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vRout">Удаляемый объект</param>
        public new void Remove(cRout vRout)
        {
            this.Remove(vRout, false);
        }

        /// <summary>
        /// Очистить всю коллекцию
        /// </summary>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public void Clear(bool bRemoveToDatabase = false)
        {
	        if (this.Count > 0)
	        {
		        for (var i = this.Count-1; i >= 0; i--)
		        {
			        cRout vRout = this[i];
			        this.Remove(vRout, bRemoveToDatabase);
		        }
	        }
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vRout">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public void Remove(cRout vRout, bool bRemoveToDatabase = false)
        {
	        bool bIsDel = true;

            if (bRemoveToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vRout.Id },
                    };

                DataTable vRes = DataProvider.GetDataTable("SELECT id FROM RoutesInTour WHERE RouteId = @Id", vParams);

                if (vRes.Rows.Count > 0)
                {
	                //MessageBox.Show("Маршрут удалить нельзя он указан в турах!", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
	                bIsDel = false;
                }
                else
                {
	                DataProvider.ExecuteQuery("DELETE FROM Routes WHERE Id = @Id", vParams);
                }
            }

            if (bIsDel)
            {
	            if (_vDictIdObject.ContainsKey(vRout.Id))
		            _vDictIdObject.Remove(vRout.Id);

	            base.Remove(vRout);
            }
        }
    }
}
