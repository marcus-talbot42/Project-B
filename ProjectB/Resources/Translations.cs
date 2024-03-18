using ProjectB.settings;

namespace ProjectB.Resources;

public class Translations
{
    private const string MAIN_MENU_NL = """
                                        Welkom!
                                        
                                        Kies een van de onderstaande opties om door te gaan:
                                        1) Log in als gast
                                        2) Log in al medewerker
                                        3) Switch language to English
                                        
                                        """;

    private const string MAIN_MENU_EN = """
                                        Welcome!
                                        
                                        Choose one of the options below to continue:
                                        1) Login as a guest
                                        2) Login as an employee
                                        3) Verander de taal naar Nederlands
                                        
                                        """;

    private const string NOT_IMPLEMENTED_NL = "Deze feature is nog niet geimplementeerd.\n";

    private const string NOT_IMPLEMENTED_EN = "This feature has not been implemented yet.\n";

    private const string LANGUAGE_CHANGED_NL = "Taal veranderd naar Nederlands.\n";

    private const string LANGUAGE_CHANGED_EN = "Language changed to English.\n";

    public static string translation(string name)
    {
        if (Settings.Language == Language.NL)
        {
            switch(name)
            {
                case "MAIN_MENU":
                    return MAIN_MENU_NL;
                case "NOT_IMPLEMENTED":
                    return NOT_IMPLEMENTED_NL;
                case "LANGUAGE_CHANGED":
                    return LANGUAGE_CHANGED_NL;
                default:
                    return String.Format("Error: Translation not found for {name}", name);
            }
        }
        
        if (Settings.Language == Language.EN)
        {
            switch (name)
            {
                case "MAIN_MENU":
                    return MAIN_MENU_EN;
                case "NOT_IMPLEMENTED":
                    return NOT_IMPLEMENTED_EN;
                case "LANGUAGE_CHANGED":
                    return LANGUAGE_CHANGED_EN;
                default:
                    return String.Format("Error: Translation not found for {name}", name);
            }
        }
        return String.Format("Error: Translation not found for {name}", name);
    }
}