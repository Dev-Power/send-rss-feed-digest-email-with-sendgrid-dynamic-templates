using Microsoft.Extensions.Options;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RssFeedDigestEmailer.Cli.Services;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings, ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient;
        _emailSettings = emailSettings.Value;
    }
    
    public async Task SendTemplatedEmail(object dynamicTemplateData, string templateId)
    {
        var from = new EmailAddress(_emailSettings.SenderEmailAddress, _emailSettings.SenderDisplayName);
        var to = new EmailAddress(_emailSettings.RecipientEmailAddress, _emailSettings.RecipientDisplayName);
        
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData);
        var response = await _sendGridClient.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Email has been sent successfully");
        }
    }
}