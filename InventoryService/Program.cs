using InventoryService.Business.Services;
using InventoryService.Infraestructure.Services.Cache.Contract;
using InventoryService.Infraestructure.Services.Cache.Implementation;
using InventoryService.Infraestructure.Services.DataBase.Contract;
using InventoryService.Infraestructure.Services.DataBase.Implementation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryService
{
    internal class Program
    {
        private static IDataBase _dataBase;
        private static ICache _cache;
        private static InventoryServiceHandler _inventoryService;
        static void Main(string[] args)
        {
            _dataBase = new FileDataBase();
            _cache = new MemoryCacheManager();
            _inventoryService = new InventoryServiceHandler(_dataBase, _cache);

            Console.WriteLine("Welcome, This is the Inventory service!");
            ProgramCicle();
            Console.WriteLine("Thank's for using this service. Bye");
        }

        static void ProgramCicle()
        {
            int optionSelected = ReadOptions();

            if (optionSelected == 3) return;
            Console.WriteLine($"Your selected option is [{optionSelected}]");
            string parameter = string.Empty;
            string by = optionSelected == 1 ? "all" : "id";

            if (optionSelected == 2)
            {
                Console.WriteLine($"Please enter id to search the database:");
                parameter = Console.ReadLine();
            }

            var products = _inventoryService.GetInventory(by, parameter);
            var productsJson = JsonConvert.SerializeObject(products);
            var jsonObject = JObject.Parse(productsJson);

            // Convertir el objeto JObject a una cadena de JSON indentada
            string indentedJson = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine($"Products: {indentedJson}");

            GetQuestionAbboutContinue();

            var optionToContinue = Console.ReadLine();

            if (optionToContinue == "1")
                ProgramCicle();
        }

        static void GetOptions()
        {
            Console.WriteLine($"1. Get All Product List.");
            Console.WriteLine($"2. Get Product List By Id.");
            Console.WriteLine($"3. Exit.");
        }

        static void GetQuestionAbboutContinue()
        {
            Console.WriteLine($"Do you want to continue?");
            Console.WriteLine($"1. Yes");
            Console.WriteLine($"2. No");
        }

        static int ReadOptions()
        {
            int option = 0;
            Console.WriteLine("Select one option from list:");
            GetOptions();
            string optionSelected = Console.ReadLine();

            if (string.IsNullOrEmpty(optionSelected))
            {
                Console.WriteLine("Please enter a number from list.");
                option = ReadOptions();
            }

            option = int.Parse(optionSelected);

            if (option <= 0 || option > 4)
            {
                Console.WriteLine("Please enter a number from list.");
                option = ReadOptions();
            }

            return option;
        }
    }
}
