using System.Collections.Generic;

using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Client
{
    public interface IPasswdClient
    {
        List<User> GetAllUsers();

        List<User> Search(UserSearch request);

        List<Group> Search(GroupSearch request);

        List<Group> GetAllGroups();

        List<Group> GetGroups(uint uid);

        Group GetGroup(uint gid);

        User GetUser(uint uid);
    }
}
