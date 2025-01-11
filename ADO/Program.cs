using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            Console.WriteLine(connectionString);
            Console.WriteLine("---------------------");

            SqlConnection connection = new SqlConnection(connectionString); //для подключения к серверу создаем объект класса SqlConnection
            //для общения с БД нужно создать объект SqlCommand,но сначала нужно сформировать комманду
            string cmd = "SELECT * FROM Authors";
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();    //если было открыто, то нужно закрыть
            SqlDataReader reader = command.ExecuteReader(); //выполнит  cmd запрос при  создании SqlC  ,возвращает сложную коллекцию объектов,похож на массив, если открыли надо закрыть
            const int padding = 25;
            for(int i = 0; i<reader.FieldCount;i++)
            {
                Console.Write(reader.GetName(i).PadRight(padding));
            }
            Console.WriteLine();

            if(!reader.IsClosed)
            {
                while(reader.Read())
                {
                    for(int i = 0;i<reader.FieldCount;i++)
                    {
                        Console.Write(reader[i].ToString().PadRight(padding));
                    }
                    Console.WriteLine();
                }
            }
            reader.Close();
            connection.Close();

        }
    }
}
