namespace InnoShop.Infrastructure.Initialize;
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

public class DBInitializer(InnoShopContext context)
{
    InnoShopContext _context = context;
    public string DeleteFromTable(string tableName)
    {
        var n = _context.Database.ExecuteSqlRaw("DELETE FROM " + tableName + "\nDBCC CHECKIDENT ('" + tableName + "', RESEED, 0);");
        return $"Удалено записей из таблицы {tableName}: {n}\n";
    }
    public string FillUserTypes()
    {
        string[] types = { "Частное лицо", "Компания", "Индивидуальный предприниматель" };
        List<UserType> userTypes = new List<UserType>();
        foreach (string type in types)
        {
            userTypes.Add(new UserType() { Name = type });

        }
        _context.UserTypes.AddRange(userTypes);
        _context.SaveChanges();
        return "Добавлено записей в таблицу UserTypes: " + userTypes.Count + "\n";
    }
    public string FillLocalities(int size)
    {
        string[] strings = GetStrings("../Infrastructure/Initialize/localities.txt");
        if (size > strings.Length)
            size = strings.Length;
        int[] indexes = GetRandomIndexes(size);
        List<Locality> localities = new List<Locality>();
        foreach (var i in indexes)
        {
            localities.Add(new Locality() { Name = strings[i] });

        }
        _context.Localities.AddRange(localities);
        _context.SaveChanges();
        return "Добавлено записей в таблицу Localities: " + localities.Count + "\n";
    }
    public string FillUsers(int quantity)
    {
        string[] emails = GenEmails(quantity);
        int added = 0;
        int[] indexes = GetRandomIndexes(quantity);
        string[] logins = GetStrings("../Infrastructure/Initialize/nicknamesfixed.txt");
        Random rnd = new Random(DateTime.Now.Millisecond);
        List<User> users = new List<User>();
        foreach (var i in indexes)
        {
            users.Add(new User()
            {
                UserName = logins[i],
                PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(logins[i]))),
                Email = emails[i],
                UserTypeId = rnd.Next(1, 4),
                LocalityId = rnd.Next(1, 501)
            });
        }
        _context.Users.AddRange(users);
        _context.SaveChanges();
        return "Добавлено записей в таблицу Users: " + users.Count + "\n";
    }
    public string FillProductTypes()
    {
        string[] types = GetStrings("../Infrastructure/Initialize/producttypes.txt");
        List<ProdType> prodTypes = new List<ProdType>();
        foreach (string type in types)
        {
            prodTypes.Add(new ProdType() { Name = type });

        }
        _context.ProdTypes.AddRange(prodTypes);
        _context.SaveChanges();
        return "Добавлено записей в таблицу ProdTypes: " + prodTypes.Count + "\n";

    }
    public string FillProducts(int quantity, int userQuantity, int typesQuantity)
    {
        Random rnd = new Random(DateTime.Now.Millisecond);
        List<Product> products = new List<Product>();
        for (int i = 0; i < quantity; i++)
        {
            bool sold = Convert.ToBoolean(rnd.Next(2));
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            dt.AddDays(-rnd.Next(10));
            products.Add(new Product()
            {
                Name = "Продукт №" + (i + 1),
                Description = "Описание" + (i + 1),
                Cost = (decimal)(rnd.NextDouble() * 100000 / 100.0),
                ProdTypeId = rnd.Next(typesQuantity) + 1,
                Sold = sold,
                UserId = rnd.Next(userQuantity) + 1,
                BuyerId = sold ? rnd.Next(userQuantity) + 1 : null,
                CreationDate = dt,
            });
        }
        _context.Products.AddRange(products);
        _context.SaveChanges();
        return "Добавлено записей в таблицу Products: " + products.Count + "\n";

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
        string bad = GetStrings("../Infrastructure/Initialize/nicknames.txt")[0];
        StreamWriter streamWriter = new StreamWriter("../Infrastructure/Initialize/nicknamesfixed.txt");
        int end = 1;
        int start = 0;
        int i = 0;
        while (end < bad.Length)
        {
            if (bad[end] == char.ToUpper(bad[end]))
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
