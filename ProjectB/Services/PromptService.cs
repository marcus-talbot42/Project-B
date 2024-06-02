using ProjectB.Choices;
using ProjectB.Models;
using ProjectB.Enums;
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
                    .Title(Translation.Get(titleKey))
                    .PageSize(10)
                    .MoreChoicesText(Translation.Get("moreItems"))
                    .AddChoices(navigationItems)).Value;

        public string AskTicketNumber(string titleKey) => Console.Ask<string>(Translation.Get(titleKey)!);

        public string AskTicketOrEmployeeNumber(string titleKey) => Console.Ask<string>(Translation.Get(titleKey)!);

        public string AskPassword(string titleKey) => Console.Prompt(
            new TextPrompt<string>(Translation.Get(titleKey)!).PromptStyle("red").Secret());

        public string AskUsername(string titleKey) => Console.Ask<string>(Translation.Get(titleKey)!);

        public Language AskLanguage(string titleKey)
        {
            var options = new List<NamedChoice<Language>>();
            foreach (var language in Enum.GetValues(typeof(Language)))
                options.Add(new NamedChoice<Language>(Translation.Get("lang_name_" + language.ToString()!.ToLower()), (Language)language));

            return Console.Prompt(
                new SelectionPrompt<NamedChoice<Language>>()
                    .Title(Translation.Get(titleKey))
                    .PageSize(10)
                    .MoreChoicesText(Translation.Get("moreItems"))
                    .AddChoices(options)).Value;
        }

        public UserRole AskRole(string titleKey)
        {
            return Console.Prompt(
                new SelectionPrompt<UserRole>()
                    .Title(Translation.Get(titleKey))
                    .PageSize(10)
                    .AddChoices(UserRole.Guest, UserRole.Guide, UserRole.DepartmentHead));
        }

        public DateOnly AskDate(string titleKey)
        {
            return Console.Prompt(
                new TextPrompt<DateOnly>(Translation.Get(titleKey)!)
                    .PromptStyle("green")
                    .ValidationErrorMessage(Translation.Get("invalidDateOnly")));
        }

        public TimeOnly AskTime(string titleKey)
        {
            return Console.Prompt(
                new TextPrompt<TimeOnly>(Translation.Get(titleKey)!)
                    .PromptStyle("green")
                    .ValidationErrorMessage(Translation.Get("invalidTimeOnly")));
        }

        public Tour AskTour(string titleKey, IEnumerable<NamedChoice<Tour>> options)
        {
            return Console.Prompt(
                new SelectionPrompt<NamedChoice<Tour>>()
                    .Title(Translation.Get(titleKey))
                    .PageSize(10)
                    .MoreChoicesText(Translation.Get("moreItems"))
                    .AddChoices(options)).Value;
        }

        public int AskNumber(string titleKey, int? min = null, int? max = null)
        {
            return Console.Prompt(
                new TextPrompt<int>(Translation.Get(titleKey))
                    .PromptStyle("green")
                    .ValidationErrorMessage(Translation.Get("invalidNumber"))
                    .Validate(inputNumber =>
                    {
                        if (min != null && inputNumber < min)
                            return ValidationResult.Error(Translation.Get("invalidNumberBelowMin"));

                        if (max != null && inputNumber > max)
                            return ValidationResult.Error(Translation.Get("invalidNumberAboveMax"));

                        return ValidationResult.Success();
                    }));
        }

        public bool AskYesNo(string titleKey, string keyYes, string keyNo)
        {
            return Console.Prompt(
                new SelectionPrompt<NamedChoice<bool>>()
                    .Title(Translation.Get(titleKey))
                    .PageSize(10)
                    .AddChoices(
                        new NamedChoice<bool>(Translation.Get(keyYes), true), 
                        new NamedChoice<bool>(Translation.Get(keyNo), false))).Value;
        }

        public void ShowSpinner(string titleKey, int delayInMs)
        {
            Console.Status().Start(Translation.Get(titleKey), ctx =>
            {
                ctx.Spinner(Spinner.Known.Material);
                Thread.Sleep(delayInMs);
            });
        }
    }
}