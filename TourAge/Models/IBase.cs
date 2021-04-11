namespace TourAge.Models
{
	public  interface IBase
	{
		/// <summary>
		/// Идентификатор объекта
		/// </summary>
		int Id { get; set; }

		/// <summary>
		/// Имя объекта
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Загрузка объекта
		/// </summary>
		/// <param name="iId"></param>
		/// <returns></returns>
		bool Load(int iId);

		/// <summary>
		/// Сохранение клиента
		/// </summary>
		/// <returns></returns>
		bool Save();
	}
}