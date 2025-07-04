namespace YY_Saver.Extensions;

public static class CharExtentions
{
    public static string ConcatToString(this IEnumerable<char> chars)
    {
        return new string(chars.ToArray());
    }

    public static IEnumerable<char> ReplaceAllExceptLettersAndDigits(this IEnumerable<char> chars, char to)
    {
        return chars.Select(symbol => !char.IsLetterOrDigit(symbol) ? to : symbol);
    }
}
