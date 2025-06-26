using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.AdapterPattern
{
    public class SlackAdapter(string slackToken): INotificationAdapter
    {
        private readonly string _slackToken = slackToken;
        private readonly SlackWebhooks _slackWebhooks = new();
        public void Notify(TeamMember teamMember, string subject, string message)
        {
            _slackWebhooks.SendMessage("xoxb-notificationbot-slacktoken", _slackToken, message);
        }

        override public string ToString()
        {
            return $"Slack Token: {_slackToken}";
        }
    }
}
