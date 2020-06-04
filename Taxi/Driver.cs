using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TaxiStation
{
    public class Driver : Employee
    {
        public int Rides { get; internal set; }
        public int ID { get; internal set; }
        internal DriverStatus status;
        public string Car { get; internal set; }
        internal double sum = 0;
        public Driver(double rate, string name, string car)
        {
            if (rate <= 0)
                throw new ArgumentException(ConstantStrings.EmployeeSalary);
            if (NameCheck(name))
                throw new ArgumentException(ConstantStrings.NotWhiteSpaceOrDigit);
            if (String.IsNullOrWhiteSpace(car))
                throw new NullReferenceException("Car name can't be whitespace or null.");
            if (!car.All(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch)))
                throw new ArgumentException("Car name must contain only letters and digits.");
            Name = name;
            Car = car;
            ID = ++TaxiPark.DriversCounter;
            Rate = rate;
            status = DriverStatus.Free;
            Rides = 0;
        }
        public override string ToString()
        {
                return $"ID -- {this.ID}\tName -- {this.Name}\nRate -- {this.Rate} UAH/km" +
                    $"\nStatus -- {this.status}\tCar -- {this.Car}\tRides -- {this.Rides}\t";
        }
        public static List<Driver> operator +(List<Driver> drivers, Driver driver)
        {
            drivers.Add(driver);
            return drivers;
        }
        public static List<Driver> operator +(Driver driver, List<Driver> drivers) => (drivers + driver);
        public static List<Driver> operator -(List<Driver> drivers, Driver driver)
        {
            drivers.RemoveAt(driver.ID - 1);
            return drivers;   
        }
        public static List<Driver> operator -(Driver driver, List<Driver> drivers) => (drivers - driver);
    }
}
