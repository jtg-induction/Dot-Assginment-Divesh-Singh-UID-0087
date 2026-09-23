namespace RestaurantManagement.Models.Enum
{
    /// <summary>
    /// Identifies the type of an address.
    /// </summary>
    public enum AddressType
    {
       
        // A residential address. 
        Home=1,
        // A workplace address.
        Work=2,
        // An address that does not fit another category.
        Other=3
    }
}
