using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Entities
{
    /// <summary>
    /// A Group is a single record of a group.
    /// </summary>
    [DataContract]
    public class Group : IComparable, IComparable<Group>
    {
        #region C-Tors

        public Group() { }

        public Group(
            string name,
            uint gid,
            IEnumerable<string> members)
        {
            Name = name;
            GID = gid;
            Members = members == null ? null : new List<string>(members);
        }

        #endregion

        #region Public Properties

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "gid")]
        public uint GID { get; set; }

        [DataMember(Name = "members", EmitDefaultValue = false)]
        public List<string> Members { get; set; }

        #endregion

        #region Public Methods

        #region Comparators

        public int CompareTo(object other)
        {
            if (null == other)
            {
                return -1;
            }
            else if (false == other is Group)
            {
                return -1;
            }

            return CompareTo((Group)other);
        }

        public int CompareTo(Group user)
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
            else if (false == obj is Group)
            {
                return false;
            }

            Group other = (Group)obj;

            bool result = true;

            result &= ParameterChecker.EqualsIncludeNull(Name, other.Name);
            result &= ParameterChecker.EqualsIncludeNull(GID, other.GID);
            result &= ParameterChecker.EqualsIncludeNullList(Members, other.Members);

            return result;
        }

        public static bool operator ==(Group obj1, Group obj2)
        {
            return true == ParameterChecker.EqualsIncludeNull(obj1, obj2);
        }
        public static bool operator !=(Group obj1, Group obj2)
        {
            return false == ParameterChecker.EqualsIncludeNull(obj1, obj2);
        }

        public static bool operator <(Group obj1, Group obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) < 0;
        }

        public static bool operator >(Group obj1, Group obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) > 0;
        }

        public static bool operator <=(Group obj1, Group obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) <= 0;
        }

        public static bool operator >=(Group obj1, Group obj2)
        {
            return ParameterChecker.CompareToIncludeNull(obj1, obj2) >= 0;
        }

        #endregion

        /// <summary>
        /// Returns the Hascode of the Group;
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Returns a logging Friendly version of the Group.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            ToStringHelper sb = new ToStringHelper();

            sb.Append("User");
            sb.AppendProperty("Name", Name);
            sb.AppendProperty("GID", GID);
            sb.AppendProperty("Members", Members);


            return sb.ToString();
        }

        #endregion
    }
}
