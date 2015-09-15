using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X2CodingLab.SensorTag.NewSensors
{
    public class MovementSensor : SensorBase
    {
        public MovementSensor()
            : base(SensorName.Movement, SensorTagUuid.UUID_MOV_SERV, SensorTagUuid.UUID_MOV_CONF, SensorTagUuid.UUID_MOV_DATA)
        {

        }
        public static MovementData CalculateAccelerometerData(byte[] rawData)
        {
            MovementData result = new MovementData();
            result.X = BitConverter.ToUInt16(rawData, 6) / 64.0;
            result.Y = BitConverter.ToUInt16(rawData, 8) / 64.0;
            result.Z = BitConverter.ToUInt16(rawData, 10) * -1 / 64.0;
            return result;
        }
        public static MovementData CalculateGyroData(byte[] rawData, double scale = 1.0)
        {
            MovementData result = new MovementData();
            result.X = BitConverter.ToUInt16(rawData, 0) * (500.0 / 65536.0) * -1 * scale;
            result.Y = BitConverter.ToUInt16(rawData, 2) * (500.0 / 65536.0) * scale;
            result.Z = BitConverter.ToUInt16(rawData, 4) * (500.0 / 65536.0) * scale;
            return result;
        }
        public static MovementData CalculateMagData(byte[] rawData)
        {
            MovementData result = new MovementData();
            result.X = BitConverter.ToUInt16(rawData, 12) * (2000.0 / 65536.0) * -1;
            result.Y = BitConverter.ToUInt16(rawData, 14) * (2000.0 / 65536.0) * -1;
            result.Z = BitConverter.ToUInt16(rawData, 16) * (2000.0 / 65536.0);
            return result;
        }
    }

    public class MovementData
    {
        public double X;
        public double Y;
        public double Z;
    }
}
