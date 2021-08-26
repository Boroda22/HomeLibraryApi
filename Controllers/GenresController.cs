using Microsoft.AspNetCore.Mvc;
using HomeLibraryApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace HomeLibraryApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly StorageContext _context;

        public GenresController(StorageContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetBooksByGenre(string genreName)
        {
            return new JsonResult(_context.GetBooksByGenre(genreName));
        }
    }
}
