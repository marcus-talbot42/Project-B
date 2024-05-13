using ProjectB.Views.Admin;

namespace ProjectB.Views.Debug;

public class DebugView(CreateGuestView createGuestView, CreateEmployeeView createEmployeeView) : AbstractView
{
    private const string DEBUG_VIEW = """
                                      Welcome to the DebugView. To continue, please select one of the options in the list below.
                                      1. Create Guest
                                      2. Create Employee
                                      3. Exit
                                      """;

    public override void Output()
    {
        while (true)
        {
            Console.WriteLine(DEBUG_VIEW);
            int option = ReadUserChoice(1, 3, DEBUG_VIEW);
            switch (option)
            {
                case 1:
                    createGuestView.Output();
                    break;
                case 2:
                    createEmployeeView.Output();
                    break;
                case 3:
                    return;
            }
        }
    }
}