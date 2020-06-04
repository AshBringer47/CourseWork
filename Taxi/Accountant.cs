using System;
using Exceptions;

namespace TaxiStation
{
    public class Accountant : Employee
    {        public Accountant(double rate, string name)
        {
            if (rate <= 0)
                throw new NegativeNumberException(ConstantStrings.EmployeeSalary);
            if (NameCheck(name))
                throw new ArgumentException(ConstantStrings.NotWhiteSpaceOrDigit);
            Rate = rate;
            Name = NameTrim(name);
        }
        // set new discount for regular customers.
        public void ChangeDiscount(int newDiscount)
        {
            if(newDiscount < 0)
            {
                throw new NegativeNumberException(ConstantStrings.NegativeNumbers);
            }
            if(newDiscount >= 0 && newDiscount < 10)
            {
                TaxiPark.Discount = 10;
                AccountantAction?.Invoke(this, new HandlerArgs($"You tried to discount value to {newDiscount}%, " +
                    $"less than minimum. New discount for regular customers is set to {TaxiPark.Discount}%."));
            }
            else if (newDiscount > 50)
            {
                TaxiPark.Discount = 50;
                AccountantAction?.Invoke(this, new HandlerArgs($"You tried to discount value to {newDiscount}%, exceeding maximal. " +
                    $"New discount for regular customers is set to {TaxiPark.Discount}%."));
            }
            else TaxiPark.Discount = newDiscount;
            AccountantAction?.Invoke(this, new HandlerArgs($"New discount for regular customers is set to {TaxiPark.Discount}%."));
        }
        // handle events.
        public event EmpolyeeHanlder AccountantAction;
    }
}