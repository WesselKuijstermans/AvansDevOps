using AvansDevOps.AdapterPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class ScrumMaster(string name, INotificationAdapter notificationAdapter) : TeamMember(name, notificationAdapter)
    {
    }
}
