using AvansDevOps.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    internal class Tester: TeamMember
    {
        public Tester(string name, INotificationAdapter notificationAdapter) : base(name, notificationAdapter)
        {
        }
    }
}
