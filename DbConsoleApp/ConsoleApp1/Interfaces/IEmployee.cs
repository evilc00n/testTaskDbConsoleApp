using Microsoft.Extensions.Configuration;

namespace ConsoleApp1.Interfaces
{
    internal interface IEmployee
    {
        /// <summary>
        /// Вставка в базу данных объекта, у которого вызван метод
        /// </summary>
        /// <param name="connectionString"></param>
        void InsertEmployeeToDatabase(string connectionString);

        /// <summary>
        /// Реализация расчёта возраста
        /// </summary>
        /// <returns></returns>
        int CalculateAge();

    }
}
