using System;
using System.Collections.Generic;

using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Caching;

namespace BrainCorp.Passwd.Business
{
    public class PasswdProvider : IPasswdProvider
    {
        #region C-Tors

        public PasswdProvider(
            IUserDataAccess userDataAccess,
            IGroupDataAccess groupDataAccess,
            ICaching cache,
            ILogger log,
            IConfigValues config)
        {
            _cache = cache;
            _log = log;
            _config = config;

            CacheItem cachedUser = _cache.Get(USER_CACHE_KEY);
            CacheItem cachedGroup = _cache.Get(GROUP_CACHE_KEY);

            if (null == cachedUser ||
                null == cachedUser.Obj ||
                cachedUser.TimeAddedUTC < getUsersLastModified(userDataAccess, log, config))
            {
                _users = loadUsers(userDataAccess, log, config, cache, cachedUser);
            }
            else
            {
                _users = (Users)cachedUser.Obj;
            }

            if (null == cachedGroup ||
                null == cachedGroup.Obj ||
                cachedGroup.TimeAddedUTC < getGroupLastModified(groupDataAccess, log, config))
            {
                _groups = loadGroups(groupDataAccess, log, config, cache, cachedGroup);
            }
            else
            {
                _groups = (Groups)cachedGroup.Obj;
            }
        }

        #endregion

        #region Public Methods

        public List<Group> GetAllGroups()
        {
            return _groups.GetAll();
        }

        public List<User> GetAllUsers()
        {
            return _users.GetAll();
        }

        public List<User> Search(IUserSearchRequest request)
        {
            UserSearch search = request.ToUserSearch();

            return _users.Search(search);
        }

        public List<Group> Search(IGroupSearchRequest request)
        {
            GroupSearch search = request.ToGroupSearch();

            return _groups.Search(search);
        }

        public List<Group> GetGroups(IUserSearchRequest request)
        {
            List<User> list = Search(request);

            if (list.Count != 1)
            {
                return new List<Group>(0);
            }

            // I've read that the /etc/groups may not have the
            // "primary" group assigned in the file, so I'll find that
            // as well.
            List<Group> groups = _groups.Search(new GroupSearchBuilder().AddMember(list[0].Name).Build());
            merge(ref groups, _groups.Search(new GroupSearchBuilder().SetGID(list[0].GID).Build()));

            return groups;
        }

        #endregion

        #region Private Methods

        private DateTime getUsersLastModified(
            IUserDataAccess userDataAccess,
            ILogger log,
            IConfigValues config)
        {
            // Just preset to something in the past.
            DateTime date = DateTime.UtcNow.AddDays(-1);

            try
            {
                date = userDataAccess.LastUpdatedUTC;
            }
            catch (Exception exception)
            {
                log.Write(
                    "An error occurred trying to load the USER LAST MODIFIED (LastUpdatedUTC) information. " +
                    "Likely this is because of a corrupted data in the file. " +
                    (config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile
                        ? "This request will fail, and it's likely that all other requests " +
                          "will as well. Please have someone look at this as soon as possible."
                        : "The service is configured to use the cached data, so the service will " +
                          "continue to work as normal (just without the changes made to the file. " +
                          "Someone will need to address the file issue soon, as USER data will get stale." + 
                          "It is possible there is some BLIP that is causing the file to not be accessible too. " + 
                          "If this error is repeating please escalate ASAP. If it's not, that would indicate a short " + 
                          "term connectivity/permissions issue that was likely self corrected."),
                    LogEntrySeverityEnum.Error,
                    exception);


                if (config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile)
                {
                    throw;
                }
            }

            return date;

        }

