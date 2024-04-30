namespace PanoramaApi.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstCharacter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Input string is empty.", nameof(str));
            }

            return $"{str.Substring(0, 1).ToUpper()}{str.Substring(1)}";
        }

        public static string GetOnlyDigits(this string str)
        {
            return string.Concat(str.Where(char.IsDigit));
        }
    }
}
