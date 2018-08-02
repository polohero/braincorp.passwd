using System;

namespace BrainCorp.Passwd.Client
{
    public interface IWebServiceConfiguration
    {
        Uri Uri { get; }

        string Url { get; }
       
    }
}
