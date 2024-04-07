using InventoryService.Domain.Models.Product;
using InventoryService.Infraestructure.Services.Cache.Contract;
using InventoryService.Infraestructure.Services.DataBase.Contract;
using Newtonsoft.Json;
using System.Diagnostics;

namespace InventoryService.Business.Services
{
    public class InventoryServiceHandler
    {
        private readonly IDataBase _dataBase;
        private readonly ICache _cache;
        public InventoryServiceHandler(
            IDataBase dataBase,
            ICache cache)
        {
            _dataBase = dataBase;
            _cache = cache;
        }

        public async Task<List<ProductModel>> GetInventory(string? by = "all", string? parameter = "")
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                List<ProductModel> inventory = new List<ProductModel>();
                by = by.ToLower();

                string valueCache = GetCacheProduct(by, parameter);

                if (!string.IsNullOrEmpty(valueCache))
                {
                    Console.WriteLine("Getting information from Cache...");
                    inventory = await GetProductsFromCache(by, valueCache, parameter);

                    return inventory;
                }

                Console.WriteLine("Getting information from DataBase...");
                inventory = await GetProductsFromDataBase(by, parameter);

                return inventory;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Error Getting data from Inventory, please review logs to more details.");
            }
            finally
            {
                stopwatch.Stop();
                // Obtener el tiempo transcurrido en formato de cadena
                string tiempoTranscurrido = stopwatch.Elapsed.ToString();
                Console.WriteLine($"Execution Elasted: [{tiempoTranscurrido}]");
            }

        }

        private string GetCacheProduct(string by, string? parameter = "")
        {

            var InCache = _cache.TryGetFromCache<string>($"{by}{parameter}", out string cachedProduct);
            if (!InCache)
                InCache = _cache.TryGetFromCache<string>($"all", out cachedProduct);

            return cachedProduct;
        }

        private async Task<List<ProductModel>> GetProductsFromDataBase(string by, string? parameter = "")
        {
            List<ProductModel> inventory = new List<ProductModel>();
            ProductModel product = null;
            switch (by)
            {
                case "id":
                    product = await _dataBase.GetProductById(int.Parse(parameter));
                    inventory.Add(product);
                    break;
                default:
                    inventory = await _dataBase.GetAllProducts();
                    break;
            }

            string dataToSave = JsonConvert.SerializeObject(inventory);
            _cache.SaveToCacheWithExpiration($"{by}{parameter}", dataToSave, TimeSpan.FromMinutes(5));

            return inventory;
        }

        private async Task<List<ProductModel>> GetProductsFromCache(string by, string cachedProduct, string? parameter = "")
        {            
            ArgumentException.ThrowIfNullOrWhiteSpace(cachedProduct);
            List<ProductModel> inventoryCache = JsonConvert.DeserializeObject<List<ProductModel>>(cachedProduct);
            List<ProductModel> inventory = new List<ProductModel>();

            switch (by)
            {
                case "id":
                    ProductModel product = inventoryCache.FirstOrDefault(p => p.Id == int.Parse(parameter));
                    inventory.Add(product);
                    break;
                default:
                    inventory = inventoryCache;
                    break;
            }

            return inventory;
        }
    }
}
