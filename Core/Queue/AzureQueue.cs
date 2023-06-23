
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

public interface IAzureQueue
{
    Task<string> CreateQueueAsync(string queueName);
    Task<string> DeleteQueueAsync(string queueName);
    Task<string> SendMessageAsync(string queueName,string message);
    Task<List<string>> ReadQueueAsync(string queueName);
    Task<string> UpdateMessage(string queueName, string text);
    
}

public class AzureQueue : IAzureQueue
{
    private const string QUEUE_NAME = "abcqueue";
    private readonly IConfiguration _configuration;
    private readonly ILogger<AzureQueue> _logger;
    private readonly string _connectionString;
    private CloudStorageAccount storageAccount;
    private CloudQueueClient queueClient;
    private CloudQueue  queue;
    public AzureQueue(IConfiguration configuration, ILogger<AzureQueue> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _connectionString = _configuration["StorageConnectionString"];  
        storageAccount = CloudStorageAccount.Parse(_connectionString);
        queueClient = storageAccount.CreateCloudQueueClient();
        queue = queueClient.GetQueueReference(QUEUE_NAME); 
        //queue.CreateIfNotExists();     
    }
    public async Task<string> CreateQueueAsync(string queueName)
    {
        queue = queueClient.GetQueueReference(queueName); 
        await queue.CreateIfNotExistsAsync();  
        return "Queuue Created";
    }
    public async Task<string> SendMessageAsync(string queueName,string message)
    {
        queue = queueClient.GetQueueReference(queueName); 
        CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(message);
        await queue.AddMessageAsync(cloudQueueMessage);
        return "Message send to queue";
    }
    public async Task<List<string>> ReadQueueAsync(string queueName)
    {
        List<string> messages = new List<string>();
        

        queue = queueClient.GetQueueReference(queueName); 
        QueueClient queueClientLocal = new QueueClient(_connectionString, queueName);
        QueueProperties properties = queueClientLocal.GetProperties();

        IEnumerable<CloudQueueMessage> messageList = await queue.GetMessagesAsync(properties.ApproximateMessagesCount);
        foreach (CloudQueueMessage m in messageList)
        {
            messages.Add(m.AsString);
        }
        return messages;
    }

    public async Task<string> DeleteQueueAsync(string queueName)
    {
        queue = queueClient.GetQueueReference(queueName); 
        await queue.DeleteIfExistsAsync();
        return "Queue Deleted";
    }
    public async Task<string> UpdateMessage(string queueName,string text)
    {
        // Instantiate a QueueClient which will be used to manipulate the queue
        QueueClient queueClient = new QueueClient(_connectionString, queueName);

        if (queueClient.Exists())
        {
            // Get the message from the queue
            QueueMessage[] message = queueClient.ReceiveMessages();

            // Update the message contents
            if(message.Length == 0)
            {
                return "No message to update";
            }
            await queueClient.UpdateMessageAsync(message[0].MessageId, 
                    message[0].PopReceipt, 
                    text,
                    TimeSpan.FromSeconds(5.0)  // Make it invisible for another 5 seconds
                );
        }else
        {
            return "No Queue ("+ QUEUE_NAME +") found";
        }

        return "1st mesage update in queue -"+ QUEUE_NAME;
    }
}