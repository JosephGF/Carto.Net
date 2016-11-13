using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCarto.Core
{
    public class Autentication
    {
        private static Autentication _autentication = null;
        private string _apiKey = String.Empty;
        private string _userName = string.Empty;

        public string ApiKey { get { return _apiKey; } }
        public string UserName { get { return _userName; } }

        public Autentication (string userName)
        {
            _userName = userName;
        }

        public Autentication(string userName, string apiKey)
        {
            _userName = userName;
            _apiKey = apiKey;
        }

        public static Autentication Autenticate(string userName, string apikey)
        {
            _autentication = _autentication == null ?  new Autentication(userName, apikey) : _autentication;
            return _autentication;
        }
    }
}
