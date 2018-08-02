using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Testing.Common;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    [TestClass]
    public class UnitTest_Entities : UnitTestBase
    {
        #region Initializations

        [TestInitialize]
        public void MyTestInitialize()
        {
            initialize();
        }

        #endregion

        #region User Tests

        [TestMethod]
        public void Test_User_ConstructorAndProperties()
        {
            User user = new User(
                "corywixom",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            evaluate(
                user.Name.Equals("corywixom"),
                "User.Name",
                "The name does not match what was expected.",
                user.Name,
                "corywixom");

            evaluate(
                user.GID.Equals(89),
                "User.GID",
                "The GID does not match what was expected.",
                user.GID.ToString(),
                "89");

            evaluate(
                user.UID.Equals(99),
                "User.UID",
                "The UID does not match what was expected.",
                user.UID.ToString(),
                "99");

            evaluate(
                user.Comment.Equals("some comment"),
                "User.Comment",
                "The Comment does not match what was expected.",
                user.Comment.ToString(),
                "some comment");

            evaluate(
                user.Home.Equals("/root"),
                "User.Home",
                "The Home does not match what was expected.",
                user.Home.ToString(),
                "/root");

            evaluate(
                user.Shell.Equals("/bin/false"),
                "User.Shell",
                "The Shell does not match what was expected.",
                user.Shell.ToString(),
                "/bin/false");
        }

        [TestMethod]
        public void Test_User_Conditional_Equals()
        {
            User user = new User(
                "corywixom",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            User same = new User(
                "corywixom",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            User different = new User(
                "corywixom",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            evaluate(
                user.Equals(same),
                "user == same",
                "The 2 should be the same.",
                user.ToString(),
                same.ToString());
            evaluate(
                user == same,
                "user == same",
                "The 2 should be the same.",
                user.ToString(),
                same.ToString());

            evaluate(
                user.Equals(different),
                "user == same",
                "The 2 should be the same.",
                user.ToString(),
                different.ToString());

            different.Name = "different";
            evaluate(
                false == user.Equals(different),
                "user != same",
                "The 2 should be NOT the same, the Name should be different.",
                user.ToString(),
                different.ToString());
            different.Name = "corywixom";

            different.UID = 1;
            evaluate(
                false == user.Equals(different),
                "user != same",
                "The 2 should be NOT the same, the UID should be different.",
                user.ToString(),
                different.ToString());
            different.UID = 99;

            different.GID = 2;
            evaluate(
                false == user.Equals(different),
                "user != same",
                "The 2 should be NOT the same, the GID should be different.",
                user.ToString(),
                different.ToString());
            different.GID = 89;

            different.Comment = "some different comment";
            evaluate(
                false == user.Equals(different),
                "user != same",
                "The 2 should be NOT the same, the Comment should be different.",
                user.ToString(),
                different.ToString());
            different.Comment = "some comment";

            different.Home = "/home";
            evaluate(
                false == user.Equals(different),
                "user != same",
                "The 2 should be NOT the same, the Home should be different.",
                user.ToString(),
                different.ToString());
            different.Home = "/root";

            different.Shell = "/something else";
            evaluate(
                false == user.Equals(different),
                "user != same",
                "The 2 should be NOT the same, the Shell should be different.",
                user.ToString(),
                different.ToString());
            different.Shell = "/bin/false";
        }

        [TestMethod]
        public void Test_User_Conditional_GTLT()
        {
            User user = new User(
                "corywixom",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            User same = new User(
                "corywixom",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            User different = new User(
                "a",
                99,
                89,
                "some comment",
                "/root",
                "/bin/false");

            evaluate(
                0 == user.CompareTo(same),
                "0 == user.CompareTo(same)",
                "The 2 should be the same.",
                user.ToString(),
                same.ToString());

            evaluate(
                0 != user.CompareTo(different),
                "0 != user.CompareTo(different)",
                "The 2 should NOT be the same.",
                user.ToString(),
                different.ToString());

            evaluate(
                user.CompareTo(different) > 0,
                "user.CompareTo(different) > 0",
                "user.CompareTo(different) > 0",
                user.ToString(),
                different.ToString());

            different.Name = "zzz";
            evaluate(
                user.CompareTo(different) < 0,
                "user.CompareTo(different) < 0",
                "user.CompareTo(different) < 0",
                user.ToString(),
                different.ToString());

        }

        [TestMethod]
        public void Test_UserSearchBuilder()
        {
            UserSearchBuilder builderCommentTest = new UserSearchBuilder();
            evaluate(
                builderCommentTest.SetComment("comment")
                    .Build()
                    .Comment.Equals("comment"),
                "builderCommentTest.SetComment",
                "The Comment should be set",
                builderCommentTest.Build().Comment,
                "comment");

            UserSearchBuilder builderGIDTest = new UserSearchBuilder();
            evaluate(
                builderGIDTest.SetGID(99)
                    .Build()
                    .GID == 99,
                "builderGIDTest.SetGID",
                "The GID should be set",
                builderGIDTest.Build().GID.ToString(),
                "99");

            UserSearchBuilder builderNameTest = new UserSearchBuilder();
            evaluate(
                builderNameTest.SetName("cory wixom")
                    .Build()
                    .Name == "cory wixom",
                "builderNameTest.SetName",
                "The Name should be set",
                builderNameTest.Build().Name.ToString(),
                "cory wixom");

            UserSearchBuilder builderUIDTest = new UserSearchBuilder();
            evaluate(
                builderUIDTest.SetUID(89)
                    .Build()
                    .UID == 89,
                "builderUIDTest.SetUID",
                "The UID should be set",
                builderUIDTest.Build().UID.ToString(),
                "89");

            UserSearchBuilder builderShellTest = new UserSearchBuilder();
            evaluate(
                builderShellTest.SetShell("/shell/test")
                    .Build()
                    .Shell == "/shell/test",
                "builderShellTest.SetShell",
                "The Shell should be set",
                builderShellTest.Build().Shell.ToString(),
                "/shell/test");

            UserSearchBuilder builderHomeTest = new UserSearchBuilder();
            evaluate(
                builderHomeTest.SetHome("/home/test")
                    .Build()
                    .Home == "/home/test",
                "builderHomeTest.SetShell",
                "The Home should be set",
                builderHomeTest.Build().Home.ToString(),
                "/home/test");
        }

        [TestMethod]
        public void Test_UserSearch()
        {
            User user = new User(
                "something",
                99,
                89,
                "something",
                "something",
                "something");

            evaluate(
                new UserSearchBuilder().Build().CheckSearch(user),
                "UserSearch Empty",
                "The UserSearch should return true",
                new UserSearchBuilder().Build().CheckSearch(user).ToString(),
                "true");

            //SetComment
            evaluate(
                new UserSearchBuilder().SetComment("something").Build().CheckSearch(user),
                "UserSearch SetComment true",
                "The UserSearch should return true: SetComment",
                new UserSearchBuilder().SetComment("something").Build().CheckSearch(user).ToString(),
                "true");
            evaluate(
                false == new UserSearchBuilder().SetComment("somethingbad").Build().CheckSearch(user),
                "UserSearch SetComment false",
                "The UserSearch should return false: SetComment",
                new UserSearchBuilder().SetComment("somethingbad").Build().CheckSearch(user).ToString(),
                "true");

            //SetName
            evaluate(
                new UserSearchBuilder().SetName("something").Build().CheckSearch(user),
                "UserSearch SetName true",
                "The UserSearch should return true: SetName",
                new UserSearchBuilder().SetName("something").Build().CheckSearch(user).ToString(),
                "true");
            evaluate(
                false == new UserSearchBuilder().SetName("somethingbad").Build().CheckSearch(user),
                "UserSearch SetName false",
                "The UserSearch should return false: SetName",
                new UserSearchBuilder().SetName("somethingbad").Build().CheckSearch(user).ToString(),
                "true");

            //SetHome
            evaluate(
                new UserSearchBuilder().SetHome("something").Build().CheckSearch(user),
                "UserSearch SetHome true",
                "The UserSearch should return true: SetHome",
                new UserSearchBuilder().SetHome("something").Build().CheckSearch(user).ToString(),
                "true");
            evaluate(
                false == new UserSearchBuilder().SetHome("somethingbad").Build().CheckSearch(user),
                "UserSearch SetHome false",
                "The UserSearch should return false: SetHome",
                new UserSearchBuilder().SetHome("somethingbad").Build().CheckSearch(user).ToString(),
                "true");

            //SetShell
            evaluate(
                new UserSearchBuilder().SetShell("something").Build().CheckSearch(user),
                "UserSearch SetShell true",
                "The UserSearch should return true: SetShell",
                new UserSearchBuilder().SetShell("something").Build().CheckSearch(user).ToString(),
                "true");
            evaluate(
                false == new UserSearchBuilder().SetShell("somethingbad").Build().CheckSearch(user),
                "UserSearch SetShell false",
                "The UserSearch should return false: SetShell",
                new UserSearchBuilder().SetShell("somethingbad").Build().CheckSearch(user).ToString(),
                "true");

            //SetGID
            evaluate(
                new UserSearchBuilder().SetGID(89).Build().CheckSearch(user),
                "UserSearch SetGID true",
                "The UserSearch should return true: SetGID",
                new UserSearchBuilder().SetGID(89).Build().CheckSearch(user).ToString(),
                "true");
            evaluate(
                false == new UserSearchBuilder().SetGID(69).Build().CheckSearch(user),
                "UserSearch SetGID false",
                "The UserSearch should return false: SetGID",
                new UserSearchBuilder().SetGID(69).Build().CheckSearch(user).ToString(),
                "true");

            //SetUID
            evaluate(
                new UserSearchBuilder().SetUID(99).Build().CheckSearch(user),
                "UserSearch SetUID true",
                "The UserSearch should return true: SetUID",
                new UserSearchBuilder().SetUID(99).Build().CheckSearch(user).ToString(),
                "true");
            evaluate(
                false == new UserSearchBuilder().SetUID(68).Build().CheckSearch(user),
                "UserSearch SetUID false",
                "The UserSearch should return false: SetUID",
                new UserSearchBuilder().SetUID(68).Build().CheckSearch(user).ToString(),
                "true");

        }

        #endregion

        #region Group Tests

        [TestMethod]
        public void Test_Group_ConstructorAndProperties()
        {
            Group group = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            evaluate(
                group.Name.Equals("corywixomsgroup"),
                "Group.Name",
                "The name does not match what was expected.",
                group.Name,
                "corywixom");

            evaluate(
                group.GID.Equals(79),
                "Group.GID",
                "The GID does not match what was expected.",
                group.GID.ToString(),
                "79");

            evaluate(
                ParameterChecker.EqualsIncludeNullList(group.Members, new string[] { "I", "dont", "know" }),
                "Group.Members",
                "The Members does not match what was expected.",
                group.ToString(),
                "{ \"I\", \"dont\", \"know\" }");
        }

        [TestMethod]
        public void Test_Group_Conditional_Equals()
        {
            Group group = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            Group same = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            Group different = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            evaluate(
                group.Equals(same),
                "group == same",
                "The 2 should be the same.",
                group.ToString(),
                same.ToString());
            evaluate(
                group == same,
                "group == same",
                "The 2 should be the same.",
                group.ToString(),
                same.ToString());
            evaluate(
                group.Equals(different),
                "group == same",
                "The 2 should be the same.",
                group.ToString(),
                different.ToString());

            different.Name = "different";
            evaluate(
                false == group.Equals(different),
                "group != same",
                "The 2 should be NOT the same, the Name should be different.",
                group.ToString(),
                different.ToString());
            different.Name = "corywixom";

            different.GID = 2;
            evaluate(
                false == group.Equals(different),
                "group != same",
                "The 2 should be NOT the same, the GID should be different.",
                group.ToString(),
                different.ToString());
            different.GID = 89;

            different.Members = new List<string>( new string[] { "I2", "dont", "know" });
            evaluate(
                false == group.Equals(different),
                "group != same",
                "The 2 should be NOT the same, the Members should be different.",
                group.ToString(),
                different.ToString());
            different.Members = new List<string>(new string[] { "I", "dont", "know" });
        }

        [TestMethod]
        public void Test_Group_Conditional_GTLT()
        {
            Group group = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            Group same = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            Group different = new Group(
                "a",
                79,
                new string[] { "I", "dont", "know" });

            evaluate(
                0 == group.CompareTo(same),
                "0 == group.CompareTo(same)",
                "The 2 should be the same.",
                group.ToString(),
                same.ToString());

            evaluate(
                0 != group.CompareTo(different),
                "0 != group.CompareTo(different)",
                "The 2 should NOT be the same.",
                group.ToString(),
                different.ToString());

            evaluate(
                group.CompareTo(different) > 0,
                "group.CompareTo(different) > 0",
                "group.CompareTo(different) > 0",
                group.ToString(),
                different.ToString());

            different.Name = "zzz";
            evaluate(
                group.CompareTo(different) < 0,
                "group.CompareTo(different) < 0",
                "group.CompareTo(different) < 0",
                group.ToString(),
                different.ToString());

        }

        [TestMethod]
        public void Test_GroupSearchBuilder()
        {
            GroupSearchBuilder builderGIDTest = new GroupSearchBuilder();
            evaluate(
                builderGIDTest.SetGID(99)
                    .Build()
                    .GID == 99,
                "builderGIDTest.SetGID",
                "The GID should be set",
                builderGIDTest.Build().GID.ToString(),
                "99");

            GroupSearchBuilder builderNameTest = new GroupSearchBuilder();
            evaluate(
                builderNameTest.SetName("cory wixom")
                    .Build()
                    .Name == "cory wixom",
                "builderNameTest.SetName",
                "The Name should be set",
                builderNameTest.Build().Name.ToString(),
                "cory wixom");

            GroupSearchBuilder builderMembersTest = new GroupSearchBuilder();
            evaluate(
                ParameterChecker.EqualsIncludeNullList(
                    builderMembersTest.AddMember("corywixom")
                        .Build()
                        .Members,
                    new string[] {"corywixom"}),
                "builderMembersTest.AddMember",
                "The member corywixom should be present",
               new ToStringHelper().Build( "members", builderMembersTest.Build().Members).ToString(),
                "new string[] { corywixom }");

            evaluate(
                ParameterChecker.EqualsIncludeNullList(
                    builderMembersTest.AddMember("cory2wixom")
                        .Build()
                        .Members,
                    new string[] { "corywixom", "cory2wixom" }),
                "builderMembersTest.AddMember_2",
                "The member corywixom and cory2wixom should be present",
               new ToStringHelper().Build("members", builderMembersTest.Build().Members).ToString(),
                "new string[] { corywixom, cory2wixom }");

            evaluate(
                ParameterChecker.EqualsIncludeNullList(
                    builderMembersTest.AddMembers( new string[] { "cory3wixom", "cory4wixom" })
                        .Build()
                        .Members,
                    new string[] { "corywixom", "cory2wixom", "cory3wixom", "cory4wixom" }),
                "builderMembersTest.AddMembers",
                "The members should all be present",
               new ToStringHelper().Build("members", builderMembersTest.Build().Members).ToString(),
                "new string[] { corywixom, cory2wixom, cory3wixom, cory4wixom }");

            evaluate(
                ParameterChecker.EqualsIncludeNullList(
                    builderMembersTest.SetMembers(new string[] { "cory3wixom", "cory4wixom" })
                        .Build()
                        .Members,
                    new string[] { "cory3wixom", "cory4wixom" }),
                "builderMembersTest.SetMembers",
                "The members should all be present",
               new ToStringHelper().Build("members", builderMembersTest.Build().Members).ToString(),
                "new string[] { cory3wixom, cory4wixom }");

        }


        [TestMethod]
        public void Test_GroupSearch()
        {
            Group group = new Group(
                "corywixomsgroup",
                79,
                new string[] { "I", "dont", "know" });

            evaluate(
                new GroupSearchBuilder().Build().CheckSearch(group),
                "GroupSearch Empty",
                "The GroupSearch should return true",
                new GroupSearchBuilder().Build().CheckSearch(group).ToString(),
                "true");

            //SetName
            evaluate(
                new GroupSearchBuilder().SetName("corywixomsgroup").Build().CheckSearch(group),
                "GroupSearch SetName true",
                "The GroupSearch should return true: SetName",
                new GroupSearchBuilder().SetName("corywixomsgroup").Build().CheckSearch(group).ToString(),
                "true");
            evaluate(
                false == new GroupSearchBuilder().SetName("somethingbad").Build().CheckSearch(group),
                "GroupSearch SetName false",
                "The GroupSearch should return false: SetName",
                new GroupSearchBuilder().SetName("somethingbad").Build().CheckSearch(group).ToString(),
                "true");

            //SetGID
            evaluate(
                new GroupSearchBuilder().SetGID(79).Build().CheckSearch(group),
                "GroupSearch SetGID true",
                "The GroupSearch should return true: SetGID",
                new GroupSearchBuilder().SetGID(79).Build().CheckSearch(group).ToString(),
                "true");
            evaluate(
                false == new GroupSearchBuilder().SetGID(69).Build().CheckSearch(group),
                "GroupSearch SetGID false",
                "The GroupSearch should return false: SetGID",
                new GroupSearchBuilder().SetGID(69).Build().CheckSearch(group).ToString(),
                "true");

            //AddMember 1
            evaluate(
                new GroupSearchBuilder().AddMember("I").Build().CheckSearch(group),
                "GroupSearch AddMember true",
                "The GroupSearch should return true: AddMember",
                new GroupSearchBuilder().AddMember("I").Build().CheckSearch(group).ToString(),
                "true");
            evaluate(
                false == new GroupSearchBuilder().AddMember("bad").Build().CheckSearch(group),
                "GroupSearch AddMember false",
                "The GroupSearch should return false: AddMember",
                new GroupSearchBuilder().AddMember("bad").Build().CheckSearch(group).ToString(),
                "true");

            //AddMember 2
            evaluate(
                new GroupSearchBuilder().AddMembers(new string[] { "I", "nope" }).Build().CheckSearch(group),
                "GroupSearch AddMember true",
                "The GroupSearch should return true: AddMembers",
                new GroupSearchBuilder().AddMembers(new string[] { "I", "nope" }).Build().CheckSearch(group).ToString(),
                "true");
            evaluate(
                false == new GroupSearchBuilder().AddMembers(new string[] { "a", "nope" }).Build().CheckSearch(group),
                "GroupSearch AddMember false",
                "The GroupSearch should return false: AddMembers",
                new GroupSearchBuilder().AddMembers(new string[] { "a", "nope" }).Build().CheckSearch(group).ToString(),
                "true");
        }

        #endregion
    }
}
