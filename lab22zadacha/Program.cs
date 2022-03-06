using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

/*Сформировать массив случайных целых чисел (размер  задается пользователем). 
 * Вычислить сумму чисел массива и максимальное число в массиве.  
 * Реализовать  решение  задачи  с  использованием  механизма  задач продолжения.*/

namespace lab22zadacha
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Укажите размер массива целым числом: ");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
            Task<int[]> task1 = new Task<int[]>(func1, n);

            Func<Task<int[]>, int[]> func2 = new Func<Task<int[]>, int[]>(SortArray); //задача-продолжение - сортировка по возрастанию
            Task<int[]> task2 = task1.ContinueWith<int[]>(func2);

            Console.Write($"\nМассив из случайных целых чисел: ");
            Action<Task<int[]>> action1 = new Action<Task<int[]>>(PrintArray); //вывод задача-продолжение
            Task task3 = task2.ContinueWith(action1);
            task1.Start();
            Console.ReadKey();
        }
        
        static int[] GetArray(object a) //метод для формирования массива
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100); //установка диапазона от 0 до 100
            }
            return array;
        }
        
        static int[] SortArray(Task<int[]> task)  //метод сортирует массив: принимает на вход готовый массив, задачу Task
        {
            int[] array = task.Result;
            for (int i = 0; i < array.Count() - 1; i++)
            {
                for (int j = i + 1; j < array.Count(); j++)
                {
                    if (array[i] > array[j])
                    {
                        int t = array[i];
                        array[i] = array[j];
                        array[j] = t;
                    }
                }
            }
            return array;
        }
        
        static void PrintArray(Task<int[]> task)   //метод для вывода на экран
        {
            int[] array = task.Result;
            int max = array[0];
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"{array[i]} ");
            }
            Console.WriteLine();
            
            foreach (int x in array)  //максимальное число в массиве
            {
                if (x > max)
                    max = x;
            }
            Console.WriteLine($"\nМаксимальное число в массиве равно: {max}");
            
            int sum = 0;
            foreach (int a in array)  //сумма чисел в массиве
            {
                sum += a;
            }
            Console.WriteLine($"\nСумма чисел в массиве равна: {sum}");
        }
    }
}
