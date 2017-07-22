using SportsStore.Domain.Abstract;
using System.Web.Mvc;
using System.Linq;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(p => p.Category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(p => p.Category == category).Count()
                }
            };

            return View(model);

            //return View(repository.Products
            //    .OrderBy(p => p.ProductID)
            //    .Skip((page - 1) * PageSize)
            //    .Take(PageSize)
            //    );
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            // TODO: Check if here should be added check for product.ImageData != null
            if(product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}