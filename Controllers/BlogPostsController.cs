using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testWebAPI.DBs;
using testWebAPI.Models;

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        // GET api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new BlogPostQuery(db);
                var result = await query.LatestPostsAsync();
                return new OkObjectResult(result);
            }
        }

        // GET api/blogposts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new BlogPostQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }
        }

        // POST api/blogposts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BlogPost body)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                body.Db = db;
                await body.InsertAsync();
                return new OkObjectResult(body);
            }
        }

        // PUT api/blogposts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]BlogPost body)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new BlogPostQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                result.Title = body.Title;
                result.Content = body.Content;
                await result.UpdateAsync();
                return new OkObjectResult(result);
            }
        }

        // DELETE api/blogposts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new BlogPostQuery(db);
                var result = await query.FindOneAsync(id);
                if (result == null)
                    return new NotFoundResult();
                await result.DeleteAsync();
                return new OkResult();
            }
        }

        // DELETE api/blogposts
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                var query = new BlogPostQuery(db);
                await query.DeleteAllAsync();
                return new OkResult();
            }
        }
    }
}
