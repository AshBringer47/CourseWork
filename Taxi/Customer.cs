using System;
using System.Collections.Generic;

namespace TaxiStation
{
    public class Customer
    {
        public string Name { get; internal set; }
        public ClientStatus CustomerType { get; internal set; }
        public CustomerStatus customerStatus { get; internal set; }
        public int ID { get; internal set; }
        public int MadeRides { get; internal set; }
        public Customer(string newName, int newKind)
        {
            if (Employee.NameCheck(newName))
                throw new ArgumentException(ConstantStrings.NotWhiteSpaceOrDigit);
            if (newKind == 0 || newKind == 1)
            {
                ID = ++TaxiPark.CustomersCounter;
                Name = Employee.NameTrim(newName);
                CustomerType = (ClientStatus)newKind;
                MadeRides = 0;
                customerStatus = CustomerStatus.NotBusy;
            }
            else 
                throw new ArgumentException("Error. Wrong type of customer.");            
        }
        public static List<Customer> operator +(List<Customer> customers, Customer customer)
        {
            customers.Add(customer);
            return customers;
        }
        public static List<Customer> operator +(Customer customer, List<Customer> customers) => (customers + customer);
        public static List<Customer> operator -(List<Customer> customers, Customer customer)
        {
            customers.RemoveAt(customer.ID);
            for (var i = customer.ID - 1; i < customers.Count; i++)
                customers[i].ID--;
            return customers;
        }        
        public static List<Customer> operator -(Customer customer, List<Customer> customers) => (customers - customer);
        public override string ToString()
        {
            return $"ID -- {this.ID}\t Name -- {this.Name}\tType -- {this.CustomerType}\t  Made orders -- {this.MadeRides}";
        }
    }
}
