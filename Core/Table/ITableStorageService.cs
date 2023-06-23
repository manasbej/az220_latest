
using Azure.Data.Tables;

public interface ITableStorageService
{
    Task<GroceryItemEntity> GetEntityAsync(string category, string id);
    Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity);
    Task<string> DeleteEntityAsync(string category, string id);
}

public class TableStorageService : ITableStorageService
{
    private const string TableName = "Item";
    private readonly IConfiguration _configuration;

    public TableStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    private async Task<TableClient> GetTableClient()
    {
        var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);

        var tableClient = serviceClient.GetTableClient(TableName);
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
    public async Task<GroceryItemEntity> GetEntityAsync(string partitionKey, string rowKey)
    {
        var tableClient = await GetTableClient();
        return await tableClient.GetEntityAsync<GroceryItemEntity>(partitionKey, rowKey);        
    }

    public async Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity)
    {
        var tableClient = await GetTableClient();
        await tableClient.UpsertEntityAsync(entity);
        return entity;
    }

    public async Task<string> DeleteEntityAsync(string category, string id)
    {
        var tableClient = await GetTableClient();
        await tableClient.DeleteEntityAsync(category, id);   
        return "Record Deleted successfully";         
    }
}