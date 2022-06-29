namespace RssFeedDigestEmailer.Cli.Services.Interfaces;

public interface IHtmlService
{
    Task<(string, string)> GetHeaderImageUrlAndPostDate(string blogPostUrl);
    Task<string> GetPostIntroduction(string rawPostHtml);
}