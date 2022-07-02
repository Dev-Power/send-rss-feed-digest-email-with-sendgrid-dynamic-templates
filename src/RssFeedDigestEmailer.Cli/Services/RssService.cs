using System.Xml;
using Microsoft.Extensions.Options;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Models;
using RssFeedDigestEmailer.Cli.Services.Interfaces;

namespace RssFeedDigestEmailer.Cli.Services;

public class RssService : IRssService
{
    private readonly IHtmlService _htmlService;
    private readonly HttpClient _httpClient;
    private readonly RssSettings _rssSettings;

    public RssService(IHtmlService htmlService, HttpClient httpClient, IOptions<RssSettings> rssSettings)
    {
        _htmlService = htmlService;
        _httpClient = httpClient;
        _rssSettings = rssSettings.Value;
    }
    
    public async Task<BlogInfo> GetBlogInfo()
    {
        using (_httpClient)
        using (HttpResponseMessage response = await _httpClient.GetAsync(_rssSettings.FeedUrl))
        using (var rawRssFeedStream = await response.Content.ReadAsStreamAsync())
        { 
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(rawRssFeedStream);
            
            var blogInfo = new BlogInfo();
            
            XmlNode lastBuildDateNode = xmlDocument.SelectSingleNode("/rss/channel/lastBuildDate");
            blogInfo.LastBuildDate = DateTime.Parse(lastBuildDateNode.InnerText);
            
            XmlNodeList itemNodeList = xmlDocument.SelectNodes("/rss/channel/item");

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            
            for (int i = 0; i < itemNodeList.Count; i++)
            {
                XmlNode titleNode = itemNodeList[i].SelectSingleNode("title");
                XmlNode linkNode = itemNodeList[i].SelectSingleNode("link");
                XmlNodeList categoryNodes = itemNodeList[i].SelectNodes("category");
                XmlNode descriptionNode = itemNodeList[i].SelectSingleNode("description");
                XmlNode authorNode = itemNodeList[i].SelectSingleNode("dc:creator", xmlNamespaceManager);
                
                var headerImageUrlAndPublishDate = await _htmlService.GetHeaderImageUrlAndPostDate(linkNode.InnerText);
                
                var blogPost = new BlogPost
                {
                    Title = titleNode.InnerText,
                    Link = linkNode.InnerText,
                    Categories = categoryNodes.Cast<XmlNode>().Select(node => node.InnerText).ToList(),
                    Author = authorNode.InnerText,
                    HeaderImageUrl = headerImageUrlAndPublishDate.Item1,
                    Description = await _htmlService.GetPostIntroduction(descriptionNode.InnerText),
                    PublishDate = DateTime.Parse(headerImageUrlAndPublishDate.Item2)
                };
                
                blogInfo.BlogPosts.Add(blogPost);
            }

            return blogInfo;   
        }
    }
}