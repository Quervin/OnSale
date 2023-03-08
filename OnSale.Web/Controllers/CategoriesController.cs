using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public CategoriesController(DataContext context, IBlobHelper blobHelper, IConverterHelper converterHelper, IImageHelper imageHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "categories");
                }

                try
                {
                    Category category = _converterHelper.ToCategory(model, imageId, path, true);
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel model = _converterHelper.ToCategoryViewModel(category);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                var path = model.ImageUrl;

                if (model.ImageFile != null)
                {
                    //imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "categories");
                }

                try
                {
                    Category category = _converterHelper.ToCategory(model, imageId, path, false);
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                var pathImage = category.ImageUrl;

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                _imageHelper.DeleteImage(pathImage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
