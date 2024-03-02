using System.Text;

namespace ConsoleApp1.Interfaces
{
    internal interface IDatabaseManager
    {
        /// <summary>
        /// Получение списка из всех уникальных записей, отсортированных по имени 
        /// </summary>
        /// <param name="connectionString"></param>
        List<Employee> GetAllUniqueEmployeesSortedByFullName(string connectionString);

        /// <summary>
        /// Создание пустой таблицы. 
        /// </summary>
        /// <param name="connectionString"></param>
        void CreateEmployeeTable(string connectionString);

        /// <summary>
        /// Результат выборки из таблицы по критерию: пол мужской, Фамилия начинается с "F".
        /// </summary>
        /// <param name="connectionString"></param>
        StringBuilder QueryEmployeesByCriteria( string connectionString);
    }
}
 