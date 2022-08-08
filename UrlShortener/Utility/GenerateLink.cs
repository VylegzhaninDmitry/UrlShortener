namespace UrlShortener.Utility
{
    public class GenerateLink
    {
        public string GenerateShortLink()
        {
            var shortLink = RandomString(6);
            return shortLink;
        }

        private string RandomString(int length)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
