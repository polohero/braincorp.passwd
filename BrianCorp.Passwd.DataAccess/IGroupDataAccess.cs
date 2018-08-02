using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.DataAccess
{
    public interface IGroupDataAccess
    {
        List<Group> GetAll();

        DateTime LastUpdatedUTC { get; }
    }
}
