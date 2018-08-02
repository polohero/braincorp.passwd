using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.DataAccess;

namespace BrainCorp.Passwd.Business
{
    public class Groups : IGroupCollection
    {
        #region C-Tors

        public Groups(
            IGroupDataAccess dataAccess)
        {
            Process(dataAccess.GetAll());
        }

        public Groups(
            List<Group> groups)
        {
            Process(groups);
        }

        public Groups(
            IGroupDataAccess dataAccess,
            ILogger logger)
        {
            _logger = logger;

            Process(dataAccess.GetAll());
        }

        public Groups(
            List<Group> groups,
            ILogger logger)
        {
            _logger = logger;

            Process(groups);
        }

        #endregion

        #region Public Methods

        public List<Group> GetAll()
        {
            return _list;
        }

        public List<Group> Search(GroupSearch search)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Group> totalList = new List<Group>(INITIAL_CAPACITY);

            if (true == search.IsNameSet)
            {
                List<uint> nameList = null;

                if (false == _groupNames.TryGetValue(search.Name, out nameList))
                {
                    nameList = new List<uint>(INITIAL_CAPACITY);
                }

                totalList.AddRange(getGroups(nameList));
            }
            if (true == search.IsGIDSet)
            {
                List<Group> gidList = null;
                if (false == _groupGIDs.TryGetValue(search.GID, out gidList))
                {
                    gidList = new List<Group>(INITIAL_CAPACITY);
                }
                totalList.AddRange(gidList);
            }
            if( true == search.IsMembersSet)
            {
                List<uint> totalMemberList = new List<uint>(INITIAL_CAPACITY);

                foreach (string member in search.Members)
                {
                    List<uint> memberList = null;

                    if (false == _groupMembers.TryGetValue(member, out memberList))
                    {
                        memberList = new List<uint>(INITIAL_CAPACITY);
                    }

                    totalMemberList.AddRange(memberList);
                }

                totalList.AddRange(getGroups(totalMemberList));
            }

            merge(ref totalList, search);

            stopwatch.Stop();
            logDebug("TimeToSearchGroups: " + stopwatch.ElapsedMilliseconds);

            return totalList;
        }

        public void Process(List<Group> groups)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _list = groups;

            process(groups);

            stopwatch.Stop();

            logDebug("TimeToLoadGroups: " + stopwatch.ElapsedMilliseconds);
        }

        public void Clear()
        {
            if (null == _groupGIDs) { _groupGIDs = new Dictionary<uint, List<Group>>(); }
            if (null == _groupNames) { _groupNames = new Dictionary<string, List<uint>>(); }
            if (null == _groupMembers) { _groupMembers = new Dictionary<string, List<uint>>(); }

            _groupGIDs.Clear();
            _groupNames.Clear();
            _groupMembers.Clear();
        }

        #endregion

        #region Private Methods

        private List<Group> getGroups(List<uint> gids)
        {
            List<Group> groups = new List<Group>(gids.Count);

            foreach (uint gid in gids)
            {
                groups.AddRange(_groupGIDs[gid]);
            }

            return groups;
        }

        private void merge(ref List<Group> groups, GroupSearch search)
        {
            if (groups.Count == 0) { return; }

            Dictionary<uint, uint> duplicates = new Dictionary<uint, uint>(groups.Count);

            for (int i = groups.Count - 1; i >= 0; i--)
            {
                if (false == duplicates.TryGetValue(groups[i].GID, out uint throwAway))
                {
                    duplicates.Add(groups[i].GID, groups[i].GID);
                }
                else
                {
                    groups.RemoveAt(i);
                    continue;
                }

                if (false == search.CheckSearch(groups[i]))
                {
                    groups.RemoveAt(i);
                }
            }
        }

        private void process(List<Group> groups)
        {
            Clear();

            foreach (Group group in groups)
            {
                processName(group);
                processGID(group);
                processMembers(group);
            }
        }

        private void processName(Group group)
        {
            if (null == group)
            {
                throw new ArgumentNullException("group", "group cannot be NULL.");
            }
            if (null == group.Name)
            {
                throw new ArgumentNullException("group.Name", "group.Name cannot be NULL.");
            }

            List<uint> groupList = null;

            if (false == _groupNames.TryGetValue(group.Name, out groupList))
            {
                groupList = new List<uint>(INITIAL_CAPACITY);
                groupList.Add(group.GID);
                _groupNames.Add(group.Name, groupList);
            }
            else
            {
                groupList.Add(group.GID);
            }
        }

        private void processMembers(Group group)
        {
            if (null == group)
            {
                throw new ArgumentNullException("group", "group cannot be NULL.");
            }
            if (null == group.Members)
            {
                throw new ArgumentNullException("group.Members", "group.Name cannot be NULL.");
            }

            foreach(string member in group.Members)
            {
                List<uint> memberList = null;

                if (false == _groupMembers.TryGetValue(member, out memberList))
                {
                    memberList = new List<uint>(INITIAL_CAPACITY);
                    memberList.Add(group.GID);
                    _groupMembers.Add(member, memberList);
                }
                else
                {
                    memberList.Add(group.GID);
                }
            }
        }

        private void processGID(Group group)
        {
            if (null == group)
            {
                throw new ArgumentNullException("group", "group cannot be NULL.");
            }

            List<Group> groupList = null;

            if (false == _groupGIDs.TryGetValue(group.GID, out groupList))
            {
                groupList = new List<Group>(INITIAL_CAPACITY);
                groupList.Add(group);
                _groupGIDs.Add(group.GID, groupList);
            }
            else
            {
                groupList.Add(group);
            }
        }


        private void logDebug(string message)
        {
            if (null != _logger)
            {
                _logger.Write(message, LogEntrySeverityEnum.Debug);
            }
        }

        #endregion

        #region Private Attributes

        private ILogger _logger;
        private const int INITIAL_CAPACITY = 100;

        private Dictionary<string, List<uint>> _groupNames =
            new Dictionary<string, List<uint>>();

        private Dictionary<uint, List<Group>> _groupGIDs =
            new Dictionary<uint, List<Group>>();

        private Dictionary<string, List<uint>> _groupMembers =
            new Dictionary<string, List<uint>>();

        private List<Group> _list = new List<Group>(INITIAL_CAPACITY);


        #endregion
    }
}
