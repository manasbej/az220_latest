using Azure.Data.Tables;

public class GroceryItemEntity : ITableEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int Price { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public Azure.ETag ETag { get; set; }
    
}