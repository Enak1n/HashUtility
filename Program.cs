using HashUtility;
using Microsoft.Extensions.Configuration;
using System;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                   .AddCommandLine(args, new Dictionary<string, string>
                   {
                       { "-p", "p" },
                       { "-s", "s" }})
                   .Build();

        string password = configuration["p"];
        string salt = configuration["s"];
        bool use3Des = args.Contains("-d");

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
        {
            Console.WriteLine("Проверьте правильность введения команды!");
            return;
        }

        var hashedPassword = PasswordHasher.Hash(password, salt, use3Des);

        Console.WriteLine(hashedPassword);
    }
}