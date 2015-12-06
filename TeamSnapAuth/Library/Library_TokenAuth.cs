using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace TeamSnapAuth.Library
{
    public class Library_TokenAuth
    {
        public async Task<bool> getTeamSnapToken(string authEndPoint, string clientID, string callBackURL)
        {
            string access_token = null;
            try
            {
                // construct Auth URL
                String teamSnapAuthURL = authEndPoint + Uri.EscapeDataString(clientID) + "&redirect_uri=" + Uri.EscapeDataString(callBackURL) + "&response_type=token&scope=read";
                Uri StartUri = new Uri(teamSnapAuthURL);
                Uri EndUri = new Uri(callBackURL);
                // Make web request
                WebAuthenticationResult WebAuthResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, StartUri, EndUri);
                if (WebAuthResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string responseData = WebAuthResult.ResponseData.ToString();
                    string subResponseData = responseData.Substring(responseData.IndexOf("access_token"));
                    String[] keyValPairs = subResponseData.Split('&');

                    string token_type = null;
                    for (int i = 0; i < keyValPairs.Length; i++)
                    {
                        String[] splits = keyValPairs[i].Split('=');
                        switch (splits[0])
                        {
                            case "access_token":
                                access_token = splits[1]; //you may want to store access_token for further use. Look at Scenario 5 (Account Management).
                                break;
                            case "token_type":
                                token_type = splits[1];
                                break;
                        }
                    }

                    ApplicationData.Current.LocalSettings.Values["Tokens"] = access_token;
                }
                else if (WebAuthResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    Debug.WriteLine("HTTP Error Response Detail : " + WebAuthResult.ResponseErrorDetail.ToString());
                    return (false);
                }
                else
                {
                    Debug.WriteLine("Error Response Status : " + WebAuthResult.ResponseStatus.ToString());
                }

            }
            catch (Exception)
            {

            }
            return (true);
        }
        
        //check if token exists
        public static bool tokenExists()
        {
            if(String.IsNullOrEmpty((string)ApplicationData.Current.LocalSettings.Values["Tokens"]))
            {
                return false;
            }
            return true;

        } 
        
        public async Task<bool> tokenIsValid()
        {
            string access_token = null;
            if (tokenExists())
            {
                access_token = (string)ApplicationData.Current.LocalSettings.Values["Tokens"];
                string href = "https://api.teamsnap.com/v3/me";
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", access_token);
                httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
                var httpResponseMessage = await httpClient.GetAsync(new Uri(href));
                Debug.WriteLine("http response status code : " + httpResponseMessage.StatusCode.ToString());
                if(httpResponseMessage.StatusCode.ToString().Equals("Ok"))
                {
                    return true;
                }
            }
            Debug.WriteLine("tokenIsValid : returning false");
            return false;
        } 
        // check if token is still  valid
        
    }
}
