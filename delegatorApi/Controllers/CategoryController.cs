using delegatorApi.Library.DataAccess;
using delegatorApi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace delegatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryData _categoryData;

        public CategoryController(CategoryData categoryData)
        {
            _categoryData = categoryData;
        }

        public List<Category> GetAll() => _categoryData.GetAll();

        [Route("ByTitle")]
        public Category GetByTitle(string title) => _categoryData.GetByTitle(title);
    }
}
