using System;

public class Customer
{
    public int Id { get; set; }
    private string name;
    private string address;
    public string Code { get; set; }
    public string Product { get; set; }
    public string Category { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ReceivedDate { get; set; }

    public bool IsPremium { get; protected set; } = false;

    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 50)
            {
                Console.WriteLine("Invalid Name! Max 50 characters allowed.");
                name = null;
            }
            else name = value;
        }
    }

    public string Address
    {
        get => address;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200)
            {
                Console.WriteLine("Invalid Address! Max 200 characters allowed.");
                address = null;
            }
            else address = value;
        }
    }

    public Customer(int id, string name, string address, string code, string product, string category)
    {
        Id = id;
        Name = name;
        Address = address;
        Code = code;
        Product = product;
        Category = category;
        OrderDate = DateTime.Now;
        ReceivedDate = DateTime.Now.AddDays(4);
    }

    public virtual void Display()
    {
        Console.WriteLine("-------------------------------------------------------------------------------------------");
        Console.WriteLine(
            $"ID:{Id} | Name:{Name} | Code:{Code} | Address:{Address}\n" +
            $"Product:{Product} | Category:{Category} | " +
            $"Order Placed:{OrderDate.ToShortDateString()} | Received:{ReceivedDate.ToShortDateString()}" +
            $"{(IsPremium ? " | Premium Customer" : "")}"
        );
        Console.WriteLine("-------------------------------------------------------------------------------------------");
    }
}

public class PremiumCustomer : Customer
{
    public int RewardPoints { get; set; }

    public PremiumCustomer(int id, string name, string address, string code, string product, string category, int rewardPoints)
        : base(id, name, address, code, product, category)
    {
        RewardPoints = rewardPoints;
        IsPremium = true;
    }

    public override void Display()
    {
        base.Display();
        Console.WriteLine($"Reward Points: {RewardPoints}");
    }
}

public abstract class CustomerManager
{
    protected Customer[] customers = new Customer[500];
    protected int count = 0;
    public abstract void Add(Customer c);
    public abstract void ViewAll();
    public abstract void Search();
    public abstract void Delete();
    public abstract void Update();
    public abstract void Sort();

    protected int Find(int id)
    {
        for (int i = 0; i < count; i++) if (customers[i].Id == id) return i;
        return -1;
    }

