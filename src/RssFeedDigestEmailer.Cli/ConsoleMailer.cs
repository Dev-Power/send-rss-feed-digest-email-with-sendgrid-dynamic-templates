using Microsoft.Extensions.Options;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services.Interfaces;

namespace RssFeedDigestEmailer.Cli;

public class ConsoleMailer
{
    private readonly IRssService _rssService;
    private readonly IEmailService _emailService;
    private readonly EmailSettings _emailSettings;

    public ConsoleMailer(IRssService rssService, IEmailService emailService, IOptions<EmailSettings> emailSettings)
    {
        _rssService = rssService;
        _emailService = emailService;
        _emailSettings = emailSettings.Value;
    }
    
    public async Task Send()
    {
        var blogInfo = await _rssService.GetBlogInfo();
        
        var dynamicEmailData = new
        {
            recipientName = _emailSettings.RecipientDisplayName,
            subject = $"Latest Posts - {DateTime.Today.ToString("dd MMMM yyyy")}",
            lastBuildDate = blogInfo.LastBuildDate.ToString("dd MMMM yyyy, HH:mm"),
            blogPostList = blogInfo.BlogPosts.Select(b => new
            {
                b.Title, 
                b.Link, 
                b.HeaderImageUrl, 
                b.Description,
                b.Author,
                PublishDate = b.PublishDate.ToString("dd MMMM yyyy"),
                Categories = string.Join("; ", b.Categories) 
            })
        };
        
        await _emailService.SendTemplatedEmail(dynamicEmailData, _emailSettings.TemplateId);
    }
}