﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Adapter
{
    public class SMSAdapter: INotificationAdapter
    {
        public void notify(string message, Entities.TeamMember teamMember)
        {
            Console.WriteLine("Sending SMS to " + teamMember.GetName() + " with message: " + message);
        }
    }
}
