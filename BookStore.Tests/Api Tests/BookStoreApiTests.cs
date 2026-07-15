using AutomationFramework.Models;
using AutomationFramework.Services;
using AutomationFramework.Tests.Tests;
using Newtonsoft.Json;

namespace AutomationFramework.Tests
{
    // ✅ S Principle - Only responsible
    // for BookStore related test cases
    //
    // ✅ O Principle - Extends BaseTest
    // without modifying it
    [TestFixture]
    public class BookStoreApiTests : BaseApiTests
    {
        private string _testIsbn = "9781449325862";

        [SetUp]
        public void Setup()
        {
            // ✅ Clear all books before each test
            // so tests dont interfere each other
            if (UserId != null && Token != null)
            {
                BookStoreService.DeleteAllBooks(UserId, Token);
                Console.WriteLine($"🗑️ Books Cleared Before Test");
            }
        }

        // =============================================
        // ✅ GET BOOKS TESTS
        // =============================================

        // ✅ TC001 - Get All Books
        [Test, Order(1)]
        public void TC001_GetAllBooks_ShouldReturn200()
        {
            var response = BookStoreService.GetAllBooks();
            var books = JsonConvert
                        .DeserializeObject<BooksResponse>
                        (response.Content);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(books.books.Count, Is.GreaterThan(0));
            Console.WriteLine($"✅ Total Books: {books.books.Count}");
        }

        // ✅ TC002 - Verify Books List is Not Empty
        [Test, Order(2)]
        public void TC002_GetAllBooks_ListShouldNotBeEmpty()
        {
            var response = BookStoreService.GetAllBooks();
            var books = JsonConvert
                        .DeserializeObject<BooksResponse>
                        (response.Content);

            Assert.That(books.books, Is.Not.Empty);
            Assert.That(books.books[0].title, Is.Not.Null);
            Console.WriteLine($"✅ First Book: {books.books[0].title}");
        }

        // ✅ TC003 - Get Book by Valid ISBN
        [Test, Order(3)]
        public void TC003_GetBookByIsbn_ValidIsbn_ShouldReturn200()
        {
            var response = BookStoreService
                           .GetBookByIsbn(_testIsbn);
            var book = JsonConvert
                       .DeserializeObject<Book>
                       (response.Content);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(book.isbn, Is.EqualTo(_testIsbn));
            Assert.That(book.title, Is.Not.Null);
            Console.WriteLine($"✅ Book Title: {book.title}");
            Console.WriteLine($"✅ Book Author: {book.author}");
        }

        // ✅ TC004 - Get Book by Invalid ISBN
        [Test, Order(4)]
        public void TC004_GetBookByIsbn_InvalidIsbn_ShouldReturn400()
        {
            var response = BookStoreService
                           .GetBookByIsbn("INVALIDISBN123");

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Console.WriteLine($"✅ Invalid ISBN Rejected!");
            Console.WriteLine($"✅ Response: {response.Content}");
        }

        // =============================================
        // ✅ ADD BOOK TESTS
        // =============================================

        // ✅ TC005 - Add Book with Valid Data
        [Test, Order(5)]
        public void TC005_AddBook_ValidData_ShouldReturn201()
        {
            var response = BookStoreService
                           .AddBook(UserId, _testIsbn, Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            Console.WriteLine($"✅ Book Added Successfully!");
        }

        // ✅ TC006 - Add Book Without Token
        [Test, Order(6)]
        public void TC006_AddBook_WithoutToken_ShouldReturn401()
        {
            var response = BookStoreService
                           .AddBook(UserId, _testIsbn, "");

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            Console.WriteLine($"✅ Unauthorized as Expected!");
        }

        // ✅ TC007 - Add Duplicate Book
        [Test, Order(7)]
        public void TC007_AddBook_DuplicateBook_ShouldReturn400()
        {
            // ✅ Add book first time
            BookStoreService.AddBook(UserId, _testIsbn, Token);

            // ✅ Try adding same book again
            var response = BookStoreService
                           .AddBook(UserId, _testIsbn, Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Console.WriteLine($"✅ Duplicate Book Rejected!");
        }

        // ✅ TC008 - Add Book with Invalid ISBN
        [Test, Order(8)]
        public void TC008_AddBook_InvalidIsbn_ShouldReturn400()
        {
            var response = BookStoreService
                           .AddBook(UserId, "INVALIDISBN", Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Console.WriteLine($"✅ Invalid ISBN Rejected!");
        }

        // =============================================
        // ✅ DELETE BOOK TESTS
        // =============================================

        // ✅ TC009 - Delete Specific Book
        [Test, Order(9)]
        public void TC009_DeleteBook_ValidData_ShouldReturn204()
        {
            // ✅ Add book first then delete
            BookStoreService.AddBook(UserId, _testIsbn, Token);

            var response = BookStoreService.DeleteBook(UserId, _testIsbn, Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(204));
            Console.WriteLine($"✅ Book Deleted Successfully!");
        }

        // ✅ TC010 - Delete Book Without Token
        [Test, Order(10)]
        public void TC010_DeleteBook_WithoutToken_ShouldReturn401()
        {
            var response = BookStoreService.DeleteBook(UserId, _testIsbn, "");

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            Console.WriteLine($"✅ Unauthorized as Expected!");
        }

        // ✅ TC011 - Delete Book with Invalid ISBN
        [Test, Order(11)]
        public void TC011_DeleteBook_InvalidIsbn_ShouldReturn400()
        {
            var response = BookStoreService
                           .DeleteBook(UserId, "INVALIDISBN", Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Console.WriteLine($"✅ Invalid ISBN Rejected!");
        }

        // ✅ TC012 - Delete All Books
        [Test, Order(12)]
        public void TC012_DeleteAllBooks_ShouldReturn204()
        {
            // ✅ Add book first then delete all
            BookStoreService.AddBook(UserId, _testIsbn, Token);

            var response = BookStoreService
                           .DeleteAllBooks(UserId, Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(204));
            Console.WriteLine($"✅ All Books Deleted Successfully!");
        }

        // ✅ TC013 - Delete All Books Without Token
        [Test, Order(13)]
        public void TC013_DeleteAllBooks_WithoutToken_ShouldReturn401()
        {
            var response = BookStoreService
                           .DeleteAllBooks(UserId, "");

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            Console.WriteLine($"✅ Unauthorized as Expected!");
        }
    }
}