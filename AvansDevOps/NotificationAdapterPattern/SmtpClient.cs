using Spectre.Console;

namespace AvansDevOps.AdapterPattern {
    public class SmtpClient {

        public static void SendEmail(string sender, string recipient, string subject, string body) {
            AnsiConsole.WriteLine($"Sender {sender} sent email to {recipient}: \nSubject: {subject}\nMessage: {body}");
        }
    }
}
