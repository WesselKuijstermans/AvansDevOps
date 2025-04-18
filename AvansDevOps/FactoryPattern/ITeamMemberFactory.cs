﻿using AvansDevOps.Adapter;
using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.FactoryPattern
{
    public interface ITeamMemberFactory
    {
        TeamMember CreateTeamMember(string name, INotificationAdapter notificationAdapter);
    }
}
