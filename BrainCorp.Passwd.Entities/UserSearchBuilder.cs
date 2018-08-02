
using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Entities
{
    public class UserSearchBuilder
    {
        #region C-Tors

        public UserSearchBuilder()
        {
            SearchCritera = new UserSearch();
        }

        #endregion

        #region Public Properties

        public UserSearch SearchCritera { get; set; }

        #endregion

        #region Public Methods

        public UserSearchBuilder SetName(string name)
        {
            ParameterChecker.NullCheck(
                "name",
                name,
                "The name field cannot be null for a UserSearch.");

            SearchCritera.Name = name;

            return this;
        }

        public UserSearchBuilder SetComment(string comment)
        {
            ParameterChecker.NullCheck(
                "comment",
                comment,
                "The comment field cannot be null for a UserSearch.");

            SearchCritera.Comment = comment;

            return this;
        }

        public UserSearchBuilder SetHome(string home)
        {
            ParameterChecker.NullCheck(
                "home",
                home,
                "The home field cannot be null for a UserSearch.");

            SearchCritera.Home = home;

            return this;
        }

        public UserSearchBuilder SetShell(string shell)
        {
            ParameterChecker.NullCheck(
                "shell",
                shell,
                "The shell field cannot be null for a UserSearch.");

            SearchCritera.Shell = shell;

            return this;
        }

        public UserSearchBuilder SetUID(uint uid)
        {
            SearchCritera.UID = uid;

            return this;
        }

        public UserSearchBuilder SetGID(uint gid)
        {
            SearchCritera.GID = gid;

            return this;
        }

        public UserSearch Build()
        {
            return SearchCritera;
        }

        #endregion
    }
}
