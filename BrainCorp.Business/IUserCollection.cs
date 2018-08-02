using System.Collections.Generic;

using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Business
{
    public interface IUserCollection
    {
        List<User> GetAll();

        List<User> Search(UserSearch search);

        void Process(List<User> userList);

        void Clear();
    }
}
