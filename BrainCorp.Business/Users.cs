using System;
using System.Diagnostics;
using System.Collections.Generic;

using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;

namespace BrainCorp.Passwd.Business
{
    public class Users : IUserCollection
    {
        #region C-Tors

        public Users(
            IUserDataAccess dataAccess)
        {
            Process(dataAccess.GetAll());
        }

        public Users(
            IUserDataAccess dataAccess,
            ILogger logger)
        {
            _logger = logger;

            Process(dataAccess.GetAll());
        }

        #endregion

        #region Public Methods

        public List<User> GetAll()
        {
            return _list;
        }

        public List<User> Search(UserSearch search)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<User> totalList = new List<User>(INITIAL_CAPACITY);

            if (true == search.IsNameSet)
            {
                List<uint> nameList = null;

                if (false == _userNames.TryGetValue(search.Name, out nameList))
                {
                    nameList = new List<uint>(INITIAL_CAPACITY);
                }

                totalList.AddRange(getUsers(nameList));
            }
            if (true == search.IsUIDSet)
            {
                List<User> uidList = null;
                if( false ==_userUIDs.TryGetValue(search.UID, out uidList))
                {
                    uidList = new List<User>(INITIAL_CAPACITY);
                }
                totalList.AddRange(uidList);
            }
            if (true == search.IsGIDSet)
            {
                List<uint> gidList = null;
                if(false == _userGIDs.TryGetValue(search.GID, out gidList))
                {
                    gidList = new List<uint>(INITIAL_CAPACITY);
                }
                totalList.AddRange(getUsers(gidList));
            }
            if (true == search.IsCommentSet)
            {
                List<uint> commentList = null;
                if(false == _userComments.TryGetValue(search.Comment, out commentList))
                {
                    commentList = new List<uint>(INITIAL_CAPACITY);
                }
                totalList.AddRange(getUsers(commentList));
            }
            if (true == search.IsHomeSet)
            {
                List<uint> homeList = new List<uint>(INITIAL_CAPACITY);
                if(false ==_userHomes.TryGetValue(search.Home, out homeList))
                {
                    homeList = new List<uint>(INITIAL_CAPACITY);
                }
                totalList.AddRange(getUsers(homeList));
            }
            if (true == search.IsShellSet)
            {
                List<uint> shellList = new List<uint>(INITIAL_CAPACITY);
               if(false == _userShells.TryGetValue(search.Shell, out shellList))
                {
                    shellList = new List<uint>(INITIAL_CAPACITY);
                }
                totalList.AddRange(getUsers(shellList));
            }

            merge(ref totalList, search);

            stopwatch.Stop();
            logDebug("TimeToSearchUsers: " + stopwatch.ElapsedMilliseconds);

            return totalList;
        }

        public void Process(List<User> users)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _list = users;

            process(users);

            stopwatch.Stop();

