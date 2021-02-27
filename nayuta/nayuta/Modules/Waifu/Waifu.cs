using System;
using System.Collections.Generic;
using nayuta.Internal;
using Newtonsoft.Json;

namespace nayuta.Modules.Waifu
{
    [Serializable]
    public class Waifu
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("submitted_by")]
        public int SubmitterID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("source")]
        public string SourceName { get; set; }
        
        [JsonProperty("source_id")]
        public int SourceID { get; set; }
        
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        
        [JsonProperty("bust")]
        private int Bust { get; set; }
        
        [JsonProperty("waist")]
        private int Waist { get; set; }
        
        [JsonProperty("hip")]
        private int Hip { get; set; }
        
        [JsonProperty("age")]
        public int Age { get; set; }
        
        [JsonProperty("height")]
        public int Height { get; set; }
        
        [JsonProperty("weight")]
        public int Weight { get; set; }
        
        [JsonProperty("birthdate")]
        public string Birthday { get; set; }
        
        [JsonProperty("bloodtype")]
        public string Bloodtype { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("images")]
        public List<WaifuImage> Images { get; set; }
        
        [JsonProperty("date_added")]
        private long _uploadTime { get; set; }

        public DateTimeOffset UploadTime
        {
            get
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(_uploadTime==0?1569596103:_uploadTime);
                return dateTimeOffset;
            }
        }

        private WaifuMeasurements _measurements = new WaifuMeasurements();
        public WaifuMeasurements Measurements
        {
            get
            {
                if (_measurements.Bust == null)
                    _measurements.Bust = Bust;
                if (_measurements.Waist == null)
                    _measurements.Waist = Waist;
                if (_measurements.Hip == null)
                    _measurements.Hip = Hip;
                
                return _measurements;
            }
        }

    }
}