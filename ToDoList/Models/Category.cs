using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDoList.Models;
using System;

namespace ToDoList.Models
{
  public class Category
  {
    private string _name;
    private int _id;
    private List<Item> _items;
    private static List<Category> _instances = new List<Category> {};

    public Category(string categoryName)
    {
        _name = categoryName;
        _id = _instances.Count + 1;
        _items = new List<Item>{};
    }

    public void Save()
    {
       _instances.Add(this);
    }

    public List<Item> GetItems()
    {
        return _items;
    }
    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public string GetName()
    {
        return _name;
    }
    public int GetId()
    {
        return _id;
    }
    public static List<Category> GetAll()
    {
        return _instances;
    }
    public static void Clear()
    {
        _instances.Clear();
    }
    public static Category Find(int searchId)
    {
        return _instances[searchId-1];
    }
  }
}
