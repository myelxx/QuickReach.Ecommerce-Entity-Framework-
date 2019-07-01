using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.API.ViewModel;
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
        private readonly ECommerceDbContext context;
        public CategoriesController(ICategoryRepository repository, IProductRepository productRepo, ECommerceDbContext context)
        {
            this.repository = repository;
            this.productRepo = productRepo;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var categories = repository.Retrieve(search, skip, count);
            return Ok(categories);
        }

        //GET Category
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = this.repository.Retrieve(id);
            return Ok(category);
        }

        //CREATE Category
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

        //Retrieve -> GetProductsByCategory
        [HttpGet("{id}/products")]
        public IActionResult GetProductsByCategory(int id)
        {
            var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            var connection = new SqlConnection(connectionString);
            var query = @"SELECT p.ID,
                               pc.ProductID, 
                               pc.CategoryID,
                               p.Name, 
                               p.Description,
                               p.Price,
                               p.ImgUrl
                        FROM Product p INNER JOIN ProductCategory pc ON p.ID = pc.ProductID
                        Where pc.CategoryID = @categoryId";

            var categories = connection.Query<SearchItemViewModel>(query, new { categoryId = id })
                                       .ToList();

            return Ok(categories);

            //var parameter = new SqlParameter("@categoryid", id);
            //var result = this.context.Query<SearchItemViewModel>()
            //                         .FromSql(@"SELECT pc.CategoryID, 
            //                                           pc.ProductID, 
            //                                           p.Name, 
            //                                           p.Description, 
            //                                           p.Price, 
            //                                           p.ImgURL 
            //                                    FROM Product p
            //                                    INNER JOIN ProductCategory pc ON p.ID = pc.ProductID 
            //                                    WHERE pc.CategoryID = @categoryid", parameter)
            //                         .AsNoTracking()
            //                         .ToList();
            //return Ok(result);
        }

        //CREATE Product Category
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

        //Remove Product Category
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

        //Add Categoroy Roll up 
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
