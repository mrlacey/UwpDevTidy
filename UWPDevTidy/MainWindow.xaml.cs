// <copyright file="MainWindow.xaml.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All Rights Reserved.
// </copyright>

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UWPDevTidy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                var names = AppTidier.GetAllAppNames().OrderBy(a => a);
                var cvs = new CollectionViewSource();
                cvs.Source = names;

                var binding = new Binding { Source = cvs };
                BindingOperations.SetBinding(this.AppList, ItemsControl.ItemsSourceProperty, binding);
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.AcceptRisk.IsChecked ?? false)
            {
                foreach (var selectedItem in this.AppList.SelectedItems)
                {
                    var installName = selectedItem.ToString()
                        .Substring(selectedItem.ToString().LastIndexOf("(") + 1)
                        .TrimEnd(')');

                    AppTidier.RemoveApp(installName);

                    // TODO: remove deleted item from the list
                }
            }
            else
            {
                // TODO: prompt
            }
        }
    }
}
