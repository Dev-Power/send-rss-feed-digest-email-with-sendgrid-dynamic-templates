using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssFeedDigestEmailer.Cli.Configuration;
using RssFeedDigestEmailer.Cli.Services;
using RssFeedDigestEmailer.Cli.Services.Interfaces;
using SendGrid.Extensions.DependencyInjection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config =>
    {
        config
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true, false);
    })
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<IDataProvider, JsonDataProvider>();
        
        services
            .AddSendGrid(options => options.ApiKey = hostBuilderContext.Configuration["SendGridSettings:ApiKey"]);

        services
            .Configure<EmailSettings>(hostBuilderContext.Configuration.GetSection("EmailSettings"));
    })
    .Build();

var emailService = (EmailService) ActivatorUtilities.CreateInstance(host.Services, typeof(EmailService));
await emailService.SendTemplatedEmail();
