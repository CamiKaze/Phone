using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;

namespace Contacts.MVC
{
    public static class GlobalVariables
    {
        public static HttpClient ContactsAPIClient = new HttpClient();

        static GlobalVariables()
        {// We can use this now within the Contacts.MVC controllers
            ContactsAPIClient.BaseAddress = new Uri("http://localhost:49908/api/");// set to API address (Contacts.API)
            ContactsAPIClient.DefaultRequestHeaders.Clear();
            ContactsAPIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}