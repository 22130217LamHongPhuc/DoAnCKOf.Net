using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Net.Models;
using API.Net.ViewModel;
using Microsoft.CodeAnalysis;

namespace API.Net.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminProductsController : ControllerBase
    {
        private readonly DbtestContext _context;

        public AdminProductsController(DbtestContext context)
        {
            _context = context;
        }

        // GET: api/AdminProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpGet("getImg/{id}")]
        public async Task<ActionResult<IEnumerable<Subimage>>> GetImg(string id)
        {
            var subimg = await _context.Subimages
                                .Where(s => s.ProductId == id)
                                .ToListAsync();

            if (subimg == null)
            {
                return NotFound();
            }
            return Ok(subimg);
        }

        [HttpPost("getImg")]
        public async Task<ActionResult> AddImg(AddSubImageViiewModel model)
        {
          
            var subImg = new Subimage
            {
                ProductId = model.ProductId,
                Image = model.Image,
                CreateDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Subimages.Add(subImg);
            int success = await _context.SaveChangesAsync();
            if (success > 0)
            {
                return Ok();
            }
            else return NotFound();
           
        }

        [HttpPost("delImg")]
        public async Task<ActionResult> DelImg(int[] arr)
        {
            int success = 0;
            foreach (int i in arr)
            {
                var subImage = _context.Subimages.FirstOrDefault(s => s.SubImagesId == i);
                if (subImage != null)
                {
                   _context.Subimages.Remove(subImage);
                    success += await _context.SaveChangesAsync();
                }
            }
            if (success > 0)
            {
                return Ok();
            }
            else return NotFound();

        }


        // GET: api/AdminProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/AdminProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProduct(UpdateProductModel model)
        {
           
            if(model.ProductId == null)
            {
                return BadRequest();
            }
            var Product = new Product()
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Price = model.Price,
                DiscountDefault = model.DiscountDefault,
                SubcategoryId = model.SubcategoryId,
                QuanlityStock = model.QuanlityStock,
                QuanlitySell = model.QuanlitySell,
                Thumbnail = model.Thumbnail,
                Description = model.Description
            };

            _context.Products.Attach(Product);

            _context.Entry(Product).Property(p => p.Name).IsModified = true;
            _context.Entry(Product).Property(p => p.Price).IsModified = true;
            _context.Entry(Product).Property(p => p.DiscountDefault).IsModified = true;
            _context.Entry(Product).Property(p => p.SubcategoryId).IsModified = true;
            _context.Entry(Product).Property(p => p.QuanlityStock).IsModified = true;
            _context.Entry(Product).Property(p => p.QuanlitySell).IsModified = true;
            _context.Entry(Product).Property(p => p.Thumbnail).IsModified = true;
            _context.Entry(Product).Property(p => p.Description).IsModified = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(model.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/AdminProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(AddProductViewModel productModel)
        {
            if (productModel != null)
            {
                var uid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                var time = DateOnly.FromDateTime(DateTime.Now);
                using var transaction = _context.Database.BeginTransaction();
                var Product = new Product()
                {
                    ProductId = uid,
                    Name = productModel.Name,
                    Price = productModel.Price,
                    DiscountDefault = productModel.DiscountDefault,
                    Description = productModel.Description,
                    SubcategoryId = productModel.SubCategoryID,
                    QuanlityStock = productModel.QuantityStock,
                    QuanlitySell = productModel.QuantitySell,
                    Thumbnail = productModel.Thumbnail,
                    CreateAt = time

                };
                _context.Products.Add(Product);
                try
                {
                   int success = await _context.SaveChangesAsync();
                    if (success <= 0 )
                    {
                        transaction.Rollback();
                        return NotFound();
                    }    
                }
                catch (DbUpdateException)
                {
                    if (ProductExists(Product.ProductId))
                    {
                        transaction.Rollback();
                        return Conflict();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                Specification spec = new()
                {
                    Dimensions = productModel.Dimensions,
                    Original = productModel.Original,
                    ProductId = uid,
                    Material = productModel.Material,
                    SpecificationId = uid
                };

                _context.Specifications.Add(spec);
                try
                {
                    int success = await _context.SaveChangesAsync();
                    if (success <= 0)
                    {
                        transaction.Rollback();
                        return NotFound();
                    }
                }
                catch (DbUpdateException)
                {
                    if (ProductExists(Product.ProductId))
                    {
                        transaction.Rollback();
                        return Conflict();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                foreach (var img in productModel.SubImg)
                {
                    Subimage addImg = new()
                    {
                        Image = img,
                        CreateDate = time,
                        ProductId = uid
                    };

                    _context.Subimages.Add(addImg);
                    try
                    {
                        int success = await _context.SaveChangesAsync();
                        if (success <= 0)
                        {
                            transaction.Rollback();
                            return NotFound();
                        }
                    }
                    catch (DbUpdateException)
                    {
                        if (ProductExists(Product.ProductId))
                        {
                            transaction.Rollback();
                            return Conflict();
                        }
                        else
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                transaction.Commit();

                return CreatedAtAction("GetProduct", new { id = Product.ProductId }, Product);

            }
            else return BadRequest(); 
           
        }

        // DELETE: api/AdminProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
