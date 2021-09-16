namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class FacebookAuthSettings
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string TokenValidationUrl { get; set; }
        public string UserInfoUrl { get; set; }
    }
}