namespace The_Car_Hub.Data
{
    public class InventoryStatus
    {
        public int Id { get; set; }
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Unavaible,
            Maintenance,
            OnSale,
            Sold,
        }
    }
}