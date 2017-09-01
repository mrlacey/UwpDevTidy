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

            this.AppList.Loaded += this.AppList_Loaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<AppDetail> Apps { get; set; }

        private async void AppList_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetStatusMessage("Loading list of apps.");

            await Task.Run(() =>
            {
                this.Apps = new ObservableCollection<AppDetail>(AppTidier.GetAllAppNames());
                this.OnPropertyChanged(nameof(this.Apps));
            });

            this.SetStatusMessage();

            this.AppList.Loaded -= this.AppList_Loaded;
        }

        private void UninstallClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Button)sender).IsEnabled = true;

                if (this.AcceptRisk.IsChecked ?? false)
                {
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
                else
                {
                    this.SetStatusMessage("You must check to agree to the terms before you can uninstall anything.");
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
