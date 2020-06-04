using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Exceptions;

namespace TaxiStation
{
    public class Order
    {
        public delegate void OrderHandler(object sender, HandlerArgs employeeHandlerArgs);
        public int ID { get; private set; }
        public double Length { get; private set; }
        public double Cost { get; private set; }
        public Driver NewDriver { get; private set; }
        public Customer NewCustomer { get; private set; }
        public OrderStatus Status;
        public Order(double newLength, Driver newDriver, Customer newCustomer)
        {
            if (newDriver.status == DriverStatus.Busy)
                throw new NullReferenceException("Can't make up order with busy driver.");
            if (newCustomer.customerStatus == CustomerStatus.InTrip)
                throw new NullReferenceException("Can't make up order with busy customer.");
            if (newLength < 0)
                throw new NegativeNumberException(ConstantStrings.NegativeNumbers);
            else if (newLength < 0.5)
            {
                throw new NullReferenceException($"Order with length = {newLength} km was not created. Length must be at least 500 meters.");
            }
            else
            {                                              
                ID = ++TaxiPark.OrdersCounter;            
                Status = OrderStatus.Taken;
                Length = newLength;
                NewDriver = newDriver;
                NewCustomer = newCustomer;
                Cost = (NewCustomer.CustomerType == ClientStatus.Regular) ? Length * NewDriver.Rate * ((double)(100 - TaxiPark.Discount) / 100) : Length * NewDriver.Rate;
            }
            
        }
        public void Info()
        {
            OrderAction?.Invoke(this, new HandlerArgs($"Your order info:\tCost -- {this.Cost} UAH\tLength -- {this.Length} km\nYour driver info:\tName -- {this.NewDriver.Name}\tCar -- {this.NewDriver.Car}"));
        }
        public override string ToString()
        {
            return $"ID -- {this.ID}\tDriver name -- {NewDriver.Name}\tLength -- {this.Length} km\tPrice -- {this.Cost} UAH\nCustomer name -- {NewCustomer.Name}\tStatus -- {Status}";
        }
        public async void CompleteOrder(TaxiPark taxiPark)
        {
            this.NewDriver.sum += Cost;
            OrderAction?.Invoke(this, new HandlerArgs($"Order #{this.ID} is accepted. {this.NewDriver.Name} is waiting for you."));
            taxiPark.Customers[NewCustomer.ID - 1].customerStatus = CustomerStatus.InTrip;
            taxiPark.Drivers[NewDriver.ID - 1].status = DriverStatus.Busy;
            this.Status = OrderStatus.Taken;
            taxiPark.Orders += this;
            await Task.Run(() =>
            {
                Thread.Sleep(20000);
                taxiPark.Customers[NewCustomer.ID - 1].customerStatus = CustomerStatus.NotBusy;
                taxiPark.Drivers[NewDriver.ID - 1].status = DriverStatus.Free;
            });
            OrderAction?.Invoke(this, new HandlerArgs($"Order #{this.ID} was completed. Thanks for using our TaxiPark"));
            this.Status = OrderStatus.Completed;
            this.NewDriver.Rides++;
            this.NewCustomer.MadeRides++;
            if (NewCustomer.MadeRides >= 3)
                NewCustomer.CustomerType = ClientStatus.Regular;
        }
        public void AbortOrder(TaxiPark taxiPark)
        {
            OrderAction?.Invoke(this, new HandlerArgs($"Order #{this.ID} was aborted."));
            this.Status = OrderStatus.Uncompleted;
            taxiPark.Orders += this;
        }
        public static List<Order> operator+(List<Order> orders, Order order)
        {
            orders.Add(order);
            return orders;
        }
        public static List<Order> operator +(Order order, List<Order> orders) => (orders + order);

        public event OrderHandler OrderAction;
    }
}
