using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Amazon;
using Amazon.Auth;
using Amazon.Internal;
using Amazon.S3;
using Amazon.S3.Model;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.DataAccess
{
    public class S3GroupDataAccess : BaseGroupDataAccess
    {
        private static AmazonS3Client _s3Client = null;

        #region C-Tors

        public S3GroupDataAccess(
            IGroupFileConfig config,
            ILogger log,
            IAWSConfig awsConfig)
        {
            if (null == config)
            {
                throw new HardFailureException(
                    "The config is NULL. ");
            }

            Log = log;
            Config = config;
            AWSConfig = awsConfig;

            if( null == _s3Client)
            {
                _s3Client = new AmazonS3Client(
                    awsConfig.AWSAccessKey,
                    awsConfig.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(awsConfig.AWSRegionEndpoint));
            }
        }

        #endregion

        #region Public Properties

        public IGroupFileConfig Config { get; set; }

        public IAWSConfig AWSConfig { get; set; }

        public override DateTime LastUpdatedUTC
        {
            get { return getFileDate(); }
        }

        #endregion

        #region Public Methods

        public override List<Group> GetAll()
        {
            return loadGroups();
        }

        #endregion

        #region Private Methods

        private DateTime getFileDate()
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "braincorppasswd",
                Key = Config.GroupFilePath
            };

            using (GetObjectResponse response = _s3Client.GetObject(request))
            {
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.LastModified.ToUniversalTime();
                }
            }

            throw new HardFailureException(
                "Unable to access group file on S3. " +
                Config.GroupFilePath);
        }

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

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "braincorppasswd",
                Key = Config.GroupFilePath
            };


            using (GetObjectResponse response = _s3Client.GetObject(request))
            {
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (Stream stream = response.ResponseStream)
                    {
                        using (TextReader tr = new StreamReader(stream, Encoding.UTF8))
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
                    }
                }
                else
                {
                    throw new HardFailureException(
                        "Unable to access group file on S3. " +
                        Config.GroupFilePath);
                }
            }

            foreach (Group group in groups.Values)
            {
                list.Add(group);
            }

            return list;
        }

        #endregion

    }
}
