using AvansDevOps.Entities;

namespace AvansDevOps.NotificationAdapterPattern {
    public class EmailAdapter(string email) : INotificationAdapter {
        private readonly string _email = email;

        public void Notify(TeamMember teamMember, string subject, string message) {
            SmtpClient.SendEmail("notifications@devops.avans.nl", _email, subject, message);
        }

        override public string ToString() {
            return $"Email: {_email}";
        }
    }
}
