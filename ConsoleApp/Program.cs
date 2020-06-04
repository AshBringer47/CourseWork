using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxiStation;
using Exceptions;

namespace ConsoleApp
{
    internal static class Program
    {
        private static void Main()            
        {
            var drivers = new List<Driver>
            {
                new Driver(50, "Alexey Konik", "Siniy Priora"),
                new Driver(50, "Arthur Ladchenko", "Joltyi Lada"),
                new Driver(50, "Chechet Nikita", "Vyshnyovaya symyorka")
            };
            var customers = new List<Customer>
            {
                new Customer("Michael Keehl", 0),
                new Customer("Nate Reaver", 0),
                new Customer("Yagami Light", 0)
            };
            TaxiPark taxiPark = new TaxiPark(drivers, customers);
            Accountant accountant = new Accountant(4500, "Daria Kariavka");
            taxiPark.TaxiAction += CustomerEvent;
            accountant.AccountantAction += CustomerEvent;
            var active = true;
            Console.WriteLine("Greetings. Thanks for using our application.");
            while (active)
            {
                try
                {
                    Console.ResetColor();
                    Console.WriteLine("\n\nChoose your account type:\n1. Customer.\n2. Accountant.\n3. Stop the program.");
                    var account = Console.ReadLine();
                    switch (account)
                    {
                        case "1":
                            var flag = true;
                            var menu = ("\nMake your choice:\n1. To make a new order\n2. To see driver database\n3. Exit this account");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            while (flag)
                            {
                                Console.WriteLine(menu);
                                var choice = Console.ReadLine();
                                switch (choice)
                                {
                                    case "1":
                                        MakeOrder(taxiPark);
                                        break;
                                    case "2":
                                        taxiPark.SeeDrivers();
                                        break;
                                    case "3":
                                        Console.WriteLine("Exiting customer's account.");
                                        Console.ResetColor();
                                        flag = false;
                                        break;
                                    case "4":
                                        taxiPark.FreeDrivers();
                                        break;
                                    default:
                                        Console.WriteLine("Error. Please, try again.");
                                        break;
                                }
                            }
                            break;
                        case "2":
                            flag = true;
                            menu = ("\nMake your choice:\n1. Change driver's car\n2. Fire a driver" +
                                    "\n3. Change discount for regular customers\n4. See list of all orders" +
                                    "\n5. See list of customers\n6. See full info about drivers" +
                                    "\n7. Hire a new driver\n8. Change customer's type\n9. Exit this account");
                            Console.ForegroundColor = ConsoleColor.Red;
                            while (flag)
                            {
                                Console.WriteLine(menu);
                                var choice = Console.ReadLine();
                                switch (choice)
                                {

                                    case "1":
                                        taxiPark.SeeDrivers();
                                        Console.WriteLine("Here is the driver's list. Enter driver's ID: ");
                                        int ID = Int32.Parse(Console.ReadLine());
                                        if (!taxiPark.FindDriver(ID))
                                        {
                                            Console.WriteLine("Please try again: ");
                                            ID = Int32.Parse(Console.ReadLine());
                                            while (!taxiPark.FindDriver(ID))
                                            {
                                                Console.WriteLine("Please try again: ");
                                                ID = Int32.Parse(Console.ReadLine());
                                            }
                                        }
                                        Console.Write("Write new driver's car: ");
                                        taxiPark.ChangeDriverCar(ID, Console.ReadLine());
                                        break;
                                    case "2":
                                        Console.WriteLine("Here is the driver's list. Enter driver's ID: ");
                                        taxiPark.SeeDrivers();
                                        ID = Int32.Parse(Console.ReadLine());
                                        if (!taxiPark.FindDriver(ID))
                                        {
                                            Console.WriteLine("Please try again: ");
                                            ID = Int32.Parse(Console.ReadLine());
                                            while (!taxiPark.FindDriver(ID))
                                            {
                                                Console.WriteLine("Please try again: ");
                                                ID = Int32.Parse(Console.ReadLine());
                                            }
                                        }
                                        taxiPark.Fire(ID);
                                        break;
                                    case "3":
                                        Console.Write("Enter new discount for regular customers: ");
                                        accountant.ChangeDiscount(Int32.Parse(Console.ReadLine()));
                                        break;
                                    case "4":
                                        Console.WriteLine("List of all orders created in TaxiStation:");
                                        taxiPark.SeeOrders();
                                        break;
                                    case "5":

                                        Console.WriteLine("List of all customer registrated in TaxiStation:");
                                        taxiPark.SeeCustomers();
                                        break;
                                    case "6":
                                        Console.WriteLine("Full info about every driver in TaxiStation:");
                                        taxiPark.SeeFullDriversInfo();
                                        break;
                                    case "7":
                                        Console.Write("Enter rate of driver per kilometer: ");
                                        double rate = Double.Parse(Console.ReadLine());
                                        Console.Write("Enter driver's name: ");
                                        string name = Console.ReadLine();
                                        Console.Write("Enter driver's car name: ");
                                        taxiPark.Hire(new Driver(rate, name, Console.ReadLine()));
                                        break;
                                    case "8":
                                        Console.WriteLine("Here is the list of customers:");
                                        taxiPark.SeeCustomers();
                                        Console.WriteLine("Choose correct ID: ");
                                        ID = Int32.Parse(Console.ReadLine());
                                        if (!taxiPark.FindCustomer(ID))
                                        {
                                            Console.WriteLine("Please try again.");
                                            ID = Int32.Parse(Console.ReadLine());
                                            while (!taxiPark.FindCustomer(ID))
                                            {
                                                Console.WriteLine("Please try again.");
                                                ID = Int32.Parse(Console.ReadLine());
                                            }
                                        }
                                        Console.WriteLine("Choose correct type of client:\n0 - regular\n1 - new");
                                        taxiPark.ChangeClientType(ID, Int32.Parse(Console.ReadLine()));
                                        break;
                                    case "9":
                                        Console.WriteLine("Exiting accountant's account.");
                                        Console.ResetColor();
                                        flag = false;
                                        break;
                                    default:
                                        Console.WriteLine("Error. Please try again");
                                        break;
                                }
                            }
                            break;
                        case "3":
                            Console.WriteLine("Program has been stopped.");
                            active = false;
                            break;
                        default:
                            Console.WriteLine("Please try again");
                            break;
                    }
                }
                catch (NegativeNumberException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (UnderflowException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrent input format.");
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private static async void MakeOrder(TaxiPark taxiPark)
        {
            try
            {
                bool activity = true;
                while (activity)
                {
                    Console.WriteLine("Are you a new customer? If no press 1, if yes - 2");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Here are our customers. Find your ID:");
                            taxiPark.SeeCustomers();
                            int CustomerID = Int32.Parse(Console.ReadLine());
                            if (!taxiPark.FindCustomer(CustomerID))
                            {
                                Console.WriteLine("Please try again.");
                                CustomerID = Int32.Parse(Console.ReadLine());
                                while (!taxiPark.FindCustomer(CustomerID))
                                {
                                    Console.WriteLine("Please try again.");
                                    CustomerID = Int32.Parse(Console.ReadLine());
                                }
                            }
                            Customer customer = taxiPark.GetCustomer(CustomerID);
                            Console.WriteLine("Here are our drivers. Choose one of them: ");
                            taxiPark.SeeDrivers();
                            int DriverID = Int32.Parse(Console.ReadLine());
                            if (!taxiPark.FindDriver(DriverID))
                            {
                                Console.WriteLine("Please try again: ");
                                DriverID = Int32.Parse(Console.ReadLine());
                                while (!taxiPark.FindDriver(DriverID))
                                {
                                    Console.WriteLine("Please try again: ");
                                    DriverID = Int32.Parse(Console.ReadLine());
                                }
                            }
                            Console.Write("Enter length of your order in km: ");
                            double Length = Double.Parse(Console.ReadLine());
                            Order order = new Order(Length, taxiPark.GetDriver(DriverID), taxiPark.GetCustomer(CustomerID));
                            order.OrderAction += CustomerEvent;
                            Console.WriteLine("Here is the info about your order: ");
                            order.Info();
                            bool temp = true;
                            while (temp)
                            {
                                Console.WriteLine("Do you agree with it?\n1 - proceed the order\n2 - abort the order");
                                string decision = Console.ReadLine();
                                switch (decision)
                                {
                                    case "1":
                                        await Task.Run(() => { order.CompleteOrder(taxiPark); });
                                        temp = false;
                                        break;
                                    case "2":
                                        order.AbortOrder(taxiPark);
                                        temp = false;
                                        break;
                                    default:
                                        Console.WriteLine("Error. Please try again.");
                                        break;
                                }
                            }
                            activity = false;
                            break;
                        case "2":
                            Console.Write("Please enter your name: ");
                            customer = new Customer(Console.ReadLine(), 1);
                            taxiPark.NewCustomer(customer);
                            Console.WriteLine("Here are our drivers. Choose one of them: ");
                            taxiPark.SeeDrivers();
                            int NewDriverID = Int32.Parse(Console.ReadLine());
                            if (!taxiPark.FindDriver(NewDriverID))
                            {
                                Console.WriteLine("Please try again.");
                                DriverID = Int32.Parse(Console.ReadLine());
                                while (!taxiPark.FindDriver(NewDriverID))
                                {
                                    Console.WriteLine("Please try again.");
                                    DriverID = Int32.Parse(Console.ReadLine());
                                }
                            }
                            Console.WriteLine("Enter length of your order in km.");
                            Length = Double.Parse(Console.ReadLine());
                            order = new Order(Length, taxiPark.GetDriver(NewDriverID), customer);
                            order.OrderAction += CustomerEvent;
                            Console.WriteLine("Here is the info about your order: ");
                            order.Info();
                            temp = true;
                            while (temp)
                            {
                                Console.WriteLine("Do you agree with it?\n1 - proceed the order\n2 - abort the order");
                                string decision = Console.ReadLine();
                                switch (decision)
                                {
                                    case "1":
                                        await Task.Run(() => { order.CompleteOrder(taxiPark); });
                                        temp = false;
                                        break;
                                    case "2":
                                        order.AbortOrder(taxiPark);
                                        temp = false;
                                        break;
                                    default:
                                        Console.WriteLine("Error. Please try again.");
                                        temp = false;
                                        break;
                                }
                            }
                            activity = false;
                            break;
                        default:
                            Console.WriteLine("Error. Please try again.");
                            break;
                    }
                }
            }
            catch (NegativeNumberException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (UnderflowException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrent input format.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void CustomerEvent(object sender, HandlerArgs employeeHandlerArgs)
        {
            Console.WriteLine(String.Format(employeeHandlerArgs.Message));
        }
    }
}