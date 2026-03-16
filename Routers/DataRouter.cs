using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
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
        if (context.Request.Method == "POST" && path == "/update")
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
            // Enable buffering in case other middleware reads the body
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // reset for downstream if needed

            if (string.IsNullOrWhiteSpace(body))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("No data provided");
                return;
            }

            var requests = JsonSerializer.Deserialize<List<DataRequest>>(body);

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
            // Build full exception text with inner exceptions and stack trace
            string fullError = GetFullExceptionText(ex);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync($"Server error:\n{fullError}");
        }
    }
    private string GetFullExceptionText(Exception ex)
    {
        var sb = new StringBuilder();
        int depth = 0;

        while (ex != null)
        {
            sb.AppendLine($"Exception level {depth}: {ex.GetType().FullName}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine("StackTrace:");
            sb.AppendLine(ex.StackTrace ?? "(no stack trace)");

            ex = ex.InnerException;
            depth++;
            if (ex != null) sb.AppendLine("--- Inner Exception ---");
        }

        return sb.ToString();
    }
}

