using Microsoft.Extensions.DependencyInjection;
using ProjectB.Database;
using ProjectB.Settings;
using ProjectB.Views;
using ProjectB.Views.Main;
using Spectre.Console;

namespace ProjectB.Client
{
    public class DepotClient : IDepotClient
    {
        private IServiceProvider ServiceProvider { get; }
        protected IDatabaseContext Context { get; }
        protected IAnsiConsole Console { get; }

        protected bool IsRunning { get; set; } = true;
        protected MenuLevel CurrentMenu { get; set; } = MenuLevel.MainMenu;

        public DepotClient(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            Console = ServiceProvider.GetService<IAnsiConsole>()!;
            Context = ServiceProvider.GetService<IDatabaseContext>()!;
        }

        public void Run()
        {
            while (IsRunning && CurrentMenu >= MenuLevel.MainMenu) {
                var mainMenu = GetView<MainMenuView>();
                mainMenu.Output();
            }
        }

        public T GetView<T>() where T : IView
        {
            return ServiceProvider.GetService<T>()!;
        }
    }
}
