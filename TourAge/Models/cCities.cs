﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace TourAge.Models
{
	public class cCities : BindingList<cCity>, IExtCollections<cCity>
    {
        private Dictionary<int, cCity> _vDictIdObject;

        public cCities()
        {
	        _vDictIdObject = new Dictionary<int, cCity>();
        }

        public void Load()
        {
            _vDictIdObject = new Dictionary<int, cCity>();

            DataTable vRes = DataProvider.GetDataTable("Select Id From Cities");

            if (vRes?.Rows.Count > 0)
            {
                foreach (DataRow vRow in vRes.Rows)
                {
                    cCity vcCity = new cCity();
                    vcCity.Load(Convert.ToInt32(vRow["Id"]));
                    this.Add(vcCity);
                }
            }
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vCity"></param>
        public new void Add(cCity vCity)
        {
            this.Add(vCity, false, true);
        }

        /// <summary>
        /// Добавление Клиента в коллекцию клиентов
        /// </summary>
        /// <param name="vCity"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Add(cCity vCity, bool bAddToDatabase = false, bool bAddToList = true)
        {
            this.Insert(this.Count, vCity, bAddToDatabase, bAddToList);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex">Индекс элемента, начинается с нуля.</param>
        /// <param name="vCity">Элемент коллекции</param>
        public new virtual void Insert(int iIndex, cCity vCity)
        {
            this.Insert(iIndex, vCity, false, true);
        }

        /// <summary>
        /// Вставка элемента в индекс по специальному индексу.
        /// </summary>
        /// <param name="iIndex"></param>
        /// <param name="vCity"></param>
        /// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
        /// <param name="bAddToList">Добавлять в список</param>
        public void Insert(int iIndex, cCity vCity, bool bAddToDatabase = false, bool bAddToList = true)
        {
            if (bAddToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Name", SqlDbType.NVarChar) { Value = vCity.Name },
                       new SqlParameter("@iCountryId", SqlDbType.Int) { Value = vCity.Country.Id },
                };

                vCity.Id = DataProvider.ExecuteQuery("INSERT INTO Cities(Name, CountryId) values(@Name, @iCountryId)", vParams, true);
            }
            _vDictIdObject.Add(vCity.Id, vCity);

            if (bAddToList)
                base.Insert(iIndex, vCity);
        }

        /// <summary>
        /// Возвращает объект по его Id
        /// </summary>
        /// <param name="Id">Идентификатор объекта</param>
        /// <returns></returns>
        public cCity GetObjectById(int Id)
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
            cCity vCity = this.GetObjectById(Id);

            if (vCity != null)
                return this.Remove(vCity, bRemoveToDatabase);

            return null;
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vCity">Удаляемый объект</param>
        public new string Remove(cCity vCity)
        {
            return this.Remove(vCity, true);
        }

        /// <summary>
        /// Удаляет объект из коллекции
        /// </summary>
        /// <param name="vCity">Удаляемый объект</param>
        /// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
        public string Remove(cCity vCity, bool bRemoveToDatabase = false)
        {
            if (bRemoveToDatabase)
            {
                List<SqlParameter> vParams = new List<SqlParameter> {
                       new SqlParameter("@Id", SqlDbType.Int) { Value = vCity.Id },
                    };

                DataTable vRes = DataProvider.GetDataTable("SELECT id FROM Routes WHERE CityStartId = @Id OR CityEndId = @Id", vParams);

                if (vRes?.Rows.Count > 0)
                {
	               return "Город удалить нельзя он указан в маршрутах!";
                }
                else
                {
	                DataProvider.ExecuteQuery("DELETE FROM Cities WHERE Id = @Id", vParams);
                }
            }

            if (_vDictIdObject.ContainsKey(vCity.Id))
	            _vDictIdObject.Remove(vCity.Id);

            base.Remove(vCity);

            return null;
        }

        /// <summary>
        /// Загрузить коллекцию городов
        /// </summary>
        /// <returns></returns>
        public static cCities Fill()
        {
	        cCities vCities = new cCities();
	        vCities.Load();
	        return vCities;
        }
    }
}
