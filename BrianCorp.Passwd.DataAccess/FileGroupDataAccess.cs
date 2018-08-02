using System;
using System.IO;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.DataAccess
{
    public class FileGroupDataAccess : BaseGroupDataAccess
    {
        #region C-Tors

        public FileGroupDataAccess(
            IGroupFileConfig config)
        {
            if (null == config)
            {
                throw new HardFailureException(
                    "The config is NULL. ");
            }

            Config = config;
        }

        public FileGroupDataAccess(
            IGroupFileConfig config,
            ILogger log)
        {
            if (null == config)
            {
                throw new HardFailureException(
                    "The config is NULL. ");
            }

            Log = log;
            Config = config;
        }

        #endregion

        #region Public Properties

        public IGroupFileConfig Config { get; set; }

        #endregion

        #region Public Methods

        public override List<Group> GetAll()
        {
            return loadGroups();
        }

        public override DateTime LastUpdatedUTC
        {
            get
            {
                return File.GetLastAccessTimeUtc(Config.GroupFilePath);
            }
        }

        #endregion

        #region Private Methods


        private List<Group> loadGroups()
        {
            const int INITIAL_CAPACITY = 100;

            List<Group> list = new List<Group>(INITIAL_CAPACITY);

            // The /etc/group file has a limit to the number of characters
            // that can be read in. You get around this by adding a new line
            // with new members but everything else is the same. So,
            // we need to read this in and keep track of each line as it
            // comes in, and check if we've already seen the group.
            Dictionary<uint, Group> groups = new Dictionary<uint, Group>(INITIAL_CAPACITY);

            if (false == File.Exists(Config.GroupFilePath))
            {
                throw new HardFailureException(
                    "The file does not EXIST. " +
                    "FilePath: " + ParameterChecker.IsNull(Config.GroupFilePath));
            }

            using (TextReader tr = new StreamReader(Config.GroupFilePath))
            {
                while (tr.Peek() != -1)
                {
                    string line = tr.ReadLine();

                    if (false == string.IsNullOrWhiteSpace(line))
                    {
                        Group group = parseLine(line);

                        if (groups.ContainsKey(group.GID))
                        {
                            groups[group.GID].Members.AddRange(group.Members);
                        }
                        else
                        {
                            groups.Add(group.GID, group);
                        }
                    }
                }
            }

            foreach(Group group in groups.Values)
            {
                list.Add(group);
            }

            return list;
        }

        #endregion
    }
}
