using AvansDevOps.Entities;

namespace AvansDevOps.NotificationAdapterPattern {
    public class SmsAdapter(string phoneNumber) : INotificationAdapter {
        private readonly string _phoneNumber = phoneNumber;
        public void Notify(TeamMember teamMember, string subject, string message) {
            SmtpClient.SendEmail("notifications@devops.avans.nl", $"{_phoneNumber}@email2smskpn.nl", subject, message);
        }

        override public string ToString() {
            return $"Phone Number: {_phoneNumber}";
        }
    }
}
