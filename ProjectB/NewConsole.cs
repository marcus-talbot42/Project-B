using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Spectre.Console;

namespace ProjectB
{
    public class Localization
    {
        private Dictionary<string, string> _translations;

        public Localization(string language)
        {
            var path = $"Localization/{language}.json";
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            else
            {
                throw new FileNotFoundException($"Localization file not found: {path}");
            }
        }

        public string Get(string key)
        {
            if (_translations.TryGetValue(key, out var value))
            {
                return value;
            }
            return $"{{UNTRANSLATED: {key}}}";
        }
    }

    public static class Program
    {
        private static Localization loc;

        public static void Main(string[] args)
        {
            loc = new Localization("nl");  // English localization
            if (!PerformLogin())
            {
                return;  // Exit if login fails
            }

            bool running = true;
            while (running)
            {
                Console.Clear();
                AnsiConsole.Write(new Rule(loc.Get("warningApplicationIsInBeta")).RuleStyle("orange1"));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title(loc.Get("mainMenuTitle"))
                        .PageSize(10)
                        .AddChoices(new[] {
                            "1: Sign up for a tour",
                            "2: Delete your sign up for a tour",
                            "3: Adjust tour",
                            "4: View guest for a tour",
                            "5: FAQ",
                            "6: Logout",
                            "9: Exit"
                        }));

                switch (choice)
                {
                    case "1: Sign up for a tour":
                        TourSignup.Process();
                        break;
                    case "2: Delete your sign up for a tour":
                        TourSignupDeletion.Process();
                        break;
                    case "3: Adjust tour":
                        TourAdjustment.Process();
                        break;
                    case "4: View guest for a tour":
                        TourGuestViewer.Process();
                        break;
                    case "5: FAQ":
                        FaqViewer.Process();
                        break;
                    case "6: Logout":
                        Logout.Process();
                        break;
                    case "9: Exit":
                        Exit.Process();
                        break;
                }

                if (running)
                {
                    AnsiConsole.Markup(loc.Get("tryAgain"));
                    Console.ReadKey();
                }
            }
        }

        private static int loginAttempts = 0;
        public static bool PerformLogin()
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                AnsiConsole.Write(new Rule(loc.Get("loginTitle")).RuleStyle("grey"));

                var username = AnsiConsole.Ask<string>(loc.Get("enterUsername"));
                var password = AnsiConsole.Prompt(
                    new TextPrompt<string>(loc.Get("enterPassword"))
                        .Secret());

                // Login out process


                if (username == "admin" && password == "admin")
                {
                    AnsiConsole.Status()
                        .Start("[green]Logging in...[/]\n", ctx =>
                        {
                            // Simulate login
                            AnsiConsole.MarkupLine("Authenticating...");
                            Thread.Sleep(5000);

                        });
                    AnsiConsole.Markup(loc.Get("loginSuccessful") + "\n");
                    return true;
                }
                else
                {
                    AnsiConsole.Markup(string.Format(loc.Get("invalidCredentials"), attempt) + "\n");

                    if (attempt == maxAttempts)
                    {
                        AnsiConsole.Markup(loc.Get("maxAttemptsReached") + "\n");
                        return false;
                    }
                }
            }

            return false;
        }
    }

    // Example of individual process classes
    public static class TourSignup
    {
        public static void Process()
        {
            AnsiConsole.Markup("[green]Processing tour signup...[/]\n");
        }
    }

    public static class TourSignupDeletion
    {
        public static void Process()
        {
            AnsiConsole.Markup("[green]Processing tour signup deletion...[/]\n");
        }
    }

    public static class TourAdjustment
    {
        public static void Process()
        {
            AnsiConsole.Markup("[green]Adjusting tour...[/]\n");
        }
    }

    public static class TourGuestViewer
    {
        public static void Process()
        {
            AnsiConsole.Markup("[green]Viewing tour guest...[/]\n");
        }
    }

    public static class FaqViewer
    {
        public static void Process()
        {
            AnsiConsole.Markup("[green]Displaying FAQs...[/]\n");
        }
    }

    public static class Logout
    {
        public static void Process()
        {
            // Login out process
            AnsiConsole.Status()
                .Start("[green]Logging out...[/]\n", ctx =>
                {
                    // Simulate logout
                    AnsiConsole.MarkupLine("Logging out...");
                    Thread.Sleep(10000);
                });
            // Return to login screen
            // bool running = false;
            Program.PerformLogin();
        }
    }

    public static class Exit
    {
        public static void Process()
        {
            AnsiConsole.Markup("[green]Exiting program...[/]\n");
            Environment.Exit(0);
        }
    }
}
