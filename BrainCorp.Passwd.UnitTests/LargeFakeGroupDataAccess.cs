using System;
using System.Diagnostics;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class LargeFakeGroupDataAccess : IGroupDataAccess
    {

        private static Random randSeed = new Random();
        private static Random rand = new Random(
            randSeed.Next(99999));

        private List<Group> _list;

        public LargeFakeGroupDataAccess(ILogger log)
        {
            Log = log;
            LastUpdatedUTC = DateTime.UtcNow;

            _list = getAll();
        }

        public List<Group> GetAll()
        {
            return _list;
        }

        private List<Group> getAll()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Group> list = new List<Group>();

            // Make everyone apart of at least 1 group.
            List<string> membersMain = new List<string>();
            for (int i = 0; i < 65000; i++)
            {
                membersMain.Add(i.ToString());
            }
            list.Add(new Group("main", 0, membersMain));

            // Then make rand.next(1000) users part of 
            // 1000 groups just to distribute.
            for (int i = 1; i < 1000; i++)
            {
                string name = "group" + i;
                int id = i;

                List<string> members = new List<string>();

                for (int j = 0; j < 10000; j++)
                {
                    members.Add(rand.Next(65000).ToString());
                }

                list.Add(new Group(name, (uint)id, members));
            }

            stopwatch.Stop();

            Log.Write("TimeToFakeGroupData: " + stopwatch.ElapsedMilliseconds, LogEntrySeverityEnum.Debug);

            return list;
        }

        public ILogger Log { get; set; }

        public DateTime LastUpdatedUTC { get; set; }
    }
}
