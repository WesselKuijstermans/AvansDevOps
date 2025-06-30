using AvansDevOps.Entities;

namespace AvansDevOps.AdapterPattern {
    public class SlackAdapter(string slackToken) : INotificationAdapter {
        private readonly string _slackToken = slackToken;
        public void Notify(TeamMember teamMember, string subject, string message) {
            SlackWebhooks.SendMessage("xoxb-notificationbot-slacktoken", _slackToken, message);
        }

        override public string ToString() {
            return $"Slack Token: {_slackToken}";
        }
    }
}
