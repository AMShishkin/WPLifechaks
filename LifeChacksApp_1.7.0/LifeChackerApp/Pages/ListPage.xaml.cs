using LifeChacksApp.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LifeChacksApp
{
    public partial class ListPage
    {
        private BitmapImage _bitmapImage;
        private KeyValuePair<string, object> _selectPair;

        private int _indexPage = 0, _lastIndexItem;
     
        public ListPage()
        {
            InitializeComponent();

            _bitmapImage = new BitmapImage();
            _selectPair = new KeyValuePair<string, object>();
            _indexPage = _lastIndexItem = this.IndexPage = 0;
        }

        private int IndexPage
        {
            get { return _indexPage; }
            set { if (value < AppHelper.Storage.Count && value >= 0) _indexPage = value; }
        }


        private void FontLogic()
        {
           

            if (!AppHelper.AppFon) TBFirst.FontFamily = TBSecond.FontFamily = TBThird.FontFamily = AppHelper.StandartFontFamily;
            else TBFirst.FontFamily = TBSecond.FontFamily = TBThird.FontFamily = AppHelper.BaseFontFamily;
        }


        // Навигация
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.FontLogic();

            if (AppHelper.AppBar) this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Default;
            else this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;

            if (AppHelper.Storage.Contains("LAST_INDEX_PAGE")) AppHelper.Storage.Remove("LAST_INDEX_PAGE");
            if (AppHelper.Storage.Contains("IS_RATE")) AppHelper.Storage.Remove("IS_RATE");
            if (AppHelper.Storage.Contains("FONT_INDEX")) AppHelper.Storage.Remove("FONT_INDEX");
            if (AppHelper.Storage.Contains("APP_BAR")) AppHelper.Storage.Remove("APP_BAR");
            if (AppHelper.Storage.Contains("APP_FON")) AppHelper.Storage.Remove("APP_FON");
            if (AppHelper.Storage.Contains("APP_EFF")) AppHelper.Storage.Remove("APP_EFF");
            if (AppHelper.Storage.Contains("APP_FON")) AppHelper.Storage.Remove("APP_FON");

            if (AppHelper.Storage.Count > 0) this.SelectedIndexLogic(false);
            else this.EmptyList();
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!AppHelper.Storage.Contains("LAST_INDEX_PAGE")) AppHelper.Storage.Add("LAST_INDEX_PAGE", AppHelper.PageIndex);
            if (!AppHelper.Storage.Contains("IS_RATE")) AppHelper.Storage.Add("IS_RATE", AppHelper.IsRate);
            if (!AppHelper.Storage.Contains("APP_BAR")) AppHelper.Storage.Add("APP_BAR", AppHelper.AppBar);
            if (!AppHelper.Storage.Contains("APP_EFF")) AppHelper.Storage.Add("APP_EFF", AppHelper.AppEff);
            if (!AppHelper.Storage.Contains("APP_FON")) AppHelper.Storage.Add("APP_FON", AppHelper.AppFon);
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
        
        
       


        private void MainPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 1) this.IndexPage--;
            else if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 2) this.IndexPage++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 0) this.IndexPage++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 2) this.IndexPage--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 0) this.IndexPage--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 1) this.IndexPage++;

            _lastIndexItem = MainPivot.SelectedIndex;

            SwipeAnimation.Begin();

            SVFirst.ScrollToVerticalOffset(0.0d);
            SVSecond.ScrollToVerticalOffset(0.0d);
            SVThird.ScrollToVerticalOffset(0.0d);

            if (AppHelper.Storage.Count > 0) this.SelectedIndexLogic(true);
            else this.EmptyList();
        }

        private void SelectedIndexLogic(bool start)
        {
            _selectPair = AppHelper.Storage.ElementAt(this.IndexPage);

            TBFirst.Text = TBSecond.Text = TBThird.Text = BaseLineDB.ResourceManager.GetString(_selectPair.Value.ToString());
            TBInfoPage.Text = "#" + _selectPair.Key;

            _bitmapImage.UriSource = new Uri("/Resources/im" + _selectPair.Key + ".jpg", UriKind.Relative);
            ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;     
        }

        private void EmptyList()
        {
            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IsEnabled = ((ApplicationBarIconButton)this.ApplicationBar.Buttons[2]).IsEnabled = false;
          
            _bitmapImage.UriSource = new Uri("/Resources/imError.jpg", UriKind.Relative);
            ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;

            TBFirst.Text = TBSecond.Text = TBThird.Text = "Пусто";
            TBInfoPage.Text = "#0";
        }
       
        private void AppListRemove_Click(object sender, EventArgs e)
        {
            if (AppHelper.Storage.Count > 0)
            {
                AppHelper.Storage.Remove(_selectPair.Key);

                if (AppHelper.Storage.Count <= 0) this.EmptyList();
                else
                {
                    this.IndexPage--;
                    this.SelectedIndexLogic(false);
                }
            }
            else this.EmptyList();
        }
        private void AppShow_Click(object sender, EventArgs e)
        {
            ShareStatusTask _shareStatusTask = new ShareStatusTask();
            _shareStatusTask.Status = "[ЛАЙФХАКС #" + _selectPair.Key + "]\n" + BaseLineDB.ResourceManager.GetString(_selectPair.Value.ToString());
            _shareStatusTask.Show();
        }

        private void ShowImage()
        {
            ApplicationBar.IsVisible = false;
            ShowBigImage.Visibility = System.Windows.Visibility.Visible;
            ShowBigImage.Source = _bitmapImage;
        }


      

        private void ImageFirst_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.ShowImage();
        }
        private void ImageSecond_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.ShowImage();
        }
        private void ImageThird_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.ShowImage();
        } 



        private void RecShadow_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            ShowBigImage.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }
        private void ShowBigImage_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            ShowBigImage.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        // Сохранить
        private void AppSave_Click(object sender, EventArgs e)
        {
            if (AppHelper.IsTrial) MessageBox.Show("Данная функция доступна в полной версии программы", "Сохранить совет в виде изображения", MessageBoxButton.OK);
            else
            {
                var streamResourceInfo = Application.GetResourceStream(new Uri("Background.png", UriKind.Relative));
                var image = new BitmapImage();
                image.SetSource(streamResourceInfo.Stream);
                var writeableBitmap = new WriteableBitmap(image);
                var bitmap = new BitmapImage(_bitmapImage.UriSource);
                var im = new Image() { Width = 480, Height = 350, Source = bitmap, Stretch = Stretch.Fill };

                var textBlockTitle = new TextBlock
                {
                    FontSize = 28,
                    Width = 200,
                    Height = 60,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Right,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = new SolidColorBrush(Color.FromArgb(210, 210, 210, 210)),
                    Text = "СОВЕТ #" + _selectPair.Key,
                };

                var textBlockText = new TextBlock
                {
                    FontSize = 26,
                    Width = 470,
                    Height = 450,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(210, 210, 210, 210)),
                    Text = BaseLineDB.ResourceManager.GetString("String" + _selectPair.Key),
                };

                writeableBitmap.Render(textBlockTitle, new TranslateTransform { X = 270, Y = 8, });
                writeableBitmap.Render(textBlockText, new TranslateTransform { X = 5, Y = 400, });

                MessageBox.Show("Совет сохранен в библиотеку фотографий.", "Выполнено", MessageBoxButton.OK);

                writeableBitmap.Render(im, new TranslateTransform { X = 0, Y = 40, });
                writeableBitmap.Invalidate();

                using (var mediaLibrary = new MediaLibrary())
                {
                    using (var stream = new MemoryStream())
                    {
                        writeableBitmap.SaveJpeg(stream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
                        stream.Seek(0, SeekOrigin.Begin);
                        mediaLibrary.SavePicture("Cовет_" + AppHelper.PageIndex + ".jpg", stream);
                    }
                }
            }
        }
    }
}