using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Headers;
using Windows.ApplicationModel.DataTransfer;

namespace TeamSnapAuth.Library
{
    public class Library_APICall
    {
        // make an API call
        public async Task<string> apiCall(String access_token, String href)
        {
            HttpClient httpClient = new HttpClient();
            Debug.WriteLine("Created httpclient. " + href);
            httpClient.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", access_token);
            httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            Debug.WriteLine("Awaiting http request after this");
            var httpResponseMessage = await httpClient.GetAsync(new Uri(href));
            Debug.WriteLine("http response status code : " + httpResponseMessage.StatusCode.ToString());
            Debug.WriteLine("Done awaiting http request");
            string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            Debug.WriteLine("Done reading response as string : " + apiResponse);
            
            var dataPackage = new DataPackage();
            dataPackage.SetText(apiResponse);
            Clipboard.SetContent(dataPackage);
            return apiResponse;
        }
    }
}
