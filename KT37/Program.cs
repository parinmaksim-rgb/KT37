using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace KT37
{
    public class Order
    {
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var orders = new List<Order>
        {
            new Order { CustomerName = "Иван",    Amount = 1500m, Date = new DateTime(2026, 3, 5) },
            new Order { CustomerName = "Анна",    Amount = 3200m, Date = new DateTime(2026, 3, 10) },
            new Order { CustomerName = "Петр",    Amount = 800m,  Date = new DateTime(2026, 2, 25) },
            new Order { CustomerName = "Иван",    Amount = 2500m, Date = new DateTime(2026, 3, 15) },
            new Order { CustomerName = "Мария",   Amount = 5400m, Date = new DateTime(2026, 3, 18) },
            new Order { CustomerName = "Дмитрий", Amount = 1200m, Date = new DateTime(2026, 3, 20) },
            new Order { CustomerName = "Анна",    Amount = 1800m, Date = new DateTime(2026, 3, 22) },
            new Order { CustomerName = "Петр",    Amount = 4100m, Date = new DateTime(2026, 3, 25) },
            new Order { CustomerName = "Дмитрий", Amount = 3000m, Date = new DateTime(2026, 3, 28) },
            new Order { CustomerName = "Мария",   Amount = 2100m, Date = new DateTime(2026, 4, 2) },
            new Order { CustomerName = "Иван",    Amount = 900m,  Date = new DateTime(2026, 3, 30) },
            new Order { CustomerName = "Анна",    Amount = 600m,  Date = new DateTime(2026, 3, 31) }
        };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ИСХОДНЫЙ СПИСОК ЗАКАЗОВ ===");
                foreach (var o in orders)
                {
                    Console.WriteLine($"Дата: {o.Date:dd.MM.yyyy} | Клиент: {o.CustomerName,-10} | Сумма: {o.Amount} руб.");
                }
                Console.WriteLine(new string('=', 45));

                DateTime startDate = ReadDate("Введите начальную дату периода (дд.мм.гггг): ");
                DateTime endDate = ReadEndDate("Введите конечную дату периода (дд.мм.гггг): ", startDate);
                bool descending = ReadSortDirection();
                int takeCount = ReadTakeCount();

                var result = (descending
                    ? orders
                        .Where(o => o.Date >= startDate && o.Date <= endDate)
                        .GroupBy(o => o.CustomerName)
                        .Select(g => new { CustomerName = g.Key, TotalSpent = g.Sum(o => o.Amount) })
                        .OrderByDescending(c => c.TotalSpent)
                    : orders
                        .Where(o => o.Date >= startDate && o.Date <= endDate)
                        .GroupBy(o => o.CustomerName)
                        .Select(g => new { CustomerName = g.Key, TotalSpent = g.Sum(o => o.Amount) })
                        .OrderBy(c => c.TotalSpent))
                    .Take(takeCount)
                    .ToList();

                Console.WriteLine();
                Console.WriteLine($"=== ИТОГОВАЯ СВОДКА ({startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}) ===");

                if (result.Count == 0)
                {
                    Console.WriteLine("За выбранный период заказов не найдено.");
                }
                else
                {
                    foreach (var customer in result)
                    {
                        Console.WriteLine($"Клиент: {customer.CustomerName,-10} | Суммарные траты: {customer.TotalSpent} руб.");
                    }
                }

                Console.WriteLine(new string('=', 45));

                if (!AskToContinue())
                {
                    break;
                }
            }
        }

        static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
                Console.WriteLine("Ошибка: неверный формат даты. Пожалуйста, используйте формат дд.мм.гггг (например, 01.03.2026).");
            }
        }

        static DateTime ReadEndDate(string prompt, DateTime startDate)
        {
            while (true)
            {
                DateTime endDate = ReadDate(prompt);
                if (endDate >= startDate)
                {
                    return endDate;
                }
                Console.WriteLine("Ошибка: конечная дата не может быть раньше начальной даты.");
            }
        }

        static bool ReadSortDirection()
        {
            while (true)
            {
                Console.Write("Выберите сортировку по сумме (1 - по убыванию, 2 - по возрастанию): ");
                string input = Console.ReadLine()?.Trim();
                if (input == "1")
                {
                    return true;
                }
                if (input == "2")
                {
                    return false;
                }
                Console.WriteLine("Ошибка: введено неверное значение. Введите 1 или 2.");
            }
        }

        static int ReadTakeCount()
        {
            while (true)
            {
                Console.Write("Введите количество клиентов для вывода (Take, например 3): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int count) && count > 0)
                {
                    return count;
                }
                Console.WriteLine("Ошибка: введите целое положительное число больше нуля.");
            }
        }

        static bool AskToContinue()
        {
            while (true)
            {
                Console.Write("Хотите выполнить новый анализ? (1 - Да, 2 - Выход): ");
                string input = Console.ReadLine()?.Trim();
                if (input == "1")
                {
                    return true;
                }
                if (input == "2")
                {
                    return false;
                }
                Console.WriteLine("Ошибка: некорректный ввод. Введите 1 для продолжения или 2 для выхода.");
            }
        }
    }
}































