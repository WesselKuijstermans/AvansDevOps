﻿using AvansDevOps.Entities;
using AvansDevOps.NotificationAdapterPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.FactoryPattern
{
    public class ScrumMasterFactory: ITeamMemberFactory
    {
        public TeamMember CreateTeamMember(string name, INotificationAdapter notificationAdapter)
        {
            return new ScrumMaster(name, notificationAdapter);
        }
    }
}
