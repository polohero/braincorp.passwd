using System;
using System.Runtime.Serialization;

using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Entities
{
    [DataContract]
    public class UserSearch
    {

        public UserSearch()
        {

        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "uid")]
        public uint UID
        {
            get
            {
                return _uid;
            }
            set
            {
                IsUIDSet = true;
                _uid = value;
            }
        }

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

        [DataMember(Name = "comment")]
        public string Comment{ get; set; }

        [DataMember(Name = "home")]
        public string Home { get; set; }

        [DataMember(Name = "shell")]
        public string Shell { get; set; }


        [DataMember(Name = "isGIDSet")]
        public bool IsGIDSet { get; set; }

        [DataMember(Name = "isUIDSet")]
        public bool IsUIDSet { get; set; }
        public bool IsNameSet { get { return false == (null == Name); } }
        public bool IsCommentSet { get { return false == (null == Comment); } }
        public bool IsShellSet { get { return false == (null == Shell); } }
        public bool IsHomeSet { get { return false == (null == Home); } }


        public override string ToString()
        {
            return new ToStringHelper()
                .Build("Name", Name)
                .Build("UID", UID)
                .Build("GID", GID)
                .Build("Comment", Comment)
                .Build("Home", Home)
                .Build("Shell", Shell)
                .ToString();
        }

        public bool CheckSearch(User user)
        {
            if( null == user)
            {
                throw new ArgumentNullException("user", "The user cannot be NULL in a search.");
            }

            bool result = true;

            result &= true == IsNameSet ? Name.Equals(user.Name) : true;
            result &= true == IsUIDSet ? UID.Equals(user.UID) : true;
            result &= true == IsGIDSet ? GID.Equals(user.GID) : true;
            result &= true == IsCommentSet ? Comment.Equals(user.Comment) : true;
            result &= true == IsHomeSet ? Home.Equals(user.Home) : true;
            result &= true == IsShellSet ? Shell.Equals(user.Shell) : true;

            return result;
        }

        private uint _gid;
        private uint _uid;

    }
}
