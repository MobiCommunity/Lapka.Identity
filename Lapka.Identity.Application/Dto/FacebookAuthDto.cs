namespace Lapka.Identity.Application.Dto
{
    public class FacebookAuthDto
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string[] ErrorMessage { get; set; }
    }
}