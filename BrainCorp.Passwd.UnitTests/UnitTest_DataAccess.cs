using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Testing.Common;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    [TestClass]
    public class UnitTest_DataAccess : UnitTestBase
    {
        #region Initializations

        // NOTE: If you modify the USER or GROUP file
        // You may need to update some tests. There are
        // assumptions that certain users need to exist
        // in the files to satisfy the test.
        private const string USER_FILE_LOCATION = "users.txt";
        private const string GROUP_FILE_LOCATION = "groups.txt";
        private const int NUMBER_OF_USERS = 19;
        private const int NUMBER_OF_GROUPS = 3;

        [TestInitialize]
        public void MyTestInitialize()
        {
            initialize();
        }

        #endregion

        #region FileUserDataAccess Tests

        [TestMethod]
        public void Test_User_FileUserDataAccess_GetAll()
        {
            FileUserDataAccess file =
                new FileUserDataAccess(
                    new UserFileConfig(USER_FILE_LOCATION),
                    TestLog);

            User compareUser = new User(
                "root",
                0, 
                0,
                "",
                "/",
                "/usr/bin/ksh"
                );

            List<User> list = file.GetAll();

            evaluate(
                list.Count == NUMBER_OF_USERS,
                "NUMBER OF USERS: GetAll",
                "The number of users should be the same.",
                list.Count.ToString(),
                NUMBER_OF_USERS.ToString());

            bool found = false;

            foreach(User user in list)
            {
                if(user.Equals(compareUser))
                {
                    found = true;
                    break;
                }
            }

            evaluate(
                found,
                "Root user: GetAll",
                "The root user wasn't found.",
                compareUser.ToString(),
                "");

        }

        [TestMethod]
        public void Test_Group_FileGroupDataAccess_GetAll()
        {
            FileGroupDataAccess group =
                new FileGroupDataAccess(
                    new GroupFileConfig(GROUP_FILE_LOCATION),
                    TestLog);

            Group compareUser = new Group(
                "general",
                1,
                new string[] { "juan", "shelley", "bob"});

            List<Group> list = group.GetAll();

            evaluate(
                list.Count == NUMBER_OF_GROUPS,
                "NUMBER_OF_GROUPS: GetAll",
                "The number of users should be the same.",
                list.Count.ToString(),
                NUMBER_OF_GROUPS.ToString());

            bool found = false;

            foreach (Group user in list)
            {
                if (user.Equals(compareUser))
                {
                    found = true;
                    break;
                }
            }

            evaluate(
                found,
                "general group: GetAll",
                "The general group wasn't found.",
                compareUser.ToString(),
                "");

        }

        #endregion

        #region Private Methods

        private User findByName(string name, List<User> list)
        {
            foreach(User user in list)
            {
                if (name.Equals(user.Name))
                {
                    return user;
                }
            }

            return null;
        }

        #endregion
    }
}
