using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainCorp.Passwd.DataAccess
{
    public interface IAWSConfig
    {
        string AWSAccessKey { get; }

        string AWSSecretAccessKey { get; }


        string AWSRegionEndpoint { get; }
    }
}
