namespace ShopSphere.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        ReadyForPickup = 2,
        Shipped = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Cancelled = 6,
        Rejected = 7
    }
}
