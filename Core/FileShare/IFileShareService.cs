using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;


public interface IAzureFileShareService
{
    Task CreateFileWithTextAsync(string fileName, string text);
    Task<string> UpdateFileWithTextAsync(string fileName, string text);
    Task<string> DeleteFileAsync(string fileName);
}

public class AzureFileShareService : IAzureFileShareService
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private const string shareName = "az220fileshare";

    private CloudStorageAccount storageAccount;
    private CloudFileClient fileClient;
    private CloudFileShare share;
    public AzureFileShareService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration["StorageConnectionString"];
        storageAccount = CloudStorageAccount.Parse(_connectionString);
        fileClient = storageAccount.CreateCloudFileClient();
        share = fileClient.GetShareReference(shareName);
    }


    public async Task CreateFileWithTextAsync(string fileName, string text)
    {
        // Ensure that the share exists.  
        if (share.Exists())
        {
            //Create file in root directory
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();            
            await rootDir.GetFileReference(fileName).UploadTextAsync(text);
        }
    }
    public async Task<string> UpdateFileWithTextAsync(string fileName, string text){
        if (share.Exists()){
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();  
            CloudFile cloudFile = rootDir.GetFileReference(fileName) ;
            if(cloudFile.Exists()){
                string from_azure_file = await cloudFile.DownloadTextAsync();
                await cloudFile.UploadTextAsync(from_azure_file + " " +text);
                return "File deleted successfully";
            }
            return "There is no such file to be add text.";
        }
        return "There Fileshare exists";
    }
    public async Task<string> DeleteFileAsync(string fileName){
        CloudFileDirectory rootDir = share.GetRootDirectoryReference();  
        CloudFile cloudFile = rootDir.GetFileReference(fileName) ;
        if(cloudFile.Exists()){
            await cloudFile.DeleteAsync();
            return "File deleted successfully";
        }
        return "There is no such file to be deleted.";
    }
}
