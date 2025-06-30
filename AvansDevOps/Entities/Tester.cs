using AvansDevOps.NotificationAdapterPattern;

namespace AvansDevOps.Entities {
    public class Tester(string name, INotificationAdapter notificationAdapter) : TeamMember(name, notificationAdapter) {
    }
}
