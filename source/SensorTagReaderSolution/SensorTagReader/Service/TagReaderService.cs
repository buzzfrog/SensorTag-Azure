using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using X2CodingLab.SensorTag;
using X2CodingLab.SensorTag.Sensors;

namespace SensorTagReader.Service
{
    public class SensorValues
    {
        public double Humidity { get; set; }
        public double Temperature { get; set; }
    }

    public class TagReaderService
    {
        HumiditySensor _humiditySensor;
        IRTemperatureSensor _tempSensor;
        public SensorValues CurrentValues { get; set; }        
        public TagReaderService()
        {
            _humiditySensor = new HumiditySensor();
            _tempSensor = new IRTemperatureSensor();
            CurrentValues = new SensorValues();
        }
        public async Task<string> TagsText()
        {
            List<DeviceInformation> tags = await GattUtils.GetDevicesOfService(_tempSensor.SensorServiceUuid);
            if (tags != null)
                return "Total: " + tags.Count();
            else
                return String.Empty;
        }
        public async Task<string> GetSensorID()
        {
            Exception exc = null;
            string SensorData = "";

            try
            {
                using (DeviceInfoService dis = new DeviceInfoService())
                {
                    await dis.Initialize();
                    SensorData += "System ID: " + await dis.ReadSystemId() + "\n";
                    SensorData += "Model Nr: " + await dis.ReadModelNumber() + "\n";
                    SensorData += "Serial Nr: " + await dis.ReadSerialNumber() + "\n";
                    SensorData += "Firmware Revision: " + await dis.ReadFirmwareRevision() + "\n";
                    SensorData += "Hardware Revision: " + await dis.ReadHardwareRevision() + "\n";
                    SensorData += "Sofware Revision: " + await dis.ReadSoftwareRevision() + "\n";
                    SensorData += "Manufacturer Name: " + await dis.ReadManufacturerName() + "\n";
                    SensorData += "Cert: " + await dis.ReadCert() + "\n";
                    SensorData += "PNP ID: " + await dis.ReadPnpId();
                }

                return SensorData;
            }
            catch (Exception ex)
            {
                exc = ex;
            }

            if (exc != null)
                SensorData += exc.Message;

            return SensorData;
        }
        public async Task<string> InitializeSensor()
        {
            await _humiditySensor.Initialize();
            await _humiditySensor.EnableSensor();
            await _tempSensor.Initialize();
            await _tempSensor.EnableSensor();

            _humiditySensor.SensorValueChanged += SensorValueChanged;
            _tempSensor.SensorValueChanged += SensorValueChanged;

            await _humiditySensor.EnableNotifications();
            await _tempSensor.EnableNotifications();

            return ("done");
        }
        private void SensorValueChanged(object sender, X2CodingLab.SensorTag.SensorValueChangedEventArgs e)
        {
            switch (e.Origin)
            {
                case SensorName.HumiditySensor:
                    CurrentValues.Humidity = HumiditySensor.CalculateHumidityInPercent(e.RawData);
                    break;
                case SensorName.TemperatureSensor:
                    CurrentValues.Temperature = IRTemperatureSensor.CalculateAmbientTemperature(e.RawData, TemperatureScale.Celsius);
                    break;
            }
        }
    }
}
