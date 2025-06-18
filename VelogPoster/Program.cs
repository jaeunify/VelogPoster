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
        
        var inputWaiter = new InputWaiter();
        var (title, content) = await inputWaiter.GetInputAsync();
        await VelogPoster.PostAsync(email, password, title, content);
    }
}