using ProjectBTest.utils;

namespace ProjectBTest.ui;

[TestClass]
public class MainMenuTest
{

    [TestMethod]
    public void TestMainMenuPrintsInDutch()
    {
        Settings.Lanuage = Lanuage.NL;
        MainMenu mainMenu = new();
        using (ConsoleOutput consoleOutput = new())
        {
            mainMenu.ShowMenu();
            Assert.AreEqual(consoleOutput.GetOuput(), Translations.translation("MAIN_MENU") + "\n");
        }
    }

    [TestMethod]
    public void TestMainMenuPrintsInEnglish()
    {
        Settings.Lanuage = Lanuage.EN;
        MainMenu mainMenu = new();
        using (ConsoleOutput consoleOutput = new())
        {
            mainMenu.ShowMenu();
            Assert.AreEqual(consoleOutput.GetOuput(), Translations.translation("MAIN_MENU") + "\n");
        }
    }
}