
using RssFeedDigestEmailer.Cli.Models;

namespace RssFeedDigestEmailer.Cli.Services.Interfaces;

public interface IRssService
{
    Task<BlogInfo> GetBlogInfo();
}