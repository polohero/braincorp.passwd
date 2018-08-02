using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;

namespace BrainCorp.Passwd.WebService.Configuration
{
    public class PasswdConfig : IUserFileConfig, IGroupFileConfig, IConfigValues, IAWSConfig
    {
        private static NameValueCollection configSection =
             (NameValueCollection)ConfigurationManager.GetSection(CONFIG_SECTION_NAME);

        private const string CONFIG_SECTION_NAME = "braincorp";
        private const string USER_FILE_PATH = "userFilePath";
        private const string GROUP_FILE_PATH = "groupFilePath";
        private const string FILE_LOCATION = "fileLocation";
        private const string FAIL_BEHAVIOR = "failBehavior";
        private const string DATA_ACCESS_SOURCE = "dataAccessSource";

        public string UserFilePath
        {
            get
            {
                string fileName = configSection[USER_FILE_PATH];

                if (FileLocation == FileLocationEnum.Relative)
                {
                    return System.Web.HttpContext.Current.Request.MapPath("~\\bin\\" + fileName);
                }
                return fileName;
            }
        }

        public string GroupFilePath
        {
            get
            {
                string fileName = configSection[GROUP_FILE_PATH];

                if (FileLocation == FileLocationEnum.Relative)
                {
                    return System.Web.HttpContext.Current.Request.MapPath("~\\bin\\" + fileName);
                }
                return fileName;
            }

        }

        public FileLocationEnum FileLocation
        {
            get
            {
                return (FileLocationEnum)Enum.Parse(typeof(FileLocationEnum), configSection[FILE_LOCATION]);
                
            }
        }


        public FailBehaviorEnum FailBehavior
        {
            get
            {
                return (FailBehaviorEnum)Enum.Parse(typeof(FailBehaviorEnum), configSection[FAIL_BEHAVIOR]);
            }
        }
        public DataAccessSourceEnum DataAccessSource
        {
            get
            {
                return (DataAccessSourceEnum)Enum.Parse(typeof(DataAccessSourceEnum), configSection[DATA_ACCESS_SOURCE]);
            }
        }

        public string AWSAccessKey
        {
            get
            {
                return configSection["AWSAccessKey"];
            }
        }

        public string AWSSecretAccessKey
        {
            get
            {
                return configSection["AWSSecretAccessKey"];
            }
        }


        public string AWSRegionEndpoint
        {
            get
            {
                return configSection["AWSRegionEndpoint"];
            }
        }

    }
}