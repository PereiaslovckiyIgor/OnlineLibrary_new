﻿using LibOnline.Models.Authors;
using LibOnline.Models.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace LibOnline.Models.Books
{
    [Table("UserBooks", Schema = "books")]
    public class GetUsersBooks
    {
        [Key]
        public int IdUserBook { get; set; }
        public int IdUser { get; set; }
        public int IdBook { get; set; }
        public int IdPage { get; set; }
    }//GetUsersBooks


    public class UserBook
    {
        [Key]
        public int IdUserBook { get; set; }
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string BooksDescription { get; set; }
        public DateTime ReleasedData { get; set; }
        public string AuthorsInfo { get; set; }
        public string BookCategories { get; set; }
        public string ImagePath { get; set; }
        public int IdPage { get; set; }
        public int CommensCount { get; set; }
        public int PageNumber { get; set; }

    }//UserBook

    public class UserBookToShow
    {
        public int IdUserBook { get; set; }
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string BooksDescription { get; set; }
        public DateTime ReleasedData { get; set; }
        public List<Author> BookAuthors { get; set; }
        public List<Category> BookCategories { get; set; }
        public string ImagePath { get; set; }
        public int IdPage { get; set; }
        public int CommensCount { get; set; }
        public int PageNumber { get; set; }


        public UserBookToShow(UserBook item)
        {
            BookAuthors = AuthorsFromXML(item.AuthorsInfo);
            BookCategories = CategoriesFromXML(item.BookCategories);

            IdUserBook = item.IdUserBook;
            IdBook = item.IdBook;
            BookName = item.BookName;
            BooksDescription = item.BooksDescription;
            ReleasedData = item.ReleasedData;
            ImagePath = item.ImagePath;
            IdPage = item.IdPage;
            CommensCount = item.CommensCount;
            PageNumber = item.PageNumber;
        }//c-tor

        private List<Author> AuthorsFromXML(string xmlAuthorsInfo)
        {
            List<Author> authors = new List<Author>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<xml>" + xmlAuthorsInfo + "</xml>");

            XmlNodeList authorsInfo = xmlDoc.GetElementsByTagName("author");

            int id;
            string fullName;
            foreach (XmlNode xn in authorsInfo)
            {
                id = int.Parse(xn["IdAuthor"].InnerText);
                fullName = xn["AuthorFullName"].InnerText;
                authors.Add(new Author(id, fullName));
            }//foreach

            return authors;
        }//AuthorsFromXML

        private List<Category> CategoriesFromXML(string xmlCategories)
        {
            List<Category> categories = new List<Category>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<xml>" + xmlCategories + "</xml>");

            XmlNodeList authorsInfo = xmlDoc.GetElementsByTagName("categories");

            int id;
            string fullName;
            foreach (XmlNode xn in authorsInfo)
            {
                id = int.Parse(xn["IdCategory"].InnerText);
                fullName = xn["CategoryName"].InnerText;
                categories.Add(new Category(id, fullName));
            }//foreach

            return categories;
        }//CategoriesFromXML

    }//BookDescriptionToShow
}
