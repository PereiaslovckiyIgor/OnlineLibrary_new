using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LibOnline.Models.Authors;
using System.Xml;

namespace LibOnline.Models.Books
{
    public class BooksCategories
    {
        [Key]
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public string AuthorsInfo { get; set; }
        //public int IdAuthor { get; set; }
        //public string AuthorFullName { get; set; }
        public int RatingValue { get; set; }
        public string BooksDescription { get; set; }
        public string ImagePath { get; set; }
        public DateTime ReleasedData { get; set; }
        public DateTime AddedDate { get; set; }
    }//BooksCategories




    public class BooksCatogoriesToShow
    {
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public List<Author> BookAuthors { get; set; }
        public int RatingValue { get; set; }
        public string BooksDescription { get; set; }
        public string ImagePath { get; set; }
        public DateTime ReleasedData { get; set; }
        public DateTime AddedDate { get; set; }

        public BooksCatogoriesToShow(BooksCategories item)
        {
            BookAuthors = AuthorsFromXML(item.AuthorsInfo);

            IdBook = item.IdBook;
            BookName = item.BookName;
            RatingValue = item.RatingValue;
            ImagePath = item.ImagePath;
            BooksDescription = item.BooksDescription;
            ReleasedData = item.ReleasedData;
            AddedDate = item.AddedDate;
        }//c-tor

        public void AddAuthor(int IdAuthor, string AuthorFullName)
        {
            BookAuthors.Add(new Author(IdAuthor, AuthorFullName));
        }//AddAuthor


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

    }//BooksCatogoriesToShow
}
