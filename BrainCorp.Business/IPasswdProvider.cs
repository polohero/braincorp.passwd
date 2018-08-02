using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Business
{
    public interface IPasswdProvider
    {
        List<User> GetAllUsers();

        List<User> Search(IUserSearchRequest request);

        List<Group> Search(IGroupSearchRequest request);

        List<Group> GetAllGroups();

        List<Group> GetGroups(IUserSearchRequest request);

    }
}
