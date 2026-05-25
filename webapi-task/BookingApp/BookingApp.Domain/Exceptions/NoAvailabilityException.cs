using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Domain.Exceptions
{
    public class NoAvailabilityException(string message) : DomainException(message)
    {  
    }
}