        private DateTime getGroupLastModified(
            IGroupDataAccess groupDataAccess,
            ILogger log,
            IConfigValues config)
        {
            // Just preset to something in the past.
            DateTime date = DateTime.UtcNow.AddDays(-1);

            try
            {
                date = groupDataAccess.LastUpdatedUTC;
            }
            catch (Exception exception)
            {
                log.Write(
                    "An error occurred trying to load the GROUP LAST MODIFIED (LastUpdatedUTC) information. " +
                    "Likely this is because of a corrupted data in the file. " +
                    (config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile
                        ? "This request will fail, and it's likely that all other requests " +
                          "will as well. Please have someone look at this as soon as possible."
                        : "The service is configured to use the cached data, so the service will " +
                          "continue to work as normal (just without the changes made to the file. " +
                          "Someone will need to address the file issue soon, as GROUP data will get stale." +
                          "It is possible there is some BLIP that is causing the file to not be accessible too. " +
                          "If this error is repeating please escalate ASAP. If it's not, that would indicate a short " +
                          "term connectivity/permissions issue that was likely self corrected."),
                    LogEntrySeverityEnum.Error,
                    exception);


                if (config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile)
                {
                    throw;
                }
            }

            return date;
        }

        private void merge(ref List<Group> groups, List<Group> groupsToAdd)
        {
            Dictionary<uint, uint> _groupIDs = new Dictionary<uint, uint>(groups.Count + groupsToAdd.Count);

            foreach(Group group in groups)
            {
                _groupIDs.Add(group.GID, group.GID);
            }

            foreach(Group group in groupsToAdd)
            {
                if( false == _groupIDs.ContainsKey(group.GID))
                {
                    groups.Add(group);
                }
            }
        }

        private Users loadUsers(
            IUserDataAccess userDataAccess, 
            ILogger log,
            IConfigValues config,
            ICaching cache,
            CacheItem cachedUser)
        {
            Users user = null;

            try
            {
                user = new Users(userDataAccess, log);
                cache.Insert(USER_CACHE_KEY, user);
            }
            catch(Exception exception)
            {
                log.Write(
                    "An error occurred trying to load the USER information. " +
                    "Likely this is because of a corrupted data in the file. " +
                    (config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile
                        ? "This request will fail, and it's likely that all other requests " + 
                          "will as well. Please have someone look at this as soon as possible."
                        : "The service is configured to use the cached data, so the service will " + 
                          "continue to work as normal (just without the changes made to the file. " + 
                          "Someone will need to address the file issue soon, as USER data will get stale."),
                    LogEntrySeverityEnum.Error,
                    exception);


                if( null == cachedUser ||
                    null == cachedUser.Obj ||
                    config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile)
                {
                    throw;
                }

                user = (Users)cachedUser.Obj;
            }

            return user;
        }

        private Groups loadGroups(
            IGroupDataAccess groupDataAccess,
            ILogger log,
            IConfigValues config,
            ICaching cache,
            CacheItem cachedGroup)
        {
            Groups group = null;

            try
            {
                group = new Groups(groupDataAccess, log);
                cache.Insert(GROUP_CACHE_KEY, group);
            }
            catch (Exception exception)
            {
                log.Write(
                    "An error occurred trying to load the GROUP information. " +
                    "Likely this is because of a corrupted data in the file. " +
                    (config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile
                        ? "This request will fail, and it's likely that all other requests " +
                          "will as well. Please have someone look at this as soon as possible."
                        : "The service is configured to use the cached data, so the service will " +
                          "continue to work as normal (just without the changes made to the file. " +
                          "Someone will need to address the file issue soon, as GROUP data will get stale."),
                    LogEntrySeverityEnum.Error,
                    exception);


                if (null == cachedGroup ||
                    null == cachedGroup.Obj ||
                    config.FailBehavior == FailBehaviorEnum.FailWithCorruptedFile)
                {
                    throw;
                }

                group = (Groups)cachedGroup.Obj;
            }

            return group;
        }

        #endregion

        #region Private Attributes

        private const string USER_CACHE_KEY = "USERS";
        private const string GROUP_CACHE_KEY = "GROUPS";

        private Users _users;
        private Groups _groups;

        private ICaching _cache;
        private ILogger _log;

        private IConfigValues _config;

        #endregion
    }
}
