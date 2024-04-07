using InventoryService.Domain.Models.Product;
using InventoryService.Infraestructure.Services.DataBase.Contract;
using Newtonsoft.Json;

namespace InventoryService.Infraestructure.Services.DataBase.Implementation
{
    public class FileDataBase : IDataBase
    {
        private List<ProductModel> products;
        private readonly string _localFile;
        public FileDataBase() 
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            _localFile = Path.Combine(directory, "DBMockProducts.json");
            // Cargar datos desde el archivo JSON al inicializar la instancia
            LoadDataFromFile();
        }

        // Método para cargar los datos desde el archivo JSON
        private async Task LoadDataFromFile()
        {
            try
            {
                string json = File.ReadAllText(_localFile);
                products = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar los datos desde el archivo JSON: {ex.Message}");
                products = new List<ProductModel>(); // Inicializar una lista vacía si hay un error
            }
        }

        // Método para guardar los cambios en el archivo JSON
        public async Task SaveProduct()
        {
            try
            {
                string json = JsonConvert.SerializeObject(products, Formatting.Indented);
                File.WriteAllText(_localFile, json);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar los datos en el archivo JSON: {ex.Message}");
            }
        }

        // Método para obtener todos los productos
        public async Task<List<ProductModel>> GetAllProducts()
        {
            Thread.Sleep(10000);
            return products;
        }

        // Método para agregar un nuevo producto
        public async Task AddProduct(ProductModel product)
        {
            product.Id = GenerateNextId(); // Asignar un ID único al nuevo producto
            product.CreationTime = DateTime.Now; // Establecer la fecha de creación
            products.Add(product); // Agregar el producto a la lista
            await SaveProduct(); // Guardar los cambios en el archivo JSON
        }

        // Método para generar un nuevo ID para el producto
        private int GenerateNextId()
        {
            return products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            Thread.Sleep(10000);
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}
