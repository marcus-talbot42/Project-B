using ProjectB.Choices;
using ProjectB.Models;
using ProjectB.Enums;

namespace ProjectB.Services
{
    public interface IPromptService
    {
        DateOnly AskDate(string titleKey);
        Language AskLanguage(string titleKey);
        int AskNumber(string titleKey, int? min = null, int? max = null);
        string AskPassword(string titleKey);
        UserRole AskRole(string titleKey);
        string AskTicketNumber(string titleKey);
        TimeOnly AskTime(string titleKey);
        Tour AskTour(string titleKey, IEnumerable<NamedChoice<Tour>> options);
        string AskUsername(string titleKey);
        Action ShowMenu(string titleKey, List<NamedChoice<Action>> navigationItems);
    }
}