using System;

public static class StringUtils
{
    public static string GetConvertedPriceString(int value)
    {
        string price = value.ToString();

        if (value >= 1000000000)
        {
            price = Math.Round(value / 1000000000.0f, 1).ToString() + "B";
        }
        else if (value >= 1000000)
        {
            price = Math.Round(value / 1000000.0f, 1).ToString() + "M";
        }
        else if (value >= 1000)
        {
            price = Math.Round(value / 1000.0f, 1).ToString() + "K";
        }

        return price;
    }
}
