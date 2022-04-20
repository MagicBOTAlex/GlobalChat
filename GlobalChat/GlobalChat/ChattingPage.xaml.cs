using GlobalChat.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlobalChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChattingPage : ContentPage
    {
        private ObservableCollection<Message> _Messages;
        public ObservableCollection<Message> Messages
        {
            get { return _Messages; }
            set {
                if (Equals(value, _Messages)) return;
                _Messages = value;
                OnPropertyChanged();
            }
        }


        public ChattingPage()
        {
            InitializeComponent();
            NameBox.Text = Preferences.Get("Name", "");

            UpdateChat();

            MessageEntry.Completed += (sender, e) => SendMessage();
        }

        private void SendMessage()
        {
            if (string.IsNullOrEmpty(MessageEntry.Text)) return;
            if (string.IsNullOrEmpty(Preferences.Get("Name", ""))) return;

            FirebaseHandler.AddMessage(Preferences.Get("Name", ""), MessageEntry.Text);

            UpdateChat();
            MessageEntry.Text = "";
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            UpdateChat();
            ((RefreshView)sender).IsRefreshing = false;
        }

        private void UpdateChat()
        {
            var data = FirebaseHandler.GetData();
            if (data == null)
            {
                Messages = null;
            }
            else
            {
                Messages = new ObservableCollection<Message>(data.Messages.ToList());
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            SendMessage();
        }
    }
}