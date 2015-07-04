using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.AppService;
using MobileCloudHackDayService.Models;
using MobileCloudHackDayService.Helpers;
using System.Threading.Tasks;
using MobileCloudHackDayService.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using LinqToTwitter;

namespace MobileCloudHackDayService.Controllers
{

    [AuthorizeLevel(Microsoft.Azure.Mobile.Security.AuthorizationLevel.Application)]
    public class UpdateTweetsController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/UpdateTweets
        public async Task<string> Get()
        {
            //Services.Log.Info("Hello from custom controller!");

            var dbCtx = new MobileServiceContext();

            long lastTweetId = 0;

            var t = dbCtx.Tweets.OrderByDescending(x => x.Date).ToList().Select(x => new { StatusId = x.StatusId }).FirstOrDefault();

            if (t != null) { lastTweetId = t.StatusId; }

            var statuses = await GetLatestsTwittersWithHashtag("mobilecloudhackday", lastTweetId);

            var newTweets = new List<Tweet>();
            foreach (var status in statuses)
            {

                var twt = new Tweet()
                {
                    Id = Guid.NewGuid().ToString(),
                    StatusId = (long)status.StatusID,
                    Text = status.Text,
                    Date = status.CreatedAt,
                    User = status.User.Name,
                    UserId = status.User.ScreenNameResponse,
                    UserImageUri = status.User.ProfileImageUrl
                };

                dbCtx.Tweets.Add(twt);
                newTweets.Add(twt);

            }

            await dbCtx.SaveChangesAsync();

            await NotifyPush(newTweets);

            return "Ok";
        }


        private async Task<List<Status>> GetLatestsTwittersWithHashtag(string hashtag, long laterThanThisID = 0)
        {
            try
            {

                hashtag = !hashtag.Contains("#") ? "#" + hashtag : hashtag;

                var authorizer = new ApplicationOnlyAuthorizer
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = Services.Settings["TwitterConsumerKey"],
                        ConsumerSecret = Services.Settings["TwitterConsumerSecret"],
                    }
                };

                await authorizer.AuthorizeAsync();
                var twitterCtx = new TwitterContext(authorizer);

                var query = twitterCtx.Search.Where(x => x.Type == SearchType.Search && x.Query == hashtag && x.Count == 100);

                if (laterThanThisID > 0)
                {
                    query = query.Where(x => x.SinceID == (ulong)laterThanThisID);
                }

                var searchresponse = query.FirstOrDefault();

                return searchresponse.Statuses;

            }
            catch (Exception ex)
            {
                return new List<Status>();
            }
        }


        private async Task NotifyPush(List<Tweet> tweets)
        {
            try
            {
                // get Notification Hubs credentials associated with this Mobile App
                string notificationHubName = this.Services.Settings.NotificationHubName;
                string notificationHubConnection = this.Services.Settings.Connections[ServiceSettingsKeys.NotificationHubConnectionString].ConnectionString;

                // connect to notification hub
                NotificationHubClient Hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);


                foreach (var tweet in tweets)
                {
                    // windows payload
                    //var windowsToastPayload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + tweet.Text + @"</text></binding></visual></toast>";

                    var windowsToastPayload = @"<toast><visual><binding template=""ToastImageAndText02""><image id=""1"" src=""imageSourceHA"" alt=""image1""/><text id=""1"">headlineTextHA</text><text id=""2"">bodyTextHA</text></binding></visual></toast>";

                    windowsToastPayload = windowsToastPayload.Replace("imageSourceHA", tweet.UserImageUri);
                    windowsToastPayload = windowsToastPayload.Replace("headlineTextHA", tweet.UserId);
                    windowsToastPayload = windowsToastPayload.Replace("bodyTextHA", tweet.Text);

                    await Hub.SendWindowsNativeNotificationAsync(windowsToastPayload);
                }
            }
            catch (Exception ex)
            {
                Services.Log.Error("Erro in push tweets: " + ex.Message);
            }
            
           
        }
    }
}
