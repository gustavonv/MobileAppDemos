using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCloudHackDayService.DataObjects
{
    public class Tweet : EntityData
    {

        public long StatusId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string User { get; set; }
        public string UserImageUri { get; set; }
        public DateTime? Date { get; set; }


    }
}
