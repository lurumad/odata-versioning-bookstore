using BookStore.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/books")]
    public class BooksController : ODataController
    {
        private BookStoreContext _db;

        public BooksController(BookStoreContext context)
        {
            _db = context;

            if (context.Books.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    context.Books.Add(b);
                    context.Presses.Add(b.Press);
                }
                context.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Books);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Ok(_db.Books);
        }

        [HttpGet]
        [Route("{key:int}")]
        public IActionResult GetBy(int key)
        {
            return Ok(_db.Books.FirstOrDefault(c => c.Id == key));
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody]Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return Created(book);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete([FromBody]int key)
        {
            Book b = _db.Books.FirstOrDefault(c => c.Id == key);
            if (b == null)
            {
                return NotFound();
            }

            _db.Books.Remove(b);
            _db.SaveChanges();
            return Ok();
        }
    }
}
