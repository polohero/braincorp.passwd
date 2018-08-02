using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Testing.Common;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    [TestClass]
    public class UnitTest_Business_Search : UnitTestBase
    {
        #region Initializations

        private static Users _largeListOfUsers = null;
        private static Users _normalListOfUsers = null;

        private static Groups _normalListOfGroups = null;
        private static Groups _largeListOfGroups = null;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _largeListOfUsers = new Users(
                new LargeFakeUserDataAccess(new TestContextLogger(context)),
                new TestContextLogger(context));
            _normalListOfUsers = new Users(
                new NormalFakeUserDataAccess(new TestContextLogger(context)),
                new TestContextLogger(context));

            _normalListOfGroups = new Groups(
                new NormalFakeGroupDataAccess(new TestContextLogger(context)),
                new TestContextLogger(context));
            _largeListOfGroups = new Groups(
                new LargeFakeGroupDataAccess(new TestContextLogger(context)),
                new TestContextLogger(context));
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            initialize();
        }

        #endregion

        #region User Tests

        #region Single Property Tests

        [TestMethod]
        public void Test_Users_Search_Name_Large()
        {
            Users users = _largeListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetName("100").Build());

            evaluate(
                list.Count == 1,
                "Search On Name",
                "The number of users should be 1.",
                list.Count.ToString(),
                "1");

            evaluate(
                list[0].Name == "100",
                "Search On Name: Check Name",
                "The name of the user should be 100.",
                list[0].Name,
                "100");
        }

        [TestMethod]
        public void Test_Users_Search_Name()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetName("cwixom").Build());

            User compareUser =
                new User("cwixom", 1, 1, "myusers", "/home/cwixom", "/ksh/shell");

            evaluate(
                list.Count == 1,
                "Search On Name",
                "The number of users should be 1.",
                list.Count.ToString(),
                "1");

            evaluate(
                list[0].Equals(compareUser),
                "Search On Name: Check cwixom",
                "The user should be the same.",
                list[0].ToString(),
                compareUser.ToString());
        }

        [TestMethod]
        public void Test_Users_Search_Name_DoesNotExist()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetName("somethingthatdoesntexist").Build());


            evaluate(
                list.Count == 0,
                "Test_Users_Search_Name_DoesNotExiste",
                "The number of users should be 0.",
                list.Count.ToString(),
                "0");
        }

        [TestMethod]
        public void Test_Users_Search_UID_Large()
        {
            Users users = _largeListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetUID(100).Build());

            evaluate(
                list.Count == 1,
                "Search On UID",
                "The number of users should be 1.",
                list.Count.ToString(),
                "1");

            evaluate(
                list[0].UID == 100,
                "Search On UID: Check UID",
                "The UID of the user should be 100.",
                list[0].UID.ToString(),
                "100");
        }

        [TestMethod]
        public void Test_Users_Search_UID()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetUID(2).Build());

            User compareUser =
               new User("myuser1", 2, 1, "myusers", "/home/myuser1", "/ksh/shell");

            evaluate(
                list.Count == 1,
                "Search On Name",
                "The number of users should be 1.",
                list.Count.ToString(),
                "1");

            evaluate(
                list[0].Equals(compareUser),
                "Search On UID: Check myuser1",
                "The user should be the same.",
                list[0].ToString(),
                compareUser.ToString());
        }

        [TestMethod]
        public void Test_Users_Search_GID_Large()
        {
            Users users = _largeListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetGID(10000).Build());

            evaluate(
                list.Count == 1001,
                "Search On GID",
                "The number of users should be 1001.",
                list.Count.ToString(),
                "1001");

            foreach(User user in list)
            {
                evaluate(
                    user.GID == 10000,
                    "Search On GID: Check GID",
                    "The GID of the user should be 10000.",
                    user.GID.ToString(),
                    "10000");
            }
        }

        [TestMethod]
        public void Test_Users_Search_GID()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetGID(2).Build());

            List<User> compareList = new List<User>();
            compareList.Add(new User("admin1", 6, 2, "admins", "/home/admin1", "/ksh/shell"));
            compareList.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));
            compareList.Add(new User("admin3", 8, 2, "admins", "/home/admin3", "/bash/shell"));
            compareList.Add(new User("admin4", 9, 2, "admins", "/home/admin4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Search On GID",
                "The list count doesn't match.",
                list.Count.ToString(),
                compareList.Count.ToString());

            foreach(User compareUser in compareList)
            {
                bool found = false;

                foreach(User user in list)
                {
                    if (compareUser.Equals(user))
                    {
                        found = true;
                    }
                }

                evaluate(
                    found,
                    "Search On GID: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_Users_Search_Comment_Large()
        {
            Users users = _largeListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetComment("1001").Build());

            evaluate(
                list.Count == 1,
                "Search On Comment",
                "The number of users should be 1.",
                list.Count.ToString(),
                "1");

            evaluate(
                list[0].Comment == "1001",
                "Search On Comment: Check Comment",
                "The Comment of the user should be 1001.",
                list[0].Comment,
                "1001");
        }

        [TestMethod]
        public void Test_Users_Search_Comment()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetComment("rootuser").Build());

            List <User> compareList = new List<User>();

            compareList.Add(new User("root", 0, 0, "rootuser", "/home", "/shell"));
            compareList.Add(new User("root2", 10, 0, "rootuser", "/home", "/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Search On Comment",
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
                    "Search On Comment: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_Users_Search_Home_Large()
        {
            Users users = _largeListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetHome("1002").Build());

            evaluate(
                list.Count == 1,
                "Search On Home",
                "The number of users should be 1.",
                list.Count.ToString(),
                "1");

            evaluate(
                list[0].Home == "1002",
                "Search On 1002: Check 1002",
                "The 1002 of the user should be 1002.",
                list[0].Home,
                "1002");
        }

        [TestMethod]
        public void Test_Users_Search_Shell_Large()
        {
            Users users = _largeListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetShell("20000").Build());

            evaluate(
                list.Count == 10001,
                "Search On Shell",
                "The number of users should be 10001.",
                list.Count.ToString(),
                "1");

            foreach (User user in list)
            {
                evaluate(
                    user.Shell == "20000",
                    "Search On Shell: Check Shell",
                    "The Shell of the user should be 20000.",
                    user.Shell.ToString(),
                    "20000");
            }
        }

        [TestMethod]
        public void Test_Users_Search_Shell()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(new UserSearchBuilder().SetShell("/bash/shell").Build());

            List<User> compareList = new List<User>();
            compareList.Add(new User("admin2", 7, 2, "admins", "/home/admin2", "/bash/shell"));
            compareList.Add(new User("admin3", 8, 2, "admins", "/home/admin3", "/bash/shell"));
            compareList.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            compareList.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Search On Shell",
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
                    "Search On Shell: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        #endregion

        #region Multi Property Tests

        [TestMethod]
        public void Test_Users_Search_GidAndComment()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(
                new UserSearchBuilder()
                .SetGID(3)
                .SetHome("/home/gidded2")
                .Build());

            List<User> compareList = new List<User>();
            //compareList.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            compareList.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            compareList.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));
           // compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_Users_Search_GidAndComment",
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
                    "Test_Users_Search_GidAndComment: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_Users_Search_GidAndShell()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(
                new UserSearchBuilder()
                .SetGID(3)
                .SetShell("/ksh/shell")
                .Build());

            List<User> compareList = new List<User>();
            compareList.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            //compareList.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            //compareList.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));
            compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_Users_Search_GidAndShell",
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
                    "Test_Users_Search_GidAndShell: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        [TestMethod]
        public void Test_Users_Search_GidAndShellAndComment()
        {
            Users users = _normalListOfUsers;

            List<User> list = users.Search(
                new UserSearchBuilder()
                .SetGID(3)
                .SetShell("/ksh/shell")
                .SetComment("gid1")
                .Build());

            List<User> compareList = new List<User>();
            compareList.Add(new User("gidded1", 11, 3, "gid1", "/home/gidded1", "/ksh/shell"));
            //compareList.Add(new User("gidded2", 12, 3, "gid1", "/home/gidded2", "/bash/shell"));
            //compareList.Add(new User("gidded3", 13, 3, "gid2", "/home/gidded2", "/bash/shell"));
            //compareList.Add(new User("gidded4", 14, 3, "gid2", "/home/gidded4", "/ksh/shell"));

            evaluate(
                list.Count == compareList.Count,
                "Test_Users_Search_GidAndShellAndComment",
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
                    "Test_Users_Search_GidAndShellAndComment: User Not found",
                    "The user should be the same.",
                    "null",
                    compareUser.ToString());
            }
        }

        #endregion

        #endregion

        #region Group Tests

        [TestMethod]
        public void Test_Group_Search_Name()
        {
            Groups groups = _normalListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().SetName("myusers").Build());

            List<Group> compareList = new List<Group>(1);
            compareList.Add(new Group("myusers", 1, new string[] { "cwixom", "myuser1", "myuser3", "myuser4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Search_Name",
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
                    "Test_Group_Search_Name: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Search_Name_Large()
        {
            Groups groups = _largeListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().SetName("group1").Build());
            evaluate(
                list.Count > 0,
                "Test_Group_Search_Name_Large",
                "The list count doesn't match.",
                list.Count.ToString(),
                ">0");
        }

        [TestMethod]
        public void Test_Group_Search_GID()
        {
            Groups groups = _normalListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().SetGID(2).Build());

            List<Group> compareList = new List<Group>(1);
            compareList.Add(new Group("admins", 2, new string[] { "admin1", "admin2", "admin3", "admin4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Search_GID",
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
                    "Test_Group_Search_GID: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Search_GID_Large()
        {
            Groups groups = _largeListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().SetGID(0).Build());
            evaluate(
                list.Count > 0,
                "Test_Group_Search_Name_Large",
                "The list count doesn't match.",
                list.Count.ToString(),
                ">0");
        }

        [TestMethod]
        public void Test_Group_Search_Member()
        {
            Groups groups = _normalListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().AddMember("gidded1").Build());

            List <Group> compareList = new List<Group>(1);
            compareList.Add(new Group("gid1", 3, new string[] { "gidded1", "gidded2" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Search_Member",
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
                    "Test_Group_Search_Member: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Search_Member_MultipleGroups()
        {
            Groups groups = _normalListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().AddMember("gidded2").Build());

            List<Group> compareList = new List<Group>(2);
            compareList.Add(new Group("gid1", 3, new string[] { "gidded1", "gidded2" }));
            compareList.Add(new Group("gid3", 5, new string[] { "gidded2", "gidded4" }));

            evaluate(
                list.Count == compareList.Count,
                "Test_Group_Search_Member_MultipleGroups",
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
                    "Test_Group_Search_Member_MultipleGroups: Group Not found",
                    "The group should be the same.",
                    "null",
                    compareGroup.ToString());
            }
        }

        [TestMethod]
        public void Test_Group_Search_Member_MultipleGroups_Large()
        {
            Groups groups = _largeListOfGroups;

            List<Group> list = groups.Search(new GroupSearchBuilder().AddMember("3").Build());
            evaluate(
                list.Count > 0,
                "Test_Group_Search_Member_MultipleGroups_Large",
                "The list count doesn't match.",
                list.Count.ToString(),
                ">0");
        }

        #endregion
    }
}
