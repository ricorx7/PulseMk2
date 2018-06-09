using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTI
{
    class TerminalAdcpViewModel : Caliburn.Micro.Screen, IDeactivate
    {
        #region Variables

        /// <summary>
        ///  Setup logger
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// ADCP Serial Port.
        /// </summary>
        private AdcpSerialPort _serialPort;

        /// <summary>
        /// Serial Port options.
        /// </summary>
        private SerialOptions _serialOption;

        /// <summary>
        /// Timer to check the serial port buffer for new data.
        /// </summary>
        private System.Timers.Timer _displayTimer;

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
        /// ADCP Data buffer.  A timer will periodically
        /// refresh this value to view the latest data.
        /// </summary>
        public string AdcpReceiveBuffer
        {
            get
            {
                if(_serialPort != null)
                {
                    return _serialPort.ReceiveBufferString;
                }

                return "";
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

        #region ADCP Send Commands

        /// <summary>
        /// History of all the previous ADCP commands.
        /// </summary>
        private ObservableCollection<string> _AdcpCommandHistory;
        /// <summary>
        /// History of all the previous ADCP commands.
        /// </summary>
        public IEnumerable AdcpCommandHistory
        {
            get { return _AdcpCommandHistory; }
        }

        /// <summary>
        /// Command currently selected.
        /// </summary>
        private string _SelectedAdcpCommand;
        /// <summary>
        /// Command currently selected.
        /// </summary>
        public string SelectedAdcpCommand
        {
            get { return _SelectedAdcpCommand; }
            set
            {
                _SelectedAdcpCommand = value;
                this.NotifyOfPropertyChange(() => this.SelectedAdcpCommand);
                this.NotifyOfPropertyChange(() => this.NewAdcpCommand);
            }
        }

        /// <summary>
        /// New command entered by the user.
        /// This will be called when the user enters
        /// in a new command to send to the ADCP.
        /// It will update the list and set the SelectedCommand.
        /// </summary>
        public string NewAdcpCommand
        {
            get { return _SelectedAdcpCommand; }
            set
            {
                //if (_SelectedAdcpCommand != null)
                //{
                //    return;
                //}
                if (!string.IsNullOrEmpty(value))
                {
                    _AdcpCommandHistory.Insert(0, value);
                    SelectedAdcpCommand = value;
                }
            }
        }


        #endregion

        #region Status Message


        /// <summary>
        /// Status message.
        /// </summary>
        private string _StatusMsg;
        /// <summary>
        /// Status message.
        /// </summary>
        public string StatusMsg
        {
            get { return _StatusMsg; }
            set
            {
                _StatusMsg = value;
                NotifyOfPropertyChange(() => StatusMsg);
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

            // Update the display
            _displayTimer = new System.Timers.Timer(1000);
            _displayTimer.Elapsed += _displayTimer_Elapsed;
            _displayTimer.AutoReset = true;
            _displayTimer.Enabled = true;
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

            _AdcpCommandHistory = new ObservableCollection<string>();

            StatusMsg = "";

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

            // Stop timer
            _displayTimer.Close();

        }

        #endregion

        #region Serial connection

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

            if(_serialPort.IsAvailable())
            {
                DisplaySerialPortStatusMsg(string.Format("Connected to ADCP on {0} - {1}", _serialOption.Port, _serialOption.BaudRate));
            }
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

            // Set status message
            DisplaySerialPortStatusMsg(string.Format("Disconnect from ADCP on {0} - {1}", _serialOption.Port, _serialOption.BaudRate));
        }

        #endregion

        #region Commands

        /// <summary>
        /// Send a BREAK.
        /// </summary>
        public void SendBreak()
        {
            if(_serialPort != null && _serialPort.IsAvailable())
            {
                Task.Run(() => _serialPort.SendBreak());
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Start pinging.
        /// </summary>
        public void StartPinging()
        {
            if(_serialPort != null && _serialPort.IsAvailable())
            {
                Task.Run(() => _serialPort.StartPinging(true));
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Start pinging.
        /// </summary>
        public void StopPinging()
        {
            if (_serialPort != null && _serialPort.IsAvailable())
            {
                Task.Run(() => _serialPort.StopPinging());
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Send CSHOW command.
        /// </summary>
        public void CshowCommand()
        {
            SendCommand("CSHOW");
        }

        /// <summary>
        /// Send SLEEP command.
        /// </summary>
        public void SleepCommand()
        {
            SendCommand("SLEEP");
        }

        /// <summary>
        /// Send CEMAC command.
        /// </summary>
        public void CemacCommand()
        {
            SendCommand("CEMAC");
        }

        /// <summary>
        /// Send STIME command.
        /// </summary>
        public void SetTimeCommand()
        {
            if(_serialPort != null && _serialPort.IsAvailable())
            {
                _serialPort.SetLocalSystemDateTime();
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Send CPZ command.
        /// </summary>
        public void CpzCommand()
        {
            SendCommand("CPZ");
        }

        /// <summary>
        /// Send compass connect command.
        /// </summary>
        public void CompassConnectCommand()
        {
            SendCommand(RTI.Commands.AdcpCommands.CMD_DIAGCPT);
        }

        /// <summary>
        /// Send compass disconnect command.
        /// </summary>
        public void CompassDisconnectCommand()
        {
            SendCommand(RTI.Commands.AdcpCommands.CMD_DIAGCPT_DISCONNECT);
        }

        /// <summary>
        /// Send Force BREAK command.
        /// </summary>
        public void ForceBreakCommand()
        {
            if (_serialPort != null && _serialPort.IsAvailable())
            {
                _serialPort.SendForceBreak();
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Send the command to the ADCP.
        /// </summary>
        /// <param name="cmd"></param>
        public void SendCommand(string cmd)
        {
            if(_serialPort != null && _serialPort.IsAvailable())
            {
                Task.Run(() => _serialPort.SendDataWaitReply(cmd));
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Send a command to the ADCP.
        /// </summary>
        public void SendACommand()
        {
            if (_serialPort != null && _serialPort.IsAvailable())
            {
                Task.Run(() => _serialPort.SendDataWaitReply(SelectedAdcpCommand));
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Clear the ADCP serial port Receive buffer.
        /// </summary>
        public void ClearReceiveBuffer()
        {
            if (_serialPort != null)
            {
                Task.Run(() =>
                {
                    _serialPort.ReceiveBufferString = "";
                    NotifyOfPropertyChange(() => this.AdcpReceiveBuffer);
                });
            }
        }

        public void ClearCommandSet()
        {
            Task.Run(() =>
            {
                AdcpCommandSet = "";
            });
        }

        /// <summary>
        /// Display a connection error message.
        /// </summary>
        private void DisplayConnectionError()
        {
            DisplaySerialPortStatusMsg(string.Format("ADCP COULD NOT CONNECT: Port: {0}, Baud: {1}", _serialOption.Port, _serialOption.BaudRate));
        }

        /// <summary>
        /// Display a serial port error message.
        /// </summary>
        /// <param name="error">Error message.</param>
        private void DisplaySerialPortStatusMsg(string error)
        {
            //if (_serialPort != null)
            //{
            //    _serialPort.ReceiveBufferString = "";
            //    _serialPort.ReceiveBufferString = error;
            //    NotifyOfPropertyChange(() => this.AdcpReceiveBuffer);
            //}
            StatusMsg = error;
        }


        #endregion

        #region Command Set

        /// <summary>
        /// Send the list of commands to the ADCP.
        /// </summary>
        public void SendCommandSetCommand()
        {
            if (_serialPort != null && _serialPort.IsAvailable())
            {
                Task.Run(() =>
                {

                    // Check if a conneciton could be made
                    if (_serialPort == null || !_serialPort.IsAvailable())
                    {
                        DisplayConnectionError();
                    }
                    else
                    {
                        // Verify there are any commands
                        if (!string.IsNullOrEmpty(_AdcpCommandSet))
                        {
                            string[] result = _AdcpCommandSet.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            // Remove all line feed, carrage returns, new lines and tabs
                            for (int x = 0; x < result.Length; x++)
                            {
                                result[x] = result[x].Replace("\n", String.Empty);
                                result[x] = result[x].Replace("\r", String.Empty);
                                result[x] = result[x].Replace("\t", String.Empty);
                            }

                            _serialPort.SendCommands(result.ToList());
                        }
                    }
                });
            }
            else
            {
                DisplayConnectionError();
            }
        }

        /// <summary>
        /// Import a command set from a file.
        /// </summary>
        public void ImportCommandSetCommand()
        {
            string fileName = "";
            try
            {
                // Show the FolderBrowserDialog.
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "All files (*.*)|*.*";
                dialog.Multiselect = false;

                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Get the files selected
                    fileName = dialog.FileName;

                    // Set the command set
                    AdcpCommandSet = File.ReadAllText(fileName);
                }
            }
            catch (Exception e)
            {
                log.Error(string.Format("Error reading command set from {0}", fileName), e);
            }
        }

        #endregion

        #region Timer

        /// <summary>
        /// Reduce the number of times the display is updated.
        /// This will update the display based off the timer and not
        /// based off when data is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _displayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.NotifyOfPropertyChange(() => this.AdcpReceiveBuffer);

            // Recording
            //this.NotifyOfPropertyChange(() => this.IsRawAdcpRecording);
            //this.NotifyOfPropertyChange(() => this.RawAdcpBytesWritten);
            //this.NotifyOfPropertyChange(() => this.RawAdcpFileName);
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Receive serial data.
        /// </summary>
        /// <param name="data"></param>
        private void _serialPort_ReceiveRawSerialDataEvent(byte[] data)
        {
            //AdcpReceiveBuffer = _serialPort.ReceiveBufferString;
        }

        #endregion
    }
}
