using AvansDevOps.AdapterPattern;
using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.FactoryPattern
{
    public class LeadDeveloperFactory: ITeamMemberFactory
    {
        public TeamMember CreateTeamMember(string name, INotificationAdapter notificationAdapter)
        {
            return new LeadDeveloper(name, notificationAdapter);
        }
    }
}
