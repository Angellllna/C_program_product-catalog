using System;
using System.Collections.Generic;

namespace NewConsoleAppp
{
    class Program
    {
        class Product
        {
            private string name;

            public string Name
            {
                get { return name; }
            }

            public int caloricContent;
            public int count;
            public int price;

            public Product(string Name, int CaloricContent, int Count, int Price)
            {
                name = Name;
                caloricContent = CaloricContent;
                count = Count;
                price = Price;
            }

            public static void PrintHeader(int cellSize = 15)
            {
                (int Left, int Top) = Console.GetCursorPosition();

                Console.Write("Назва: ");
                Console.SetCursorPosition(Left + cellSize, Top);
                Console.Write("Кiлькiсть: ");
                Console.SetCursorPosition(Left + 2 * cellSize, Top);
                Console.Write("Калорiйнiсть: ");
                Console.SetCursorPosition(Left + 3 * cellSize, Top);
                Console.WriteLine("Цiна: ");
            }

            public void Print(int cellSize = 15)
            {
                (int Left, int Top) = Console.GetCursorPosition();

                Console.Write(name);
                Console.SetCursorPosition(Left + cellSize, Top);
                Console.Write(count);
                Console.SetCursorPosition(Left + 2 * cellSize, Top);
                Console.Write(caloricContent);
                Console.SetCursorPosition(Left + 3 * cellSize, Top);
                Console.WriteLine(price);
            }
            ~Product()
            {
                Console.WriteLine("об'єкт знищено (Product)");
            }
        }

        static bool cmpByFactor(in Product lhs, in Product rhs)
        {
            return (Convert.ToDouble(lhs.caloricContent) / Convert.ToDouble(lhs.price))
                 > (Convert.ToDouble(rhs.caloricContent) / Convert.ToDouble(rhs.price));
        }
        class Catalog
        {
            private List<Product> products = new List<Product>();
            private bool sorted = false;
            public List<Product> Products
            {
                get { return products; }
            }
            public void Add(Product product)
            {
                foreach (Product p in products)
                {
                    if (p.Name == product.Name &&
                        p.caloricContent == product.caloricContent &&
                        p.price == product.price)
                    {
                        p.count += product.count;
                        return;
                    }
                }

                sorted = false;
                products.Add(product);
            }

            public Catalog GetMenu(int CaloricContent, int Price)
            {
                if (!sorted)
                {
                    sorted = true;

                    for (int i = 0; i < products.Count; ++i)
                    {
                        int q = i;
                        for (int j = i + 1; j < products.Count; ++j)
                            if (cmpByFactor(products[j], products[q]))
                                q = j;
                        Product h = products[i];
                        products[i] = products[q];
                        products[q] = h;
                    }
                }

                int sumPrice = 0;
                Catalog menu = new Catalog();

                for (int i = 0; i < products.Count; ++i)
                {
                    if (CaloricContent <= 0)
                        break;
                    if (products[i].count * products[i].caloricContent <= CaloricContent)
                    {
                        sumPrice += products[i].price * products[i].count;
                        menu.Add(products[i]);

                        CaloricContent -= products[i].count * products[i].caloricContent;
                    }
                    else
                    {
                        int cnt = (CaloricContent + products[i].caloricContent - 1) / products[i].caloricContent;
                        Product newProduct = new Product(products[i].Name, products[i].caloricContent, cnt, products[i].price);

                        sumPrice += products[i].price * cnt;
                        menu.Add(newProduct);

                        CaloricContent = 0;
                    }
                }

                if (sumPrice <= Price && CaloricContent <= 0)
                    return menu;

                return null;
            }
            public void Remove(Catalog menu)
            {
                //To Do
            }

            public void Print(in int cellSize = 15)
            {
                Console.WriteLine("Продукти в списку: ");
                Product.PrintHeader();
                for (int i = 0; i < products.Count; ++i)
                    products[i].Print();
            }
            ~Catalog()
            {
                Console.WriteLine("об'єкт знищено (Catalog)");
            }
        }
        static void Main(string[] args)
        {
            Catalog myCatalog = new Catalog();
            Product Apple = new Product("Apple", 520, 1, 30);
            Product Banana = new Product("Banana", 890, 1, 45);

            myCatalog.Add(Apple);
            myCatalog.Add(Apple);
            myCatalog.Add(Banana);

            Catalog menu = myCatalog.GetMenu(1000, 300);

            if (menu != null)
            {
                Console.WriteLine(menu.Products.Count);

                menu.Print();
            }

            Console.ReadKey();
        }
    }
}
