using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidLibrary;

namespace GAT_Produkcja.Utilities.ScaleService
{

    public class USBScale
    {
        #region Fields

        private HidDevice scale;
        private HidDeviceData inData;

        #endregion
        #region Properties
        public bool IsConnected
        {
            get
            {
                return scale == null ? false : scale.IsConnected;
            }
        }
        public decimal ScaleStatus
        {
            get
            {
                return inData.Data[1];
            }
        }
        public decimal ScaleWeightUnits
        {
            get
            {
                return inData.Data[2];
            }
        }

        #endregion

        public HidDevice GetDevice()
        {
            HidDevice hidDevice;
            // Stamps.com Scale
            hidDevice = HidDevices.Enumerate(0x1446, 0x6A73).FirstOrDefault();
            if (hidDevice != null)
                return hidDevice;

            // Metler Toledo
            hidDevice = HidDevices.Enumerate(0x0eb8).FirstOrDefault();
            if (hidDevice != null)
                return hidDevice;


            hidDevice = HidDevices.Enumerate(0x0403, 0x6010).FirstOrDefault();
            if (hidDevice != null)
                return hidDevice;


            return null;
        }
        public bool Connect()
        {
            // Find a Scale
            HidDevice device = GetDevice();
            if (device != null)
                return Connect(device);
            else
                return false;
        }
        public bool Connect(HidDevice device)
        {
            scale = device;
            int waitTries = 0;
            scale.OpenDevice();

            // sometimes the scale is not ready immedietly after
            // Open() so wait till its ready
            while (!this.IsConnected && waitTries < 10)
            {
                Thread.Sleep(50);
                waitTries++;
            }
            return this.IsConnected;
        }
        public void Disconnect()
        {
            if (this.IsConnected)
            {
                scale.CloseDevice();
                scale.Dispose();
            }
        }
        public void DebugScaleData()
        {
            for (int i = 0; i < inData.Data.Length; ++i)
            {
                Console.WriteLine("Byte {0}: {1}", i, inData.Data[i]);
            }
        }
        public void GetWeight(out decimal? weight, out bool? isStable)
        {
            weight = null;
            isStable = false;

            if (this.IsConnected)
            {
                inData = scale.Read(250);
                // Byte 0 == Report ID?
                // Byte 1 == Scale Status (1 == Fault, 2 == Stable @ 0, 3 == In Motion, 4 == Stable, 5 == Under 0, 6 == Over Weight, 7 == Requires Calibration, 8 == Requires Re-Zeroing)
                // Byte 2 == Weight Unit
                // Byte 3 == Data Scaling (decimal placement) - signed byte is power of 10
                // Byte 4 == Weight LSB
                // Byte 5 == Weight MSB

                weight = (decimal?)BitConverter.ToInt16(new byte[] { inData.Data[4], inData.Data[5] }, 0) *
                                 Convert.ToDecimal(Math.Pow(10, (sbyte)inData.Data[3]));

                switch (Convert.ToInt16(inData.Data[2]))
                {
                    case 3:  // Kilos
                        weight = weight * (decimal?)2.2;
                        break;
                    case 11: // Ounces
                        weight = weight * (decimal?)0.0625;
                        break;
                    case 12: // Pounds
                             // already in pounds, do nothing
                        break;
                }
                isStable = inData.Data[1] == 0x4;
            }
        }
    }
}
