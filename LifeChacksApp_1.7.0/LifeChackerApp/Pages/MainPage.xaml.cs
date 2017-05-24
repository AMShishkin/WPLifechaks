using LifeChacksApp.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;

namespace LifeChacksApp
{
    public partial class MainPage
    {
        private readonly BitmapImage _bitmapImage;
        private readonly Uri _uriHeart, _uriHeartBreak;

        private MessageBoxResult _messageResult;
        private int _noRateIndexPage, _lastIndexItem;
        private Dictionary<int, string> liveTileContent;

     
 
       

       
   

        public MainPage()
        {
            InitializeComponent();

            _uriHeart = new Uri("/Assets/appbar.heart.png", UriKind.Relative);
            _uriHeartBreak = new Uri("/Assets/appbar.heart.break.png", UriKind.Relative);
            _bitmapImage = new BitmapImage();
       ;
            
            if (!AppHelper.Storage.Contains("LAST_INDEX_PAGE")) AppHelper.Storage.Add("LAST_INDEX_PAGE", AppHelper.PageIndex);
            if (!AppHelper.Storage.Contains("IS_RATE")) AppHelper.Storage.Add("IS_RATE", false);
            if (!AppHelper.Storage.Contains("APP_BAR")) AppHelper.Storage.Add("APP_BAR", false);
            if (!AppHelper.Storage.Contains("APP_EFF")) AppHelper.Storage.Add("APP_EFF", true);
            if (!AppHelper.Storage.Contains("APP_FON")) AppHelper.Storage.Add("APP_FON", true);

            AppHelper.PageIndex = (int)AppHelper.Storage["LAST_INDEX_PAGE"];
            AppHelper.IsRate = (bool)AppHelper.Storage["IS_RATE"];
            AppHelper.AppBar = (bool)AppHelper.Storage["APP_BAR"];
            AppHelper.AppEff = (bool)AppHelper.Storage["APP_EFF"];
            AppHelper.AppFon = (bool)AppHelper.Storage["APP_FON"];

            if (AppHelper.IsTrial) AppHelper.MAXSIZEBASEDB = 100;
            else AppHelper.MAXSIZEBASEDB = 850;

            PBar.Maximum = AppHelper.MAXSIZEBASEDB;

            liveTileContent = new Dictionary<int, string>();
            liveTileContent.Add(10, "Избавиться от катышек на джинсах поможет бритва!");
            liveTileContent.Add(100, "Хранить грибы лучше в бумажном, а не в пластиковом пакете!");
            liveTileContent.Add(105, "Кладите на дно чемодана антистатические салфетки. Одежда будет свежей!");
            liveTileContent.Add(106, "Перед путешествием отсканируйте все важные документы!");
            liveTileContent.Add(115, "Перегревается ноутбук? Поставьте его на бумажную упаковку из-под яиц!");

            liveTileContent.Add(120, "Пятна на замше можно удалить при помощи пилочки для ногтей!");
            liveTileContent.Add(124, "Шпильки и заколки удобно хранить в коробочке 'Tic-Tac'!");
            liveTileContent.Add(13, "Чтобы не отбить пальцы молотком, используйте прищепку!");
            liveTileContent.Add(136, "Прозрачный лак поверх ниток — и ваши пуговицы всегда на месте!");
            liveTileContent.Add(154, "Мягкий сыр и тому подобные продукты можно резать зубной нитью!");

            liveTileContent.Add(98, "Лук в чулках сохранит свежесть до 8 месяцев!");
            liveTileContent.Add(84, "Холодный душ может помочь избавиться от перхоти!");
            liveTileContent.Add(74, "Храните простыни внутри наволочек из того же комплекта!");
            liveTileContent.Add(681, "Не отворачивается шуруп — стукните по нему молотком!");
            liveTileContent.Add(68, "Вернуть линолеуму былой лоск можно, отполировав его зубной пастой!");

            liveTileContent.Add(673, "Выворачивайте вещи наизнанку перед стиркой. Вещь прослужит дольше!");
            liveTileContent.Add(669, "Яйца вкрутую можно организовать и в духовке!");
            liveTileContent.Add(396, "Крепкий раствор соли поможет очистить замёрзшие оконные стёкла!");
            liveTileContent.Add(311, "Таблетница — лучший контейнер для украшений!");
            liveTileContent.Add(99, "Для того чтобы сыр не сох, смажьте свежий срез сливочным маслом!");



            

            















        }


