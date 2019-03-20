using System;
using System.Collections.Generic;
using LibOnline.Models.Authors;
using LibOnline.Models.Categories;
using System.Xml;
using System.ComponentModel.DataAnnotations;

namespace LibOnline.Models.Books
{
    public class BookDescription
    {
        [Key]
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string BooksDescription { get; set; }
        public DateTime ReleasedData { get; set; }
        public string AuthorsInfo { get; set; }
        public string BookCategories { get; set; }
        public int RatingValue { get; set; }
        public string ImagePath { get; set; }
    }//BookDescription


    public class BookDescriptionToShow
    {
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string BooksDescription { get; set; }
        public DateTime ReleasedData { get; set; }
        public List<Author> BookAuthors { get; set; }
        public List<Category> BookCategories { get; set; }
        public int RatingValue { get; set; }
        public string ImagePath { get; set; }



        public BookDescriptionToShow(BookDescription item)
        {
            BookAuthors = AuthorsFromXML(item.AuthorsInfo);
            BookCategories = CategoriesFromXML(item.BookCategories);

            IdBook = item.IdBook;
            BookName = item.BookName;
            BooksDescription = item.BooksDescription;
            ReleasedData = item.ReleasedData;
            RatingValue = item.RatingValue;
            ImagePath = item.ImagePath;
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

}//Books Namespace
