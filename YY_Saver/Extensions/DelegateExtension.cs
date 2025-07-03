namespace YY_Saver.Extensions;

public static class DelegateExtension
{
    public static void SafetyCall(Action action)
    {
        try
        {
            action();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Operation cancellation requested.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error:\n {e.Message}");
        }
    }

    public static async Task SafetyCall(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Operation cancellation requested.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error:\n {e.Message}");
        }
    }
}
