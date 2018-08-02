using System.Collections.Generic;

using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Entities
{
    public class GroupSearchBuilder
    {
        #region C-Tors

        public GroupSearchBuilder()
        {
            SearchCritera = new GroupSearch();
        }

        #endregion

        #region Public Properties

        public GroupSearch SearchCritera { get; set; }

        #endregion

        #region Public Methods

        public GroupSearchBuilder SetName(string name)
        {
            ParameterChecker.NullCheck(
                "name",
                name,
                "The name field cannot be null for a GroupSearch.");

            SearchCritera.Name = name;

            return this;
        }

        public GroupSearchBuilder SetGID(uint gid)
        {
            SearchCritera.GID = gid;

            return this;
        }

        public GroupSearchBuilder AddMember(string member)
        {
            ParameterChecker.NullCheck(
                "member",
                member,
                "The member field cannot be null for a GroupSearch.");

            SearchCritera.Members.Add(member);

            return this;
        }

        public GroupSearchBuilder AddMembers(IEnumerable<string> members)
        {
            ParameterChecker.NullCheck(
                "members",
                members,
                "The members field cannot be null for a GroupSearch.");

            SearchCritera.Members.AddRange(members);

            return this;
        }


        public GroupSearchBuilder SetMembers(IEnumerable<string> members)
        {
            ParameterChecker.NullCheck(
                "members",
                members,
                "The members field cannot be null for a GroupSearch.");

            SearchCritera.Members = new List<string>(members);

            return this;
        }


        public GroupSearch Build()
        {
            return SearchCritera;
        }

        #endregion
    }
}
