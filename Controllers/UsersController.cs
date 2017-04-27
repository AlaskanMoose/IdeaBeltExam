using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BeltExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace BeltExam.Controllers
{
    public class UsersController : Controller{
        private BeltExamContext _context;
        public UsersController(BeltExamContext context){
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index(){
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User model){
            if(ModelState.IsValid){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                model.Password = Hasher.HashPassword(model, model.Password);
                _context.Add(model);
                _context.SaveChanges();
                User CurrentUser = _context.Users.SingleOrDefault(user => user.Email == model.Email);
                HttpContext.Session.SetInt32("CurrUserId", CurrentUser.UserId);
                return RedirectToAction("Dash", "Posts");
            }
            ViewBag.Count = 2;
            ViewBag.Errors = ModelState.Values;
            return View("Index", model);
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password){
            if(Email != null && Password != null){
                if(_context.Users.Any(user => user.Email == Email)){
                    var FoundUser = _context.Users.SingleOrDefault(user => user.Email == Email);
                    var Hasher = new PasswordHasher<User>();
                    
                    if(0 != Hasher.VerifyHashedPassword(FoundUser, FoundUser.Password, Password)){
                        HttpContext.Session.SetInt32("CurrUserId", FoundUser.UserId);
                        return RedirectToAction("Dash", "Posts");
                    }
                }
                ViewBag.Count = 1;
                ViewBag.Error = "Either User or Password is Incorrect";
            }else{
                ViewBag.Count = 1;
                ViewBag.Error = "You cannot leave any fields blank!";
            }
            return View("Index");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }    
    }
}