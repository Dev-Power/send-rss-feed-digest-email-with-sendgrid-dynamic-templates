namespace RssFeedDigestEmailer.Cli.Services.Interfaces;

public interface IEmailService
{
    Task SendTemplatedEmail(object dynamicTemplateData, string templateId);
}