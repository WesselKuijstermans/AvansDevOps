using AvansDevOps.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class ScrumMaster: TeamMember
    {
        public ScrumMaster(string name, INotificationAdapter notificationAdapter) : base(name, notificationAdapter)
        {
        }
    }
}
