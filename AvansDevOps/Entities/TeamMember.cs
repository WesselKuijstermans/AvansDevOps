using AvansDevOps.AdapterPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public abstract class TeamMember(string name, INotificationAdapter noticationChannel)
    {
        private readonly string name = name;
        private readonly INotificationAdapter notificationChannel = noticationChannel;

        public string GetName()
        {
            return this.name;
        }

        public string getContactInfo()
        {
            // return string field from notificationadapter
            return notificationChannel.ToString();
        }

        public void Notify(string subject, string message)
        {
            this.notificationChannel.Notify(this, subject, message);
        }

        public override string ToString()
        {
            return $"{this.name}, {this.notificationChannel.ToString()}";
        }
    }
}
