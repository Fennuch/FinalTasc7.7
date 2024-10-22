using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net;

namespace FinalTasc7._7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserInterface.UserRegistration();
            UserInterface.ShowProducts();
            UserInterface.ProductOrder();

        }
    }


    abstract class Delivery
    {
        public string Address;
    }


    class HomeDelivery : Delivery
    {
        /* ... */
    }


    class PickPointDelivery : Delivery
    {
        /* ... */
    }


    class ShopDelivery : Delivery
    {
        /* ... */
    }

    abstract class Product
    {
        public string Name { get; set; }
        public int Price { get; set; }

        public int Id { get; set; }

    }

    class Food : Product
    {
        public string Compound { get; set; }
    }

    class Technic : Product
    {
        public string Manufacturer { get; set; }
    }

    class Book : Product
    {
       public string Publisher { get; set; }
    }

    class Storage
    {
        public List<Product> Products { get; set; }

        public Storage()
        {
            Products = new List<Product>() 
            { 
                new Food(){ Name = "Сухпаек Азиатский", Compound = "Рис и мясо", Id =0, Price = 50 },
                new Food(){ Name = "Сухпаек Кавказкий", Compound = "Рис и мясо", Id = 1, Price = 70},
                new Food(){ Name = "Сухпаек Вьетнамский ", Compound = "Рис и мясо", Id = 2, Price = 90 },
                new Technic(){ Name = "Телевизор", Manufacturer = "LG", Id = 3, Price = 5000 },
                new Technic(){ Name = "Телефон", Manufacturer  = "Ксиоми", Id = 4, Price = 9000 },
                new Technic(){ Name = "Посудомойка", Manufacturer  = "Эпл", Id = 5, Price = 7000 },
                new Book(){ Name = "Фэнтези",  Publisher = "Эксмо", Id = 6, Price = 500 },
                new Book(){ Name = "Классика", Publisher = "РИПОЛ", Id = 7 , Price = 700 },
                new Book(){ Name = "Хоррор", Publisher = "Росмэн", Id = 8, Price = 990 },
            };
        }
    }

    enum CustomerType
    {
        person,
        company
    }

    class Customer
    {
        public CustomerType customerType;
        public string Name { get; set; }
        public long Telephone { get; set; }

    }

    class Order<TDelivery,TProduct> where TDelivery : Delivery where TProduct : Product
    {
        public TDelivery Delivery;
        public TProduct Product;
        public int countProduct;
        public string Description;
        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }

        public Order(TDelivery delivery, TProduct product, int countProduct, string description)
        {
            this.Delivery = delivery ;
            this.Product = product;
            this.countProduct = countProduct;
            this.Description = description;
        }


        public void ShowOrder()
        {
            Console.WriteLine("Заказ оформлен:");
            Console.WriteLine($"Название товара: {Product.Name}");
            Console.WriteLine($"Кол-во товара: {countProduct}");
            Console.WriteLine($"Адрес: {Delivery.Address}");
            Console.WriteLine($"Описание: {Description}");
        }
    }

    static class UserInterface
    {
        public static Storage storage = new Storage();
        public static Customer customer = new Customer();
        
       public static void UserRegistration()
       {
            
            
            Console.Write($"Введите имя: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите телефон: ");
            long telephone = long.Parse(Console.ReadLine());
            Console.WriteLine("1)Физ.лицо или 2)Юр.лицо? ");
            int customerType = int.Parse(Console.ReadLine());
            customer = new Customer { Name = name, Telephone = telephone, customerType = customerType == 1 ? CustomerType.person : CustomerType.company };
        }
        public static void ShowProducts()
        {
            foreach(var item in storage.Products)
            {
                Console.Write($"Id продукта: {item.Id}  ");
                Console.Write($"Название продукта: {item.Name}          ");
                Console.Write($"Цена продукта: {item.Price}              ");                
                if (item is Food)
                {
                    Food food = (Food)item;
                    Console.WriteLine($"Состав еды: {food.Compound}");
                }
                if (item is Technic)
                {
                    Technic technic = (Technic)item;
                    Console.WriteLine($"Производство техники: {technic.Manufacturer}") ;
                }
                if (item is Book)
                {
                    Book book = (Book)item;
                    Console.WriteLine($"Издатель книг: {book.Publisher}") ;
                }
            }
        }

        public static void ProductOrder()
        {
            while(true)
            {
                Console.WriteLine("\nВыбериете продукт по Id: ");
                int selection = int.Parse(Console.ReadLine());

                if (storage.Products[selection] == null)
                {
                    continue;
                }

                Product  product = storage.Products[selection];

                Console.WriteLine("Введите кол-во продукта: ");
                int countProduct = int.Parse(Console.ReadLine());

                string address;
                int choice;
                if (customer.customerType == CustomerType.company)
                {
                    Console.WriteLine("Введите адрес магазина:  ");
                    choice = 3;
                    address = Console.ReadLine();
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("куда хотите отправить?");
                        Console.WriteLine("1) В пункт выдачи или \n2) Доставка в дом?");
                        choice = int.Parse(Console.ReadLine());
                        if (choice == 1)
                        {
                            Console.WriteLine("Напишите адресс пункт выдачи:");
                            address = Console.ReadLine();
                            break;
                        }
                        else if (choice == 2)
                        {
                            Console.WriteLine("Напишите адресс дома:");
                            address = Console.ReadLine();
                            break;
                        }
                        else { continue; }
                    }
                }

                string typeProducr;
                if (storage.Products[selection] is Food) 
                {
                    CreateOrder(choice, (Food)product, address, countProduct, "Описание");                    
                } 
                else if(storage.Products[selection] is Book) 
                {
                    CreateOrder(choice, (Book)product, address, countProduct, "Описание");
                } 
                else if (storage.Products[selection] is Technic) 
                {
                    CreateOrder(choice, (Technic)product, address, countProduct, "Описание");
                }

            }
            
        }

        static void CreateOrder<T>(int type, T product, string address, int countProduct, string description) where T : Product
        {

            switch (type) { 
                case 1:
                    PickPointDelivery pickPointDelivery = new PickPointDelivery() {Address= address };
                    SelectDelivery<PickPointDelivery, T>(pickPointDelivery, product, countProduct, description);
                    break;
                case 2:
                    HomeDelivery homeDelivery = new HomeDelivery() { Address = address };
                    SelectDelivery<HomeDelivery, T>(homeDelivery, product, countProduct, description);
                    break;
                case 3:
                    ShopDelivery shopDelivery = new ShopDelivery() { Address = address };
                    SelectDelivery<ShopDelivery, T>(shopDelivery, product, countProduct, description);
                    break;
            }
            
        }

        static void SelectDelivery<T1, T2>(T1 type, T2 product, int countProduct, string description)  where T1 : Delivery where T2: Product
        {
            Order<T1, T2> order = new Order<T1, T2>( type, product, countProduct, description);
            order.ShowOrder();

        }
    }

}