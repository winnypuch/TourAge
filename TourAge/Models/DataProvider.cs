using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

public static class DataProvider
{
	public static DataTable GetDataTable(string sql, List<SqlParameter> vParams = null)
	{
		try
		{
			DataTable vDataTable = new DataTable();
			SqlConnection vDbConnection = DBConnection.Instance().Connection;

			if (vDbConnection != null)
			{
				using (SqlCommand vCmd = new SqlCommand(sql, vDbConnection))
				{
					if (vParams != null)
					{
						foreach (SqlParameter vParam in vParams)
						{
							vCmd.Parameters.Add(vParam);
						}
					}

					using (SqlDataReader vDataReader = vCmd.ExecuteReader())
					{
						vDataTable.Load(vDataReader);
						vDbConnection.Close();
						return vDataTable;
					}
				}
			}

			return null;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return null;
		}
	}

	public static int ExecuteQuery(string sSql, List<SqlParameter> vParams = null, bool bReturnLastInsertRow = false) //метод возврата
	{
		int iResult = 0;
		try
		{
			SqlConnection vDbConnection = DBConnection.Instance().Connection;

			if (vDbConnection != null)
			{
				using (SqlCommand vCommand = new SqlCommand(sSql, vDbConnection))
				{
					if (vParams != null)
					{
						foreach (SqlParameter vParam in vParams)
						{
							vCommand.Parameters.Add(vParam);
						}
					}

					if (bReturnLastInsertRow)
					{
						if (vCommand.ExecuteNonQuery() > 0)
						{

							vCommand.CommandText = "select LAST_INSERT_ID()";

							// The row ID is a 64-bit value - cast the Command result to an Int64.
							//
							int iLastRowID64 = Convert.ToInt32(vCommand.ExecuteScalar());

							// Then grab the bottom 32-bits as the unique ID of the row.
							//
							iResult = (int) iLastRowID64;
						}
					}
					else
					{
						iResult = vCommand.ExecuteNonQuery();
					}

					vDbConnection.Close();
					return iResult;
				}
			}

			return iResult;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return iResult;
		}
	}

	const int MaxDaysCountForReport = 15;

	public static Dictionary<int, decimal> CustomRatings
	{
		get
		{
			if (HttpContext.Current.Session["CustomRatings"] == null)
				HttpContext.Current.Session["CustomRatings"] = new Dictionary<int, decimal>();
			return (Dictionary<int, decimal>) HttpContext.Current.Session["CustomRatings"];
		}
	}

	public static List<string> LocationRatings = new List<string>() {"BBB", "A", "AA", "AAA"};

	public static Dictionary<int, string> GetMonths()
	{
		var results = new Dictionary<int, string>();
		for (int i = 0; i < 12; i++)
		{
			results.Add(i, DateTimeFormatInfo.CurrentInfo.MonthNames[i]);
		}

		return results;
	}

	public static List<int> GetCardExpiredYears()
	{
		var years = new List<int>();
		for (int i = 0; i < 10; i++)
		{
			years.Add(DateTime.Now.Year + i);
		}

		return years;
	}

	public static List<string> GetStates()
	{
		return new List<string>()
		{
			"AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN",
			"MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA",
			"WA", "WV", "WI", "WY"
		};
	}

	public static List<string> GetLocationRatings()
	{
		return new List<string>()
		{
			"", "AAA", "AA", "A", "BBB"
		};
	}

	public static string GetQueryString(NameValueCollection parameters)
	{
		var array = (from key in parameters.AllKeys
			select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(parameters[key]))).ToArray();
		return string.Join("&", array);
	}

	static int GetTextWidth(string text, Font font)
	{
		var gr = Graphics.FromHwnd(IntPtr.Zero);
		gr.PageUnit = GraphicsUnit.Inch;
		SizeF size = gr.MeasureString(text, font);
		return Convert.ToInt32(size.Width * 100);
	}

	static XRTableRow CreateReportRow(string text, string price)
	{
		var row = new DevExpress.XtraReports.UI.XRTableRow();
		var cell1 = new XRTableCell();
		var cell2 = new XRTableCell();
		cell1.Text = text;
		cell2.Text = price;
		cell2.TextAlignment = TextAlignment.MiddleRight;
		row.Cells.Add(cell1);
		row.Cells.Add(cell2);
		return row;
	}

	public static List<string> GetDefaultHotelLocations()
	{
		return new List<string>()
		{
			"Автобусы",
			"Такси"
		};
	}
}


public class DBConnection
{
	private DBConnection()
	{
	}

	private SqlConnection _vConnection = null;

	public SqlConnection Connection
	{
		get
		{
			if (_vConnection == null)
			{
				try
				{
					//String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", "localhost", 3306, "root", "Tamerlanec2306", "base", "none");
					//"server=localhost;user id=bar;password=foobar;database=foo;"
					//String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", "localhost", 3306, "root", "Tamerlanec2306", "base", "none")
					//_vConnection = new SqlConnection(String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", "localhost", 3306, "root", "Tamerlanec2306", "base", "none"));
					_vConnection = new SqlConnection(String.Format("server={0};uid={1}; password={2}; database={3};", "127.0.0.1", "root", "Tamerlanec2306", "base"));
					//String.Format("server={0};uid={1}; pwd={2}; database={3};", "127.0.0.1", "root", "Tamerlanec2306", "base")
					//string.Format("Server=localhost; database={0}; UID=root; password={1}", "base", "Tamerlanec2306")
					_vConnection.Open();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			else if (_vConnection.State == ConnectionState.Closed)
			{
				_vConnection.Open();
			}

			return _vConnection;
		}
	}

	private static DBConnection _instance = null;

	public static DBConnection Instance()
	{
		if (_instance == null)
			_instance = new DBConnection();
		return _instance;
	}

	public void Close()
	{
		_vConnection.Close();
	}
}