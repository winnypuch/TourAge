using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
	public class cTours : BindingList<cTour>, IExtCollections<cTour>
    {

        private Dictionary<int, cTour> _vDictIdObject;

        public cTours()
        {
	        _vDictIdObject = new Dictionary<int, cTour>();
        }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cTour>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Tours");

            if (vRes.Rows.Count > 0)
            {
                foreach (DataRow vRow in vRes.Rows)
                {
                    cTour vcTour = new cTour();
                    vcTour.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcTour);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vTour"></param>
        public new void Add(cTour vTour)
        {
            this.Add(vTour, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vTour"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cTour vTour, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vTour, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vTour">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cTour vTour)
        {
            this.Insert(iIndex, vTour, false, true);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="vTour"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cTour vTour, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vTour.Name },
                       new SqlParameter("@CostOfLiving", SqlDbType.Decimal) {Value = vTour.CostOfLiving},
                       new SqlParameter("@TDateBegin", SqlDbType.DateTime) {Value = vTour.TDateBegin},
                       new SqlParameter("@TDateEnd", SqlDbType.DateTime) {Value = vTour.TDateEnd},
                    };

                vTour.Id = DataProvider.ExecuteQuery("INSERT INTO Tours(Name, CostOfLiving, TDateBegin, TDateEnd) values(@Name, @CostOfLiving, @TDateBegin, @TDateEnd)", vParams, true);
            }
            _vDictIdObject.Add(vTour.Id, vTour);

            if (bAddToList)
                base.Insert(iIndex, vTour);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cTour GetObjectById(int Id)
        {
            return _vDictIdObject.ContainsKey(Id) ? _vDictIdObject[Id] : null;
        }

        /// <summary>
        /// Удаляет объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        public void RemoveById(int Id, bool bRemoveToDatabase = false)
        {
            cTour vTour = this.GetObjectById(Id);

            if (vTour != null)
                this.Remove(vTour, bRemoveToDatabase);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vTour">Удаляемый объект</param>
        public new void Remove(cTour vTour)
        {
            this.Remove(vTour, false);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vTour">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public void Remove(cTour vTour, bool bRemoveToDatabase = false)
        {
	        bool bIsDel = true;

            if (bRemoveToDatabase)
            {
	            vTour.RemoveRoutes();
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vTour.Id },
                    };

                DataTable vRes = DataProvider.GetDataTable("SELECT id FROM Orders WHERE TourId = @Id", vParams);

                if (vRes.Rows.Count > 0)
                {
	                //MessageBox.Show("Тур удалить нельзя он указан в заказах!", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
	                bIsDel = false;
                }
                else
                {
	                DataTable vRes2 = DataProvider.GetDataTable("SELECT id FROM RoutesInTour WHERE TourId = @Id", vParams);
	                if (vRes2.Rows.Count > 0)
	                {
		                //MessageBox.Show("Тур удалить нельзя у него есть маршруты!", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
		                bIsDel = false;
	                }
	                else
	                {
		                DataProvider.ExecuteQuery("DELETE FROM Tours WHERE Id = @Id", vParams);
	                }
                }
            }

            if (bIsDel)
            {
	            if (_vDictIdObject.ContainsKey(vTour.Id))
		            _vDictIdObject.Remove(vTour.Id);

	            base.Remove(vTour);
            }
        }
    }
}
