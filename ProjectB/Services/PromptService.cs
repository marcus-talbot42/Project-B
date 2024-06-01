using ProjectB.Choices;
using ProjectB.Models;
using ProjectB.Settings;
using Spectre.Console;

namespace ProjectB.Services
{
    public class PromptService(ITranslationService translationService, IDateTimeService dateTimeService, IGuestService guestService, ITourService tourService, IAnsiConsole console) : IPromptService
    {
        public IAnsiConsole Console { get; set; } = console;
        public ITranslationService Translation { get; set; } = translationService;
        public IDateTimeService DateTime { get; set; } = dateTimeService;
        public IGuestService GuestService { get; set; } = guestService;
        public ITourService TourService { get; set; } = tourService;

        public Action ShowMenu(string titleKey, List<NamedChoice<Action>> navigationItems) => Console.Prompt(
                new SelectionPrompt<NamedChoice<Action>>()
                    .Title(Translation.GetTranslationString(titleKey))
                    .PageSize(10)
                    .MoreChoicesText(Translation.GetTranslationString("moreItems"))
                    .AddChoices(navigationItems)).Value;

        public string AskTicketNumber(string titleKey) => Console.Ask<string>(Translation.GetTranslationString(titleKey)!);

        public string AskPassword(string titleKey) => Console.Prompt(
            new TextPrompt<string>(Translation.GetTranslationString(titleKey)!).PromptStyle("red").Secret());

        public string AskUsername(string titleKey) => Console.Ask<string>(Translation.GetTranslationString(titleKey)!);

        public Language AskLanguage(string titleKey)
        {
            var options = new List<NamedChoice<Language>>();
            foreach (var language in Enum.GetValues(typeof(Language)))
                options.Add(new NamedChoice<Language>(Translation.GetTranslationString("lang_name_" + language.ToString()!.ToLower())!, (Language)language));

            return Console.Prompt(
                new SelectionPrompt<NamedChoice<Language>>()
                    .Title(Translation.GetTranslationString(titleKey))
                    .PageSize(10)
                    .MoreChoicesText(Translation.GetTranslationString("moreItems"))
                    .AddChoices(options)).Value;
        }

        public UserRole AskRole(string titleKey)
        {
            return Console.Prompt(
                new SelectionPrompt<UserRole>()
                    .Title(Translation.GetTranslationString(titleKey))
                    .PageSize(10)
                    .AddChoices(UserRole.Guest, UserRole.Guide, UserRole.DepartmentHead));
        }

        public DateOnly AskDate(string titleKey)
        {
            return Console.Prompt(
                new TextPrompt<DateOnly>(Translation.GetTranslationString(titleKey)!)
                    .PromptStyle("green")
                    .ValidationErrorMessage(Translation.GetTranslationString("invalidDateOnly")!));
        }

        public Tour AskTour(string titleKey, IEnumerable<NamedChoice<Tour>> options)
        {
            return Console.Prompt(
                new SelectionPrompt<NamedChoice<Tour>>()
                    .Title(Translation.GetTranslationString(titleKey))
                    .PageSize(10)
                    .MoreChoicesText(Translation.GetTranslationString("moreItems"))
                    .AddChoices(options)).Value;
        }
    }
}