using System;

namespace ConsoleApp1.Interfaces
{
    internal interface IRandomGenerator
    {
        /// <summary>
        /// Генерация случайного пользователя 
        /// </summary>
        /// <returns></returns>
        Employee GenerateRandomEmployee();


        /// <summary>
        /// Генерация пользователя с фамилией, начинающейся на F и мужским полом
        /// </summary>
        /// <returns></returns>
        Employee GenerateRandomEmployeeWithF();


    }
}
