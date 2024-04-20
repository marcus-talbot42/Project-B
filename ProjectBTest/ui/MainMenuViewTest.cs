using ProjectB.Resources;
using ProjectB.settings;
using ProjectB.Views;
using ProjectBTest.utils;

namespace ProjectBTest.ui;

[TestClass]
public class MainMenuViewTest
{
    
    [TestMethod]
    public void TestMainMenuPrintsInDutch()
    {
        Settings.Language = Language.NL;
        MainMenuView mainMenuView = new();
        using (ConsoleOutput consoleOutput = new ())
        {
            mainMenuView.Execute();
            Assert.AreEqual(consoleOutput.GetOuput(), Translations.translation("MAIN_MENU") + "\n");
        }
    }
    
    [TestMethod]
    public void TestMainMenuPrintsInEnglish()
    {
        Settings.Language = Language.EN;
        MainMenuView mainMenuView = new();
        using (ConsoleOutput consoleOutput = new ())
        {
            mainMenuView.Execute();
            Assert.AreEqual(consoleOutput.GetOuput(), Translations.translation("MAIN_MENU") + "\n");
        }
    }
}