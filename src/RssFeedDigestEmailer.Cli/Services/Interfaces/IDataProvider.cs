namespace RssFeedDigestEmailer.Cli.Services.Interfaces;

public interface IDataProvider
{
    Task<object> GetEmailData();
}