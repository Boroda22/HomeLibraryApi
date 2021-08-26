using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeLibraryApi.Models;
using HomeLibraryApi.Services;

namespace HomeLibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly StorageContext _context;

        public BooksController(StorageContext context)
        {
            _context = context;
        }


        // GET ALL: api/Books
        [HttpGet]
        public JsonResult GetBooks()
        {
            return new JsonResult(_context.GetBooks());
        }

        // GET object: api/Books/5
        [HttpGet("{id}")]
        public JsonResult GetBook(int id)
        {
            return new JsonResult(_context.GetBookById(id));
        }
        
        // Update: api/Books/5
        [HttpPut("{id}")]
        public string PutBook(int id, Book book)
        {
            return _context.UpdateBook(id, book) == true ? "Объект обновлен" : "Объект не обновлен";
        }

        // Create: api/Books
        [HttpPost]
        public string PostBook(Book book)
        {
            return _context.CreateBook(book) == true ? "Объект создан" : "Объект не создан";
        }

        // Delete: api/Books/5
        [HttpDelete("{id}")]
        public string DeleteBook(int id)
        {
            return _context.DeleteBook(id) == true ? "Объект удален" : "Объект не удален";
        }
    }
}
