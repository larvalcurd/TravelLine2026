namespace BookingApp.Domain.Exceptions
{
    public class NotFoundException( string message ) : DomainException( message )
    {
    }
}