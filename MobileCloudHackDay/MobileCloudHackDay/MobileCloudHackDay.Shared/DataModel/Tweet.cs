using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCloudHackDay.Core.DataModel
{
    public class Tweet
    {
        public string Id { get; set; }
        public long StatusId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string User { get; set; }
        public string UserImageUri { get; set; }
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public string UserToPresentation { get { return User + " (@" + UserId + ")"; } }

        [JsonIgnore]
        public string TextToPresentation { get { return @"""" + Text + @""""; } }

    }
}
