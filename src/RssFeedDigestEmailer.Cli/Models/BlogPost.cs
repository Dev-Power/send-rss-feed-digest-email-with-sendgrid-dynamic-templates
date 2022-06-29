namespace RssFeedDigestEmailer.Cli.Models;

public class BlogPost
{
    public string Title { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public List<string> Categories { get; set; } = new List<string>();
    public string Author { get; set; }
    public string HeaderImageUrl { get; set; }
    public DateTime PublishDate { get; set; }
}
