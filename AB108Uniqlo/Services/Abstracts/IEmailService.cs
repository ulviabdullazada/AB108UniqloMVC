namespace AB108Uniqlo.Services.Abstracts
{
    public interface IEmailService
    {
        void SendEmailConfirmation(string reciever, string name, string token);
    }
}
