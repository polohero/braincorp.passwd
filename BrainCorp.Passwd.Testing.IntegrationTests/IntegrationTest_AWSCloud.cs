using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.Client;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Caching;
using BrainCorp.Passwd.Testing.Common;

namespace BrainCorp.Passwd.Testing.IntegrationTests
{
    [TestClass]
    public class IntegrationTest_AWSCloud : UnitTestBase
    {
        #region Initializations/Cleanup

        private static Random randSeed = new Random();
        private static Random rand = new Random(
            randSeed.Next(99999));

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            List<User> users = client.GetAllUsers();
            _userToSearchFor = users[new Random().Next(users.Count - 1)];

            List<Group> groups = client.GetAllGroups();
            _groupToSearchFor = groups[new Random().Next(groups.Count - 1)];
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            initialize();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Test_GetAllUsers()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            List<User> users = client.GetAllUsers();

            evaluate(
                users.Count > 0,
                "Test_GetAllUsers",
                "Test_GetAllUsers",
                 users.Count.ToString(),
                 " users.Count > 0");
        }

        [TestMethod]
        public void Test_GetAllGroups()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            List<Group> groups = client.GetAllGroups();

            evaluate(
                groups.Count > 0,
                "Test_GetAllGroups",
                "Test_GetAllGroups",
                 groups.Count.ToString(),
                 " groups.Count > 0");
        }

        [TestMethod]
        public void Test_GetGroup()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            Group group = client.GetGroup(_groupToSearchFor.GID);

            evaluate(
                null != group,
                "Test_GetGroup",
                "Test_GetGroup",
                 "",
                 "");

            evaluate(
                _groupToSearchFor.Equals(group),
                "Test_GetGroup: Validate Group Data",
                "Test_GetGroup: Validate Group Data",
                 group.ToString(),
                 _groupToSearchFor.ToString());
        }

        [TestMethod]
        public void Test_GetUser()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            User user = client.GetUser(_userToSearchFor.UID);

            evaluate(
                null != user,
                "Test_GetUser",
                "Test_GetUser",
                 "",
                 "");

            evaluate(
                _userToSearchFor.Equals(user),
                "Test_GetUser: Validate User Data",
                "Test_GetUser: Validate User Data",
                 user.ToString(),
                 _userToSearchFor.ToString());
        }

        [TestMethod]
        public void Test_UserSearch_SetUID()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            UserSearchBuilder builder = new UserSearchBuilder();
            builder.SetUID(_userToSearchFor.UID);

            List<User> list = client.Search(builder.Build());

            evaluate(
                null != list,
                "Test_UserSearch_SetUID",
                "Test_UserSearch_SetUID",
                 "",
                 "");

            evaluate(
                list.Count == 1,
                "Test_UserSearch_SetUID: Count",
                "Test_UserSearch_SetUID: Count",
                 list.Count.ToString(),
                 "1");

            evaluate(
                _userToSearchFor.Equals(list[0]),
                "Test_UserSearch_SetUID: Validate User Data",
                "Test_UserSearch_SetUID: Validate User Data",
                 list[0].ToString(),
                 _userToSearchFor.ToString());
        }

        [TestMethod]
        public void Test_UserSearch_SetName()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            UserSearchBuilder builder = new UserSearchBuilder();
            builder.SetName(_userToSearchFor.Name);

            List<User> list = client.Search(builder.Build());

            evaluate(
                null != list,
                "Test_UserSearch_SetName",
                "Test_UserSearch_SetName",
                 "",
                 "");

            evaluate(
                list.Count == 1,
                "Test_UserSearch_SetName: Count",
                "Test_UserSearch_SetName: Count",
                 list.Count.ToString(),
                 "1");

            evaluate(
                _userToSearchFor.Equals(list[0]),
                "Test_UserSearch_SetName: Validate User Data",
                "Test_UserSearch_SetName: Validate User Data",
                 list[0].ToString(),
                 _userToSearchFor.ToString());
        }

        [TestMethod]
        public void Test_GetUser_NotFound()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            Exception exception = null;

            try
            {
                client.GetUser(9999999);
            }
            catch(ApplicationException application)
            {
                exception = application;
            }

            evaluate(
                null != exception,
                "Test_GetUser_NotFound",
                "Test_GetUser_NotFound",
                 "",
                 "");

            evaluate(
                exception.Message.Contains("NotFound"),
                "Test_GetUser_NotFound",
                "Test_GetUser_NotFound",
                 "",
                 "");

        }

        [TestMethod]
        public void Test_GetGroup_NotFound()
        {
            PasswdClient client = new PasswdClient(
                new PasswdClientFactory(new PasswdWebServiceConfiguration()));

            Exception exception = null;

            try
            {
                client.GetGroup(9999999);
            }
            catch (ApplicationException application)
            {
                exception = application;
            }

            evaluate(
                null != exception,
                "Test_GetGroup_NotFound",
                "Test_GetGroup_NotFound",
                 "",
                 "");
            evaluate(
                exception.Message.Contains("NotFound"),
                "Test_GetGroup_NotFound",
                "Test_GetGroup_NotFound",
                 "",
                 "");

        }

        #endregion

        #region Private Attributes

        private static User _userToSearchFor = null;
        private static Group _groupToSearchFor = null;

        #endregion
    }
}
