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
    public class S3UserDataAccess : BaseUserDataAccess
    {
        private static AmazonS3Client _s3Client = null;

        #region C-Tors

        public S3UserDataAccess(
            IUserFileConfig config,
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

            if (null == _s3Client)
            {
                _s3Client = new AmazonS3Client(
                    awsConfig.AWSAccessKey,
                    awsConfig.AWSSecretAccessKey,
                    RegionEndpoint.GetBySystemName(awsConfig.AWSRegionEndpoint));
            }
        }

        #endregion

        #region Public Properties

        public IUserFileConfig Config { get; set; }

        public IAWSConfig AWSConfig { get; set; }

        public override DateTime LastUpdatedUTC
        {
            get { return getFileDate(); }
        }

        #endregion

        #region Public Methods

        public override List<User> GetAll()
        {
            return loadUsers();
        }

        #endregion

        #region Private Methods

        private DateTime getFileDate()
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "braincorppasswd",
                Key = Config.UserFilePath
            };

            using (GetObjectResponse response = _s3Client.GetObject(request))
            {
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.LastModified.ToUniversalTime();
                }
            }

            throw new HardFailureException(
                "Unable to access user file on S3. " +
                Config.UserFilePath);
        }

        private List<User> loadUsers()
        {
            const int INITIAL_CAPACITY = 100;

            List<User> list = new List<User>(INITIAL_CAPACITY);

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "braincorppasswd",
                Key = Config.UserFilePath
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
                                    list.Add(parseLine(line));
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new HardFailureException(
                        "Unable to access user file on S3. " +
                        Config.UserFilePath);
                }
            }

            return list;

        }

        #endregion

    }
}
