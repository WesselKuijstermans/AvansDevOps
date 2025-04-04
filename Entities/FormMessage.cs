using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    internal class FormMessage
    {
        private TeamMember sender;
        private string message;

        public FormMessage(TeamMember sender, string message)
        {
            this.sender = sender;
            this.message = message;
        }

        public TeamMember GetSender()
        {
            return this.sender;
        }

        public string GetMessage()
        {
            return this.message;
        }

        public void SetSender(TeamMember sender)
        {
            this.sender = sender;
        }

        public void SetMessage(string message)
        {
            this.message = message;
        }
    }
}
