using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Modules;
using RecompildPOS.Resources.Constants.Picker;
using RecompildPOS.Resources.Keys;
using RecompildPOS.ViewModels.Sync;
using RecompildPOS.Views.Base;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Views.Sync
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncPage : BasePage
    {
        public SyncViewModel ViewModel => BindingContext as SyncViewModel;

        void Handle_SwitchToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            ViewModel.AutoSyncToggled();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            App.BackgroundTaskTime = PickerConstants.MinutesList[minutesPicker.SelectedIndex];
            ModulesConfig.SyncTime = App.BackgroundTaskTime;
            if (Preferences.ContainsKey(AppKeys.SyncTime))
            {
                Preferences.Get(AppKeys.SyncTime, App.BackgroundTaskTime);
            }
            else
            {
                Preferences.Set(AppKeys.SyncTime, App.BackgroundTaskTime);

            }
        }


        public SyncPage()
        {
            InitializeComponent();
            var selectedIndex = PickerConstants.MinutesList.ToList().FindIndex((x) => x == App.BackgroundTaskTime);
            minutesPicker.SelectedIndex = selectedIndex;
        }
    }
}