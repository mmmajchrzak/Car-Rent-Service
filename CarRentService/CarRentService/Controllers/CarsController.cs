using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRentService.Data;
using CarRentService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentService.Controllers
{
    
    public class CarsController : Controller
    {

        CarModelContext db;


        private readonly UserManager<IdentityUser> _userManager;



        public CarsController(CarModelContext db, UserManager<IdentityUser> userManager)
        {

            this.db = db;
            _userManager = userManager;

        }


        
        public async Task<IActionResult> Index(string searchString)
        {

            TempData["userId"] = _userManager.GetUserName(HttpContext.User);






            var cars = from m in db.Car
                       select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (User.IsInRole("admins"))
                {
                    cars = cars.Where(c => c.Brand.StartsWith(searchString));
                }
                else
                {
                    cars = cars.Where(c => c.Brand.StartsWith(searchString) && c.Reservation == null);
                }
                return View(await cars.ToListAsync());
            }
            else
            {
                if (User.IsInRole("admins"))
                {
                    return View(db.Car.OrderBy(c => c.Brand));
                }
                else
                {
                    return View(db.Car.OrderBy(c => c.Brand).Where(c => c.Reservation == null));
                }
            }

        }

        [Authorize]
        public async Task<IActionResult> Reserv(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await db.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }



        [Authorize]
        [HttpPost]


        public ActionResult Reserv(int id, [Bind("Id,Brand,Model,YearOfProduction,Power,Price,Reservation")] CarModel car)
        {

            bool IsReservationExist = db.Car.Any
                    (x => x.Reservation == car.Reservation && car.Reservation != null);
            if (IsReservationExist == true)
            {
                ModelState.AddModelError("Reservation", "Dokonałeś już maksymalnej liczby rezerwacji");
                return View(car);
            }
            else
            {
                try
                {

                    db.Car.Attach(car);
                    db.Entry(car).Property(x => x.Reservation).IsModified = true;
                    db.SaveChanges();


                }

                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                var cars = from m in db.Car
                           select m;

                if (!User.IsInRole("admins"))

                {
                    TempData["Res"] = "Zarezerwowano samochód: " + car.Brand;
                }

                return RedirectToAction(nameof(Index));
            }



        }




        [Authorize(Roles = "admins")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CarModel car)
        {
            if (ModelState.IsValid)
            {
                db.Car.Add(car);
                await db.SaveChangesAsync();
                TempData["Info"] = "Dodano samochód: " + car.Brand;
                return RedirectToAction(nameof(Create));
            }
            return View(car);

        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await db.Car.SingleOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "admins")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await db.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Brand,Model,YearOfProduction,Power,Price,Reservation")] CarModel car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(car);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        [Authorize(Roles = "admins")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = await db.Car.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "admins")]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await db.Car.FindAsync(id);
            db.Car.Remove(car);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }


}

