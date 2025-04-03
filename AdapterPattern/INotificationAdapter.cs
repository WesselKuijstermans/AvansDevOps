using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Adapter
{
    internal interface INotificationAdapter
    {
        void notify(string message, TeamMember teamMember);
    }
}
