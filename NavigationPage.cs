﻿namespace Portal.View
{
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;


    public class NavigationPage : Page
    {
        public static NavigationPage Current;

        public NavigationPage()
        {
            this.Loaded += this.NavigationPage_Loaded;
        }

        void NavigationPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // To access the app bars from the code of a derived page, 
            // use NavigationPage.Current.TopAppBar or NavigationPage.Current.BottomAppBar.
            Current = this;

            // Navigation controls go in the top app bar.
            AppBar navBar = new AppBar();
            navBar.Background = new SolidColorBrush(new Color() { A = 255, R = 204, G = 0, B = 0 });
            navBar.Content = new NavigationControl();
            this.TopAppBar = navBar;

            // CommandBar for Help command goes in the BottomAppBar.
            CommandBar commandBar = new CommandBar();
            commandBar.Background = new SolidColorBrush(new Color() { A = 255, R = 204, G = 0, B = 0 });
            // Create the Help button.
            AppBarButton helpButton = new AppBarButton();

            helpButton.Icon = new SymbolIcon(Symbol.Help);
            helpButton.Label = "Help";
            helpButton.Click += this.helpButton_Click;
            // Add the Help button to the command bar.
            commandBar.PrimaryCommands.Add(helpButton);
            this.BottomAppBar = commandBar;
        }

        void helpButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Portal.View.Settings.HelpSettings helpSettingsFlyout = new Portal.View.Settings.HelpSettings();
            // When the settings flyout is opened from the app bar instead of from
            // the setting charm, use the ShowIndependent() method.
            helpSettingsFlyout.ShowIndependent();

            // Close the app bars.
            this.TopAppBar.IsOpen = false;
            this.BottomAppBar.IsOpen = false;
        }
    }
}
