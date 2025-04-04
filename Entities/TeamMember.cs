using AvansDevOps.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public abstract class TeamMember
    {
        private string name;
        private INotificationAdapter notificationChannel;

        public TeamMember(string name, INotificationAdapter noticationChannel)
        {
            this.name = name;
            this.notificationChannel = noticationChannel;
        }

        public string GetName()
        {
            return this.name;
        }

        public void Notify(string message)
        {
            this.notificationChannel.notify(message, this);
        }
    }
}
