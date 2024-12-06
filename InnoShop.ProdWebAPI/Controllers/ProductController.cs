using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using InnoShop.DTO.Models;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProductController(IServiceManager service) : Controller
    {
        readonly IServiceManager _service = service;
        [HttpPost]
        public IActionResult Index(ProductFilterDTO dto,int page = 1)
        {
            try
            {
                if (dto.Search == null)
                    dto.Search = "";
                if (dto.MinPrice == null)
                    dto.MinPrice = 0;
                if (dto.MaxPrice == null)
                    dto.MaxPrice = 0;
                var products = _service.ProductService.GetPage(30, page,dto);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult ForUser(ProductFilterDTO dto)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            try
            {
                if (dto.Search == null)
                    dto.Search = "";
                if (dto.MinPrice == null)
                    dto.MinPrice = 0;
                if (dto.MaxPrice == null)
                    dto.MaxPrice = 0;
                var products = _service.UserService.GetUser(user.Id).ProductsUser.Where(p=>(dto.CategoryId != 0 ? dto.CategoryId == p.ProdTypeId : true) && p.Cost >= dto.MinPrice && p.Cost <= dto.MaxPrice&& (p.Description.ToUpper().Contains(dto.Search.ToUpper()) || p.Name.ToUpper().Contains(dto.Search.ToUpper()) || p.ProdAttribs.Any(t => t.Name.ToUpper().Contains(dto.Search.ToUpper())))).OrderByDescending(p => p.CreationDate).ToList();
                //var products = _service.ProductService.GetProductsByCondition(p => p.UserId == user.Id&& (dto.CategoryId != 0 ? dto.CategoryId == p.ProdTypeId : true) && p.Cost >= dto.MinPrice && p.Cost <= dto.MaxPrice).OrderByDescending(p=>p.CreationDate).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult BoughtForUser(ProductFilterDTO dto)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            try
            {
                var products = _service.UserService.GetUser(user.Id).ProductsBuyer.Where(p => (dto.CategoryId != 0 ? dto.CategoryId == p.ProdTypeId : true) && p.Cost >= dto.MinPrice && p.Cost <= dto.MaxPrice).OrderByDescending(p => p.CreationDate).ToList();
                //var products = _service.ProductService.GetProductsByCondition(p => p.UserId == user.Id&& (dto.CategoryId != 0 ? dto.CategoryId == p.ProdTypeId : true) && p.Cost >= dto.MinPrice && p.Cost <= dto.MaxPrice).OrderByDescending(p=>p.CreationDate).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult PendingForUser(ProductFilterDTO dto)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            try
            {
                var products = _service.UserService.GetUser(user.Id).ProductsUser.Where(p => (dto.CategoryId != 0 ? dto.CategoryId == p.ProdTypeId : true) && p.Cost >= dto.MinPrice && p.Cost <= dto.MaxPrice&&!p.Sold&&p.BuyerId!=null).OrderByDescending(p => p.CreationDate).ToList();
                //var products = _service.ProductService.GetProductsByCondition(p => p.UserId == user.Id&& (dto.CategoryId != 0 ? dto.CategoryId == p.ProdTypeId : true) && p.Cost >= dto.MinPrice && p.Cost <= dto.MaxPrice).OrderByDescending(p=>p.CreationDate).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult Buy([FromBody] int id)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            if (user == null)
                return Unauthorized();
            var product = _service.ProductService.GetProduct(id);
            try
            {
                if(product.UserId!=user.Id&&product.BuyerId==null)
                {
                    product.BuyerId = user.Id;
                    _service.ProductService.Edit(product);
                    return Ok(product);
                }
                return BadRequest(product);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult ConfirmBought([FromBody]int id)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            if (user == null)
                return Unauthorized();
            var product = _service.ProductService.GetProduct(id);
            try
            {
                if (product.UserId == user.Id && product.BuyerId != null)
                {
                    product.Sold = true;
                    _service.ProductService.Edit(product);
                    return Ok(product);
                }
                return BadRequest(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult RejectBought([FromBody] int id)
        {
            Console.WriteLine(id);
            var user = _service.UserService.GetUserFromIdentity(User);
            if (user == null)
                return Unauthorized();
            var product = _service.ProductService.GetProduct(id);
            try
            {
                if (product.UserId == user.Id && product.BuyerId != null)
                {
                    product.BuyerId = null;
                    product.Buyer = null;
                    _service.ProductService.Edit(product);
                    return Ok(product);
                }
                return BadRequest(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductEditData data = new()
            {
                Categories = _service.ProdTypeService.GetAllProdTypes().Select(p => new SelectListItem(p.Name, p.Id.ToString())),
                Users = _service.UserService.GetAllUsers().Select(p => new SelectListItem(p.UserName, p.Id.ToString()))
            };
            return Ok(data);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(ProductEditDTO productForCreate)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            try
            {
                if (productForCreate.UserId == user.Id)
                {
                    var product = new Product()
                    {
                        Id = productForCreate.Id,
                        Name = productForCreate.Name,
                        Description = productForCreate.Description,
                        Cost = productForCreate.Cost,
                        ProdTypeId = productForCreate.ProdTypeId,
                        CreationDate = productForCreate.CreationDate,
                        Public = productForCreate.Public,
                        UserId = productForCreate.UserId,
                        Sold = productForCreate.Sold,
                        BuyerId = productForCreate.BuyerId
                    };
                    var tags = JsonConvert.DeserializeObject<List<ProdAttrib>>(productForCreate.ProdAttribs);
                    foreach (var tag in tags)
                    {
                        product.ProdAttribs.Add(tag);
                    }
                    _service.ProductService.Add(product);
                    return Ok(product);
                }
                else
                    return BadRequest();
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Product product = _service.ProductService.GetProduct(id);
            return Ok(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProductEditData data = new()
            {
                Product = _service.ProductService.GetProduct(id),
                Categories = new SelectList(_service.ProdTypeService.GetAllProdTypes(),"Id","Name"),
                Users = new SelectList(_service.UserService.GetAllUsers(),"Id","UserName")
            };
            return Ok(data);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(ProductEditDTO productForEdit)
        {
            
            var user = _service.UserService.GetUserFromIdentity(User);
            var role = _service.UserService.GetRole(User);
            if (user.Id == productForEdit.UserId || role == "Admin")
            {
            var product = _service.ProductService.GetProduct(productForEdit.Id);

            product.Name = productForEdit.Name;
            product.Description = productForEdit.Description;
            product.Cost = productForEdit.Cost;
            product.ProdTypeId = productForEdit.ProdTypeId;
            product.CreationDate = productForEdit.CreationDate;
            product.Public = productForEdit.Public;
            product.UserId = productForEdit.UserId;
            product.Sold = productForEdit.Sold;
            product.BuyerId = productForEdit.BuyerId;

            var existingTags = product.ProdAttribs.ToList();
            var tags = JsonConvert.DeserializeObject<List<ProdAttrib>>(productForEdit.ProdAttribs);
            var tagsToAdd = tags.Where(nt => !existingTags.Any(et => et.Name == nt.Name)).ToList();

            // Теги, которые нужно удалить
            var tagsToRemove = existingTags.Where(et => !tags.Any(nt => nt.Name == et.Name)).ToList();
            foreach (var tagToRemove in tagsToRemove)
            {
                product.ProdAttribs.Remove(tagToRemove);
            }

            // Добавляем новые теги
            foreach (var tagToAdd in tagsToAdd)
            {
                product.ProdAttribs.Add(tagToAdd);
            }
            _service.ProductService.Edit(product);
                return Ok(product);
            }
            else
                return BadRequest();
            //try
            //{
            //}
            //catch (Exception ex) { return BadRequest(ex.Message); }

        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _service.UserService.GetUserFromIdentity(User);
            var role = _service.UserService.GetRole(User);
            var product = _service.ProductService.GetProduct(id);
            try
            {
                if (user.Id == product.UserId || role == "Admin")
                {
                    _service.ProductService.Remove(product);
                    return NoContent();
                }
                else
                    return BadRequest();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
