using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Adapter
{
    public class EmailAdapter : INotificationAdapter
    {
        public void notify(string message, TeamMember teamMember)
        {
            Console.WriteLine("Sending email to " + teamMember.GetName() + " with message: " + message);
        }
    }
}
