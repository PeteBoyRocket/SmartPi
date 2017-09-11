using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartPi.Interface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // DON't CHECK THIS IN!!!
        private string clientSecret = "";

        public MainPage()
        {
            this.InitializeComponent();

            this.AuthenticateAsync();
        }

        private async System.Threading.Tasks.Task AuthenticateAsync()
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

                using (var httpClient = new HttpClient())
                {
                    var tokenResult = await httpClient.GetAsync(tokenUri);
                    if (tokenResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                       var xx = await tokenResult.Content.ReadAsStringAsync();
                    }

                    //  var resultx = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, tokenUri, redirectUri);

                }
            }
        }
    }
}