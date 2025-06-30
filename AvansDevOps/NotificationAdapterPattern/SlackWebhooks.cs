using Spectre.Console;

namespace AvansDevOps.AdapterPattern {
    public class SlackWebhooks {
        public static void SendMessage(string sender, string recipient, string message) {
            AnsiConsole.WriteLine($"Sender {sender} sent Slack message to {recipient}:\n{message}");
        }
    }
}
