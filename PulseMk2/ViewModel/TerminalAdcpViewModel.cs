using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTI
{
    class TerminalAdcpViewModel : Caliburn.Micro.Screen, IDeactivate
    {
        #region Variables

        /// <summary>
        /// ADCP Serial Port.
        /// </summary>
        private AdcpSerialPort _serialPort;

        /// <summary>
        /// Serial Port options.
        /// </summary>
        private SerialOptions _serialOption;

        #endregion

        #region Properties

        #region Serial Port Settings

        /// <summary>
        /// List of all the comm ports on the computer.
        /// </summary>
        public List<string> CommPortList { get; set; }

        /// <summary>
        /// List of all the baud rates.
        /// </summary>
        public List<int> BaudRateList { get; set; }

        /// <summary>
        /// List of all the Data Bit options.
        /// </summary>
        public List<int> DataBitList { get; set; }

        /// <summary>
        /// List of all the Parity options.
        /// </summary>
        public List<System.IO.Ports.Parity> ParityList { get; set; }

        /// <summary>
        /// List of all the Stop Bit options.
        /// </summary>
        public List<System.IO.Ports.StopBits> StopBitList { get; set; }

        /// <summary>
        /// Selected Baud Rate.
        /// </summary>
        public string SelectedCommPort
        {
            get { return _serialOption.Port; }
            set
            {
                _serialOption.Port = value;
                NotifyOfPropertyChange(() => SelectedCommPort);
            }
        }

        /// <summary>
        /// Selected Baud Rate.
        /// </summary>
        public int SelectedBaudRate
        {
            get { return _serialOption.BaudRate; }
            set
            {
                _serialOption.BaudRate = value;
                NotifyOfPropertyChange(() => SelectedBaudRate);
            }
        }

        /// <summary>
        /// Selected Data Bit.
        /// </summary>
        public int SelectedDataBit
        {
            get { return _serialOption.DataBits; }
            set
            {
                _serialOption.DataBits = value;
                NotifyOfPropertyChange(() => SelectedDataBit);
            }
        }

        /// <summary>
        /// Selected Parity.
        /// </summary>
        public System.IO.Ports.Parity SelectedParity
        {
            get { return _serialOption.Parity; }
            set
            {
                _serialOption.Parity = value;
                NotifyOfPropertyChange(() => SelectedParity);
            }
        }

        /// <summary>
        /// Selected Stop Bits.
        /// </summary>
        public System.IO.Ports.StopBits SelectedStopBit
        {
            get { return _serialOption.StopBits; }
            set
            {
                _serialOption.StopBits = value;
                NotifyOfPropertyChange(() => SelectedStopBit);
            }
        }

        /// <summary>
        /// Flag if connected to the serial port.
        /// </summary>
        private bool _IsConnected;
        /// <summary>
        /// Flag if connected to the serial port.
        /// </summary>
        public bool IsConnected
        {
            get { return _IsConnected; }
            set
            {
                _IsConnected = value;
                NotifyOfPropertyChange(() => IsConnected);

                // Change the serial connection
                ChangeSerialPortConnection(value);
            }
        }

        #endregion

        #region Receive Buffer

        /// <summary>
        /// ADCP Data buffer.
        /// </summary>
        private string _AdcpReceiveBuffer;
        /// <summary>
        /// ADCP Data buffer.
        /// </summary>
        public string AdcpReceiveBuffer
        {
            get { return _AdcpReceiveBuffer; }
            set
            {
                _AdcpReceiveBuffer = value;
                NotifyOfPropertyChange(() => AdcpReceiveBuffer);
            }
        }

        #endregion

        #region Command File

        /// <summary>
        /// Command Set from the file.
        /// </summary>
        private string _AdcpCommandSet;
        /// <summary>
        /// Command Set from the file.
        /// </summary>
        public string AdcpCommandSet
        {
            get { return _AdcpCommandSet; }
            set
            {
                _AdcpCommandSet = value;
                NotifyOfPropertyChange(() => AdcpCommandSet);
            }
        }

        #endregion

        #region Send Command

        /// <summary>
        /// New ADCP command to send.
        /// </summary>
        private string _NewAdcpCommand;
        /// <summary>
        /// New ADCP command to send.
        /// </summary>
        public string NewAdcpCommand
        {
            get { return _NewAdcpCommand; }
            set
            {
                _NewAdcpCommand = value;
                NotifyOfPropertyChange(() => NewAdcpCommand);
            }
        }

        

        #endregion

        #endregion

        /// <summary>
        /// Initialize the terminal.
        /// </summary>
        public TerminalAdcpViewModel()
        {
            // Init the settings
            Init();

            
        }

        #region Initialize

        /// <summary>
        /// Initialize the display.
        /// </summary>
        public void Init()
        {
            // Init the serial options
            _serialOption = new SerialOptions();
            _serialPort = null;

            _IsConnected = false;
            SelectedBaudRate = 115200;
            SelectedDataBit = 8;
            SelectedParity = System.IO.Ports.Parity.None;
            SelectedStopBit = System.IO.Ports.StopBits.One;
            CommPortList = SerialOptions.PortOptions;
            BaudRateList = SerialOptions.BaudRateOptions;
            DataBitList = SerialOptions.DataBitsOptions;
            ParityList = SerialOptions.ParityOptions;
            StopBitList = SerialOptions.StopBitsOptions;
            NotifyOfPropertyChange(() => CommPortList);
            NotifyOfPropertyChange(() => BaudRateList);
            NotifyOfPropertyChange(() => DataBitList);
            NotifyOfPropertyChange(() => ParityList);
            NotifyOfPropertyChange(() => StopBitList);
            NotifyOfPropertyChange(() => IsConnected);
        }

        #endregion

        #region Deactivate

        /// <summary>
        /// Call if closing the screen.
        /// </summary>
        /// <param name="close"></param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            // If shutting down, disconnect if still connected
            if(close)
            {
                Disconnect();
            }

        }

        #endregion

        /// <summary>
        /// Scan for a comm port.
        /// </summary>
        public void ScanCommPort()
        {
            CommPortList = SerialOptions.PortOptions;
            NotifyOfPropertyChange(() => CommPortList);
        }

        /// <summary>
        /// Connect of disconnect the serial port.
        /// </summary>
        /// <param name="isConnect"></param>
        public void ChangeSerialPortConnection(bool isConnect)
        {
            if(isConnect)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Connect the serial port.
        /// </summary>
        private void Connect()
        {
            // If still connected, disconnect
            if (_serialPort != null)
            {
                Disconnect();
            }

            // Connect and begin receiving data
            _serialPort = new AdcpSerialPort(_serialOption);
            _serialPort.Connect();
            _serialPort.ReceiveRawSerialDataEvent += _serialPort_ReceiveRawSerialDataEvent;
        }

        /// <summary>
        /// Disconnect the serial port.
        /// </summary>
        private void Disconnect()
        {
            if (_serialPort != null)
            {
                _serialPort.ReceiveRawSerialDataEvent -= _serialPort_ReceiveRawSerialDataEvent;
                _serialPort.Disconnect();
                _serialPort = null;
            }
        }

        #region Commands

        /// <summary>
        /// Send a BREAK.
        /// </summary>
        public void SendBreak()
        {
            if(_serialPort != null)
            {
                _serialPort.SendBreak();
            }
        }

        /// <summary>
        /// Start pinging.
        /// </summary>
        public void StartPinging()
        {
            if(_serialPort != null)
            {
                _serialPort.StartPinging(true);
            }
        }

        /// <summary>
        /// Start pinging.
        /// </summary>
        public void StopPinging()
        {
            if (_serialPort != null)
            {
                _serialPort.StopPinging();
            }
        }

        /// <summary>
        /// Send a command to the ADCP.
        /// </summary>
        public void SendCommand()
        {
            if (_serialPort != null)
            {
                _serialPort.SendData(_NewAdcpCommand);
            }
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Receive serial data.
        /// </summary>
        /// <param name="data"></param>
        private void _serialPort_ReceiveRawSerialDataEvent(byte[] data)
        {
            AdcpReceiveBuffer = _serialPort.ReceiveBufferString;
        }

        #endregion
    }
}
