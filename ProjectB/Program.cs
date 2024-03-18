using ProjectB.ui;

MainMenu menu = new ();

while (true)
{
    menu.ShowMenu();
    menu.HandleInput(Console.ReadKey().KeyChar);
}

