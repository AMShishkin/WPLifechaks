using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;
using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;

namespace LifeChacksApp
{
    public partial class SettingsPage
    {   
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void FontLogic()
        {
            if (!AppHelper.AppFon) TBBaseInfo.FontFamily = ButNews.FontFamily = SwitchAnimation.FontFamily = SwitchPanel.FontFamily = SwitchFont.FontFamily = AppHelper.StandartFontFamily;
            else TBBaseInfo.FontFamily = ButNews.FontFamily = SwitchAnimation.FontFamily = SwitchPanel.FontFamily = SwitchFont.FontFamily = AppHelper.BaseFontFamily;
        }



        // Навигация
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.FontLogic();

            SwitchPanel.IsChecked = AppHelper.AppBar;
            SwitchAnimation.IsChecked = AppHelper.AppEff;
            SwitchFont.IsChecked = AppHelper.AppFon;

            if (SwitchPanel.IsChecked.Value) SwitchPanel.Content = "БОЛЬШАЯ";
            else SwitchPanel.Content = "МАЛЕНЬКАЯ";

            if (SwitchAnimation.IsChecked.Value) SwitchAnimation.Content = "ВКЛЮЧЕНЫ";
            else SwitchAnimation.Content = "ВЫКЛЮЧЕНЫ";

            if (SwitchFont.IsChecked.Value) SwitchFont.Content = "АВТОРСКИЙ";
            else SwitchFont.Content = "СТАНДАРТНЫЙ";

            this.AppBarLogic();
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (AppHelper.AppEff)
            {
                SlideTransitionMode slideTransitionMode = (SlideTransitionMode)Enum.Parse(typeof(SlideTransitionMode), "SlideDownFadeOut", false);
                TransitionElement transitionElement = new SlideTransition { Mode = slideTransitionMode };

                PhoneApplicationPage phoneApplicationPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;

                ITransition transition = transitionElement.GetTransition(phoneApplicationPage);
                transition.Completed += delegate { transition.Stop(); };
                transition.Begin();
            }
        }


        private void AppEmail_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask { Subject = "ЛАЙФХАКC", To = "AMShishkin.vrn@gmail.com" };
            emailComposeTask.Show();
        }
        private void AppRate_Click(object sender, EventArgs e)
        {
            AppHelper.Storage["IS_RATE"] = AppHelper.IsRate = true;
            AppHelper.Storage.Save();
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
        private void AppSearch_Click(object sender, EventArgs e)
        {
            MarketplaceSearchTask marketplaceSearchTaskAll = new MarketplaceSearchTask {SearchTerms = "AMShishkin"};
            marketplaceSearchTaskAll.Show();
        }
        private void AppShowVk_Click(object sender, EventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://vk.com/id88412402", UriKind.Absolute);
            webBrowserTask.Show();
        }

      
       

        private void AppBarLogic()
        {
            if (AppHelper.AppBar) this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Default;
            else this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
        }



        // Переключатель анимации
        private void SwitchAnimation_Checked(object sender, RoutedEventArgs e)
        {
            SwitchAnimation.Content = "ВКЛЮЧЕНЫ";

            AppHelper.Storage["APP_EFF"] = AppHelper.AppEff = true;
            AppHelper.Storage.Save();
        }
        private void SwitchAnimation_Unchecked(object sender, RoutedEventArgs e)
        {
            SwitchAnimation.Content = "ВЫКЛЮЧЕНЫ";

            AppHelper.Storage["APP_EFF"] = AppHelper.AppEff = false;
            AppHelper.Storage.Save();
        }
        // Переключатель панели
        private void SwitchPanel_Checked(object sender, RoutedEventArgs e)
        {
            SwitchPanel.Content = "БОЛЬШАЯ";

            AppHelper.Storage["APP_BAR"] = AppHelper.AppBar = true;
            AppHelper.Storage.Save();

            this.AppBarLogic();
        }
        private void SwitchPanel_Unchecked(object sender, RoutedEventArgs e)
        {
            SwitchPanel.Content = "МАЛЕНЬКАЯ";

            AppHelper.Storage["APP_BAR"] = AppHelper.AppBar = false;
            AppHelper.Storage.Save();

            this.AppBarLogic();
        }
        // Переключатель шрифта
        private void SwitchFont_Checked(object sender, RoutedEventArgs e)
        {
            SwitchFont.Content = "БАЗОВЫЙ";

            AppHelper.Storage["APP_FON"] = AppHelper.AppFon = true;
            AppHelper.Storage.Save();

            this.FontLogic();
        }
        private void SwitchFont_Unchecked(object sender, RoutedEventArgs e)
        {
            SwitchFont.Content = "СТАНДАРТНЫЙ";
            AppHelper.Storage["APP_FON"] = AppHelper.AppFon = false;
            AppHelper.Storage.Save();

            this.FontLogic();


        }


        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("+ Функция сохранения совета в виде изображения.\n+ В демо версии доступны первые 100 советов", "Техническое обновление v1.6.8", MessageBoxButton.OK);
        }     
    }
}