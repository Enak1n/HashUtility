using HashUtility;

class Program
{
    static void Main(string[] args)
    {
        string password = null;
        string salt = null;
        bool use3Des = false;

        if(args.Length == 0)
        {
            Console.WriteLine("Проверьте правильность введения команды!");
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-p":
                    try
                    {
                        password = args[++i];
                    }
                    catch
                    {
                        Console.WriteLine("Проверьте правильность введения команды!");
                        return;
                    }
                    break;
                case "-s":
                    try
                    {
                        salt = args[++i];
                    }
                    catch
                    {
                        Console.WriteLine("Проверьте правильность введения команды!");
                        return;
                    }
                    break;
                case "-d":
                    use3Des = true;
                    break;
                default:
                    Console.WriteLine("Проверьте правильность введения команды!");
                    return;
            }
        }

        var hashedPassword = PasswordHasher.Hash(password, salt, use3Des);

        Console.WriteLine(hashedPassword);
    }
}