            logDebug("TimeToLoadUsers: " + stopwatch.ElapsedMilliseconds);
        }

        public void Clear()
        {
            if (null == _userNames) { _userNames = new Dictionary<string, List<uint>>(); }
            if (null == _userUIDs) { _userUIDs = new Dictionary<uint, List<User>>(); }
            if (null == _userGIDs) { _userGIDs = new Dictionary<uint, List<uint>>(); }
            if (null == _userComments) { _userComments = new Dictionary<string, List<uint>>(); }
            if (null == _userHomes) { _userHomes = new Dictionary<string, List<uint>>(); }
            if (null == _userShells) { _userShells = new Dictionary<string, List<uint>>(); }

            _userNames.Clear();
            _userUIDs.Clear();
            _userGIDs.Clear();
            _userComments.Clear();
            _userHomes.Clear();
            _userShells.Clear();
        }

        #endregion

        #region Private Methods

        private List<User> getUsers(List<uint> uids)
        {
            List<User> users = new List<User>(uids.Count);

            foreach(uint uid in uids)
            {
                users.AddRange(_userUIDs[uid]);
            }

            return users;
        }

        private void merge(ref List<User> users, UserSearch search)
        {
            if(users.Count == 0) { return; }

            Dictionary<uint, uint> duplicates = new Dictionary<uint, uint>(users.Count);

            for ( int i = users.Count - 1; i >= 0; i--)
            {
                if( false == duplicates.TryGetValue(users[i].UID, out uint throwAway))
                {
                    duplicates.Add(users[i].UID, users[i].UID);
                }
                else
                {
                    users.RemoveAt(i);
                    continue;
                }

                if( false == search.CheckSearch(users[i]))
                {
                    users.RemoveAt(i);
                }
            }
        }

        private void process(List<User> users)
        {
            Clear();

            foreach (User user in users)
            {
                processName(user);
                processUID(user);
                processGID(user);
                processComment(user);
                processHome(user);
                processShell(user);
            }
        }

        private void processName(User user)
        {
            if(null == user)
            {
                throw new ArgumentNullException("user", "User cannot be NULL.");
            }
            if( null  == user.Name)
            {
                throw new ArgumentNullException("user.Name", "User.Name cannot be NULL.");
            }

            List<uint> userList = null;

            if(false == _userNames.TryGetValue(user.Name, out userList))
            {
                userList = new List<uint>(INITIAL_CAPACITY);
                userList.Add(user.UID);
                _userNames.Add(user.Name, userList);
            }
            else
            {
                userList.Add(user.UID);
            }
        }

        private void processUID(User user)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user", "User cannot be NULL.");
            }

            List<User> userList = null;

            if (false == _userUIDs.TryGetValue(user.UID, out userList))
            {
                userList = new List<User>(INITIAL_CAPACITY);
                userList.Add(user);
                _userUIDs.Add(user.UID, userList);
            }
            else
            {
                userList.Add(user);
            }
        }

         private void processGID(User user)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user", "User cannot be NULL.");
            }

            List<uint> userList = null;

            if (false == _userGIDs.TryGetValue(user.GID, out userList))
            {
                userList = new List<uint>(INITIAL_CAPACITY);
                userList.Add(user.UID);
                _userGIDs.Add(user.GID, userList);
            }
            else
            {
                userList.Add(user.UID);
            }
        }

        private void processComment(User user)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user", "User cannot be NULL.");
            }
            if (null == user.Comment)
            {
                throw new ArgumentNullException("user.Comment", "User.Comment cannot be NULL.");
            }

            List<uint> userList = null;

            if (false == _userComments.TryGetValue(user.Comment, out userList))
            {
                userList = new List<uint>(INITIAL_CAPACITY);
                userList.Add(user.UID);
                _userComments.Add(user.Comment, userList);
            }
            else
            {
                userList.Add(user.UID);
            }
        }

        private void processHome(User user)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user", "User cannot be NULL.");
            }
            if (null == user.Home)
            {
                throw new ArgumentNullException("user.Home", "User.Home cannot be NULL.");
            }

            List<uint> userList = null;

            if (false == _userHomes.TryGetValue(user.Home, out userList))
            {
                userList = new List<uint>(INITIAL_CAPACITY);
                userList.Add(user.UID);
                _userHomes.Add(user.Home, userList);
            }
            else
            {
                userList.Add(user.UID);
            }
        }

        private void processShell(User user)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user", "User cannot be NULL.");
            }
            if (null == user.Shell)
            {
                throw new ArgumentNullException("user.Shell", "User.Shell cannot be NULL.");
            }

            List<uint> userList = null;

            if (false == _userShells.TryGetValue(user.Shell, out userList))
            {
                userList = new List<uint>(INITIAL_CAPACITY);
                userList.Add(user.UID);
                _userShells.Add(user.Shell, userList);
            }
            else
            {
                userList.Add(user.UID);
            }
        }

        private void logDebug(string message)
        {
            if( null != _logger)
            {
                _logger.Write(message, LogEntrySeverityEnum.Debug);
            }
        }

        #endregion

        #region Private Attributes

        private ILogger _logger;
        private const int INITIAL_CAPACITY = 100;

        private Dictionary<string, List<uint>> _userNames =
            new Dictionary<string, List<uint>>();

        private Dictionary<uint, List<User>> _userUIDs =
            new Dictionary<uint, List<User>>();

        private Dictionary<uint, List<uint>> _userGIDs =
            new Dictionary<uint, List<uint>>();

        private Dictionary<string, List<uint>> _userComments =
            new Dictionary<string, List<uint>>();

        private Dictionary<string, List<uint>> _userHomes =
            new Dictionary<string, List<uint>>();

        private Dictionary<string, List<uint>> _userShells =
            new Dictionary<string, List<uint>>();

        private List<User> _list = new List<User>(INITIAL_CAPACITY);


        #endregion

    }
}
