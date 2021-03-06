using Microsoft.Extensions.Options;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RssFeedDigestEmailer.Cli.Services;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly IDataProvider _dataProvider;
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings, ISendGridClient sendGridClient, IDataProvider dataProvider)
    {
        _sendGridClient = sendGridClient;
        _dataProvider = dataProvider;
        _emailSettings = emailSettings.Value;
    }
    
    public async Task SendTemplatedEmail()
    {
        var dynamicEmailData = await _dataProvider.GetEmailData();
        
        var from = new EmailAddress(_emailSettings.SenderEmailAddress, _emailSettings.SenderDisplayName);
        var to = new EmailAddress(_emailSettings.RecipientEmailAddress, _emailSettings.RecipientDisplayName);
        
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, _emailSettings.TemplateId, dynamicEmailData);
        var response = await _sendGridClient.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Email has been sent successfully");
        }
    }
}