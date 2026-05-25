namespace BookingApp.Domain.Exceptions
{
    public class DomainException(string message) : System.Exception(message)
    {
    }
}