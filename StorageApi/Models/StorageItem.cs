namespace StorageApi.Models
{
    public class StorageItem
    {
        public StorageItem(string itemName)
        {
            ItemName = itemName;
        }

        public int Id { get; set; }

        public string ItemName { get; private set; }

        public int ItemAmount { get; set; }
    }
}
