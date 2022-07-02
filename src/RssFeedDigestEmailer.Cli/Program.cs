using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssFeedDigestEmailer.Cli;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services;
using RssFeedDigestEmailer.Cli.Services.Interfaces;
using SendGrid.Extensions.DependencyInjection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config =>
    {
        config.AddUserSecrets(Assembly.GetExecutingAssembly(), true, false);
    })
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services
            .AddTransient<IRssService, RssService>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<IHtmlService, HtmlService>()
            .AddSingleton<HttpClient>();
        
        services
            .AddSendGrid(options => options.ApiKey = hostBuilderContext.Configuration["SendGridSettings:ApiKey"]);
        
        services   
            .Configure<EmailSettings>(hostBuilderContext.Configuration.GetSection("EmailSettings"))
            .Configure<RssSettings>(hostBuilderContext.Configuration.GetSection("RssSettings"));
    })
    .Build();

var consoleMailer = (ConsoleMailer) ActivatorUtilities.CreateInstance(host.Services, typeof(ConsoleMailer));
await consoleMailer.Send();
