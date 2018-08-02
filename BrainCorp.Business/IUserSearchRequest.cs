using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Business
{
    public interface IUserSearchRequest
    {
        UserSearch ToUserSearch();

        bool IsAnythingSet { get; }

        string ToQueryString();

        void FromUserSearch(UserSearch search);
    }
}
