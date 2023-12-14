using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var productToUpdate = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (productToUpdate != null)
            {
                productToUpdate.Title = product.Title;
                productToUpdate.Description = product.Description;
                productToUpdate.ISBN = product.ISBN;
                productToUpdate.Author = product.Author;
                productToUpdate.ListPrice = product.ListPrice;
                productToUpdate.Price = product.Price;
                productToUpdate.Price50 = product.Price50;
                productToUpdate.Price100 = product.Price100;
                productToUpdate.CategoryId = product.CategoryId;

                if (productToUpdate.ImageUrl != null)
                {
                    productToUpdate.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
