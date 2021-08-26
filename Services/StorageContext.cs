using HomeLibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeLibraryApi.Services
{
    public class StorageContext : DbContext
    {
        /// <summary>
        /// заполнение первоначальными данными в случае их отсутствия
        /// </summary>
        private void fillDefaultData()
        {
            if (!Books.Any())
            {
                AddRange(
                    new Book
                    {
                        Name = "Война и мир",
                        Author = "Толстой Л.Н.",
                        Genres = new List<Genre> {
                                    new Genre { Name = "Классика" },
                                    new Genre { Name = "Драма"} }
                    },

                    new Book
                    {
                        Name = "Понедельник начинается в субботу",
                        Author = "бр. Стругацкие",
                        Genres = new List<Genre> {
                                    new Genre { Name = "Фантастика" },
                                    new Genre { Name = "Приключения"} }
                    },

                    new Book
                    {
                        Name = "1984",
                        Author = "Оруэл Джордж",
                        Genres = new List<Genre> {
                                    new Genre { Name = "Утопия"} }
                    });

                SaveChanges();
            }
        }

        public StorageContext(DbContextOptions<StorageContext> options) : base(options)
        {
            Database.EnsureCreated();
            // очень костыльно, скорее всего можно было решить как-то через внедрение зависимостей
            fillDefaultData();
        }       
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }



        /// <summary>
        /// Возвращает список книг
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Book> GetBooks()
        {
            return Books.Include("Genres").ToList();
        }

        /// <summary>
        /// Возвращает книгу по ее идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Book GetBookById(int id)
        {
            var book = Books.Find(id);

            return book;
        }

        /// <summary>
        /// Обновление данных сущности
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool UpdateBook(int id, Book book)
        {
            bool result = false;

            Entry(book).State = EntityState.Modified;
            try
            {
                SaveChanges();
                result = true;
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return result;            
        }

        /// <summary>
        /// Добавление сущности в БД
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool CreateBook(Book book)
        {
            bool result = false;
            Books.Add(book);
            try
            {
                SaveChanges();
                result = true;
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBook(int id)
        {
            bool result = false;

            var book = Books.Find(id);
            if(book == null)
            {
                return false;
            }

            try
            {
                Books.Remove(book);
                SaveChanges();
                result = true;
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Возвращает коллекцию книг по жанру
        /// </summary>
        /// <param name="genreTitle"> название жанра</param>
        /// <returns></returns>
        public IEnumerable<Book> GetBooksByGenre(string genreTitle)
        {
            var genre = Genres.Include("Books").FirstOrDefault(x => x.Name == genreTitle);
            if(genre != null)
            {
                return genre.Books;
            }
            else
            {
                return new List<Book>();
            }
        }    
    }
}
