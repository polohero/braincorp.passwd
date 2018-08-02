using System;
using System.Diagnostics;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class LargeFakeUserDataAccess : IUserDataAccess
    {
        public LargeFakeUserDataAccess(ILogger log)
        {
            Log = log;
            LastUpdatedUTC = DateTime.UtcNow;
            _list = getAll();
        }

        private List<User> _list;

        public ILogger Log { get; set; }

        public List<User> GetAll()
        {
            return _list;
        }

        private List<User> getAll()
        {
            List<User> users = new List<User>();

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 65000; i++)
            {
                users.Add(new User(
                    i.ToString(),
                    (uint)i,
                    roundTo(i, 1000), // Assuming there will be about 65 different groups
                    i.ToString(), // Assuming there will be about 65000 different comments
                    i.ToString(), // Everyone would probably have their own home.
                    roundTo(i, 10000).ToString() // Probably only a couple different shells.
                    ));
            }

            stopwatch.Stop();

            Log.Write("TimeToFakeData: " + stopwatch.ElapsedMilliseconds, LogEntrySeverityEnum.Debug);

            return users;
        }

        private uint roundTo(int valueToRound, int nearest)
        {
            return (uint)(Math.Round((double)valueToRound / (double)(int.Parse(nearest.ToString())), 0) * (long)nearest);
        }

        public DateTime LastUpdatedUTC { get; set; }

    }
}
