using System;
using System.Collections.Generic;
using Exceptions;
using System.Threading.Tasks;

namespace TaxiStation
{
    public class TaxiPark : IAdmin
    {
        public delegate void TaxiHandler(object sender, HandlerArgs employeeHandlerArgs);
        internal static int DriversCounter = 0;
        internal static int CustomersCounter = 0;
        internal static int OrdersCounter = 0;
        public static int Discount { get; internal set; } = 10;
        public List<Driver> Drivers { get; internal set; }
        public List<Customer> Customers { get; internal set; }
        public List<Order> Orders { get; internal set; }
        public TaxiPark(List<Driver> newDrivers, List<Customer> newCustomers)
        {
            Drivers = newDrivers;
            Customers = newCustomers;
            Orders = new List<Order> { };
        }
        // fires a driver by ID.
        public void Fire(int ID)
        {
            if (DriversCounter == 0)
            {
                throw new UnderflowException(ConstantStrings.NoDrivers);
            }
            this.Drivers.RemoveAt(ID - 1);
            DriversCounter--;
            for (var i = ID - 1; i < Drivers.Count; i++)
                Drivers[i].ID--;
        }
        // hires a driver.
        public void Hire(Driver driver)
        {
            Drivers += driver;
        }
        // gets admin info about drivers.
        public void SeeFullDriversInfo()
        {
            if (DriversCounter == 0)
            {
                TaxiAction?.Invoke(this, new HandlerArgs(ConstantStrings.NoDrivers));
            }
            else
            {
                foreach (var Driver in this.Drivers)
                    TaxiAction?.Invoke(Driver, new HandlerArgs(Driver.ToString() + $"\tEarned -- {Driver.sum} uah"));
            }
        }
        // checks, if there is a driver with this ID.
        public bool FindDriver(int ID)
        {
            if (ID - 1 >= Drivers.Count)
            {
                TaxiAction?.Invoke(this, new HandlerArgs($"Driver with ID {ID} wasn't found."));
                return false;
            }
            else if (ID < 0)
                throw new NegativeNumberException(ConstantStrings.NegativeNumbers);
            else
                return true;
        }
        // get driver by ID.
        public Driver GetDriver(int ID) => Drivers[ID - 1];
        // checks, if there is a customer with this ID.
        public bool FindCustomer(int ID)
        {
            if (ID - 1 >= Customers.Count)
            {
                TaxiAction?.Invoke(this, new HandlerArgs(ConstantStrings.NoCustomer));
                return false;
            }
            else if (ID < 0)
                throw new NegativeNumberException(ConstantStrings.NegativeNumbers);
            else
                return true;
        }
        // get customer by ID.
        public Customer GetCustomer(int ID) => Customers[ID - 1];
        // gets info about drivers.
        public void SeeDrivers()
        {
            if (DriversCounter == 0)
            {
                TaxiAction?.Invoke(this, new HandlerArgs(ConstantStrings.NoDrivers));
            }
            else
            {
            foreach (var Driver in this.Drivers)
                TaxiAction?.Invoke(Driver, new HandlerArgs(Driver.ToString()));
            }
        }
        // gets info about customers.
        public void SeeCustomers()
        {
            foreach (var Customer in this.Customers)
                TaxiAction?.Invoke(Customer, new HandlerArgs(Customer.ToString()));
        }
        // gets info about orders.
        public void SeeOrders()
        {
            if (Orders.Count == 0)
                TaxiAction?.Invoke(this, new HandlerArgs("No orders yet."));
            else
                foreach (var Order in this.Orders)
                    TaxiAction?.Invoke(Order, new HandlerArgs(Order.ToString()));
        }
        // adds new customer to the list.
        public void NewCustomer(Customer customer)
        {
            Customers += customer;
        }
        // change client's type.
        public void ChangeClientType(int ID, int newKind)
        {
            if(newKind == 0 || newKind == 1)
            {
                Customers[ID - 1].CustomerType = (ClientStatus)newKind;
            }
            else
            {
                throw new ArgumentException("Error. Wrong customer type.");
            }
        }       
        // changes driver's car by ID. 
        public void ChangeDriverCar(int ID, string newCar)
        {
            if (Drivers.Count == 0)
                TaxiAction?.Invoke(this, new HandlerArgs(ConstantStrings.NoDrivers));
            else if (ID - 1>= Drivers.Count)
            {
                TaxiAction?.Invoke(Drivers[ID - 1], new HandlerArgs("No such driver found. Please try again"));
            }
            else if (ID < 0)
            {
                throw new NegativeNumberException("No negative numbers are allowed");
            }
            else
            {
                Drivers[ID - 1].Car = newCar;
            }
        }
        // checks if there are free drivers.
        internal bool IsFree()
        {
            if(DriversCounter == 0)
            {
                return false;
            }
            foreach(var driver in Drivers)
            {
                if(driver.status == DriverStatus.Free)
                {
                    return true;
                }
            }
            return false;
        }
        // get info about free drivers.
        public async void FreeDrivers()
        {
            if (Drivers.Count == 0)
                TaxiAction?.Invoke(this, new HandlerArgs(ConstantStrings.NoDrivers));
            else if(IsFree())
            {
                foreach (var driver in Drivers)
                {
                    if(driver.status == DriverStatus.Free)
                    {
                        TaxiAction?.Invoke(this, new HandlerArgs(driver.ToString() + $"\tStatus -- {driver.status}"));
                    }
                }
            }
            else
            {
                TaxiAction?.Invoke(this, new HandlerArgs("Sorry, there are no free drivers in the TaxiStation.\n"));
                await Task.Run(() =>
                {
                    while(!IsFree())
                    {
                        if (Drivers.Count == 0)
                        {
                            TaxiAction?.Invoke(this, new HandlerArgs(ConstantStrings.NoDrivers));
                            break;
                        }
                    }
                    if (Drivers.Count != 0)
                    {
                        TaxiAction?.Invoke(this, new HandlerArgs("There are free drivers in TaxiStation.\n"));
                        FreeDrivers();
                    }
                });
            }
        }
        // handle events.
        public event TaxiHandler TaxiAction;
    }
}
