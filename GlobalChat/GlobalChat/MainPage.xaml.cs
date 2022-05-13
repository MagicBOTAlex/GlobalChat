using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GlobalChat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            UpdatePromoteText();

            if (!string.IsNullOrEmpty(Preferences.Get("Name", "")))
            {
                StartChatting();
            }

            NameEntry.Completed += (sender, e) => LoginSubmit();
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            LoginSubmit();
        }

        private async void LoginSubmit()
        {
            if (NameEntry.Text is null || NameEntry.Text.Length <= 0)
            {
                bool answer = await DisplayAlert("Retard", "Atleast input a single character", "Sure", "K");
                Preferences.Set("promote", true);
                UpdatePromoteText();
                return;
            }

            Preferences.Set("Name", NameEntry.Text);

            StartChatting();
        }

        void UpdatePromoteText()
        {
            RandsomeText.IsVisible = Preferences.Get("promote", false);
            OnPropertyChanged();
            if (Preferences.ContainsKey("promote"))
            {
                Preferences.Clear("promote");
            }
        }

        private async void StartChatting()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Broke ass", "You can't even afford internet. LOL", @"¯\_(ツ)_/¯");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            ((App)App.Current).ChangeScreen(new ChattingPage());
        }
    }
}
