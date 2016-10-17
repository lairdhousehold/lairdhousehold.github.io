using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Bangazon.Models;

namespace Bangazon.Controllers
{
    [ProducesAttribute("application/json")]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private BangazonContext context;

        public ProductController(BangazonContext ctx)
        {
            context = ctx;
        }
        // GET api/values
        [HttpGet]
         public IActionResult Get()
        {
            IQueryable<object> products = from product in context.Product select product;

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);

        }
        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Product product = context.Product.Single(m => m.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }
                
                return Ok(product);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Models.Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Product.Add(product);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
         private bool ProductExists(int id)
        {
            return context.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}
