namespace Lapka.Identity.Application.Services.User
{
    public interface IRng
    {
        public string Generate(int length = 50, bool removeSpecialChars = false);
    }
}