        private void FontLogic()
        {
           // TBLogo.FontFamily = AppHelper.BaseFontFamily;

            if (!AppHelper.AppFon) TBFirst.FontFamily = TBSecond.FontFamily = TBThird.FontFamily = AppHelper.StandartFontFamily;
            else TBFirst.FontFamily = TBSecond.FontFamily = TBThird.FontFamily = AppHelper.BaseFontFamily;
        }



        private void LikeLogic()
        {
            if (AppHelper.Storage.Contains(AppHelper.PageIndex.ToString()))
            {
                LikeLogo.Visibility = System.Windows.Visibility.Visible;
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = "удалить";
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IconUri = _uriHeartBreak;
            }
            else
            {
                LikeLogo.Visibility = System.Windows.Visibility.Collapsed;         
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = "добавить";
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IconUri = _uriHeart;
            }
        }

        private void SelectedIndexLogic()
        {
            // Демо 
            if (AppHelper.IsTrial && AppHelper.PageIndex == 100)
            {

                TBFirst.Text = TBSecond.Text = TBThird.Text = "Понравилось приложение?\nХочется еще больше житейских хитростей и советов?\nПриобретите полную версию приложения и наслаждайтесь сотнями житейских советов со всего мира!\n( ... -> полная версия )\n\nВ полной версии приложения Вы получите:\n+ Более 800 житейских советов\n+ Регулярное пополнение приложения новыми советами и житейскими хитростями\n+ Возможность сохранить совет в виде изображения\n+ Отличное настроение и сотни житейских хитростей для улучшения жизни\n\nP.S.\nСамые интересные и полезные советы находятся в полной версии программы";
                _bitmapImage.UriSource = new Uri("/Resources/imFull.jpg", UriKind.Relative);
                TBSelect.Text = "**";
                ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;
                PBar.Value = 100;
            }
            else
            {
                TBFirst.Text = TBSecond.Text = TBThird.Text = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex);
                _bitmapImage.UriSource = new Uri("/Resources/im" + AppHelper.PageIndex + ".jpg", UriKind.Relative);
                TBSelect.Text = "#" + AppHelper.PageIndex;
                ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;

                PBar.Value = AppHelper.PageIndex;
                this.LikeLogic();
            }
        }

        private void Assessment()
        {
            if (_noRateIndexPage >= 50)
            {
                MPStartAnimation.Stop();
              
                _noRateIndexPage = -50;
                _messageResult = MessageBox.Show("Помогите разработчику сделать программу лучше! Оставьте отзыв и оцените приложение!", "Понравились советы?", MessageBoxButton.OKCancel);
            }
            else _noRateIndexPage++;

            if (_messageResult == MessageBoxResult.Cancel)
            {
                MPStartAnimation.Begin();
               
                _messageResult = MessageBoxResult.None;
            }
            else if (_messageResult == MessageBoxResult.OK)
            {
                AppHelper.Storage["IS_RATE"] = AppHelper.IsRate = true;
                AppHelper.Storage.Save();

                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
                
            }
        }
        private void SelectAdvice(string text)
        {
            var tbSelectIndex = 0;
            try
            {
                for (var i = 0; i < text.Length; i++)
                {
                    if (text[i] == ',' || text[i] == '.' || text[i] == '#' || i > 5)
                    {
                        text = text.Substring(0, i); break;
                    }
                }

                if (text != "") tbSelectIndex = Convert.ToInt32(text);
                else tbSelectIndex = -1;



                if (tbSelectIndex > AppHelper.MAXSIZEBASEDB || tbSelectIndex < 0) TBSelect.Text = "СОВЕТ #" + AppHelper.PageIndex;
                else TBSelect.Text = "СОВЕТ #" + text; 
                
                AppHelper.PageIndex = tbSelectIndex;
                SelectedIndexLogic();
            }
            catch (Exception)
            {
                AppHelper.PageIndex = 0;
                SelectedIndexLogic();
            }
        }

 





