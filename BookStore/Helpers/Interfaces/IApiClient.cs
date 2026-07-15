using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers.Interfaces
{
    public interface IApiClient
    {
        RestResponse ExecuteRequest(RestRequest request);
    }
}
