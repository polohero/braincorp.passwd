using System;
using System.Web;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Exceptions;
using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Business
{
    public class GroupSearchRequest : IGroupSearchRequest
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "gid")]
        public string GID { get; set; }

        [DataMember(Name = "member")]
        public List<string> Member { get; set; }

        [IgnoreDataMember]
        public bool IsAnythingSet
        {
            get
            {
                if (null != Name ||
                    null != GID ||
                    (null != Member && 
                    Member.Count > 0))
                {
                    return true;
                }
                return false;
            }
        }

        public GroupSearch ToGroupSearch()
        {
            GroupSearchBuilder builder = new GroupSearchBuilder();

            if(null != Name)
            {
                builder.SetName(Name);
            }

            if (null != GID)
            {
                if (uint.TryParse(GID, out uint gid))
                {
                    builder.SetGID(gid);
                }
                else
                {
                    throw new HardFailureException(
                        "The GID is limited to an unsigned integer value. " +
                        "Please confirm that you are sending the correct values to the service " +
                        "Recevied: " + ParameterChecker.IsNull(GID));

                }
            }

            if( null != Member)
            {
                foreach(string member in Member)
                {
                    builder.AddMember(member);
                }
            }

            return builder.Build();
        }

        public void FromGroupSearch(GroupSearch search)
        {
            if (null == search)
            {
                throw new ArgumentNullException("search", "UserSearch canot be NULL.");
            }

            Name = search.IsNameSet ? search.Name : null;
            GID = search.IsGIDSet ? search.GID.ToString() : null;
            Member = search.IsMembersSet ? search.Members : new List<string>();

        }

        public string ToQueryString()
        {
            string delimeter = "?";

            ToStringHelper sb = new ToStringHelper();

            append(ref delimeter, "name", Name, sb);
            append(ref delimeter, "gid", GID, sb);
            
            foreach(string member in Member)
            {
                append(ref delimeter, "member", member, sb);
            }

            return sb.ToString();
        }

        private static void append(ref string delimeter, string name, string value, ToStringHelper sb)
        {
            if (null != value)
            {
                sb.Append(delimeter + name + "=" + HttpUtility.UrlEncode(value));
                delimeter = "&";
            }
        }
    }
}
