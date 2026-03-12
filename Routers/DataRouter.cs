using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using Services;

public class DataRouter
{
    private readonly DataService _service = new DataService();

    public async Task RouteAsync(HttpContext context)
    {
        string path = context.Request.Path.Value?.ToLower() ?? "";
        if (context.Request.Method == "POST" && path == "/insert-data")
        {
            await HandleInsertData(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Not Found");
    }

    private async Task HandleInsertData(HttpContext context)
    {
        try
        {
            var requests = await JsonSerializer.DeserializeAsync<List<DataRequest>>(context.Request.Body);

            if (requests == null || requests.Count == 0)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("No data provided");
                return;
            }

            DateTime now = DateTime.UtcNow;
            requests.ForEach(r => r.ReceivedAt = now);

            var (success, message) = await _service.UpsertBoardgamesAsync(requests);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = success ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success,
                message,
                count = requests.Count
            }));
        }
        catch (JsonException jex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"Invalid JSON: {jex.Message}");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync($"Server error: {ex.Message}");
        }
    }
}