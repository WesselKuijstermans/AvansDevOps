using AvansDevOps.Entities;

namespace AvansDevOps.NotificationAdapterPattern {
    public interface INotificationAdapter {
        void Notify(TeamMember teamMember, string subject, string message);
        string ToString();
    }
}
