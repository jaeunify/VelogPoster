using Microsoft.Playwright;

namespace VelogPoster;

public static class VelogPoster
{
    public static async Task PostAsync(string email, string password, string title, string content)
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false // 디버깅 중엔 false
        });

        var page = await browser.NewPageAsync();

// 1. Velog 로그인
        await page.GotoAsync("https://velog.io");
        await page.SelectAndClickAsync("로그인", 1);
        await page.Locator("a[href*='v2.velog.io/api/v2/auth/social/redirect/github']").ClickAsync();
        await page.FillAsync("input[name='login']", email);
        await page.FillAsync("input[name='password']", password);
        await page.ClickAsync("input[type='submit']");
        await page.WaitForURLAsync("https://velog.io/");

// 2. 글쓰기 페이지 이동
        await page.GotoAsync("https://velog.io/write");

// 3. 제목 + 본문 작성
        await page.Locator("textarea[placeholder='제목을 입력하세요']").FillAsync(title);
        await page.Keyboard.PressAsync("Tab"); // CodeMirror로 포커스 이동
        await page.Keyboard.TypeAsync(content);

// 4. 임시저장 하기
        await page.ClickAsync("text=임시저장");
        Console.WriteLine("Velog에 포스트 임시저장 완료!");
        await page.WaitForTimeoutAsync(2000); // 2초 대기
        await browser.CloseAsync();
    }
}