using ConsoleApp1.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text;


namespace ConsoleApp1
{
    class Program
    {
        private static readonly string _errorMessage = "Неверный формат ввода";
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("no arguments specified.");
                return;
            }



            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            if (configuration == null)
            {
                Console.WriteLine("Конфигурация не найдена");
                return;
            }




            string connectionString = configuration.GetConnectionString("PostgresSQL");
            if (connectionString == null)
            {
                Console.WriteLine("Строка подключения не найдена");
                return;
            }


            switch (args[0])
            {
                case "1":
                    FirstOperationMode(connectionString);
                    break;
                case "2":
                    if (args.Length == 4)
                    {
                        try
                        {
                            string fullName = args[1];
                            DateOnly birthDate = DateOnly.Parse(args[2]);
                            string gender = args[3];
                            SecondOperationMode(connectionString, fullName, birthDate, gender);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(_errorMessage);
                        }

                    }
                    else
                    {
                        
                        Console.WriteLine(_errorMessage);
                    }
                    break;
                case "3":
                    ThirdOperationMode(connectionString);
                    break;
                case "4":
                    FourthOperationMode(connectionString);
                    break;
                case "5":
                    FifthOperationMode(connectionString);
                    break;
                default: Console.WriteLine(_errorMessage);
                    break;
            }
        }

        static void FirstOperationMode(string connectionString)
        {
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.CreateEmployeeTable(connectionString);
        }

        static void SecondOperationMode(
            string connectionString, string fullName, DateOnly birthDate, string gender)
        {
            Employee newEmployee = new Employee()
            {
                FullName = fullName,
                BirthDate = birthDate,
                Gender = gender
            };
            newEmployee.InsertEmployeeToDatabase(connectionString);
        }

        static void ThirdOperationMode(string connectionString)
        {
            try
            {
                DatabaseManager databaseManager = new DatabaseManager();
                var employees = databaseManager.GetAllUniqueEmployeesSortedByFullName(connectionString);
                foreach (var employee in employees)
                {

                    Console.WriteLine($"FullName = {employee.FullName}, " +
                        $"BirthDate = {employee.BirthDate}, " +
                        $"Gender = {employee.Gender}, Age = {employee.CalculateAge()}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void FourthOperationMode(string connectionString)
        {
            Employee[] millionOfEmployees = new Employee[1000000];
            Employee[] hundredOfEmployees = new Employee[100];
            RandomGenerator randomGenerator = new RandomGenerator();
            //Заполнение данных для массива на 1000000 элементов
            for (int i = 0; i < 1000000; i++)
            {
                Employee employee = randomGenerator.GenerateRandomEmployee();

                millionOfEmployees[i] = employee;

            }
            //Заполнение данных для массива на 100 элементов
            for (int i = 0; i < 100; i++)
            {
                Employee employee = randomGenerator.GenerateRandomEmployeeWithF();
                hundredOfEmployees[i] = employee;
            }
            Employee.FillDatabaseAutomatically(connectionString, millionOfEmployees);
            Employee.FillDatabaseAutomatically(connectionString, hundredOfEmployees);
            
            //освобождение памяти
            millionOfEmployees = null;
            hundredOfEmployees = null;
            GC.Collect();

        }

        static void FifthOperationMode(string connectionString)
        {

            DatabaseManager databaseManager = new DatabaseManager();

            StringBuilder stringBuilder = new StringBuilder();

            var stopwatch = Stopwatch.StartNew();
            stringBuilder = databaseManager.QueryEmployeesByCriteria(connectionString);
            stopwatch.Stop();

            Console.WriteLine(stringBuilder.ToString());
            Console.WriteLine($"Выполнение запроса заняло {stopwatch.ElapsedMilliseconds} миллисекунд");
        }

    }

}

