namespace MultyTreadConsole;

public class Counter
{
    public static int TheNumber = 0;
    public void Count(string name)
    {
        for (int i = 0; i < 10000; i++)
        {
            TheNumber++;
            if (TheNumber % 2 == 0)
            {
                Console.WriteLine($"{name} {TheNumber} + ");
            }
            else
            {
                // TheNumber is odd
                if (TheNumber % 2 == 0)
                {
                    Console.WriteLine("????????????????????????????");
                }

                Console.WriteLine($"{name} {TheNumber} - ");
            }
        }
    }
}