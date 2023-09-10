namespace SilentMike.DietMenu.Proxy.WebApi.Middlewares;

using SilentMike.DietMenu.Proxy.WebApi.Services;

internal static class ProxyPipelineIdentityTokenMiddleware
{
    private const string LOGOUT_PATH = "/logout";
    private const string UI_CLUSTER_ID = "UiCluster";

    public static void UseProxyPipelineIdentityTokenMiddleware(this IReverseProxyApplicationBuilder pipeline)
    {
        var tokenService = pipeline.ApplicationServices.GetRequiredService<TokenService>();

        pipeline.Use((context, next) =>
        {
            lock (context.Session.Id)
            {
                context.Session.SetString("sessionId", context.Session.Id);

                var proxyFeature = context.GetReverseProxyFeature();

                var isValidateTokenRequired = !string.IsNullOrEmpty(proxyFeature.Route.Config.AuthorizationPolicy)
                                              && !proxyFeature.Route.Config.AuthorizationPolicy.Equals("Anonymous", StringComparison.InvariantCultureIgnoreCase);

                if (!isValidateTokenRequired)
                {
                    return next();
                }

                var result = tokenService.Handle(context).Result;

                if (result)
                {
                    return next();
                }

                if (UI_CLUSTER_ID.Equals(proxyFeature.Cluster.Config.ClusterId))
                {
                    context.Response.Redirect(LOGOUT_PATH);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }

                return Task.CompletedTask;
            }
        });
    }
}
