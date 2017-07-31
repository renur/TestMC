using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;

namespace OAuthAuthentication.Droid
{
    [Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    // Walthrough Step 4
    //      Intercepting/Catching/Detecting [redirect] url change 
    //      App Linking / Deep linking - custom url schemes
    //      
    // 
    [
      IntentFilter
      (
          actions: new[] { Intent.ActionView },
          Categories = new[]
                  {
                        Intent.CategoryDefault,
                        Intent.CategoryBrowsable
                  },
          DataSchemes = new[]
                  {
                        "com.xamarin.traditional.standard.samples.oauth.providers.android",
                        "com.googleusercontent.apps.1093596514437-d3rpjj7clslhdg3uv365qpodsl5tq4fn",
                        "fb1889013594699403://localhost/path",
                  },
          //DataHost = "localhost",
          DataPath = "/oauth2redirect"
      )
  ]
    public class ActivityCustomUrlSchemeInterceptor : Activity
    {
        string message;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ActivityCustomUrlSchemeInterceptor.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(uri_android.ToString());

            // load redirect_url Page
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            this.Finish();

            return;
        }
    }
}
