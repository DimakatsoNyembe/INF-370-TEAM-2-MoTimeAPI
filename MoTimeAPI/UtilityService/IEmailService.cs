using MoTimeAPI.Helpers;

namespace MoTimeAPI.UtilityService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
