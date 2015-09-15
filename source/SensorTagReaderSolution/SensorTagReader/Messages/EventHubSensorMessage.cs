using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagReader.Messages
{
    public class EventHubSensorMessage
    {
        [JsonProperty("sensorName")]
        public string SensorName { get; set; }

        [JsonProperty("time")]
        public DateTime TimeWhenRecorded { get; set; }

        [JsonIgnore]
        public double Temperature { get; set; }

        [JsonIgnore]
        public double Humidity { get; set; }

        [JsonProperty("temperature")]
        public double TemperatureTruncated
        {
            get { return Math.Round(Temperature, 2); }
        }

        [JsonProperty("humidity")]
        public double HumidityTruncated
        {
            get { return Math.Round(Humidity, 2); }
        }

    }
}
