using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.DataAccess
{
    public abstract class BaseGroupDataAccess : BaseDataAccess, IGroupDataAccess
    {
        public abstract List<Group> GetAll();

        public abstract DateTime LastUpdatedUTC { get; }

        /// <summary>
        /// Validates a single line is valid for a Group.
        /// A sample line is:
        /// //root: !:0:0::/:/ usr / bin / ksh
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string[] validateLine(string line)
        {
            const char COLON = ':';
            const int EXPECTED_LENGTH = 4;
            //groupname:group-password:GID:username-list

            ParameterChecker.WhitespaceCheck(
                line,
                line,
                "The line that contains the group information is not valid. " +
                "It is an empty string. This would indicate that there is " +
                "corrupted information in the group file and needs to be addressed." +
                "line: " + ParameterChecker.IsNull(line));

            string[] split = line.Split(COLON);

            if (split.Length != EXPECTED_LENGTH)
            {
                throw new HardFailureException(
                    "The line that contains the group information is not valid. " +
                    "It does not contain the correct number of columns, or possibly does not " +
                    "contain a COLON. This would indicate that there is " +
                    "corrupted information in the group file and needs to be addressed." +
                    "line: " + ParameterChecker.IsNull(line));
            }

            if (false == uint.TryParse(split[2], out uint throwAway))
            {
                throw new HardFailureException(
                    "The line that contains the group information is not valid. " +
                    "The GID field is not an integer. " +
                    "This would indicate that there is " +
                    "corrupted information in the group file and needs to be addressed." +
                    "line: " + ParameterChecker.IsNull(line));
            }


            return split;
        }

        private string[] splitMembers(string memberData)
        {
            const char COMMA = ',';

            if (string.IsNullOrWhiteSpace(memberData))
            {
                return new string[] { string.Empty };
            }

            if( false == memberData.Contains(COMMA.ToString()))
            {
                return new string[] { memberData };
            }

            return memberData.Split(COMMA);
        }

        /// <summary>
        /// Parses a single line from the file.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected Group parseLine(string line)
        {
            //root: !:0:0::/:/ usr / bin / ksh
            string[] data = validateLine(line);

            return new Group(
                data[0].TrimEnd(),
                uint.Parse(data[2]),
                splitMembers(data[3].TrimEnd()));
        }
    }
}
