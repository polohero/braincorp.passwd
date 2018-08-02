using BrainCorp.Passwd.Entities;

namespace BrainCorp.Passwd.Business
{
    public interface IGroupSearchRequest
    {
        GroupSearch ToGroupSearch();

        bool IsAnythingSet { get; }

        string ToQueryString();

        void FromGroupSearch(GroupSearch search);
    }
}
