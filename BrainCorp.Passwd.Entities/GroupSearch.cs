using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BrainCorp.Passwd.Common.Utilities;
using BrainCorp.Passwd.Common.Extensions;

namespace BrainCorp.Passwd.Entities
{
    [DataContract]
    public class GroupSearch
    {
        public GroupSearch()
        {
            const int INITIAL_CAPACITY = 50;

            Members = new List<string>(INITIAL_CAPACITY);
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "gid")]
        public uint GID
        {
            get
            {
                return _gid;
            }
            set
            {
                IsGIDSet = true;
                _gid = value;
            }
        }

        [DataMember(Name = "members")]
        public List<string> Members { get; set; }

        [DataMember(Name = "isGIDSet")]
        public bool IsGIDSet { get; set; }

        public bool IsNameSet
        {
            get
            {
                return null == Name ? false : true;
            }
        }
        public bool IsMembersSet
        {
            get
            {
                return Members.Count == 0 ? false : true;
            }
        }

        public override string ToString()
        {
            return new ToStringHelper()
                .Build("Name", Name)
                .Build("GID", GID)
                .Build("Members", Members)
                .ToString();
        }

        public bool CheckSearch(Group group)
        {
            if (null == group)
            {
                throw new ArgumentNullException("group", "The group cannot be NULL in a search.");
            }

            bool result = true;

            result &= true == IsNameSet ? Name.Equals(group.Name): true ;
            result &= true == IsGIDSet ? GID.Equals(group.GID) : true;
            result &= true == IsMembersSet ? Members.ContainsAny(group.Members) : true;

            return result;
        }

        private uint _gid;
    }
}
