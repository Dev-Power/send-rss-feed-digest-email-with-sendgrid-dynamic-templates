namespace RssFeedDigestEmailer.Cli.Models;

public class BlogInfo
{
    public DateTime LastBuildDate { get; set; }
    public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}