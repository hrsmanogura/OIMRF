namespace OIMRF.Services.Auth
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
