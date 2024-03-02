using CsvHelper.Configuration;

namespace ConsoleApp1
{
    internal sealed class EmployeeMap : ClassMap<Employee>
    {

        /// <summary>
        /// Класс для настройки записи экземпляров сущности в csv файл
        /// </summary>
        public EmployeeMap()
        {
            Map(m => m.Id).Ignore();
            Map(m => m.FullName).Name("FullName"); 
            Map(m => m.BirthDate).Name("BirthDate").TypeConverterOption.Format("yyyy-MM-dd"); 
            Map(m => m.Gender).Name("Gender"); 
            
        }
    }
}
