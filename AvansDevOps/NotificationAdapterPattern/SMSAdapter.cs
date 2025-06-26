using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.AdapterPattern
{
    public class SMSAdapter(string phoneNumber): INotificationAdapter
    {
        private readonly string _phoneNumber = phoneNumber;
        private readonly SmtpClient _smtpClient = new();
        public void Notify(TeamMember teamMember, string subject, string message)
        {
            _smtpClient.SendEmail("notifications@devops.avans.nl", $"{_phoneNumber}@email2smskpn.nl", subject, message);
        }

        override public string ToString()
        {
            return $"Phone Number: {_phoneNumber}";
        }
    }
}
