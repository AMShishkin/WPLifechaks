using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace LifeChacksApp
{
    public partial class App : Application
    {
        public static PhoneApplicationFrame RootFrame { get; private set; }

        private LicenseInformation _license = new LicenseInformation();

        public App()
        {
            // Глобальный обработчик неперехваченных исключений.
            UnhandledException += Application_UnhandledException;

            // Стандартная инициализация XAML
            InitializeComponent();

            AppHelper.IsTrial = _license.IsTrial();

            if (!AppHelper.IsTrial) AdDuplex.AdDuplexTrackingSDK.StartTracking("1a27bbd1-5f82-4561-b93c-1c65d4ea1984");

            // Инициализация телефона
            InitializePhoneApplication();
        }

        // Код для выполнения при запуске приложения (например, из меню "Пуск")
        // Этот код не будет выполняться при повторной активации приложения
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if (!AppHelper.IsTrial) AdDuplex.AdDuplexTrackingSDK.StartTracking("1a27bbd1-5f82-4561-b93c-1c65d4ea1984");

        }

        // Код для выполнения при активации приложения (переводится в основной режим)
        // Этот код не будет выполняться при первом запуске приложения
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!AppHelper.IsTrial) AdDuplex.AdDuplexTrackingSDK.StartTracking("1a27bbd1-5f82-4561-b93c-1c65d4ea1984");
        }

        // Код для выполнения при деактивации приложения (отправляется в фоновый режим)
        // Этот код не будет выполняться при закрытии приложения
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
             AppHelper.Storage["LAST_INDEX_PAGE"] = AppHelper.PageIndex;
             AppHelper.Storage["IS_RATE"] = AppHelper.IsRate;
             AppHelper.Storage["APP_BAR"] = AppHelper.AppBar;
             AppHelper.Storage["APP_EFF"] = AppHelper.AppEff;
             AppHelper.Storage["APP_FON"] = AppHelper.AppFon;
             AppHelper.Storage.Save();
        }

        // Код для выполнения при закрытии приложения (например, при нажатии пользователем кнопки "Назад")
        // Этот код не будет выполняться при деактивации приложения
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            AppHelper.Storage["LAST_INDEX_PAGE"] = AppHelper.PageIndex;
            AppHelper.Storage["IS_RATE"] = AppHelper.IsRate;
            AppHelper.Storage["APP_BAR"] = AppHelper.AppBar;
            AppHelper.Storage["APP_EFF"] = AppHelper.AppEff;
            AppHelper.Storage["APP_FON"] = AppHelper.AppFon;
            AppHelper.Storage.Save();
        }

        // Код для выполнения в случае ошибки навигации
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Ошибка навигации; перейти в отладчик
                Debugger.Break();
            }
        }

        // Код для выполнения на необработанных исключениях
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // Произошло необработанное исключение; перейти в отладчик
                Debugger.Break();
            }
        }

        #region Инициализация приложения телефона

        // Избегайте двойной инициализации
        private bool phoneApplicationInitialized = false;

        // Не добавляйте в этот метод дополнительный код
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Создайте кадр, но не задавайте для него значение RootVisual; это позволит
            // экрану-заставке оставаться активным, пока приложение не будет готово для визуализации.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Обработка сбоев навигации
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Обработка запросов на сброс для очистки стека переходов назад
            RootFrame.Navigated += CheckForResetNavigation;

            // Убедитесь, что инициализация не выполняется повторно
            phoneApplicationInitialized = true;
        }

        // Не добавляйте в этот метод дополнительный код
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Задайте корневой визуальный элемент для визуализации приложения
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Удалите этот обработчик, т.к. он больше не нужен
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // Если приложение получило навигацию "reset", необходимо проверить
            // при следующей навигации, чтобы проверить, нужно ли выполнять сброс стека
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Отменить регистрацию события, чтобы оно больше не вызывалось
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Очистка стека только для "новых" навигаций (вперед) и навигаций "обновления"
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // Очистка всего стека страницы для согласованности пользовательского интерфейса
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // ничего не делать
            }
        }

        #endregion
    }
}
    