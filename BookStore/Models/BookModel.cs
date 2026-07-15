using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Models
{
    public class Book
    {
        public string isbn { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string author { get; set; }
        public string publish_date { get; set; }
        public string publisher { get; set; }
        public int pages { get; set; }
        public string description { get; set; }
        public string website { get; set; }
    }

    public class BooksResponse
    {
        public List<Book> books { get; set; }
    }

    public class AddBookRequest
    {
        public string userId { get; set; }
        public List<ISBN> collectionOfIsbns { get; set; }
    }

    public class ISBN
    {
        public string isbn { get; set; }
    }
}
