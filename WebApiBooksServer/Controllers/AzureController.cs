using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BooksDataAccess;

namespace WebApiBooksServer.Controllers
{
    public class AzureController : ApiController
    {
        public class BooksController : ApiController
        {
            // /api/Books
            public IEnumerable<BooksTable> Get()
            {
                using (AzureDBEntities entities = new AzureDBEntities())
                {
                    return entities.BooksTable.ToList();//BooksTables.ToList();
                }
            }
            // /api/Books/1
            public BooksTable Get(int id)
            {
                using (AzureDBEntities entities = new AzureDBEntities())
                {
                    return entities.BooksTable.FirstOrDefault(e => e.ID == id);//BooksTables.FirstOrDefault(e => e.ID == id);
                }
            }
            //POST
            /*[ResponseType(typeof(BooksTable))]
            public IHttpActionResult PostBook (BooksTable books)
            {
                BooksDBEntities db = new BooksDBEntities();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                db.BooksTables.Add(books);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = books.ID }, books);
            }*/
            [HttpPost]
            public HttpResponseMessage AddBook([FromBody] BooksTable books)
            {
                try
                {
                    using (AzureDBEntities entities = new AzureDBEntities())
                    {
                        entities.BooksTable.Add(books);//BooksTables.Add(books);
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, books);
                        message.Headers.Location = new Uri(Request.RequestUri + books.ID.ToString());
                        return message;
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            public IHttpActionResult DeleteBook(int id)
            {
                AzureDBEntities db = new AzureDBEntities();
                BooksTable books = db.BooksTable.Find(id);//BooksTables.Find(id);
                if (books == null)
                {
                    return NotFound();
                }
                db.BooksTable.Remove(books);//BooksTables.Remove(books);
                db.SaveChanges();
                return Ok(books);
            }
            [HttpPut]
            public HttpResponseMessage UpdateBook(int id, [FromBody]BooksTable books)
            {
                try
                {
                    using (AzureDBEntities db = new AzureDBEntities())
                    {
                        var dbentity = db.BooksTable.FirstOrDefault(e => e.ID == id);//BooksTables.FirstOrDefault(e => e.ID == id);
                        if (dbentity == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Książka o ID = " + id.ToString() + "nie została znaleziona");
                        }
                        else
                        {
                            dbentity.ID = books.ID;
                            dbentity.TITLE = books.TITLE;
                            dbentity.VERSION = books.VERSION;
                            dbentity.AUTHOR = books.AUTHOR;
                            dbentity.GENRE = books.GENRE;
                            dbentity.YEAR = books.YEAR;
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, dbentity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }
    }
}
