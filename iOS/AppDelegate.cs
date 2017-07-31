using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace OAuthAuthentication.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();

            MobileCenter.Start("d2e63fd2-ef00-40a3-91d2-f9cbbab5c9a1", typeof(Analytics), typeof(Crashes));

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }

    public partial class AppDelegate
    {
        public override bool OpenUrl
                (
                    UIApplication application,
                    NSUrl url,
                    string sourceApplication,
                    NSObject annotation
                )
        {
#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("OpenURL Called");
            sb.Append("     url         = ").AppendLine(url.AbsoluteUrl.ToString());
            sb.Append("     application = ").AppendLine(sourceApplication);
            sb.Append("     annotation  = ").AppendLine(annotation?.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(url.AbsoluteString);

            // load redirect_url Page
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            return true;
        }
    }

}
