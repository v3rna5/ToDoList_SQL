using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {

       [HttpGet("/categories")]
       public ActionResult Index()
       {
           List<Category> allCategories = Category.GetAll();
           return View(allCategories);
       }

       [HttpGet("/categories/new")]
       public ActionResult CreateForm()
       {
           return View();
       }

       [HttpPost("/categories/new")]
       public ActionResult Create()
       {
           Category newCategory = new Category(Request.Form["category-name"]);
           newCategory.Save();
           List<Category> allCategories = Category.GetAll();
           return View("Index", allCategories);
       }

       [HttpGet("/categories/{id}")]
       public ActionResult Details(int id)
       {
           Dictionary<string, object> model = new Dictionary<string, object>();
           Category selectedCategory = Category.Find(id);
           List<Item> categoryItems = Item.GetAll();
           model.Add("category", selectedCategory);
           model.Add("items", categoryItems);
           return View(model);
       }

       [HttpPost("/items")]
       public ActionResult CreateItem()
       {
         Dictionary<string, object> model = new Dictionary<string, object>();
         Category foundCategory = Category.Find(Int32.Parse(Request.Form["category-id"]));
         Item newItem = new Item(Request.Form["item-description"]);
         newItem.SetDate(Request.Form["item-due"]);
         newItem.Save();
         foundCategory.AddItem(newItem);
         List<Item> categoryItems = foundCategory.GetItems();
         model.Add("items", categoryItems);
         model.Add("category", foundCategory);
         return View("Details", model);
       }
    }
}
