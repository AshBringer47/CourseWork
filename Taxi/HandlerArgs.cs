namespace TaxiStation
{
    public class HandlerArgs
    {
        public string Message { get; }
        public HandlerArgs(string message)
        {
            Message = message;
        }
    }
}
