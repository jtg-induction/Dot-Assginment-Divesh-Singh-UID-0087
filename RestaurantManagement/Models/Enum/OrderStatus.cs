namespace RestaurantManagement.Models.Enum
{
    /// <summary>
    /// Defines the possible statuses of a restaurant order.
    /// </summary>
    public enum OrderStatus
    {
        //An order has been placed.
        Placed = 1,
        //An order has been accepted.
        Accepted = 2,
        //An order has been rejected.
        Rejected = 3,
        //An order has been dispatched.
        Dispatched = 4,
        // <summary>An order is out for delivery.
        Delivered = 5,
        // <summary>An order has been cancelled.
        Cancelled = 6


    }
}

