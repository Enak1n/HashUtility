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

        string password = configuration["Password"];
        string salt = configuration["Salt"];
        bool use3Des = args.Contains("-d");
        bool isHelp = args.Contains("-h") || args.Contains("--help");

        if (isHelp)
        {
            Console.WriteLine("-p - пароль для хеширование");
            Console.WriteLine("-s - произвольный ключ хеширования");
            Console.WriteLine("-d - флаг для использования 3Des");
            return;
        }

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
        {
            Console.WriteLine("Проверьте правильность введения команды! Введите --help или -h для получения справки! ");
            return;
        }

        var hashedPassword = PasswordHasher.Hash(password, salt, use3Des);

        Console.WriteLine(hashedPassword);
    }
}
