using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Business
{
    public interface IGroupCollection
    {
        List<Group> GetAll();

        List<Group> Search(GroupSearch search);

        void Process(List<Group> groupList);

        void Clear();
    }
}
