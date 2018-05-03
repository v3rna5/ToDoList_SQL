using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDoList.Models;
using System;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    private int _id;
    private string _dueDate;
    private static List<Item> _instances = new List<Item> {};

    public Item (string Description)
    {
      _description = Description;
    }

    public override bool Equals(System.Object otherItem)
    {
        if (!(otherItem is Item))
        {
            return false;
        }
        else
        {
            Item newItem = (Item) otherItem;
            bool idEquality = (this.GetId() == newItem.GetId());
            bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
            return (idEquality && descriptionEquality);
        }
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO `items` (`description`, `due_date`) VALUES (@ItemDescription, @ItemDueDate);";

        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@ItemDescription";
        description.Value = this._description;
        cmd.Parameters.Add(description);

        MySqlParameter due_date = new MySqlParameter();
        due_date.ParameterName = "@ItemDueDate";
        due_date.Value = this._dueDate;
        cmd.Parameters.Add(due_date);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public int GetId()
    {
        return _id;
    }

    public string GetDescription()
    {
        return _description;
    }

    public string GetDate()
    {
        return _dueDate;
    }

    public void SetDescription(string newDescription)
    {
        _description = newDescription;
    }

    public void SetId(int newId)
    {
        _id = newId;
    }

    public void SetDate(string newDate)
    {
        _dueDate = newDate;
    }

    public static List<Item> GetAll()
    {
        List<Item> allItems = new List<Item> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM items;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int itemId = rdr.GetInt32(0);
          string itemDescription = rdr.GetString(1);
          //string itemDueDate = rdr.GetString(2);
          Item newItem = new Item(itemDescription);
          newItem.SetId(itemId);
          //newItem.SetDate(itemDueDate);
          allItems.Add(newItem);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allItems;
    }

    public static void DeleteAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM items;";

        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public static void ClearAll()
    {
        _instances.Clear();
    }

    public static Item Find(int id)
    {
        MySqlConnection conn = DB.Connection();
          conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";

        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = id;
        cmd.Parameters.Add(thisId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        int itemId = 0;
        string itemDescription = "";

        while (rdr.Read())
        {
            itemId = rdr.GetInt32(0);
            itemDescription = rdr.GetString(1);
        }

        Item foundItem= new Item(itemDescription);
        foundItem.SetId(itemId);

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }

        return foundItem;
    }
  }
}
