using Microsoft.Playwright;
namespace VelogPoster;

class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("❗ 이메일과 비밀번호를 인자로 전달해주세요.");
            Console.WriteLine("사용법: dotnet run VelogPoster <이메일> <비밀번호>");
            return;
        }

        string email = args[0];
        string password = args[1];
        
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false // 디버깅 중엔 false
        });

        var page = await browser.NewPageAsync();

        // 1. Velog 로그인
        await page.GotoAsync("https://velog.io");
        await page.SelectAndClickAsync("로그인", 1);
        await page.ClickAsync("a[href*='auth/v3/social/redirect/google']");
        await page.FillAsync("input[name=identifier]", email);
        await page.Keyboard.PressAsync("Enter");
    }
}