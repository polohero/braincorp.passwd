using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.DataAccess
{
    public abstract class BaseUserDataAccess : BaseDataAccess, IUserDataAccess
    {
        public abstract List<User> GetAll();

        public abstract DateTime LastUpdatedUTC { get; }

        /// <summary>
        /// Validates a single line is valid for a user.
        /// A sample line is:
        /// //root: !:0:0::/:/ usr / bin / ksh
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string[] validateLine(string line)
        {
            const char COLON = ':';
            const int EXPECTED_LENGTH = 7;
            //root: !:0:0::/:/ usr / bin / ksh

            ParameterChecker.WhitespaceCheck(
                line,
                line,
                "The line that contains the user information is not valid. " +
                "It is an empty string. This would indicate that there is " +
                "corrupted information in the user file and needs to be addressed." +
                "line: " + ParameterChecker.IsNull(line));

            string[] split = line.Split(COLON);

            if (split.Length != EXPECTED_LENGTH)
            {
                throw new HardFailureException(
                    "The line that contains the user information is not valid. " +
                    "It does not contain the correct number of columns, or possibly does not " +
                    "contain a COLON. This would indicate that there is " +
                    "corrupted information in the user file and needs to be addressed." +
                    "line: " + ParameterChecker.IsNull(line));
            }

            if( false == uint.TryParse(split[2], out uint throwAway))
            {
                throw new HardFailureException(
                    "The line that contains the user information is not valid. " +
                    "The User ID number (UID) field is not an integer. " +
                    "This would indicate that there is " +
                    "corrupted information in the user file and needs to be addressed." +
                    "line: " + ParameterChecker.IsNull(line));
            }

            if (false == uint.TryParse(split[3], out throwAway))
            {
                throw new HardFailureException(
                    "The line that contains the user information is not valid. " +
                    "The User's group ID number (GID) is not an integer. " +
                    "This would indicate that there is " +
                    "corrupted information in the user file and needs to be addressed." +
                    "line: " + ParameterChecker.IsNull(line));
            }

            return split;
        }

        /// <summary>
        /// Parses a single line from the file.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected User parseLine(string line)
        {
            //root: !:0:0::/:/ usr / bin / ksh
            string[] data = validateLine(line);

            return new User(
                data[0].TrimEnd(),
                uint.Parse(data[2]),
                uint.Parse(data[3]),
                data[4].TrimEnd(),
                data[5].TrimEnd(),
                data[6].TrimEnd());
        }
    }
}
