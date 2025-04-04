using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.FormMessageObersverPattern
{
    internal interface IFormMessageObserver
    {
        void FormUpdate(string formMessage, List<TeamMember> membersToNotify);
    }
}
