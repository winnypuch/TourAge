using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
	public class cCountries : BindingList<cCountry>, IExtCollections<cCountry>
    {
	    private Dictionary<int, cCountry> _vDictIdObject;

	    public cCountries()
	    {
		    _vDictIdObject = new Dictionary<int, cCountry>();
	    }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cCountry>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Countries");

            if (vRes?.Rows.Count > 0)
            {
                foreach (DataRow vRow in vRes.Rows)
                {
                    cCountry vcCountry = new cCountry();
                    vcCountry.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcCountry);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vCountry"></param>
        public new void Add(cCountry vCountry)
        {
            this.Add(vCountry, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vCountry"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cCountry vCountry, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vCountry, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vCountry">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cCountry vCountry)
        {
            this.Insert(iIndex, vCountry, false, true);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="vCountry"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cCountry vCountry, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vCountry.Name },
                    };
                vCountry.Id = DataProvider.ExecuteQuery("INSERT INTO Countries(Name) values(@Name)", vParams, true);
            }
            _vDictIdObject.Add(vCountry.Id, vCountry);

            if (bAddToList)
                base.Insert(iIndex, vCountry);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cCountry GetObjectById(int Id)
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
            cCountry vCountry = this.GetObjectById(Id);

            if (vCountry != null)
                return this.Remove(vCountry, bRemoveToDatabase);
            return null;
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vCountry">Удаляемый объект</param>
        public new string Remove(cCountry vCountry)
        {
            return this.Remove(vCountry, true);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vCountry">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public string Remove(cCountry vCountry, bool bRemoveToDatabase = false)
        {
            if (bRemoveToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id",  SqlDbType.Int) { Value = vCountry.Id },
                    };
                DataTable vRes = DataProvider.GetDataTable("SELECT id FROM Cities WHERE CountryId = @Id", vParams);

                if (vRes?.Rows.Count > 0)
                {
	                return "Страну удалить нельзя она указана в городах!";
                }
                else
                {
	                DataProvider.ExecuteQuery("DELETE FROM Countries WHERE Id = @Id", vParams);
                }
            }

            if (_vDictIdObject.ContainsKey(vCountry.Id))
	            _vDictIdObject.Remove(vCountry.Id);

            base.Remove(vCountry);


            return null;
        }

        /// <summary>
        /// Загрузить коллекцию стран
        /// </summary>
        /// <returns></returns>
        public static cCountries Fill()
        {
	        cCountries vCountries = new cCountries();
	        vCountries.Load();
	        return vCountries;
        }
    }
}
