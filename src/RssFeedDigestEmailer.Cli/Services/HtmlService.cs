using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using RssFeedDigestEmailer.Cli.Services.Interfaces;

namespace RssFeedDigestEmailer.Cli.Services;

public class HtmlService : IHtmlService
{
    public async Task<(string, string)> GetHeaderImageUrlAndPostDate(string blogPostUrl)
    {
        var config = AngleSharp.Configuration.Default.WithDefaultLoader();
        var address = blogPostUrl;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(address);
        var cellSelector = "#header_image > img";
        var cell = document.QuerySelector(cellSelector);
        var headerImgSrc = cell.Attributes.GetNamedItem("src");

        var publishDateCellSelector = "body > main > section > ul > article > header > div > div.article-authors > span";
        var publishDateCell = document.QuerySelector(publishDateCellSelector);
        
        return (headerImgSrc?.Value, publishDateCell?.InnerHtml);
    }

    public async Task<string> GetPostIntroduction(string rawPostHtml)
    {
        var parser = new HtmlParser();
        var document = parser.ParseDocument(rawPostHtml);
        
        var cellSelector = "div:nth-child(1) > p:nth-child(1)";
        var cell = document.QuerySelector(cellSelector);
        
        return cell?.InnerHtml;
    }
}