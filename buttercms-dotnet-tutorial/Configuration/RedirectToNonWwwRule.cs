using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace buttercms_dotnet_tutorial.Configuration
{
    public class RedirectToNonWwwRule : IRule
    {
        public virtual void ApplyRule(RewriteContext context)
        {
            var req = context.HttpContext.Request;
            if (req.Host.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            if (!req.Host.Value.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var wwwHost = new HostString($"{req.Host.Value.Replace("www.", string.Empty)}");
            var newUrl = UriHelper.BuildAbsolute(req.Scheme, wwwHost, req.PathBase, req.Path, req.QueryString);
            var response = context.HttpContext.Response;
            response.StatusCode = 301;
            response.Headers[HeaderNames.Location] = newUrl;
            context.Result = RuleResult.EndResponse;
        }
    }

    public static class RewriteOptionsExtensions
    {
        public static RewriteOptions AddRedirectToNonWww(this RewriteOptions options)
        {
            options.Rules.Add(new RedirectToNonWwwRule());
            return options;
        }
    }
}
