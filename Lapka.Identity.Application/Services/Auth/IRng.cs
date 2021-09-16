namespace Lapka.Identity.Application.Services.Auth
{
    public interface IRng
    {
        public string Generate(int length = 50, bool removeSpecialChars = false);
    }
}