namespace WebShopWebApi;

public class Constants
{
    public static readonly Guid EXECUTOR_ID = Guid.Empty;
    public static readonly Guid INVENTORY_ID = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid ORDER_ID = Guid.Parse("00000000-0000-0000-0000-000000000002");
    public static readonly Guid SHIPMENT_ID = Guid.Parse("00000000-0000-0000-0000-000000000003");
    public static readonly Guid PAYMENT_ID = Guid.Parse("00000000-0000-0000-0000-000000000004");
    public const string ORDER_STATE_MACHINE = "order-state-machine";
    public const string INVENTORY_STATE_MACHINE = "inventory-state-machine";
    public const string SHIPMENT_STATE_MACHINE = "shipment-state-machine";
    public const string PAYMENT_STATE_MACHINE = "payment-state-machine";
}
