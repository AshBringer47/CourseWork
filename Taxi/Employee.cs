using System.Linq;

namespace TaxiStation
{
    public abstract class Employee
    {
        public delegate void EmpolyeeHanlder(object sender, HandlerArgs employeeHandlerArgs);
        protected internal double Rate;
        protected internal string Name;
        // checks if name is null or empty.
        internal static bool NameCheck(string Name)
        {
            Name = NameTrim(Name);
            if (string.IsNullOrEmpty(Name) || Name.Length == 0)
                return true;
            if (Name.Any(char.IsDigit))
                return true;
            else
                return false;
        }
        // trims string, until it has only 1 space between words.
        internal static string NameTrim(string Name)
        {
            Name = Name.Trim();
            while (Name.Contains("  "))
                Name = Name.Replace("  ", " ");
            return Name;
        }
    }
}
