using Spectre.Console;

namespace AvansDevOps.NotificationAdapterPattern {
    public static class SmtpClient {

        public static void SendEmail(string sender, string recipient, string subject, string body) {
            AnsiConsole.WriteLine($"Sender {sender} sent email to {recipient}: \nSubject: {subject}\nMessage: {body}");
        }
    }
}
