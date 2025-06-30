using AvansDevOps.NotificationAdapterPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class LeadDeveloper(string name, INotificationAdapter noticationChannel) : TeamMember(name, noticationChannel)
    {
    }
}
