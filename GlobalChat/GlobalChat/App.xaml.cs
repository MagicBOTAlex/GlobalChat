using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlobalChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void ChangeScreen(Page page)
        {
            MainPage = page;
        }
    }
}
