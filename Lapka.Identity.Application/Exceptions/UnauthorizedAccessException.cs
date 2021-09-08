namespace Lapka.Identity.Application.Exceptions
{
    public class UnauthorizedAccessException : AppException
    {
        public UnauthorizedAccessException() : base("Unauthorized access")
        {
        }

        public override string Code => "unauthorized_access";
    }
}