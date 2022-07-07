using Microsoft.Extensions.Options;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services.Interfaces;

namespace RssFeedDigestEmailer.Cli.Services;

public class TwilioBlogDataProvider : IDataProvider
{
    private readonly IRssService _rssService;
    private readonly EmailDataSettings _emailDataSettings;

    public TwilioBlogDataProvider(IRssService rssService, IOptions<EmailDataSettings> emailDataSettings)
    {
        _rssService = rssService;
        _emailDataSettings = emailDataSettings.Value;
    }
    
    public async Task<object> GetEmailData()
    {
        var blogInfo = await _rssService.GetBlogInfo();
        
        return new
        {
            recipientName = _emailDataSettings.RecipientName,
            subject = $"{_emailDataSettings.SubjectPrefix} - {FormatDate(DateTime.Today)}",
            lastBuildDate = FormatDate(blogInfo.LastBuildDate, showTime: true),
            blogPostList = blogInfo.BlogPosts.Select(b => new
            {
                title = b.Title, 
                link = b.Link, 
                headerImageUrl = b.HeaderImageUrl, 
                description = b.Description,
                author = b.Author,
                publishDate = FormatDate(b.PublishDate),
                categories = string.Join("; ", b.Categories) 
            })
        };

        string FormatDate(DateTime date, bool showTime = false)
        {
            return (showTime) ? date.ToString("dd MMMM yyyy, HH:mm") : date.ToString("dd MMMM yyyy");
        }
    }
}