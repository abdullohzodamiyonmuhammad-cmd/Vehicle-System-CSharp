using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConsoleApplication1
{
    internal abstract class Vehicle
    {
        protected Vehicle(string name, double consumptionPer100Km)
        {
            Name = name;
            ConsumptionPer100Km = consumptionPer100Km;
        }

        public string Name { get; private set; }
        public double ConsumptionPer100Km { get; private set; }

        public abstract string Move();

        public abstract decimal CalculateFuelCost(double distanceKm, decimal fuelPrice);

        public double CalculateFuelAmount(double distanceKm)
        {
            return ConsumptionPer100Km * distanceKm / 100.0;
        }

        public static string Format(double value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        public static string Format(decimal value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }

    internal class Car : Vehicle
    {
        public Car()
            : base("Автомобили сабук", 8.0)
        {
        }

        public override string Move()
        {
            return Name + " бо чор чархи худ дар роҳ ҳаракат мекунад.";
        }

        public override decimal CalculateFuelCost(double distanceKm, decimal fuelPrice)
        {
            double liters = CalculateFuelAmount(distanceKm);
            return (decimal)liters * fuelPrice;
        }
    }

    internal class Bus : Vehicle
    {
        private const double LoadPerPassenger = 0.005;

        public Bus()
            : base("Автобус", 25.0)
        {
            Passengers = 40;
        }

        public int Passengers { get; private set; }

        public override string Move()
        {
            return Name + " бо " + Passengers + " мусофир аз истгоҳ ба истгоҳ ҳаракат мекунад.";
        }

        public override decimal CalculateFuelCost(double distanceKm, decimal fuelPrice)
        {
            double loadFactor = 1.0 + Passengers * LoadPerPassenger;
            double liters = CalculateFuelAmount(distanceKm) * loadFactor;
            return (decimal)liters * fuelPrice;
        }
    }

    internal class Motorcycle : Vehicle
    {
        public Motorcycle()
            : base("Мотосикл", 4.0)
        {
        }

        public override string Move()
        {
            return Name + " бо ду чарх тез ва чолок ҳаракат мекунад.";
        }

        public override decimal CalculateFuelCost(double distanceKm, decimal fuelPrice)
        {
            double liters = CalculateFuelAmount(distanceKm);
            return (decimal)liters * fuelPrice;
        }
    }

    internal class Program
    {
        private static List<Vehicle> vehicles = new List<Vehicle>();

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            vehicles.Add(new Car());
            vehicles.Add(new Bus());
            vehicles.Add(new Motorcycle());

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                choice = (choice == null) ? "" : choice.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CalculateOne();
                        break;

                    case "2":
                        CompareAll();
                        break;

                    case "3":
                        ShowMovement();
                        break;

                    case "0":
                        Console.WriteLine("Хайр!");
                        return;

                    default:
                        Console.WriteLine("Хато: интихоби нодуруст.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("=== Системаи наqлиёт ===");
            Console.WriteLine("1. Харҷи сӯзишворӣ барои як намуди наqлиёт");
            Console.WriteLine("2. Муқоисаи ҳамаи намудҳо");
            Console.WriteLine("3. Нишон додани ҳаракати ҳамаи намудҳо");
            Console.WriteLine("0. Баромадан");
            Console.Write("Интихоб: ");
        }

        private static void CalculateOne()
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + vehicles[i].Name);
            }

            int number = ReadInt("Намуди наqлиётро интихоб кунед: ", 1, vehicles.Count);
            Vehicle vehicle = vehicles[number - 1];

            double distance = ReadDouble("Масофа (км): ");
            decimal price = (decimal)ReadDouble("Нархи як литр сӯзишворӣ: ");

            Console.WriteLine();
            Console.WriteLine(vehicle.Move());
            PrintResult(vehicle, distance, price);
        }

        private static void CompareAll()
        {
            double distance = ReadDouble("Масофа (км): ");
            decimal price = (decimal)ReadDouble("Нархи як литр сӯзишворӣ: ");

            Console.WriteLine();
            Console.WriteLine("--- Муқоиса барои " + Vehicle.Format(distance) + " км ---");

            for (int i = 0; i < vehicles.Count; i++)
            {
                PrintResult(vehicles[i], distance, price);
            }
        }

        private static void ShowMovement()
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                Console.WriteLine(vehicles[i].Move());
            }
        }

        private static void PrintResult(Vehicle vehicle, double distance, decimal price)
        {
            decimal cost = vehicle.CalculateFuelCost(distance, price);

            Console.WriteLine(vehicle.Name + ":");
            Console.WriteLine("   Сарфи асосӣ: " + Vehicle.Format(vehicle.ConsumptionPer100Km) + " л / 100 км");
            Console.WriteLine("   Харҷи сӯзишворӣ: " + Vehicle.Format(cost));
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                int value;
                if (int.TryParse(input, out value) && value >= min && value <= max)
                    return value;

                Console.WriteLine("Хато: адади бутун аз " + min + " то " + max + " ворид кунед.");
            }
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                input = (input == null) ? "" : input.Trim().Replace(',', '.');

                double value;
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                    && value > 0 && value <= 1000000)
                {
                    return value;
                }

                Console.WriteLine("Хато: адади аз сифр зиёд ворид кунед.");
            }
        }
    }
}