        // Навигация
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.FontLogic();

            if (AppHelper.IsTrial)
            {
                Banner.Visibility = System.Windows.Visibility.Visible;
                Banner.IsEnabled = true;
                Banner.Margin = new Thickness(0, 35, 0, 0);
                Banner.IsAnimationEnabled = false;
            }
            else Banner.Visibility = System.Windows.Visibility.Collapsed;


            // Панель быстрого доступа
            if (AppHelper.AppBar) this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Default;
            else this.ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;

            if (!AppHelper.IsTrial && ApplicationBar.MenuItems.Count >= 3) ApplicationBar.MenuItems.RemoveAt(2);

                
                

            SwipeAnimation.Begin();

            this.LikeLogic();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ShowBigImage.Visibility = System.Windows.Visibility.Collapsed;

            Random rn = new Random();


            int value = rn.Next(0, liveTileContent.Count);

            var appTile = ShellTile.ActiveTiles.First();
            var appTileDate = new StandardTileData();
            appTileDate.Title = "Лайфхакс";
            appTileDate.BackTitle = "Лайфхакc #" + liveTileContent.ElementAt(value).Key;
            appTileDate.BackContent = liveTileContent.ElementAt(value).Value;
            appTile.Update(appTileDate);
        }

        // Настройки
        private void AppSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));

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
        // Рассказать
        private void AppShow_Click(object sender, EventArgs e)
        {
            ShareStatusTask sharedStatusTask = new ShareStatusTask() { Status = "[ЛАЙФХАКС #" + AppHelper.PageIndex + "]\n" + BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex) };
            sharedStatusTask.Show();
        }
        // Добавить в избранное
        private void AppListAdd_Click(object sender, EventArgs e)
        {
            if (AppHelper.IsTrial && AppHelper.PageIndex == 51)
            {

            }
            else
            {
                if (!AppHelper.Storage.Contains(AppHelper.PageIndex.ToString())) AppHelper.Storage.Add(AppHelper.PageIndex.ToString(), "String" + AppHelper.PageIndex);
                else AppHelper.Storage.Remove(AppHelper.PageIndex.ToString());

                AppHelper.Storage.Save();

                this.LikeLogic();
                this.SelectedIndexLogic();
            }
        }
        // Избранное
        private void AppList_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ListPage.xaml", UriKind.Relative));

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
                    Text = "СОВЕТ #" + AppHelper.PageIndex,
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
                    Text = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex),
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
       


        private void MainPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 1) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 2) AppHelper.PageIndex++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 0) AppHelper.PageIndex++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 2) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 0) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 1) AppHelper.PageIndex++;
            
            _lastIndexItem = MainPivot.SelectedIndex;

            SwipeAnimation.Begin();

            SVFirst.ScrollToVerticalOffset(0.0d);
            SVSecond.ScrollToVerticalOffset(0.0d);
            SVThird.ScrollToVerticalOffset(0.0d);

            this.SelectedIndexLogic();
        }


        private void ShowImage()
        {
            ShowBigImage.Visibility = System.Windows.Visibility.Visible;
            ShowBigImage.Source = _bitmapImage;

            ApplicationBar.IsVisible = false;

            Banner.Margin = new Thickness(0, 0, 0, 0);
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
     
        private void RecShadow_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            ShowBigImage.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;

            Banner.Margin = new Thickness(0, 35, 0, 0);
        }
        private void ShowBigImage_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            ShowBigImage.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;

            Banner.Margin = new Thickness(0, 35, 0, 0);
        }



        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (!AppHelper.IsRate) this.Assessment();
        }
		
        private void TBSelect_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TBSelect.Text = "";
        }


        private void TBSelect_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.SelectAdvice(TBSelect.Text);
        }

        private void MainPivot_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            this.Focus();
        }

        private void AppFull_Click(object sender, EventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;
            marketplaceDetailTask.Show();
        }
    }
}