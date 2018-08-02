using System;
using System.Web;
using System.Runtime.Serialization;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Exceptions;
using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Business
{
    [DataContract]
    public class UserSearchRequest : IUserSearchRequest
    {
        public UserSearchRequest() { }


        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "uid")]
        public string UID  {get; set; }

        [DataMember(Name = "gid")]
        public string GID { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "home")]
        public string Home { get; set; }

        [DataMember(Name = "shell")]
        public string Shell { get; set; }

        [IgnoreDataMember]
        public bool IsAnythingSet
        {
            get
            {
                if (null != Name ||
                    null != Comment ||
                    null != Home ||
                    null != Shell ||
                    null != UID ||
                    null != GID)
                {
                    return true;
                }
                return false;
            }
        }

        public UserSearch ToUserSearch()
        {
            UserSearchBuilder builder = new UserSearchBuilder();

            if(null != Name)
            {
                builder.SetName(Name);
            }
            if (null != Comment)
            {
                builder.SetComment(Comment);
            }
            if (null != Home)
            {
                builder.SetHome(Home);
            }
            if (null != Shell)
            {
                builder.SetShell(Shell);
            }
            if (null != UID)
            {
                if (uint.TryParse(UID, out uint uid))
                {
                    builder.SetUID(uid);
                }
                else
                {
                    throw new HardFailureException(
                        "The UID is limited to an unsigned integer value. " +
                        "Please confirm that you are sending the correct values to the service " +
                        "Recevied: " + ParameterChecker.IsNull(UID));

                }  
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

            return builder.Build();
        }

        public void FromUserSearch(UserSearch search)
        {
            if( null == search)
            {
                throw new ArgumentNullException("search", "UserSearch canot be NULL.");
            }

            Name = search.IsNameSet ? search.Name : null;
            UID = search.IsUIDSet ? search.UID.ToString() : null;
            GID = search.IsGIDSet ? search.GID.ToString() : null;
            Comment = search.IsCommentSet ? search.Comment : null;
            Home = search.IsHomeSet ? search.Home : null;
            Shell = search.IsShellSet ? search.Shell : null;
        }

        public string ToQueryString()
        {
            string delimeter = "?";

            ToStringHelper sb = new ToStringHelper();

            append(ref delimeter, "name", Name, sb);
            append(ref delimeter, "uid", UID, sb);
            append(ref delimeter, "gid", GID, sb);
            append(ref delimeter, "comment", Comment, sb);
            append(ref delimeter, "home", Home, sb);
            append(ref delimeter, "shell", Shell, sb);

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
