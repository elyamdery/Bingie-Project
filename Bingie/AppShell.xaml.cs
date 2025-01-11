using Bingie.Views;
using Bingie.Views.Auth;

namespace Bingie
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("history", typeof(HistoryPage));
            Routing.RegisterRoute("daystatistics", typeof(DayStatisticsPage));
            Routing.RegisterRoute("register", typeof(RegistrationPage));
        }
    }
}