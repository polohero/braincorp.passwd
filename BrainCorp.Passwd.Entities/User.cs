using System;
using System.Runtime.Serialization;

using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Entities
{
    /// <summary>
    /// A user is a singe record of a user. 
    /// </summary>
    [DataContract]
    public class User : IComparable, IComparable<User>
    {
        #region C-Tors

        public User() { }
        public User(
            string name,
            uint uid,
            uint gid,
            string comment,
            string home,
            string shell)
        {
            Name = name;
            UID = uid;
            GID = gid;
            Comment = comment;
            Home = home;
            Shell = shell;
        }

        #endregion

        #region Public Properties

        [DataMember(Name ="name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "uid")]
        public uint UID { get; set; }

        [DataMember(Name = "gid")]
        public uint GID { get; set; }

        [DataMember(Name = "comment", EmitDefaultValue = false)]
        public string Comment { get; set; }

        [DataMember(Name = "home", EmitDefaultValue = false)]
        public string Home { get; set; }

        [DataMember(Name = "shell", EmitDefaultValue = false)]
        public string Shell { get; set; }

        #endregion

        #region Public Methods

        #region Comparators

        public int CompareTo(object other)
        {
            if (null == other)
            {
                return -1;
            }
            else if (false == other is User)
            {
                return -1;
            }

            return CompareTo((User)other);
        }

        public int CompareTo(User user)
        {
            if (null == user)
            {
                return -1;
            }

            if (null == Name || user.Name == null)
            {
                return -1;
            }

            return Name.CompareTo(user.Name);
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            else if (false == obj is User)
            {
                return false;
            }

            User other = (User)obj;

            bool result = true;

            result &= ParameterChecker.EqualsIncludeNull(Name, other.Name);
            result &= ParameterChecker.EqualsIncludeNull(UID, other.UID);
            result &= ParameterChecker.EqualsIncludeNull(GID, other.GID);
            result &= ParameterChecker.EqualsIncludeNull(Comment, other.Comment);
            result &= ParameterChecker.EqualsIncludeNull(Home, other.Home);
            result &= ParameterChecker.EqualsIncludeNull(Shell, other.Shell);

            return result;
        }

        public static bool operator == (User obj1, User obj2)
        {
            return true == ParameterChecker.EqualsIncludeNull(obj1, obj2);
        }
        public static bool operator != (User obj1, User obj2)
        {
            return false == ParameterChecker.EqualsIncludeNull(obj1, obj2);
        }

        public static bool operator < (User obj1, User obj2)
        {
            return  ParameterChecker.CompareToIncludeNull(obj1, obj2) < 0;
        }

        public static bool operator >(User obj1, User obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) > 0;
        }

        public static bool operator <= (User obj1, User obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) <= 0;
        }

        public static bool operator >=(User obj1, User obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) >= 0;
        }

        #endregion

        /// <summary>
        /// Returns the Hascode of the User;
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Returns a logging Friendly version of the User.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            ToStringHelper sb = new ToStringHelper();

            sb.Append("User");
            sb.AppendProperty("Name", Name);
            sb.AppendProperty("UID", UID);
            sb.AppendProperty("GID", GID);
            sb.AppendProperty("Comment", Comment);
            sb.AppendProperty("Home", Home);
            sb.AppendProperty("Shell", Shell);

            return sb.ToString();
        }

        #endregion
    }
}
