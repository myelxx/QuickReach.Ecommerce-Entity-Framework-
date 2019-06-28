using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;
using QuickReach.ECommerce.Infra.Data.Repository;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repository;
        private readonly IProductRepository productRepo;
        public CategoriesController(ICategoryRepository repository, IProductRepository productRepo)
        {
            this.repository = repository;
            this.productRepo = productRepo;
        }

        [HttpGet]
        public IActionResult Get(string search="", int skip = 0, int count = 10)
        {
            var categories = repository.Retrieve(search, skip, count);
            return Ok(categories);
        }

        //GET api/values/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = this.repository.Retrieve(id);
            return Ok(category);
        }

        //CREATE category
        [HttpPost]
        public IActionResult Post([FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newCategory);

            return CreatedAtAction(nameof(this.Get), new { id = newCategory.ID }, newCategory);
        }

        [HttpPut("{id}/products")]
        public IActionResult AddCategoryProduct(int id, [FromBody] ProductCategory entity)
        {
            var category = repository.Retrieve(id);
            var product = productRepo.Retrieve(entity.ProductID);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }    

            if (category == null)
            {
                return NotFound();
            }

            if (product == null)
            {
                return NotFound();
            }

            category.AddProduct(entity);

            repository.Update(id, category);
            return Ok(category);

        }

        //Remove product category
        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteCategoryProduct(int id, int productId)
        {
            var category = repository.Retrieve(id);
            var product = productRepo.Retrieve(productId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            if (category == null)
            {
                return NotFound();
            }
            if (product == null)
            {
                return NotFound();
            }

            category.RemoveProduct(productId);
            repository.Update(id, category);
            return Ok();
        }

        //Add categoroy roll up 
        [HttpPut("{id}/sub")]
        public IActionResult AddSubCategories(int id, [FromBody] Category child)
        {
            var category = this.repository.Retrieve(id);

            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            if (category == null)
            {
                return NotFound();
            }

            if (category == null)
            {
                return NotFound();
            }

            category.AddChild(child.ID);
            repository.Update(id, category);

            return Ok(category);
        }

        //remove categoroy roll up 
        [HttpPut("{parentId}/sub/{childId}")]
        public IActionResult DeleteSubCategories(int parentId, int childId)
        {
            var category = this.repository.Retrieve(parentId);

            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            if (category == null)
            {
                return NotFound();
            }

            if (category == null)
            {
                return NotFound();
            }

            category.RemoveChildCategory(childId);
            repository.Update(parentId, category);

            return Ok(category);
        }

        //PUT api'/values/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newCategory);

            return Ok(repository);
        }

        //Delete
        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            this.repository.Delete(id);
            return Ok();
        }
    }
}
