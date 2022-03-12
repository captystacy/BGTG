namespace POS.Infrastructure.Helpers;

public static class TemplateHelper
{
    private const char PlusChar = '+';
    private const char MinusChar = '-';

    public static char GetPlusOrMinus(bool condition)
    {
        return condition ? PlusChar : MinusChar;
    }
}