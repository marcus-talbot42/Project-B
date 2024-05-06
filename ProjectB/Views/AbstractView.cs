namespace ProjectB.Views;

public abstract class AbstractView : IView
{

    private static DateTime _timeSinceLastInput = DateTime.Now;
    
    protected int ReadUserChoice(int min, int max, string invalidChoiceMessage)
    {
        int option;
        _timeSinceLastInput = DateTime.Now;
        while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out option) || (option < min || option > max))
        {
            _timeSinceLastInput = DateTime.Now;
            Console.Clear();
            Console.WriteLine(invalidChoiceMessage);
        }
        return option;
    }

    protected bool HasSessionExpired()
    {
        return (DateTime.Now - _timeSinceLastInput).TotalSeconds > 120;
    }

    public abstract void Output();
}