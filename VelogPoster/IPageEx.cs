using Microsoft.Playwright;

namespace VelogPoster;

public static class IPageEx
{
    public static async Task SelectAndClickAsync(this IPage page, string buttonText, int? idx = null)
    {
        var selector = $"button:has-text('{buttonText}')";
        
        // 버튼이 로드될 때까지 명시적으로 대기
        await page.WaitForSelectorAsync(selector);
        
        // "로그인" 텍스트가 있는 모든 버튼 찾기
        var buttons = page.Locator(selector);

        // 총 몇 개 버튼인지 세기
        int count = await buttons.CountAsync();
        if (count == 0)
        {
            throw new Exception($"{buttonText} 텍스트를 가진 버튼이 없습니다.");
        }

        // 버튼이 하나 뿐이면 클릭하고 종료
        if (count == 1)
        {
            await buttons.Nth(0).ClickAsync();
            return;
        }

        if (idx.HasValue)
        {
            await buttons.Nth(idx.Value).ClickAsync();
            return;
        }
        
        Console.WriteLine($"[총 {count}개] '로그인' 텍스트를 가진 버튼이 있습니다.");

        // 각 버튼의 정보 출력
        for (int i = 0; i < count; i++)
        {
            var outerHtml = await buttons.Nth(i).EvaluateAsync<string>("el => el.outerHTML");
            Console.WriteLine($"[{i}] ▶ {outerHtml}");
        }

        // 사용자로부터 입력 받기
        Console.Write("몇 번째 버튼을 클릭할까요? (0부터 시작): ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index >= 0 && index < count)
        {
            await buttons.Nth(index).ClickAsync();
            Console.WriteLine($"{index}번 버튼 클릭 완료");
        }
        else
        {
            Console.WriteLine("❌ 잘못된 인덱스입니다.");
        }
    }
}