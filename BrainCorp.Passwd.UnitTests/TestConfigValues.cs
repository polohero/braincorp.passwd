using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class TestConfigValues : IConfigValues
    {
        public FailBehaviorEnum FailBehavior
        {
            get { return FailBehaviorEnum.ContinueWithOldData; }
        }

        public DataAccessSourceEnum DataAccessSource
        {
            get { return DataAccessSourceEnum.FileOnDisk; }
        }

        public string AWSAccessKey
        {
            get { return ""; }
        }
        public string AWSSecretAccessKey
        {
            get { return ""; }
        }

        public string AWSRegionEndpoint
        {
            get { return ""; }
        }

        public FileLocationEnum FileLocation { get { return FileLocationEnum.Relative; } }
    }
}
