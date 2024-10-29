namespace InnoShop.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography;
using InnoShop.Domain.Data;
using Microsoft.EntityFrameworkCore;
using InnoShop.Domain.Models;

public class DBInitializer (InnoShopContext context)
{
    InnoShopContext _context = context;
    public void DeleteFromTable(string tableName)
    {
        _context.Database.ExecuteSqlRaw("TRUNCATE TABLE " + tableName+ "\nDBCC CHECKIDENT ('" + tableName + "', RESEED, 0);");
    }
    public void FillUserTypes()
    {
        string[] types = { "Частное лицо", "Компания", "Индивидуальный предприниматель" };
        List<UserType> userTypes = new List<UserType>();
        foreach (string type in types)
        {
            userTypes.Add(new UserType() {Name=type });
            
        }
        _context.UserTypes.AddRange(userTypes);
        Console.WriteLine("Добавлено записей: " + userTypes.Count);
    }
    public void FillLocalities(int size)
    {
        string[] strings = GetStrings("D:\\work\\Course 3\\Term 1\\РПБДИС\\lab1\\localities.txt");
        if (size > strings.Length)
            size = strings.Length;
        int[] indexes = GetRandomIndexes(size);
        List<Locality> localities = new List<Locality>();
        foreach(var i in indexes)
        {
            localities.Add(new Locality() { Name = strings[i] });

        }
        _context.Localities.AddRange(localities);
        Console.WriteLine("Добавлено записей: " + localities.Count);
    }
    public void FillUsers(int quantity)
    {
        string[] emails = GenEmails(quantity);
        int added = 0;
        int[] indexes = GetRandomIndexes(quantity);
        string[] logins = GetStrings("D:\\work\\Course 3\\Term 1\\РПБДИС\\lab1\\nicknamesfixed.txt");
        Random rnd = new Random(DateTime.Now.Millisecond);
        List<User> users = new List<User>();
        foreach (var i in indexes)
        {
            users.Add(new User()
            {
                Login = logins[i],
                PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(logins[i]))),
                Email = emails[i],
                UserTypeId = rnd.Next(1, 4),
                LocalityId = rnd.Next(1, 501)
            });
        }
        _context.Users.AddRange(users);
        Console.WriteLine("Добавлено записей: " + users.Count);
    }
    public void FillProductTypes()
    {
        string[] types = GetStrings("D:\\work\\Course 3\\Term 1\\РПБДИС\\lab1\\producttypes.txt");
        List<ProdType> prodTypes = new List<ProdType>();
        foreach (string type in types)
        {
            prodTypes.Add(new ProdType() { Name = type });

        }
        _context.ProdTypes.AddRange(prodTypes);
        Console.WriteLine("Добавлено записей: " + prodTypes.Count);

    }
    public void FillProducts(int quantity, int userQuantity, int typesQuantity)
    {
        Random rnd = new Random(DateTime.Now.Millisecond);
        List<Product> products = new List<Product>();
        for (int i = 0; i < quantity; i++)
        {
            int sold = rnd.Next(2);
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - rnd.Next(10), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            products.Add(new Product()
            {
                Name = "Продукт №"+(i+1),
                Description = "Описание"+(i+1),
                Cost = (decimal)(rnd.NextDouble()* 100000 / 100.0),
                ProdTypeId = rnd.Next(typesQuantity)+1,
                Sold = Convert.ToBoolean(rnd.Next(2)),
                UserId = rnd.Next(userQuantity) + 1,
                BuyerId = rnd.Next(userQuantity) + 1,
                CreationDate = dt,
            });
        }
        _context.Products.AddRange(products);
        Console.WriteLine("Добавлено записей: " + products.Count);

    }
    private static string[] GetStrings(string filePath)
    {
        StreamReader streamReader = new StreamReader(filePath);
        List<string> names = new List<string>();
        while (!streamReader.EndOfStream)
        {
            names.Add(streamReader.ReadLine());
        }
        return names.ToArray();
    }
    public static void FillFile()
    {
        string bad = GetStrings("D:\\work\\Course 3\\Term 1\\РПБДИС\\lab1\\nicknames.txt")[0];
        StreamWriter streamWriter = new StreamWriter("D:\\work\\Course 3\\Term 1\\РПБДИС\\lab1\\nicknamesfixed.txt");
        int end = 1;
        int start = 0;
        int i = 0;
        while (end < bad.Length)
        {
            if (bad[end] == Char.ToUpper(bad[end]))
            {
                streamWriter.WriteLine(bad.Substring(start, end - start));
                start = end;
                //Console.WriteLine(good[i]);
                i++;
            }
            end++;
        }
    }
    private static string[] GenEmails(int quantity)
    {
        Random rnd = new Random(DateTime.Now.Millisecond);
        string sample = "abcdefghijklmnopqrstuvwxyz0123456789";
        string[] domains = { "gmail.com", "yandex.ru", "outlook.com", "mail.ru" };
        string[] emails = new string[quantity];
        for (int i = 0; i < quantity; i++)
        {
            string email = new string(sample.OrderBy(x => rnd.Next()).Take(rnd.Next(1, 30)).ToArray()) + "@" + domains[rnd.Next(domains.Length)];
            emails[i] = email;
            //Console.WriteLine(email);
        }
        return emails;
    }
    private static int[] GetRandomIndexes(int quantity)
    {
        int[] indexes = new int[quantity];
        for (int i = 0; i < quantity; i++)
            indexes[i] = i;
        Random.Shared.Shuffle(indexes);
        return indexes;
    }
}
