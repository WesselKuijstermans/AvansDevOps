using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.AdapterPattern
{
    public class EmailAdapter(string email) : INotificationAdapter
    {
        private readonly string _email = email;
        private readonly SmtpClient _smtpClient = new();

        public void Notify(TeamMember teamMember, string subject, string message)
        {
            _smtpClient.SendEmail("notifications@devops.avans.nl", _email, subject, message);
        }

        override public string ToString()
        {
            return $"Email: {_email}";
        }
    }
}
