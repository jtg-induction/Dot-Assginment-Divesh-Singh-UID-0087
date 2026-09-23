using System;

namespace RestaurantManagement.Exceptions
{
    public class ResourceException : Exception
    {
        public ResourceException(string message) : base(message)
        {
        }
    }
}
