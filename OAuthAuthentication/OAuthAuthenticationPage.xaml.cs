using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Forms;
namespace OAuthAuthentication
{
    public partial class OAuthAuthenticationPage : ContentPage
    {

        Xamarin.Auth.OAuth2Authenticator authenticator = null;
        bool native_ui = true;
        string fb_app_id = "1568885556505338";

        private Color _backgroundColor = Color.Black;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }


        private Color _textColor = Color.White;
        public Color TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                _textColor = value;
                OnPropertyChanged();
            }
        }

        private ICommand _pressedCommand = null;
        public ICommand PressedGestureCommand
        {
            get
            {
                if (_pressedCommand == null)
                    _pressedCommand = new Command((parameter) =>
                    {
                        BackgroundColor = Color.White;
                        TextColor = Color.Black;
                    });

                return _pressedCommand;
            }


        }

        private ICommand _releasedCommand = null;
        public ICommand ReleasedGestureCommand
        {
            get
            {
                if (_releasedCommand == null)
                    _releasedCommand = new Command((parameter) =>
                    {
                        BackgroundColor = Color.Black;
                        TextColor = Color.White;
                    });

                return _releasedCommand;
            }

        }

        public OAuthAuthenticationPage()
        {
            InitializeComponent();
            this.BindingContext = this;

        }

        void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                //getProfileButton.IsEnabled = true;

                //if (this.account != null)
                //  store.Delete(this.account, ServiceId);

                //store.Save(account = e.Account, ServiceId);

                //getProfileButton.IsEnabled = true;

                //if (account.Properties.ContainsKey("expires_in"))
                //{
                //  var expires = int.Parse(account.Properties["expires_in"]);
                //  statusText.Text = "Token lifetime is: " + expires + "s";
                //}
                //else
                //{
                //  statusText.Text = "Authentication succeeded";
                //}

                //if (account.Properties.ContainsKey("refresh_token"))
                //refreshButton.IsEnabled = true;
            }
            else
            {
                //  statusText.Text = "Authentication failed";
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            //statusText.Text = "Authentication error: " + e.Message;
        }

        void Facebook_Clicked(object sender, System.EventArgs e)
        {
            authenticator
             = new Xamarin.Auth.OAuth2Authenticator
             (
                 clientId:
                     new Func<string>
                        (
                            () =>
                            {
                                string retval_client_id = "oops something is wrong!";

                                retval_client_id = fb_app_id;
                                return retval_client_id;
                            }
                        ).Invoke(),
                 authorizeUrl:
                     new Func<Uri>
                        (
                            () =>
                            {
                                string uri = null;
                                if (native_ui)
                                {
                                    uri = "https://www.facebook.com/v2.9/dialog/oauth";
                                }
                                else
                                {
                                    // old
                                    uri = "https://m.facebook.com/dialog/oauth/";
                                }
                                return new Uri(uri);
                            }
                        ).Invoke(),
                 redirectUrl:
                     new Func<Uri>
                        (
                            () =>
                            {
                                string uri = null;
                                if (native_ui)
                                {
                                    uri =
                                        $"fb{fb_app_id}://authorize"
                                        ;
                                }
                                else
                                {
                                    uri =
                                        $"fb{fb_app_id}://authorize"
                                        ;
                                }
                                return new Uri(uri);
                            }
                        ).Invoke(),
                 scope: "", // "basic", "email",
                 getUsernameAsync: null,
                 isUsingNativeUI: native_ui
             )
             {
                 AllowCancel = true,
             };

            authenticator.Completed +=
                (s, ea) =>
                    {
                        StringBuilder sb = new StringBuilder();

                        if (ea.Account != null && ea.Account.Properties != null)
                        {
                            sb.Append("Token = ").AppendLine($"{ea.Account.Properties["access_token"]}");
                        }
                        else
                        {
                            sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
                        }

                        Navigation.PushAsync(new DashBoardPage());
                        return;
                    };

            authenticator.Error +=
                (s, ea) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Error = ").AppendLine($"{ea.Message}");

                        DisplayAlert
                                (
                                    "Authentication Error",
                                    sb.ToString(),
                                    "OK"
                                );
                        return;
                    };

            // after initialization (creation and event subscribing) exposing local object 
            AuthenticationState.Authenticator = authenticator;

            PresentUILoginScreen(authenticator);

            return;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            authenticator
               = new Xamarin.Auth.OAuth2Authenticator
               (
                   clientId:
                       new Func<string>
                          (
                               () =>
                               {
                                   string retval_client_id = "oops something is wrong!";

                                   // some people are sending the same AppID for google and other providers
                                   // not sure, but google (and others) might check AppID for Native/Installed apps
                                   // Android and iOS against UserAgent in request from 
                                   // CustomTabs and SFSafariViewContorller
                                   // TODO: send deliberately wrong AppID and note behaviour for the future
                                   // fitbit does not care - server side setup is quite liberal
                                   switch (Xamarin.Forms.Device.RuntimePlatform)
                                   {
                                       case "Android":
                                           retval_client_id = "1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn.apps.googleusercontent.com";
                                           break;
                                       case "iOS":
                                           retval_client_id = "1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh.apps.googleusercontent.com";
                                           break;
                                       case "Windows":
                                           retval_client_id = "1093596514437-t7ocfv5tqaskkd53llpfi3dtdvk4t35h.apps.googleusercontent.com";
                                           break;
                                   }
                                   return retval_client_id;
                               }
                         ).Invoke(),
                   clientSecret: null,   // null or ""
                   authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                   accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                   redirectUrl:
                       new Func<Uri>
                          (
                               () =>
                               {

                                   string uri = null;

                                   // some people are sending the same AppID for google and other providers
                                   // not sure, but google (and others) might check AppID for Native/Installed apps
                                   // Android and iOS against UserAgent in request from 
                                   // CustomTabs and SFSafariViewContorller
                                   // TODO: send deliberately wrong AppID and note behaviour for the future
                                   // fitbit does not care - server side setup is quite liberal
                                   switch (Xamarin.Forms.Device.RuntimePlatform)
                                   {
                                       case "Android":
                                           uri =
                                               "com.xamarin.traditional.standard.samples.oauth.providers.android:/oauth2redirect"
                                               //"com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn:/oauth2redirect"
                                               ;
                                           break;
                                       case "iOS":
                                           uri =
                                               "com.xamarin.traditional.standard.samples.oauth.providers.ios:/oauth2redirect"
                                               //"com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect"
                                               ;
                                           break;
                                       case "Windows":
                                           uri =
                                               "com.xamarin.auth.windows:/oauth2redirect"
                                               //"com.googleusercontent.apps.1093596514437-cajdhnien8cpenof8rrdlphdrboo56jh:/oauth2redirect"
                                               ;
                                           break;
                                   }

                                   return new Uri(uri);
                               }
                           ).Invoke(),
                   scope:
                                //"profile"
                                "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.login"
                                ,
                   getUsernameAsync: null,
                                isUsingNativeUI: native_ui
               )
               {
                   AllowCancel = true,
               };

            authenticator.Completed +=
                (s, ea) =>
                    {
                        StringBuilder sb = new StringBuilder();

                        if (ea.Account != null && ea.Account.Properties != null)
                        {
                            sb.Append("Token = ").AppendLine($"{ea.Account.Properties["access_token"]}");
                        }
                        else
                        {
                            sb.Append("Not authenticated ").AppendLine($"Account.Properties does not exist");
                        }

                        Navigation.PushAsync(new DashBoardPage());
                        //DisplayAlert
                        //(
                        //    "Authentication Results",
                        //    sb.ToString(),
                        //    "OK"
                        //);

                        return;
                    };

            authenticator.Error +=
                (s, ea) =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Error = ").AppendLine($"{ea.Message}");

                        DisplayAlert
                                (
                                    "Authentication Error",
                                    sb.ToString(),
                                    "OK"
                                );
                        return;
                    };

            // after initialization (creation and event subscribing) exposing local object 
            AuthenticationState.Authenticator = authenticator;

            PresentUILoginScreen(authenticator);

            return;
        }

        public static bool navigation_push_modal = false;
        public List<string> NavigationTypes => _NavigationTypes;

        List<string> _NavigationTypes = new List<string>()
        {
            "PushModalAsync",
            "PushAsync",
        };


        private void PresentUILoginScreen(OAuth2Authenticator authenticator)
        {
            if (forms_implementation_renderers)
            {
                // Renderers Implementaion

                Xamarin.Auth.XamarinForms.AuthenticatorPage ap;
                ap = new Xamarin.Auth.XamarinForms.AuthenticatorPage()
                {
                    Authenticator = authenticator,
                };

                NavigationPage np = new NavigationPage(ap);

                if (navigation_push_modal == true)
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        PushModal");
                    System.Diagnostics.Debug.WriteLine("        Custom Renderers");

                    Navigation.PushModalAsync(np);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        Push");
                    System.Diagnostics.Debug.WriteLine("        Custom Renderers");

                    Navigation.PushAsync(np);
                }
            }
            else
            {
                // Presenters Implementation

                if (navigation_push_modal == true)
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        PushModal");
                    System.Diagnostics.Debug.WriteLine("        Presenters");

                    Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                    presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(authenticator);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        Push");
                    System.Diagnostics.Debug.WriteLine("        Presenters");

                    Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                    presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(authenticator);
                }
            }

            return;
        }

        bool forms_implementation_renderers = false;
        public List<string> FormsImplementations => _FormsImplementations;

        List<string> _FormsImplementations = new List<string>()
        {
            "Custom Renderers",
            "Presenters (Dependency Service/Injection)",
        };




    }

    public class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator;
    }
}
