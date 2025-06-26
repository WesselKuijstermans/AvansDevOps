using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.AdapterPattern
{
    public interface INotificationAdapter
    {
        void Notify(TeamMember teamMember, string subject, string message);
        string ToString();
    }
}
