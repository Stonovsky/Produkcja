using GAT_Produkcja.Utilities.ScaleService.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.ScaleService
{
    public abstract class ScaleReaderBase
    {
        #region Fields

        protected SerialPort serialPort;
        protected string scaleMessage;
        #endregion

        #region Properties
        public ScaleModel ScaleModel { get; set; } = new ScaleModel();
        protected virtual string ComPortName { get; set; }
        protected abstract string MessageToScale { get; }
        public string Message { get; set; }
        public decimal Weight { get; set; }
        #endregion

        /// <summary>
        /// Otwiera port wskazany w Properties + dopisuje delegata reagujacego na message z wagi
        /// </summary>
        protected virtual void SerialPortOpen()
        {
            string[] portNames = SerialPort.GetPortNames();

            serialPort = new SerialPort(ComPortName, 9600, Parity.None, 8, StopBits.One);
            serialPort.Close();
            serialPort.Dispose();

            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.Open();
        }
        /// <summary>
        /// Zamyka porty
        /// </summary>
        protected virtual void SerialPortClose()
        {
            serialPort.Close();
            serialPort.Dispose();
        }

        /// <summary>
        /// Delegat uruchamiany po uzyskaniu wiadomosci z wagi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort)sender;
            Thread.Sleep(300);
            scaleMessage = sp.ReadExisting();

            //scaleMessage = serialPort.ReadExisting();
            if (scaleMessage.Length < 5)
                scaleMessage = string.Empty;
            else
            {
                Message = MessageParser(scaleMessage);
                Weight = WeightParser(scaleMessage);
            }
        }

        /// <summary>
        /// Wysyla wiadomosc <see cref="MessageToScale"/> do wagi 
        /// </summary>
        /// <returns></returns>
        protected async Task SendMessage()
        {
            SerialPortOpen();

            serialPort.WriteLine(MessageToScale);
            await Task.Delay(500);

            SerialPortClose();
        }

        /// <summary>
        /// Parsuje ramke (message) otrzymana z wagi
        /// </summary>
        /// <param name="scaleMessage"></param>
        /// <returns></returns>
        protected abstract string MessageParser(string scaleMessage);

        protected decimal WeightParser(string scaleMessage)
        {
            var message = MessageParser(scaleMessage);

            decimal.TryParse(message, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal weight);

            return weight;
        }

        /// <summary>
        /// Pobiera wage w <see cref="decimal"/>
        /// </summary>
        /// <returns></returns>
        public virtual async Task<decimal> GetWeight()
        {
            await SendMessage();

            return Weight;
        }

        /// <summary>
        /// Pobiera wage w <see cref="string"/>
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string> GetWeightInString()
        {
            await SendMessage();
            return Message;
        }


    }
}
