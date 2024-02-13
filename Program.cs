using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        Console.Write("Enter your username: ");
        string userName = Console.ReadLine();

        Console.Write("Emter your password: ");
        string userPassword = Console.ReadLine();
        
        string hashedPassword = HashPassword(userPassword);
        
        bool isStrongPassword = CheckPasswordComplexity(userPassword);

        if (isStrongPassword)
        {
            Console.WriteLine("Password complexity: Strong");
        }
        else
        {
            Console.WriteLine("Password complexity: Weak");
        }
        
        int passwordStrength = CalculatePasswordStrength(userPassword);
        Console.WriteLine($"Password strength: {passwordStrength}%");

        string salt = GenerateSalt();
        
        StringBuilder saltedPasswordBuilder = new StringBuilder();
        saltedPasswordBuilder.Append(userPassword).Append(salt);
        string saltedPassword = saltedPasswordBuilder.ToString();
        
        DateTime passwordExpirationDate = DateTime.Now.AddMonths(3);
        Console.WriteLine($"Your password will expire on: {passwordExpirationDate}");

        Console.WriteLine($"Name: {userName}");
        Console.WriteLine($"Salt: {salt}");
        Console.WriteLine($"Hashed password: {hashedPassword}");
    }

    static string GenerateSalt()
    {
        byte[] randomBytes = new byte[64];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        string salt = Convert.ToBase64String(randomBytes);

        return salt;
    }

    static string HashPassword(string password)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    static bool CheckPasswordComplexity(string password)
    {
        return password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit) && password.Any(char.IsSymbol);
    }

    static int CalculatePasswordStrength(string password)
    {
        int strength = 0;
        
        strength += password.Length * 5;

        if (password.Any(char.IsUpper) && password.Any(char.IsLower))
        {
            strength += 20;
        }

        if (password.Any(char.IsDigit))
        {
            strength += 15;
        }

        if (password.Any(char.IsSymbol))
        {
            strength += 25;
        }

        return Math.Min(strength, 100); 
    }
}
