using System;
using System.IO;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.DataAccess
{
    public class FileUserDataAccess : BaseUserDataAccess
    {
        #region C-Tors

        public FileUserDataAccess(
            IUserFileConfig config)
        {
            if( null == config)
            {
                throw new HardFailureException(
                    "The config is NULL. ");
            }

            Config = config;
        }

        public FileUserDataAccess(
            IUserFileConfig config,
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

        public IUserFileConfig Config { get; set; }

        #endregion

        #region Public Methods

        public override List<User> GetAll()
        {
            return loadUsers();
        }

        public override DateTime LastUpdatedUTC
        {
            get
            {
               return File.GetLastAccessTimeUtc(Config.UserFilePath);
            }
        }

        #endregion

        #region Private Methods

        private List<User> loadUsers()
        {
            const int INITIAL_CAPACITY = 100;

            List<User> list = new List<User>(INITIAL_CAPACITY);

            if (false == File.Exists(Config.UserFilePath))
            {
                throw new HardFailureException(
                    "The file does not EXIST. " +
                    "FilePath: " + ParameterChecker.IsNull(Config.UserFilePath));
            }

            using (TextReader tr = new StreamReader(Config.UserFilePath))
            {
                while(tr.Peek() != -1)
                {
                    string line = tr.ReadLine();

                    if(false == string.IsNullOrWhiteSpace(line))
                    {
                        list.Add(parseLine(line));
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
