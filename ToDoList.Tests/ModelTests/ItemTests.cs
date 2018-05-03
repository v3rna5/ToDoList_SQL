using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{

    [TestClass]
    public class ItemTests : IDisposable
    {
        public void Dispose()
        {
            Item.DeleteAll();
        }
        public ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_testt;";
        }


        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {
            //Arrange
            //Act
            int result = Item.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
        {
            // Arrange, Act
            Item firstItem = new Item("Mow the lawn");
            Item secondItem = new Item("Mow the lawn");

            // Assert
            Assert.AreEqual(firstItem, secondItem);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ItemList()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn");
            testItem.SetDate("05/10/18");

            //Act
            testItem.Save();
            List<Item> result = Item.GetAll();
            List<Item> testList = new List<Item>{testItem};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn");
          testItem.SetDate("05/10/18");

          //Act
          testItem.Save();
          Item savedItem = Item.GetAll()[0];

          int result = savedItem.GetId();
          int testId = testItem.GetId();

          //Assert
          Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
            //Arrange
            Item testItem = new Item("Mow the lawn");
            testItem.SetDate("05/10/18");
            testItem.Save();

            //Act
            Item foundItem = Item.Find(testItem.GetId());

            //Assert
            Assert.AreEqual(testItem, foundItem);
        }
    }
}
