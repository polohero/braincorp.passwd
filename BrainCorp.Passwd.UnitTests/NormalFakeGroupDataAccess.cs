using System;
using System.Diagnostics;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class NormalFakeGroupDataAccess : IGroupDataAccess
    {
        public NormalFakeGroupDataAccess(ILogger log)
        {
            Log = log;
            LastUpdatedUTC = DateTime.UtcNow;
            GroupList = getList();
        }

        public List<Group> GetAll()
        {
            return new List<Group>(GroupList);
        }

        private List<Group> getList()
        {
            List<Group> list = new List<Group>();

            list.Add(new Group("rootuser", 0, new string[] { "root", "root2" }));
            list.Add(new Group("myusers", 1, new string[] { "cwixom", "myuser1", "myuser3", "myuser4" }));
            list.Add(new Group("admins", 2, new string[] { "admin1", "admin2", "admin3", "admin4" }));
            list.Add(new Group("gid1", 3, new string[] { "gidded1", "gidded2" }));
            list.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));
            list.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));

            return list;
        }



        private uint roundTo(int valueToRound, int nearest)
        {
            return (uint)(Math.Round((double)valueToRound / (double)(int.Parse(nearest.ToString())), 0) * (long)nearest);
        }

        public List<Group> GroupList { get; set; }

        public ILogger Log { get; set; }

        public DateTime LastUpdatedUTC { get; set; }
    }
}
