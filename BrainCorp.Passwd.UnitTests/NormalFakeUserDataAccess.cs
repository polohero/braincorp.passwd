using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class NormalFakeUserDataAccess : IUserDataAccess
    {
        public NormalFakeUserDataAccess(ILogger log)
        {
            Log = log;
            LastUpdatedUTC = DateTime.UtcNow;

            UserList = getList();
        }

        public List<User> GetAll()
        {
            return new List<User>(UserList);
        }

        private List<User> getList()
        {
            List<User> list = new List<User>();

            list.Add(new User("root", 0, 0, "rootuser", "/home", "/shell"));
            list.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));
            list.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));
            list.Add(new User("myuser2", 3, 1, "myusers", "/home/myuser2", "/ksh/shell"));
            list.Add(new User("myuser3", 4, 1, "myusers", "/home/myuser3", "/ksh/shell"));
            list.Add(new User("myuser4", 5, 1, "myusers", "/home/myuser4", "/ksh/shell"));
            list.Add(new User("admin1", 6, 2, "admins", "/home/admin1", "/ksh/shell"));
            list.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));
            list.Add(new User("admin3", 8, 2, "admins", "/home/admin3", "/bash/shell"));
            list.Add(new User("admin4", 9, 2, "admins", "/home/admin4", "/ksh/shell"));
            list.Add(new User("root2", 10, 0, "rootuser", "/home", "/shell"));
            list.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            list.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            list.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));
            list.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            return list;
        }

        public List<User> UserList { get; set; }

        public ILogger Log { get; set; }

        public DateTime LastUpdatedUTC { get; set; }
    }
}
