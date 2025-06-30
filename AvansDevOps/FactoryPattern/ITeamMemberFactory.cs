using AvansDevOps.Entities;
using AvansDevOps.NotificationAdapterPattern;

namespace AvansDevOps.FactoryPattern {
    public interface ITeamMemberFactory {
        TeamMember CreateTeamMember(string name, INotificationAdapter notificationAdapter);
    }
}
