// <copyright file="MainWindow.xaml.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UWPDevTidy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = this;

            this.Title += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            this.OnlyDevApps.IsChecked = true;

            this.OnlyDevApps.Checked += this.OnlyDevApps_Checked;
            this.OnlyDevApps.Unchecked += this.OnlyDevApps_Checked;

            this.AppList.Loaded += this.AppList_Loaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<AppDetail> Apps { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnlyDevApps_Checked(object sender, RoutedEventArgs e)
        {
            await this.ReloadAppList();
        }

        private async void AppList_Loaded(object sender, RoutedEventArgs e)
        {
            await this.ReloadAppList();

            this.AppList.Loaded -= this.AppList_Loaded;
        }

        private async Task ReloadAppList()
        {
            this.SetStatusMessage("Loading list of apps.");

            bool onlyDevApps = this.OnlyDevApps.IsChecked ?? false;

            await Task.Run(() =>
            {
                this.Apps = new ObservableCollection<AppDetail>(AppTidier.GetAllAppNames(onlyDevApps));
                this.OnPropertyChanged(nameof(this.Apps));
            });

            this.SetStatusMessage();
        }

        private void UninstallClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = true;

                var selected = new Dictionary<string, string>();

                foreach (var item in this.AppList.SelectedItems)
                {
                    var app = (AppDetail)item;
                    selected.Add(app.ProductFamilyName, app.DisplayName);
                }

                if (selected.Count == 0)
                {
                    this.SetStatusMessage("Select apps to uninstall.");
                }
                else
                {
                    foreach (var selectedItem in selected)
                    {
                        this.SetStatusMessage($"Uninstalling: {selectedItem.Value}");

                        AppTidier.RemoveApp(selectedItem.Key);

                        this.Apps.Remove(this.Apps.First(a => a.ProductFamilyName == selectedItem.Key));

                        this.SetStatusMessage();
                    }
                }
            }
            finally
            {
                ((Button)sender).IsEnabled = true;
            }
        }

        private void SetStatusMessage(string message = "")
        {
            this.DisplayedStatusInfo.Text = message;

            // Force UI update to display message
            this.DisplayedStatusInfo.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
    }
}
