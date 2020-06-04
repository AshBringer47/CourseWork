namespace TaxiStation
{
    interface IAdmin
    {
        // See info about every driver.
        void SeeFullDriversInfo();
        // Remove driver from taxistation.
        void Fire(int ID);
        // Hire a driver to the taxistation.
        void Hire(Driver driver);
    }
}
