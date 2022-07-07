using Newtonsoft.Json;
using RssFeedDigestEmailer.Cli.Services.Interfaces;

namespace RssFeedDigestEmailer.Cli.Services;

public class JsonDataProvider : IDataProvider
{
    public async Task<object> GetEmailData()
    {
        string jsonFilePath = "./Data/DummyData.json";
        string rawContents = await File.ReadAllTextAsync(jsonFilePath);
        return JsonConvert.DeserializeObject(rawContents);
    }
}