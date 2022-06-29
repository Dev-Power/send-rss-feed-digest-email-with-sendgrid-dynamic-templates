using Microsoft.Extensions.Options;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RssFeedDigestEmailer.Cli.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly SendGridSettings _sendGridSettings;

    public EmailService(IOptions<EmailSettings> emailSettings, IOptions<SendGridSettings> sendGridSettings)
    {
        _emailSettings = emailSettings.Value;
        _sendGridSettings = sendGridSettings.Value;
    }
    
    public async Task SendTemplatedEmail(object dynamicTemplateData, string templateId)
    {
        var client = new SendGridClient(_sendGridSettings.ApiKey);
        var from = new EmailAddress(_emailSettings.SenderEmailAddress, _emailSettings.SenderDisplayName);
        var to = new EmailAddress(_emailSettings.RecipientEmailAddress, _emailSettings.RecipientDisplayName);
        
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
        var response = await client.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Email has been sent successfully");
        }
    }
}