
namespace BrainCorp.Passwd.Entities
{
    public interface IConfigValues
    {
        FailBehaviorEnum FailBehavior { get; }
        DataAccessSourceEnum DataAccessSource { get;  }

        FileLocationEnum FileLocation { get; }
        

        string AWSAccessKey { get;}

        string AWSSecretAccessKey { get; }

        string AWSRegionEndpoint { get; }
    }

    public enum FileLocationEnum
    {
        Absolute = 0,
        Relative = 1
    }

    public enum FailBehaviorEnum
    {
        ContinueWithOldData=0,
        FailWithCorruptedFile = 1
    }

    public enum DataAccessSourceEnum
    {
        FileOnDisk=1,
        FileInS3 = 2
    }
}
