using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace TeamSnapAuth.Library
{
    public class Library_TokenAuth
    {
        public async Task<String> getTeamSnapToken(string authEndPoint, string clientID, string callBackURL)
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
                    return (WebAuthResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    return (WebAuthResult.ResponseStatus.ToString());
                }

            }
            catch (Exception)
            {

            }
            return (access_token);
        }
       
        // check for error in call to token
        // check if token is still  valid
        
    }
}
