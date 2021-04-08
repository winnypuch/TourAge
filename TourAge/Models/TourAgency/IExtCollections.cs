namespace TourAgency
{
	public interface IExtCollections<T>
	{
		/// <summary>
		/// Загрузка коллекции
		/// </summary>
		void Load();

		/// <summary>
		/// Добавление объект в коллекцию объектов
		/// </summary>
		/// <param name="vObject">Объект</param>
		/// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
		/// <param name="bAddToList">Добавлять в список</param>
		void Add(T vObject, bool bAddToDatabase = false, bool bAddToList = true);

		/// <summary>
		/// Вставка элемента в коллекцию по специальному индексу.
		/// </summary>
		/// <param name="iIndex">Индекс</param>
		/// <param name="vObject">Объект</param>
		/// <param name="bAddToDatabase">Добавлять или нет объект в базу данных</param>
		/// <param name="bAddToList">Добавлять в список</param>
		void Insert(int iIndex, T vObject, bool bAddToDatabase = false, bool bAddToList = true);

		/// <summary>
		/// Возвращает объект по его Id
		/// </summary>
		/// <param name="Id">Идентификатор объекта</param>
		/// <returns></returns>
		T GetObjectById(int Id);

		/// <summary>
		/// Удаляет объект по его Id
		/// </summary>
		/// <param name="Id">Идентификатор объекта</param>
		/// <param name="bRemoveToDatabase">Удалить из базы данных</param>
		void RemoveById(int Id, bool bRemoveToDatabase = false);

		/// <summary>
		/// Удаляет объект из коллекции
		/// </summary>
		/// <param name="vObject">Удаляемый объект</param>
		/// <param name="bRemoveToDatabase">Удалять или нет объект из базы данных</param>
		void Remove(T vObject, bool bRemoveToDatabase = false);
	}
}