using System.IO;

using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.DataAccess
{
    public class GroupFileConfig : IGroupFileConfig
    {
        public GroupFileConfig(string filePath)
        {
            if (false == File.Exists(filePath))
            {
                throw new HardFailureException(
                    "The file was not found. " +
                    "Check that the file is visble to this " +
                    "user and actually in the correct location. " +
                    "FilePath: " + ParameterChecker.IsNull(filePath)
                    );

            }

            GroupFilePath = filePath;
        }

        public string GroupFilePath { get; set; }
    }
}
