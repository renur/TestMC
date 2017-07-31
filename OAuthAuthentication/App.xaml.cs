
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Xamarin.Forms;

namespace OAuthAuthentication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Microsoft.Azure.Mobile.MobileCenter.Start("android={b24cccf0-0f70-42cd-b94e-a6c61dbe9add},iOS={d2e63fd2-ef00-40a3-91d2-f9cbbab5c9a1}", typeof(Analytics), typeof(Crashes));
            MainPage = new NavigationPage(new OAuthAuthenticationPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
