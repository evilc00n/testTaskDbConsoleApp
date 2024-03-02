using ConsoleApp1.Interfaces;
using CsvHelper.Configuration;
using CsvHelper;
using Npgsql;
using System.Globalization;
using System.Reflection;

namespace ConsoleApp1
{
    public class Employee : IEmployee
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Gender { get; set; }

        /// <inheritdoc />
        public int CalculateAge()
        {

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - BirthDate.Year;

            if (BirthDate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    


        /// <inheritdoc />
        public void InsertEmployeeToDatabase(string connectionString)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var tableName = nameof(Employee);
                    PropertyInfo[] prop = typeof(Employee).GetProperties();
                    connection.Open();


                    //используется интерполяция строк, потому что имена полей 
                    //и название таблицы заданы статически через имена свойств и имя класса
                    //для изменяемых значений используются параметры
                    using (var command =
                        new NpgsqlCommand($"INSERT INTO " +
                        $"{tableName} ({prop[1].Name}, {prop[2].Name}, {prop[3].Name}) " +
                        "VALUES (@fullName, @birthDate, @gender)", connection))
                    {
                        command.Parameters.AddWithValue("@fullName", FullName);
                        command.Parameters.AddWithValue("@birthDate", BirthDate);
                        command.Parameters.AddWithValue("@gender", Gender);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

        }

        /// <summary>
        /// Заполненеие БД миллионом записей при помощи создания файла csv, копирования оттуда данных и удаления данного файла
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="listOfEmployees"></param>
        public static void FillDatabaseAutomatically(string connectionString, Employee[] listOfEmployees)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string fileName = "text.csv";

            string fullPath = Path.Combine(projectDirectory, fileName);

            WriteToFile(connectionString, fullPath, listOfEmployees);
            ReadFromFileAndDeleteHim(connectionString, fullPath);

        }



        private static void WriteToFile(string connectionString, string pathToFile, Employee[] listOfEmployees)
        {
            try
            {
                var tableName = nameof(Employee);
                PropertyInfo[] properties = typeof(Employee).GetProperties();

                var config = new CsvConfiguration(new CultureInfo("en-IN"))
                {
                    Delimiter = ";",
                    HasHeaderRecord = false,
                    Encoding = System.Text.Encoding.UTF8,
                };

                using (var writer = new StreamWriter(pathToFile, false, System.Text.Encoding.UTF8))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    csv.WriteRecords(listOfEmployees);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

        }

        private static void ReadFromFileAndDeleteHim(string connectionString, string pathToFile)
        {

            var tableName = nameof(Employee);
            PropertyInfo[] properties = typeof(Employee).GetProperties();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();



                    using (var writer = connection.BeginTextImport($"COPY {tableName}(" +
                        $"{properties[1].Name}, {properties[2].Name}, {properties[3].Name}) " +
                        $"FROM STDIN (FORMAT csv,  DELIMITER ';')"))
                    {
                        using (var file = new StreamReader(pathToFile, System.Text.Encoding.UTF8))
                        {
                            while (!file.EndOfStream)
                            {
                                var line = file.ReadLine();
                                writer.WriteLine(line);
                            }
                        }
                    }
                }


            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            try
            {

                File.Delete(pathToFile);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            return;

        }





    }
}
