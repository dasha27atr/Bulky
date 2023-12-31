﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> ProductList = _unitOfWork.Product.GetAll().ToList();

            return View(ProductList);
        }

        public IActionResult Upsert(int? id)
        {
            // ViewBag in Action
            //ViewBag.CategoryList = CategoryList;

            // ViewData in Action
            //ViewData["CategoryList"] = CategoryList;

            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                // create
                return View(productViewModel);
            }
            else
            {
                // update
                productViewModel.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(productViewModel);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string productPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\product");

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        // delete the old image
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
                    file.CopyTo(fileStream);
                     
                    productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productViewModel.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productViewModel.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productViewModel.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product has been created successfully";

                return RedirectToAction("Index");
            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productViewModel);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = _unitOfWork.Product.Get(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product has been deleted successfully";

            return RedirectToAction("Index");
        }
    }
}