using System;
using System.Diagnostics;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class CorruptedFileFakeUserDataAccess : IUserDataAccess
    {
        public DateTime LastUpdatedUTC
        {
            get
            {
                return DateTime.UtcNow.AddDays(1);
            }
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
