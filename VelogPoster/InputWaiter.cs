using System.Net;
using System.Text;
using System.Text.Json;

namespace VelogPoster;

public class InputWaiter
{
    public async Task<(string Title, string Content)> GetInputAsync()
    {
        const int port = 51205;

        using var listener = new HttpListener();
        listener.Prefixes.Add($"http://+:{port}/");
        listener.Start();

        Console.WriteLine($"📡 HTTP 입력 대기 중... http://+:{port}/");
        Console.WriteLine("POST / with JSON body: { \"title\": \"...\", \"content\": \"...\" }");

        var context = await listener.GetContextAsync();
        var request = context.Request;

        if (request.HttpMethod != "POST")
        {
            context.Response.StatusCode = 405;
            await context.Response.OutputStream.DisposeAsync();
            throw new InvalidOperationException("POST 요청만 지원됩니다.");
        }

        using var reader = new StreamReader(request.InputStream);
        var body = await reader.ReadToEndAsync();

        var input = JsonSerializer.Deserialize<PostInput>(body);
        if (input == null || string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Content))
        {
            context.Response.StatusCode = 400;
            await context.Response.OutputStream.DisposeAsync();
            throw new InvalidOperationException("유효하지 않은 JSON 본문입니다.");
        }

        context.Response.StatusCode = 200;
        var responseMsg = Encoding.UTF8.GetBytes("입력 성공");
        await context.Response.OutputStream.WriteAsync(responseMsg, 0, responseMsg.Length);
        await context.Response.OutputStream.DisposeAsync();

        return (input.Title, input.Content);
    }

    private class PostInput
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}