using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X2CodingLab.SensorTag.NewSensors
{
    public class IRTemperatureSensor : SensorBase
    {
        public IRTemperatureSensor()
            : base(SensorName.TemperatureSensor, SensorTagUuid.UUID_IRT_SERV, SensorTagUuid.UUID_IRT_CONF, SensorTagUuid.UUID_IRT_DATA)
        {

        }
        public static double CalculateAmbientTemperature(byte[] rawData, TemperatureScale scale)
        {
            var temperature = BitConverter.ToUInt16(rawData, 2) / 128.0;
            if (scale == TemperatureScale.Farenheit)
                return ConvertToFahrenheit(temperature);
            return temperature;
        }
        private static double ConvertToFahrenheit(double temperature)
        {
            return temperature * 1.8 + 32;
        }
        //TODO: FOR COMPLETENESS - ADD CODE FOR AMBIENT TEMP
    }
    public enum TemperatureScale
    {
        Celsius,
        Farenheit
    }
}
