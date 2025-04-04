using AvansDevOps.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class LeadDeveloper: TeamMember
    {
        public LeadDeveloper(string name, INotificationAdapter noticationChannel) : base(name, noticationChannel)
        {

        }
    }
}
