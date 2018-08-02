using System;
using System.Diagnostics;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class MissingFileFakeUserDataAccess : IUserDataAccess
    {
        public DateTime LastUpdatedUTC
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
