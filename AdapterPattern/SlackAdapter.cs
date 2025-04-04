using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Adapter
{
    public class SlackAdapter: INotificationAdapter
    {
        public void notify(string message, Entities.TeamMember teamMember)
        {
            Console.WriteLine("Sending slack message to " + teamMember.GetName() + " with message: " + message);
        }    
    }
}
