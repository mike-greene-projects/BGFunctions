using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BggCfg;

public class Function : IHttpFunction
{
    private readonly DataRouter _router = new DataRouter();

    public Task HandleAsync(HttpContext context)
    {
        return _router.RouteAsync(context);
    }
}