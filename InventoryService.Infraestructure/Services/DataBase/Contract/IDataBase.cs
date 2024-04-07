using InventoryService.Domain.Models.Product;

namespace InventoryService.Infraestructure.Services.DataBase.Contract
{
    public interface IDataBase
    {
        public Task SaveProduct();
        public Task<List<ProductModel>> GetAllProducts();
        public Task<ProductModel> GetProductById(int id);
        public Task AddProduct(ProductModel product);
    }
}
