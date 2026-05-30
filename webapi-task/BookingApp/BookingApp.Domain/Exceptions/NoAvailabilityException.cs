namespace BookingApp.Domain.Exceptions
{
    public class NoAvailabilityException(string message) : DomainException(message)
    {  
    }
}