namespace RssFeedDigestEmailer.Cli.Configuration;

public class EmailSettings
{
    public string SenderEmailAddress { get; set; }
    public string SenderDisplayName { get; set; }
    public string RecipientEmailAddress { get; set; }
    public string RecipientDisplayName { get; set; }
    public string TemplateId { get; set; }
}