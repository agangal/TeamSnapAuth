using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamSnapAuth.Library;
using Windows.Storage;

namespace TeamSnapAuth
{
    public class LibraryManager
    {
        private string clientID;
        private string authEndPoint;
        private string callBackURL;
        public Library_APICall APILibrary;
        public Library_TokenAuth TokenLibrary;
        public LibraryManager()
        {
            
            APILibrary = new Library_APICall();
            TokenLibrary = new Library_TokenAuth();
        }

        public async Task<String> getToken()
        {
            string resp = await TokenLibrary.getTeamSnapToken(authEndPoint, clientID, callBackURL);
            Debug.WriteLine("access token : " + resp);
            return resp;
        }

        public async Task<String> callAPI(string access_token, string api)
        {
            //string access_token = (string)ApplicationData.Current.LocalSettings.Values["Tokens"];
            string resp = await APILibrary.apiCall(access_token, api);
            return resp;
        }
    }
}
