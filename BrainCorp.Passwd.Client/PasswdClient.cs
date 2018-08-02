using System;
using System.Net.Http;
using System.Collections.Generic;

using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.Client
{
    public class PasswdClient : PasswdClientBase, IPasswdClient
    {
        #region C-Tor

        public PasswdClient(PasswdClientFactory clientFactory)
            : base(clientFactory)
        {

        }

        #endregion

        #region Public Methods


        public List<Group> GetAllGroups()
        {
            var response = GetHttpClient().GetAsync("groups").Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<List<Group>>().Result;
        }

        public List<User> GetAllUsers()
        {
            var response = GetHttpClient().GetAsync("users").Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<List<User>>().Result;
        }

        public List<Group> GetGroups(uint uid)
        {
            var response = GetHttpClient().GetAsync(string.Format("users/{0}/groups", uid)).Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<List<Group>>().Result;
        }

        public List<User> Search(UserSearch search)
        {
            UserSearchRequest request = new UserSearchRequest();
            request.FromUserSearch(search);

            string requestURL = "users/query" + request.ToQueryString();

            var response = GetHttpClient().GetAsync(requestURL).Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<List<User>>().Result;
        }

        public List<Group> Search(GroupSearch search)
        {
            GroupSearchRequest request = new GroupSearchRequest();
            request.FromGroupSearch(search);

            string requestURL = "group/query" + request.ToQueryString();

            var response = GetHttpClient().GetAsync(requestURL).Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<List<Group>>().Result;
        }

        public Group GetGroup(uint gid)
        {
            var response = GetHttpClient().GetAsync(string.Format("groups/{0}", gid)).Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<Group>().Result;
        }

        public User GetUser(uint uid)
        {
            var response = GetHttpClient().GetAsync(string.Format("users/{0}", uid)).Result;

            if (!response.IsSuccessStatusCode)
            {
                ToApplicationException(response);
            }

            return response.Content.ReadAsAsync<User>().Result;
        }

        #endregion

    }
}
