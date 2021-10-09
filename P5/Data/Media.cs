namespace The_Car_Hub.Data
{
    public class Media
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
    }
}