using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

public class Day2 : ChallengeDay
{
    public Day2Shop? ChallengeShop { get; set; }
    public Day2Cart ChallengeCart { get; set; } = new Day2Cart();

    public Day2()
        : base(
            2,
            @"
Shopping Cart System
------------------------
- Shop
- Product
- Cart
",
            @"
Key Controls:
1. Recreate Shop
2. Add Product
3. Display Shop Details
4. Display Product List
5. Search Product
    1. Add Product
    2. Remove Product
6. Display Cart
7. Clear Cart
            ",
            50
        ) { }

    public override void InputInterface()
    {
        if (ChallengeShop == null)
        {
            CreateShopUI();
        }

        Console.WriteLine("\nEntry:");
        var action = Console.ReadLine();
        Console.WriteLine();
        switch (action)
        {
            case "1":
                ClearCartUI();
                CreateShopUI();
                break;
            case "2":
                CreateProductUI();
                break;
            case "3":
                ChallengeShop?.DisplayShopDetails();
                break;
            case "4":
                ChallengeShop?.DisplayProductList();
                break;
            case "5":
                SearchProductUI();
                break;
            case "6":
                ChallengeCart.DisplayCart();
                break;
            case "7":
                ClearCartUI();
                break;
            default:
                return;
        }

        InputInterface();
    }

    public void CreateShopUI()
    {
        Console.WriteLine("Create Shop:");
        Console.WriteLine("Name:");
        string? shopName = Console.ReadLine();
        Console.WriteLine("Address:");
        string? shopAddress = Console.ReadLine();
        Console.WriteLine("Operating Hours:");
        string? shopOperatingHours = Console.ReadLine();

        ChallengeShop = new Day2Shop(
            shopName ?? "Unknown",
            shopAddress ?? "-",
            shopOperatingHours ?? "-"
        );

        Console.WriteLine("\n! Shop Created");
    }

    public void CreateProductUI()
    {
        Console.WriteLine("Add Product");
        Console.WriteLine("Name:");
        string? productName = Console.ReadLine();
        Console.WriteLine("Price:");
        decimal? productPrice = Convert.ToInt32(Console.ReadLine());

        Day2Product newProduct = new Day2Product(productName ?? "Unknown", productPrice ?? 0);
        ChallengeShop?.AddProduct(newProduct);
    }

    public void SearchProductUI()
    {
        Console.WriteLine("Search Product:");
        string? searchName = Console.ReadLine();
        Console.WriteLine();
        var searchShop = ChallengeShop?.SearchProductByName(searchName ?? "-");

        if (searchShop != null)
        {
            Console.WriteLine("Entry");
            string? entry = Console.ReadLine();
            switch (entry)
            {
                case "1":
                    Console.WriteLine("Quantity:");
                    var quantity = Convert.ToInt32(Console.ReadLine());
                    ChallengeCart.AddItem(searchShop, quantity);
                    break;
                case "2":
                    ChallengeCart.RemoveItem(searchShop);
                    break;
                default:
                    return;
            }
        }
        Console.WriteLine();
    }

    public void ClearCartUI()
    {
        ChallengeCart = new Day2Cart();
        Console.WriteLine("! Cart Cleared");
    }
}

public class Day2Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Day2Product(string name, decimal price)
    {
        this.Name = name;
        this.Price = price;
    }

    public void DisplayProductDetails()
    {
        Console.WriteLine($"{Name}  {Price}$");
    }
}

public class Day2Shop
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string OperatingHours { get; set; }

    public Dictionary<string, Day2Product> Products { get; set; } = [];

    public Day2Shop(string name, string address, string operatingHours)
    {
        this.Name = name;
        this.Address = address;
        this.OperatingHours = operatingHours;
    }

    public void DisplayShopDetails()
    {
        Console.WriteLine(
            $@"
{Name}
----------------------
Address: {Address}
Operating Hours: {OperatingHours}
            "
        );
    }

    public void DisplayProductList()
    {
        Console.WriteLine(
            $@"
Product List
----------------------"
        );
        int count = 1;
        foreach (var item in Products)
        {
            Console.WriteLine($"{count++}.");
            item.Value.DisplayProductDetails();
        }
    }

    public Day2Product? SearchProductByName(string name)
    {
        if (Products.ContainsKey(name))
        {
            Products[name].DisplayProductDetails();
            return Products[name];
        }
        else
        {
            Console.WriteLine("! No Item Found");
            return null;
        }
    }

    public void AddProduct(Day2Product product)
    {
        Products.Add(product.Name, product);
        Console.WriteLine("\n! Product Added");
    }
}

public class Day2Cart
{
    public string OrderId { get; private set; } = Guid.NewGuid().ToString();
    public Dictionary<Day2Product, int> CartList { get; set; } = [];
    public decimal TotalPrice { get; set; } = 0;

    public void DisplayCart()
    {
        Console.WriteLine(
            @$"
Shoppping Cart: {OrderId}
-----------------------------------------------------------"
        );

        foreach (var item in CartList)
        {
            Console.WriteLine(
                @$"
{item.Key.Name}     x{item.Value}                            {item.Key.Price * item.Value}$"
            );
        }
        Console.WriteLine(
            @$"
-----------------------------------------------------------
Total: {TotalPrice}$
            "
        );
    }

    public void AddItem(Day2Product product, int quantity)
    {
        if (CartList.ContainsKey(product))
        {
            CartList[product] += quantity;
        }
        else
        {
            CartList.Add(product, quantity);
        }
        TotalPrice += (quantity * product.Price);
        Console.WriteLine("! Products added");
    }

    public void RemoveItem(Day2Product product)
    {
        TotalPrice -= (product.Price * CartList[product]);
        CartList.Remove(product);
        Console.WriteLine("! Product Removed");
    }
}
