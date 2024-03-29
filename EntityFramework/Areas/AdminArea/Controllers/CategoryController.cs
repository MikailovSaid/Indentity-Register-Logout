﻿using EntityFramework.Data;
using EntityFramework.Exceptions;
using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.AsNoTracking().Where(m => !m.IsDeleted).ToListAsync();
            return View(categories);
        }

        //For Detail
        public async Task<IActionResult> Detail(int id)
        {
            var category = _context.Categories.FirstOrDefault(m => m.Id == id);
            return Json(new 
            {
                categoryName = category.Name,
                action = "Detail",
                Id =id
            });
        }
        



        //For Edit
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _context.Categories.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id != category.Id) return BadRequest();


            try
            {
                Category dbCategory = await _context.Categories.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();

                if (dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim()) return RedirectToAction(nameof(Index));

                bool isExist = _context.Categories.Where(m => !m.IsDeleted).Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());

                if (isExist)
                {
                    ModelState.AddModelError("Name", "Bu Category Artiq Movcuddur");
                    return View();
                }

                //dbCatgegory == category but use without AsNoTracking, it is good for one changes but more than one it is recommended use AsNoTracking
                //dbCategory.Name = category.Name;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            
        }




        //For Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.Include(m=>m.Products).Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();

            if (category == null) return NotFound();

            //_context.Categories.Remove(category);

            category.IsDeleted = true;

            foreach (var product in category.Products)
            {
                product.IsDeleted = true;
            }

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }





        //For Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExist = _context.Categories.Where(m=> !m.IsDeleted).Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Category Artiq Movcuddur");
                return View();
            }

           await _context.Categories.AddAsync(category);
           await  _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
