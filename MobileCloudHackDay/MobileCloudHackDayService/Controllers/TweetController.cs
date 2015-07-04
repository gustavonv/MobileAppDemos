using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.AppService;
using MobileCloudHackDayService.DataObjects;
using MobileCloudHackDayService.Models;

namespace MobileCloudHackDayService.Controllers
{
    public class TweetController : TableController<Tweet>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Tweet>(context, Request, Services);
        }

        // GET tables/Tweet
        public IQueryable<Tweet> GetAllTweet()
        {
            return Query(); 
        }

        // GET tables/Tweet/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Tweet> GetTweet(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Tweet/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Tweet> PatchTweet(string id, Delta<Tweet> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Tweet
        public async Task<IHttpActionResult> PostTweet(Tweet item)
        {
            Tweet current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Tweet/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTweet(string id)
        {
             return DeleteAsync(id);
        }

    }
}