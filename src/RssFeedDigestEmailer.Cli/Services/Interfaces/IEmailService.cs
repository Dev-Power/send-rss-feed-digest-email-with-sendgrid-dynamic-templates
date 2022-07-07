namespace RssFeedDigestEmailer.Cli.Services.Interfaces;

public interface IEmailService
{
    Task SendTemplatedEmail();
}