    protected int FindCode(string code)
    {
        for (int i = 0; i < count; i++)
            if (customers[i].Code.Equals(code, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
    }
}

public class MyCustomerManager : CustomerManager
{
    public override void Add(Customer c)
    {
        if (c.Name == null || c.Address == null) { Console.WriteLine("Invalid Name or Address. Customer not added."); return; }
        if (Find(c.Id) != -1) { Console.WriteLine("ID exists!"); return; }
        if (FindCode(c.Code) != -1) { Console.WriteLine("Code exists!"); return; }
        if (count < 500) { customers[count++] = c; Console.WriteLine("Added!"); }
        else Console.WriteLine("Customer list full!");
    }

    public override void ViewAll()
    {
        if (count == 0) { Console.WriteLine("No customers!"); return; }
        for (int i = 0; i < count; i++) customers[i].Display();
    }

    public override void Search()
    {
        Console.Write("Search by (1)ID (2)Name (3)Code: ");
        string opt = Console.ReadLine();
        if (opt == "1")
        {
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
            int idx = Find(id);
            if (idx != -1) customers[idx].Display();
            else Console.WriteLine("Not found!");
        }
        else if (opt == "2")
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            bool found = false;
            for (int i = 0; i < count; i++)
                if (customers[i].Name.ToLower().Contains(name.ToLower()))
                { customers[i].Display(); found = true; }
            if (!found) Console.WriteLine("Not found!");
        }
        else if (opt == "3")
        {
            Console.Write("Code: ");
            string code = Console.ReadLine();
            int idx = FindCode(code);
            if (idx != -1) customers[idx].Display();
            else Console.WriteLine("Not found!");
        }
    }

    public override void Delete()
    {
        Console.Write("ID to delete: ");
        int id = int.Parse(Console.ReadLine());
        int idx = Find(id);
        if (idx == -1) { Console.WriteLine("Not found!"); return; }

        customers[idx].Display();
        Console.Write("Are you sure you want to remove this Customer? (Y/N): ");
        if (Console.ReadLine().ToUpper() == "Y")
        {
            for (int i = idx; i < count - 1; i++) customers[i] = customers[i + 1];
            count--; Console.WriteLine("Removed!");
        }
        else Console.WriteLine("Cancelled!");
    }

    public override void Update()
    {
        Console.Write("ID to update: ");
        int id = int.Parse(Console.ReadLine());
        int idx = Find(id);
        if (idx == -1) { Console.WriteLine("Not found!"); return; }

        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("Address: "); string addr = Console.ReadLine();
        Console.Write("Product: "); string prod = Console.ReadLine();
        Console.Write("Category: "); string cat = Console.ReadLine();

        if (!string.IsNullOrEmpty(name)) customers[idx].Name = name;
        if (!string.IsNullOrEmpty(addr)) customers[idx].Address = addr;
        if (!string.IsNullOrEmpty(prod)) customers[idx].Product = prod;
        if (!string.IsNullOrEmpty(cat)) customers[idx].Category = cat;
        Console.WriteLine("Updated!");
    }

    public override void Sort()
    {
        Console.Write("Sort by (1)Name (2)ID: ");
        bool byName = Console.ReadLine() == "1";
        for (int i = 0; i < count - 1; i++)
            for (int j = 0; j < count - i - 1; j++)
                if ((byName && customers[j].Name.CompareTo(customers[j + 1].Name) > 0) ||
                    (!byName && customers[j].Id > customers[j + 1].Id))
                {
                    var temp = customers[j]; customers[j] = customers[j + 1]; customers[j + 1] = temp;
                }
        ViewAll();
    }
}

class Program
{
    static void Main()
    {
        var m = new MyCustomerManager();
        m.Add(new Customer(101, "Rajesh Kumar", "Bengaluru", "C001", "Laptop", "Electronics"));
        m.Add(new Customer(102, "Anita Sharma", "Delhi", "C002", "Washing Machine", "Home Appliance"));
        m.Add(new Customer(103, "Ravi Patel", "Ahmedabad", "C003", "Mobile", "Electronics"));
        m.Add(new Customer(104, "Priya Reddy", "Hyderabad", "C004", "Shoes", "Clothing"));
        m.Add(new Customer(105, "Amit Verma", "Lucknow", "C005", "Refrigerator", "Home Appliance"));
        m.Add(new Customer(106, "Sneha Nair", "Kochi", "C006", "Headphones", "Electronics"));

        m.Add(new PremiumCustomer(201, "Neha Singh", "Mumbai", "P101", "Smartphone", "Electronics", 120));
        m.Add(new PremiumCustomer(202, "Arjun Mehta", "Jaipur", "P102", "Smartwatch", "Electronics", 90));
        m.Add(new PremiumCustomer(203, "Meera Iyer", "Chennai", "P103", "Designer Saree", "Clothing", 150));
        m.Add(new PremiumCustomer(204, "Vikram Das", "Kolkata", "P104", "LED TV", "Electronics", 200));

        while (true)
        {
            Console.WriteLine("\n=== Customer Management ===\n1.Add Regular 2.Add Premium 3.View 4.Search 5.Update 6.Delete 7.Sort 8.Exit");
            Console.Write("Enter your Choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                    Console.Write("Name: "); string name = Console.ReadLine();
                    Console.Write("Address: "); string addr = Console.ReadLine();
                    Console.Write("Code: "); string code = Console.ReadLine();
                    Console.Write("Product: "); string prod = Console.ReadLine();
                    Console.Write("Category: "); string cat = Console.ReadLine();
                    m.Add(new Customer(id, name, addr, code, prod, cat));
                    break;

                case "2":
                    Console.Write("ID: "); int pid = int.Parse(Console.ReadLine());
                    Console.Write("Name: "); string pname = Console.ReadLine();
                    Console.Write("Address: "); string paddr = Console.ReadLine();
                    Console.Write("Code: "); string pcode = Console.ReadLine();
                    Console.Write("Product: "); string pprod = Console.ReadLine();
                    Console.Write("Category: "); string pcat = Console.ReadLine();
                    Console.Write("Reward Points: "); int points = int.Parse(Console.ReadLine());
                    m.Add(new PremiumCustomer(pid, pname, paddr, pcode, pprod, pcat, points));
                    break;

                case "3": m.ViewAll(); break;
                case "4": m.Search(); break;
                case "5": m.Update(); break;
                case "6": m.Delete(); break;
                case "7": m.Sort(); break;
                case "8":
                    Console.Write("Exit? (Y/N): ");
                    if (Console.ReadLine().ToUpper() == "Y") { Console.WriteLine("Thank You! Visit again"); return; }
                    break;
                default: Console.WriteLine("Invalid!"); break;
            }
        }
    }
}

