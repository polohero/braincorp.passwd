using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.DataAccess
{
    public interface IUserDataAccess
    {
        List<User> GetAll();
       
        DateTime LastUpdatedUTC { get; }
    }
}
