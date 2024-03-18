using ProjectB.Resources;
using ProjectB.settings;
using ProjectB.ui;
using ProjectBTest.utils;

namespace ProjectBTest.ui;

[TestClass]
public class MainMenuTest
{
    
    [TestMethod]
    public void TestMainMenuPrintsInDutch()
    {
        Settings.Language = Language.NL;
        MainMenu mainMenu = new();
        using (ConsoleOutput consoleOutput = new ())
        {
            mainMenu.ShowMenu();
            Assert.AreEqual(consoleOutput.GetOuput(), Translations.translation("MAIN_MENU") + "\n");
        }
    }
    
    [TestMethod]
    public void TestMainMenuPrintsInEnglish()
    {
        Settings.Language = Language.EN;
        MainMenu mainMenu = new();
        using (ConsoleOutput consoleOutput = new ())
        {
            mainMenu.ShowMenu();
            Assert.AreEqual(consoleOutput.GetOuput(), Translations.translation("MAIN_MENU") + "\n");
        }
    }
}