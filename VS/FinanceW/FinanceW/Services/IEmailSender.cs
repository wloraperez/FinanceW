using System.Threading.Tasks;

namespace FinanceW.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
