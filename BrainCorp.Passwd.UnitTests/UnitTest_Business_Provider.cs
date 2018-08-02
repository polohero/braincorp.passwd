using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Caching;
using BrainCorp.Passwd.Testing.Common;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    [TestClass]
    public class UnitTest_Business_Provider : UnitTestBase
    {
        #region Initializations

        private static LargeFakeUserDataAccess _largeListOfUsers = null;
        private static NormalFakeUserDataAccess _normalListOfUsers = null;

        private static LargeFakeGroupDataAccess _largeListOfGroups = null;
        private static NormalFakeGroupDataAccess _normalListOfGroups = null;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _largeListOfUsers = new LargeFakeUserDataAccess(new TestContextLogger(context));
            _normalListOfUsers = new NormalFakeUserDataAccess(new TestContextLogger(context));
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            initialize();

            // Since I muck with these list in one of the tests,
            // I need to make sure it's reset on each test.
            _normalListOfGroups = new NormalFakeGroupDataAccess(new TestContextLogger(TestContext));
            _normalListOfUsers = new NormalFakeUserDataAccess(new TestContextLogger(TestContext));
        }

        #endregion

        #region User Tests

        [TestMethod]
        public void Test_User_Provider_GetAllUsers()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.GetAllUsers();

            List<User> compareList = new List<User>();
            compareList.Add(new User("root", 0, 0, "rootuser", "/home", "/shell"));
            compareList.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));
            compareList.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));
            compareList.Add(new User("myuser2", 3, 1, "myusers", "/home/myuser2", "/ksh/shell"));
            compareList.Add(new User("myuser3", 4, 1, "myusers", "/home/myuser3", "/ksh/shell"));
            compareList.Add(new User("myuser4", 5, 1, "myusers", "/home/myuser4", "/ksh/shell"));
            compareList.Add(new User("admin1", 6, 2, "admins", "/home/admin1", "/ksh/shell"));
            compareList.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));
            compareList.Add(new User("admin3", 8, 2, "admins", "/home/admin3", "/bash/shell"));
            compareList.Add(new User("admin4", 9, 2, "admins", "/home/admin4", "/ksh/shell"));
            compareList.Add(new User("root2", 10, 0, "rootuser", "/home", "/shell"));
            compareList.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            compareList.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            compareList.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));
            compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetAllUsers",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetAllUsers: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetGroups_Single()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.UID = "1";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetGroups(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("myusers", 1, new string[] { "cwixom", "myuser1", "myuser3", "myuser4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetGroups_Single",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetGroups_Single: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetGroups_Single_Bad()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.UID = "1111111";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetGroups(searchRequest);

            List<Group> compareList = new List<Group>();

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetGroups_Single_Bad",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetGroups_Multiple()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.UID = "14";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetGroups(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));
            compareList.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));
            compareList.Add(new Group("gid1", 3, new string[] { "gidded1", "gidded2" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetAllUsers",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetAllUsers: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_UID()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.UID = "14";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();

            compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_UID",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_UID: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_UID_Bad()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.UID = "14111111";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_UID",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Name()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Name = "myuser1";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetAllUsers",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetAllUsers: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Name_Bad()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Name = "myuser1111111";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetAllUsers",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Home()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Home = "/home";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("root", 0, 0, "rootuser", "/home", "/shell"));
            compareList.Add(new User("root2", 10, 0, "rootuser", "/home", "/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Home",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_Home: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Home_Bad()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Home = "/homasdfasdfasdfe";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Home",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Shell()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Shell = "/ksh/shell";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));
            compareList.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));
            compareList.Add(new User("myuser2", 3, 1, "myusers", "/home/myuser2", "/ksh/shell"));
            compareList.Add(new User("myuser3", 4, 1, "myusers", "/home/myuser3", "/ksh/shell"));
            compareList.Add(new User("myuser4", 5, 1, "myusers", "/home/myuser4", "/ksh/shell"));
            compareList.Add(new User("admin1", 6, 2, "admins", "/home/admin1", "/ksh/shell"));
            compareList.Add(new User("admin4", 9, 2, "admins", "/home/admin4", "/ksh/shell"));
            compareList.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Shell",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_Shell: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Shell_Bad()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Shell = "/ksh/shasdfsdfsdfell";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Shell_Bad",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_GID()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.GID = "1";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));
            compareList.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));
            compareList.Add(new User("myuser2", 3, 1, "myusers", "/home/myuser2", "/ksh/shell"));
            compareList.Add(new User("myuser3", 4, 1, "myusers", "/home/myuser3", "/ksh/shell"));
            compareList.Add(new User("myuser4", 5, 1, "myusers", "/home/myuser4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_GID",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_GID: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_GID_Bad()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.GID = "111111";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_GID_Bad",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Comment()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Comment = "myusers";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));
            compareList.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));
            compareList.Add(new User("myuser2", 3, 1, "myusers", "/home/myuser2", "/ksh/shell"));
            compareList.Add(new User("myuser3", 4, 1, "myusers", "/home/myuser3", "/ksh/shell"));
            compareList.Add(new User("myuser4", 5, 1, "myusers", "/home/myuser4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Comment",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_Comment: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Multiple_CommentHome()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Comment = "myusers";
            searchRequest.Home = "/home/cwixom";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Multiple_CommentHome",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_Multiple_CommentHome: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Multiple_CommentShell()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Comment = "admins";
            searchRequest.Shell = "/bash/shell";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));
            compareList.Add(new User("admin3", 8, 2, "admins", "/home/admin3", "/bash/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Multiple_CommentShell",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_Multiple_CommentShell: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_Multiple_CommentShellHome()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();
            searchRequest.Comment = "admins";
            searchRequest.Shell = "/bash/shell";
            searchRequest.Home = "/home/admin2";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            compareList.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_Multiple_CommentShellHome",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (User compareUser in compareList)
            {
                bool found = false;

                foreach (User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_User_Provider_GetUser_Multiple_CommentShellHome: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_User_Provider_GetUser_NoSearch()
        {
            UserSearchRequest searchRequest = new UserSearchRequest();

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.Search(searchRequest);

            List<User> compareList = new List<User>();
            //compareList.Add(new User("root", 0, 0, "rootuser", "/home", "/shell"));
            //compareList.Add(new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell"));
            //compareList.Add(new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell"));
            //compareList.Add(new User("myuser2", 3, 1, "myusers", "/home/myuser2", "/ksh/shell"));
            //compareList.Add(new User("myuser3", 4, 1, "myusers", "/home/myuser3", "/ksh/shell"));
            //compareList.Add(new User("myuser4", 5, 1, "myusers", "/home/myuser4", "/ksh/shell"));
            //compareList.Add(new User("admin1", 6, 2, "admins", "/home/admin1", "/ksh/shell"));
            //compareList.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));
            //compareList.Add(new User("admin3", 8, 2, "admins", "/home/admin3", "/bash/shell"));
            //compareList.Add(new User("admin4", 9, 2, "admins", "/home/admin4", "/ksh/shell"));
            //compareList.Add(new User("root2", 10, 0, "rootuser", "/home", "/shell"));
            //compareList.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            //compareList.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            //compareList.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));
            //compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_User_Provider_GetUser_NoSearch",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetAllUsers_FileUpdated()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.GetAllUsers();

            // Add a user and set the date such that the cache should not clear.
            _normalListOfUsers.UserList.Add(new User("fake", 9999, 9999, "fake1", "fake2", "fake3"));
            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> cachedList = provider.GetAllUsers();

            evaluate(
                (_normalListOfUsers.UserList.Count - cachedList.Count) == 1,
                "Test_User_Provider_GetAllUsers_FileUpdated",
                "The list count should not match.",
                list.Count.ToString(),
                cachedList.Count.ToString());

            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> newList = provider.GetAllUsers();

            evaluate(
                 _normalListOfUsers.UserList.Count == newList.Count,
                "Test_User_Provider_GetAllUsers_FileUpdated",
                "The list count doesn't match.",
                list.Count.ToString(),
                newList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetAllUsers_CorruptedFile_UsesCache()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.GetAllUsers();

            // Add a user and set the date such that the cache should not clear.
            _normalListOfUsers.UserList.Add(new User("fake", 9999, 9999, "fake1", "fake2", "fake3"));
            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);

            provider =
                new PasswdProvider(
                        new CorruptedFileFakeUserDataAccess(),
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> cachedList = provider.GetAllUsers();

            evaluate(
                (_normalListOfUsers.UserList.Count - cachedList.Count) == 1,
                "Test_User_Provider_GetAllUsers_FileUpdated",
                "The list count should not match.",
                list.Count.ToString(),
                cachedList.Count.ToString());

            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> newList = provider.GetAllUsers();

            evaluate(
                 _normalListOfUsers.UserList.Count == newList.Count,
                "Test_User_Provider_GetAllUsers_FileUpdated",
                "The list count doesn't match.",
                list.Count.ToString(),
                newList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetAllUsers_MissingFile_UsesCache()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.GetAllUsers();

            // Add a user and set the date such that the cache should not clear.
            _normalListOfUsers.UserList.Add(new User("fake", 9999, 9999, "fake1", "fake2", "fake3"));
            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);

            provider =
                new PasswdProvider(
                        new MissingFileFakeUserDataAccess(),
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> cachedList = provider.GetAllUsers();

            evaluate(
                (_normalListOfUsers.UserList.Count - cachedList.Count) == 1,
                "Test_User_Provider_GetAllUsers_FileUpdated",
                "The list count should not match.",
                list.Count.ToString(),
                cachedList.Count.ToString());

            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> newList = provider.GetAllUsers();

            evaluate(
                 _normalListOfUsers.UserList.Count == newList.Count,
                "Test_User_Provider_GetAllUsers_FileUpdated",
                "The list count doesn't match.",
                list.Count.ToString(),
                newList.Count.ToString());
        }

        [TestMethod]
        public void Test_User_Provider_GetAllUsers_CorruptedFile_DoesNotUseCache()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<User> list = provider.GetAllUsers();

            // Add a user and set the date such that the cache should not clear.
            _normalListOfUsers.UserList.Add(new User("fake", 9999, 9999, "fake1", "fake2", "fake3"));
            _normalListOfUsers.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);

            Exception exception = null;

            try
            {

                provider =
                    new PasswdProvider(
                            new CorruptedFileFakeUserDataAccess(),
                            _normalListOfGroups,
                            new InfiniteReferenceCaching(),
                            TestLog,
                            new FailOnFileIssuesConfigValues()
                        );

                List<User> cachedList = provider.GetAllUsers();
            }
            catch(NotImplementedException notImplemented)
            {
                exception = notImplemented;
            }

            evaluate(
                null != exception,
                "Exception should have fired.",
                "Exception should have fired",
                "exception",
                 "exception");
        }

        #endregion

        #region Group Tests

        [TestMethod]
        public void Test_Group_Provider_GetAllGroups()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetAllGroups();

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("rootuser", 0, new string[] { "root", "root2" }));
            compareList.Add(new Group("myusers", 1, new string[] { "cwixom", "myuser1", "myuser3", "myuser4" }));
            compareList.Add(new Group("admins", 2, new string[] { "admin1", "admin2", "admin3", "admin4" }));
            compareList.Add(new Group("gid1", 3, new string[] { "gidded1", "gidded2" }));
            compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));
            compareList.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_GetAllGroups",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_GetAllGroups: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_Search_GID()
        {
            GroupSearchRequest searchRequest = new GroupSearchRequest();
            searchRequest.GID = "4";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.Search(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_GetAllGroups",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_GetAllGroups: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_Search_Name()
        {
            GroupSearchRequest searchRequest = new GroupSearchRequest();
            searchRequest.Name = "gid2";

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.Search(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_Search_Name",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_Search_Name: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_Search_Member_SingleMemberSingleGroup()
        {
            GroupSearchRequest searchRequest = new GroupSearchRequest();
            searchRequest.Member = new List<string>(new string[] { "root" });

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.Search(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("rootuser", 0, new string[] { "root", "root2" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_Search_Member_SingleMemberSingleGroup",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_Search_Member_SingleMemberSingleGroup: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_Search_Member_SingleMemberMultiGroup()
        {
            GroupSearchRequest searchRequest = new GroupSearchRequest();
            searchRequest.Member = new List<string>(new string[] { "gidded4" });

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.Search(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));
            compareList.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_Search_Member_SingleMemberMultiGroup",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_Search_Member_SingleMemberMultiGroup: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_Search_Member_MultiMemberMultiGroup()
        {
            GroupSearchRequest searchRequest = new GroupSearchRequest();
            searchRequest.Member = new List<string>(new string[] { "gidded4", "root" });

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.Search(searchRequest);

            List<Group> compareList = new List<Group>();
            compareList.Add(new Group("rootuser", 0, new string[] { "root", "root2" }));
            compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));
            compareList.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_Search_Member_MultiMemberMultiGroup",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_Search_Member_MultiMemberMultiGroup: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_Search_Nothing()
        {
            GroupSearchRequest searchRequest = new GroupSearchRequest();
            //searchRequest.Member = new List<string>(new string[] { "gidded4", "root" });

            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new NullCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.Search(searchRequest);

            List<Group> compareList = new List<Group>();
            //compareList.Add(new Group("rootuser", 0, new string[] { "root", "root2" }));
            //compareList.Add(new Group("myusers", 1, new string[] { "cwixom", "myuser1", "myuser3", "myuser4" }));
            //compareList.Add(new Group("admins", 2, new string[] { "admin1", "admin2", "admin3", "admin4" }));
            //compareList.Add(new Group("gid1", 3, new string[] { "gidded1", "gidded2" }));
            //compareList.Add(new Group("gid2", 4, new string[] { "gidded3", "gidded4" }));
            //compareList.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Provider_Search_Nothing",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach (Group compareGroup in compareList)
            {
                bool found = false;

                foreach (Group group in list)
                {
                    if (compareGroup.Equals(group))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Test_Group_Provider_Search_Nothing: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Provider_GetAllGroups_FileUpdated()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetAllGroups();

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);
            _normalListOfGroups.GroupList.Add(new Group("fake", 99999, new string[] { "faked" }));

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> cachedList = provider.GetAllGroups();

            evaluate(
                (_normalListOfGroups.GroupList.Count - cachedList.Count) == 1,
                "Test_Group_Provider_GetAllGroups_FileUpdated",
                "The list count should not match.",
                list.Count.ToString(),
                cachedList.Count.ToString());

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> newList = provider.GetAllGroups();

            evaluate(
                 _normalListOfGroups.GroupList.Count == newList.Count,
                "Test_Group_Provider_GetAllGroups_FileUpdated",
                "The list count doesn't match.",
                list.Count.ToString(),
                newList.Count.ToString());
        }

        [TestMethod]
        public void Test_Group_Provider_GetAllGroups_CorruptedFile_UsesCache()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetAllGroups();

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);
            _normalListOfGroups.GroupList.Add(new Group("fake", 99999, new string[] { "faked" }));

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        new CorruptedFileFakeGroupDataAccess(),
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> cachedList = provider.GetAllGroups();

            evaluate(
                (_normalListOfGroups.GroupList.Count - cachedList.Count) == 1,
                "Test_Group_Provider_GetAllGroups_CorruptedFile_UsesCache",
                "The list count should not match.",
                list.Count.ToString(),
                cachedList.Count.ToString());

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> newList = provider.GetAllGroups();

            evaluate(
                 _normalListOfGroups.GroupList.Count == newList.Count,
                "Test_Group_Provider_GetAllGroups_CorruptedFile_UsesCache",
                "The list count doesn't match.",
                list.Count.ToString(),
                newList.Count.ToString());
        }

        [TestMethod]
        public void Test_Group_Provider_GetAllGroups_MissingFile_UsesCache()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetAllGroups();

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);
            _normalListOfGroups.GroupList.Add(new Group("fake", 99999, new string[] { "faked" }));

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        new MissingFileFakeGroupDataAccess(),
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> cachedList = provider.GetAllGroups();

            evaluate(
                (_normalListOfGroups.GroupList.Count - cachedList.Count) == 1,
                "Test_Group_Provider_GetAllGroups_CorruptedFile_UsesCache",
                "The list count should not match.",
                list.Count.ToString(),
                cachedList.Count.ToString());

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(1);

            provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> newList = provider.GetAllGroups();

            evaluate(
                 _normalListOfGroups.GroupList.Count == newList.Count,
                "Test_Group_Provider_GetAllGroups_CorruptedFile_UsesCache",
                "The list count doesn't match.",
                list.Count.ToString(),
                newList.Count.ToString());
        }

        [TestMethod]
        public void Test_Group_Provider_GetAllGroups_CorruptedFile_DoesNotUseCache()
        {
            PasswdProvider provider =
                new PasswdProvider(
                        _normalListOfUsers,
                        _normalListOfGroups,
                        new InfiniteReferenceCaching(),
                        TestLog,
                        new TestConfigValues()
                    );

            List<Group> list = provider.GetAllGroups();

            _normalListOfGroups.LastUpdatedUTC = DateTime.UtcNow.AddDays(-1);
            _normalListOfGroups.GroupList.Add(new Group("fake", 99999, new string[] { "faked" }));

            Exception exception = null;

            try
            {

                provider =
                    new PasswdProvider(
                            _normalListOfUsers,
                            new CorruptedFileFakeGroupDataAccess(),
                            new InfiniteReferenceCaching(),
                            TestLog,
                            new FailOnFileIssuesConfigValues()
                        );

                List<Group> cachedList = provider.GetAllGroups();
            }
            catch(NotImplementedException notImplemented)
            {
                exception = notImplemented;
            }

            evaluate(
                null != exception,
                "Exception should have fired.",
                "Exception should have fired",
                "exception",
                "exception");

        }


        #endregion
    }
}
