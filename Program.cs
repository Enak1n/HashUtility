using HashUtility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var switchMappings = new Dictionary<string, string>
        {
            { "-p", "password" },
            { "--password", "password" },
            { "-s", "salt" },
            { "--salt", "salt" },
        };

        var configuration = new ConfigurationBuilder()
            .AddCommandLine(args, switchMappings)
            .Build();

        string password = configuration["password"];
        string salt = configuration["salt"];
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
