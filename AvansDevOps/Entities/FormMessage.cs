using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class FormMessage(TeamMember sender, string message)
    {
        private TeamMember sender = sender;
        private string message = message;

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
