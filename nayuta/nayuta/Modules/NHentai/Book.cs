using System;
using nayuta.Internal;
using Newtonsoft.Json;

namespace nayuta.Modules.NHentai
{
    public class Book
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("media_id")]
        public int GalleryID { get; set; }
        
        [JsonProperty("title")]
        public BookTitle Title { get; set; }
        
        [JsonProperty("tags")]
        public BetterList<BookTag> Tags { get; set; }
        
        [JsonProperty("num_pages")]
        public int PageCount { get; set; }
        
        [JsonProperty("upload_date")]
        private long _uploadTime { get; set; }

        public DateTimeOffset UploadTime
        {
            get
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(_uploadTime);
                return dateTimeOffset;
            }
        }

        [JsonProperty("error")] 
        public string Error { get; set; } = null;
        
        public string Language { get; set; }

        public void Build()
        {
            Language = Tags.Find(a => a.Type == "language").Name;
        }
    }
}