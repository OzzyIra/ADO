using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Library2
{
	class Library
	{
		public static string connectionString = "";
		static SqlConnection connection = null;
		static readonly string delimiter1 = "\n--------------------------------------";
		static readonly string delimiter2 = "\n=====================================";

	static Library()
		{
			connectionString = ConfigurationManager.ConnectionStrings["Library"].ConnectionString;
			connection = new SqlConnection(connectionString);
			Console.WriteLine(connectionString);
		}

		public static void Select(string fields, string tables, string condition="", int padding = 20)
		{
			string cmd = $"SELECT {fields} FROM {tables} ";
			if (condition.Length > 0) cmd += $"WHERE {condition}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();



			if(reader.HasRows)
			{
				Console.WriteLine(delimiter1);
				for (int i = 0; i < reader.FieldCount; i++)
					Console.Write(reader.GetName(i).PadRight(padding));
				Console.WriteLine(delimiter2);
				while(reader.Read())
				{
					for (int i = 0; i < reader.FieldCount; i++)
						Console.Write(reader[i].ToString().PadRight(padding));
					Console.WriteLine();
				}
			Console.WriteLine(delimiter1);
			}

			reader.Close();
			connection.Close();
		}
		public static void Insert(string table,string fields, string values)
		{
			string cmd = $"INSERT {table}({fields})VALUES ({values})";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			
		}
		public static void InsertAuthor(int id,string last_name,string first_name)
		{
			string cmd = "INSERT Authors(author_id,last_name,first_name) VALUES(@id,@last_name,@first_name)";
			SqlCommand command = new SqlCommand(cmd, connection);

			SqlParameter p_id = new SqlParameter("@id", SqlDbType.Int); p_id.Value = id;
			SqlParameter p_last_name = new SqlParameter("@last_name", SqlDbType.NVarChar, 150);p_last_name.Value = last_name;
			SqlParameter p_first_name = new SqlParameter("@first_name", SqlDbType.NVarChar, 150); p_first_name.Value = first_name;

			command.Parameters.Add(p_id);
			command.Parameters.Add(p_last_name);
			command.Parameters.Add(p_first_name);

			connection.Open();
			command.ExecuteNonQuery();

			connection.Close();
		}
		public static void InsertBook(int id,string title,int size, string date,string author)
		{
			string cmd =
				"INSERT Books(book_id,book_title,book_size,publish_date,author)" +
				"VALUES(@id,@title,@size,@date,@author)";
			SqlCommand command = new SqlCommand(cmd,connection);
			command.Parameters.Add("@id", SqlDbType.Int).Value = id;
			command.Parameters.Add("@title", SqlDbType.NVarChar, 256).Value = title;
			command.Parameters.Add("@size", SqlDbType.Int).Value = size;
			command.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
			command.Parameters.Add("@author", SqlDbType.Int).Value = GetAuthorID(author);

			connection.Open();
			command.ExecuteNonQuery();

			connection.Close();

		}
		public static int GetAuthorID(string full_name)
		{
			string cmd = "SELECT author_id FROM Authors WHERE last_name = @last_name AND first_name = @first_name";
			SqlCommand command = new SqlCommand(cmd, connection);
			command.Parameters.Add("@first_name", SqlDbType.NVarChar, 150).Value = full_name.Split(' ').First();
			command.Parameters.Add("@last_name", SqlDbType.NVarChar, 150).Value = full_name.Split(' ').Last();
			connection.Open();
			int id = Convert.ToInt32(command.ExecuteScalar());
			connection.Close();
			return id;
		}


	}
}
