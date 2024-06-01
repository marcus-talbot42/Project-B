using ProjectB.Choices;
using ProjectB.Models;
using ProjectB.Settings;

namespace ProjectB.Services
{
    public interface IPromptService
    {
        DateOnly AskDate(string titleKey);
        Language AskLanguage(string titleKey);
        string AskPassword(string titleKey);
        UserRole AskRole(string titleKey);
        string AskTicketNumber(string titleKey);
        Tour AskTour(string titleKey, IEnumerable<NamedChoice<Tour>> options);
        string AskUsername(string titleKey);
        Action ShowMenu(string titleKey, List<NamedChoice<Action>> navigationItems);
    }
}