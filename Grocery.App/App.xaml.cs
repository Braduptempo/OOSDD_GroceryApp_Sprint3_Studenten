using Grocery.App.Views;

namespace Grocery.App
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        public App(IServiceProvider serviceProvider, LoginView viewModel)
        {
            InitializeComponent();
            Services = serviceProvider;

            //MainPage = new AppShell();
            MainPage = new NavigationPage(viewModel);
        }
    }
}
