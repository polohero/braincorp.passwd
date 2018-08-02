using System;
using BrainCorp.Passwd.Client;

namespace BrainCorp.Passwd.Testing.IntegrationTests
{
    public class PasswdWebServiceConfiguration : IWebServiceConfiguration
    {
        public Uri Uri
        {
            get { return new Uri(Url); }
        }

        public string Url
        {
            get { return "http://www.corywixom.com/api/"; }
        }
    }
}
