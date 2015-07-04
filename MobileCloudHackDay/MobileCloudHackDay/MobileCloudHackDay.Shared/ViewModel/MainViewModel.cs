using GalaSoft.MvvmLight;
using MobileCloudHackDay.Core.DataModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;

namespace MobileCloudHackDay.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {

        private Tweet _featuredTweet;

        public Tweet FeaturedTweet
        {
            get { return _featuredTweet; }
            set { Set(ref _featuredTweet, value); }
        }


        private ObservableCollection<Tweet> _tweetsList;

        public ObservableCollection<Tweet> TweetsList
        {
            get { return _tweetsList; }
            set { Set(ref _tweetsList, value); }
        }


        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }


        public MainViewModel()
        {

            TweetsList = new ObservableCollection<Tweet>();

            if (IsInDesignMode)
            {
                FeaturedTweet = new Tweet() { Text = "qwdwqdqdqwdq dwq dqwnd 'qw qwdqd ;aa das das asd aqwdwqdqdqwdq dwq dqwnd 'qw qwdqd ;aa das das asd acaa", User = "Some user", UserId = "@dqwdqwdasdas"};

                TweetsList.Add(FeaturedTweet);
                TweetsList.Add(FeaturedTweet);
                TweetsList.Add(FeaturedTweet);
                TweetsList.Add(FeaturedTweet);
                TweetsList.Add(FeaturedTweet);
                TweetsList.Add(FeaturedTweet);
            }
            else
            {
                
            }
        }

        public async Task UpdateList(bool askServiceToUpdate = false)
        {
            try
            {
                if (IsBusy) { return; }

                IsBusy = true;

                if (askServiceToUpdate)
                {
                    await App.MobileService.InvokeApiAsync("UpdateTweets", HttpMethod.Get, null);
                }


                var tweetTable = App.MobileService.GetTable<Tweet>();

                var query = tweetTable.CreateQuery();

                query = query.IncludeTotalCount();
                query = query.OrderBy(x => x.Date); 
                
                var queryResult = await query.ToCollectionAsync(50);

                while (queryResult.HasMoreItems)
                {
                    await queryResult.LoadMoreItemsAsync();
                }

                foreach (var tweet in queryResult)
                {
                    if (!TweetsList.Any(x => x.Id  == tweet.Id))
                    {
                        TweetsList.Insert(0, tweet);
                        await Task.Delay(80);

                        FeaturedTweet = tweet;
                    }
                }

                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}