using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecompildPOS.Helpers.CommandLocker;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecompildPOS.Components.AutoComplete
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoCompleteEntry : ContentView
    {
        ObservableCollection<string> _data = new ObservableCollection<string>();
        public ICommand SearchCommand => new Command(SearchCommandLocker.Execute);

        private CommandLockerHelper SearchCommandLocker =>
            new CommandLockerHelper( () => {  Search(); });

        private void Search()
        {
            SearchListView.IsVisible = true;
            SearchListView.BeginRefresh();

            try
            {
                var dataEmpty = _data.Where(i => i.ToLower().Contains(SearchBar.Text.ToLower()));

                if (string.IsNullOrWhiteSpace(SearchBar.Text))
                    SearchListView.IsVisible = false;
                else if (dataEmpty.Max().Length == 0)
                    SearchListView.IsVisible = false;
                else
                    SearchListView.ItemsSource = _data.Where(i => i.ToLower().Contains(SearchBar.Text.ToLower()));
            }
            catch (Exception ex)
            {
                SearchListView.IsVisible = false;

            }
            SearchListView.EndRefresh();
        }

        public AutoCompleteEntry()
        {
            InitializeComponent();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            String selectedItem = e.Item as string;
            SearchBar.Text = selectedItem;
            SearchListView.IsVisible = false;

            ((ListView)sender).SelectedItem = null;
        }
    }
}