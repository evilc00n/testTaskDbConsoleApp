using ConsoleApp1.Interfaces;
using Npgsql;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    internal class RandomGenerator : IRandomGenerator
    {
        private static readonly Random random = new Random();

        /// <inheritdoc />
        public Employee GenerateRandomEmployee()
        {
            string[] maleFirstNames = { "Petr", "Ivan", "Alexei", "Dmitri", "Nikolai" };
            string[] maleLastNames = { "Ireev", "Fadeev", "Olgin", "Elshin", "Torov" };

            string[] femaleFirstNames = { "Anna", "Maria", "Olga", "Elena", "Natalia" };
            string[] femaleLastNames = { "Ireeva", "Fadeeva", "Olgina", "Elshina", "Torova" };


            bool isMale = random.Next(2) == 0;

            Employee employee = Generate(maleFirstNames,
                                maleLastNames, 
                                femaleFirstNames, 
                                femaleLastNames, 
                                isMale);


            return employee;
        }

        /// <inheritdoc />
        public Employee GenerateRandomEmployeeWithF()
        {
            string[] maleFirstNames = { "Petr", "Ivan", "Alexei", "Dmitri", "Nikolai" };
            string[] maleLastNames = { "Fireev", "Fadeev", "Folgin", "Felshin", "Ftorov" };

            string[] femaleFirstNames = { "Anna", "Maria", "Olga", "Elena", "Natalia" };
            string[] femaleLastNames = { "Fireeva", "Fadeeva", "Folgina", "Felshina", "Ftorova" };

            Employee employee = Generate(maleFirstNames,
                                maleLastNames,
                                femaleFirstNames,
                                femaleLastNames, 
                                true);


            return employee;
        }


        //конкретная реализация генерации для данного класса
        private Employee Generate(string[] maleFirstNames,
                                  string[] maleLastNames,
                                  string[] femaleFirstNames,
                                  string[] femaleLastNames,
                                  bool isMale)
        {
            string firstName;
            string lastName;

            if (isMale)
            {
                firstName = maleFirstNames[random.Next(maleFirstNames.Length)];
                lastName = maleLastNames[random.Next(maleLastNames.Length)];
            }
            else
            {
                firstName = femaleFirstNames[random.Next(femaleFirstNames.Length)];
                lastName = femaleLastNames[random.Next(femaleLastNames.Length)];
            }

            string middleName = isMale ? "Vasilevich" : "Dmitrievna";
            string gender = isMale ? "Male" : "Female";



            // Генерация даты рождения в пределах разумного (например, в пределах последних 50 лет)
            DateTime minDateOfBirth = DateTime.Now.AddYears(-50);
            DateTime maxDateOfBirth = DateTime.Now.AddYears(-20);
            DateTime dateOfBirth = minDateOfBirth.AddDays(random.Next((int)(maxDateOfBirth - minDateOfBirth).TotalDays));

            Employee employee = new Employee() 
            {
                FullName = lastName + " " + firstName + " " + middleName,
                BirthDate = DateOnly.FromDateTime(dateOfBirth),
                Gender = gender
            };
            return employee;
        }
    }
}
