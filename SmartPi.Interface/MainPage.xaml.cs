using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartPi.Interface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // DON't CHECK THIS IN!!!
        private const string clientSecret = "";
        private const string tokenPath = "token";
        private Uri endpointUri = new Uri("https://graph.api.smartthings.com/api/smartapps/endpoints");

        public MainPage()
        {
            this.InitializeComponent();

            LoadAsync();
        }

        private async Task LoadAsync()
        {
            var token = await LoadTokenAsync();

            if (token == null)
            {
                await this.AuthenticateAsync();

                token = await LoadTokenAsync();
            }

            var endpoints = await this.GetEndpointsAsync(token);
        }

        private async Task<TokenResponse> LoadTokenAsync()
        {
            try
            {
                var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var sampleFile = await storageFolder.GetFileAsync(tokenPath);

                var text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(text);

                return tokenResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task AuthenticateAsync()
        {
            var authorisationUri = new Uri("https://graph.api.smartthings.com/oauth/authorize?response_type=code&client_id=bef68ad8-8056-4c74-bde1-cc12081c5db3&scope=app");

            var redirectUri = new Uri("http://localhost:4567/oauth/callback");

            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorisationUri, redirectUri);

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var uri = new Uri(result.ResponseData);

                // Like ?code=Ieqy8R
                var code = uri.Query.Replace("?code=", string.Empty);

                var tokenUri = new Uri($"https://graph.api.smartthings.com/oauth/token?grant_type=authorization_code&code={code}&client_id=bef68ad8-8056-4c74-bde1-cc12081c5db3&client_secret={clientSecret}");

                var response = await this.DoAGet(tokenUri);

                try
                {
                    var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    var file = await storageFolder.CreateFileAsync(tokenPath, Windows.Storage.CreationCollisionOption.ReplaceExisting);

                    await Windows.Storage.FileIO.WriteTextAsync(file, response);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async Task<string> DoAGet(Uri uri, string token = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var tokenResult = await httpClient.GetAsync(uri);
                if (tokenResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await tokenResult.Content.ReadAsStringAsync();
                    return response;
                }
            }

            return string.Empty;
        }

        private async Task<IEnumerable<SmartApp>> GetEndpointsAsync(TokenResponse token)
        {
            var response = await this.DoAGet(endpointUri, token.AccessToken);

            try
            {
                var smartApps = JsonConvert.DeserializeObject<IEnumerable<SmartApp>>(response);
                return smartApps;
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}