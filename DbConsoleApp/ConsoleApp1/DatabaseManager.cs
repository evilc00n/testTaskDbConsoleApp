using ConsoleApp1.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Text;


namespace ConsoleApp1
{
    internal class DatabaseManager : IDatabaseManager
    {
        /// <inheritdoc />
        public List<Employee> GetAllUniqueEmployeesSortedByFullName(string connectionString)
        {
            var tableName = nameof(Employee);
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            List<Employee> employees = new List<Employee>();    
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    //используется интерполяция строк, потому что имена полей 
                    //и название таблицы заданы статически через имена свойств и имя класса
                    using (var command = new NpgsqlCommand($"SELECT DISTINCT ON " +
                        $"({properties[1].Name}, {properties[2].Name}) " +
                        $"{properties[1].Name}, {properties[2].Name}, {properties[3].Name} " +
                        $"FROM {tableName} ORDER BY {properties[1].Name}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(
                                    new Employee()
                                    {
                                        FullName = reader.GetString(0),
                                        BirthDate = DateOnly.FromDateTime(reader.GetDateTime(1)),
                                        Gender = reader.GetString(2),
                                    });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return employees;
        }

        /// <inheritdoc />
        public void CreateEmployeeTable(string connectionString)
        {
            //в данной реализации не используется ORM
            var tableName = nameof(Employee);
            PropertyInfo[] properties = typeof(Employee).GetProperties();

        

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;


                    //используется интерполяция строк, потому что имена полей 
                    //и название таблицы заданы статически через имена свойств и имя класса
                    command.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {tableName} (
                        {properties[0].Name} BIGSERIAL PRIMARY KEY,
                        {properties[1].Name} TEXT NOT NULL,
                        {properties[2].Name} DATE NOT NULL,
                        {properties[3].Name} VARCHAR(10) NOT NULL
                        );";
                    command.ExecuteNonQuery();
                }
            }
        }




        /// <inheritdoc />
        public StringBuilder QueryEmployeesByCriteria(string connectionString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                
                var criteria = "SELECT fullname, birthdate, gender FROM employee WHERE gender = 'Male' AND fullname LIKE 'F%'";
                
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand(criteria, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                stringBuilder.AppendLine($"FullName = {reader.GetString(0)}, " +
                                    $"BirthDate = {DateOnly.FromDateTime(reader.GetDateTime(1))}, Gender = {reader.GetString(2)}" );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                stringBuilder = new StringBuilder().AppendLine($"Произошла ошибка: {ex.Message}");
            }
            return stringBuilder;

        }
    }

}
