using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssFeedDigestEmailer.Cli;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services;
using RssFeedDigestEmailer.Cli.Services.Interfaces;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilderContext, services) => services
        .AddTransient<IRssService, RssService>()
        .AddTransient<IEmailService, EmailService>()
        .AddTransient<IHtmlService, HtmlService>()
        .Configure<SendGridSettings>(hostBuilderContext.Configuration.GetSection("SendGridSettings"))
        .Configure<EmailSettings>(hostBuilderContext.Configuration.GetSection("EmailSettings"))
        .Configure<RssSettings>(hostBuilderContext.Configuration.GetSection("RssSettings"))
    )
    .Build();

var consoleMailer = (ConsoleMailer) ActivatorUtilities.CreateInstance(host.Services, typeof(ConsoleMailer));
await consoleMailer.Send();
