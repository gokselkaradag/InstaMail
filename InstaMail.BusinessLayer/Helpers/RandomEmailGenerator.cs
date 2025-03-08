namespace InstaMail.BusinessLayer.Helpers;

public class RandomEmailGenerator
{
    private static readonly Random _random = new Random();
    private const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

    public static string Generate(int length = 10, string domain = "instamail.gokselkaradag.com.tr")
    {
        var username = new char[length];
        for (int i = 0; i < length; i++)
        {
            username[i] = chars[_random.Next(chars.Length)];
        }

        return $"{new string(username)}@{domain}";
    }
}