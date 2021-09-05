using Arheisel.Log;
using System;
using System.IO.Ports;
using System.Threading;

namespace SAL_Core.IO.Connection
{
    class Serial : IConnection
    {
        private readonly string COM;

        private SerialPort serial;

        public Serial(string com)
        {
            COM = com;
            StartSerial();
        }

        private void StartSerial(bool retry = false, int retryCount = 0)
        {
            try
            {
                serial = new SerialPort
                {
                    PortName = COM,
                    BaudRate = 115200,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None,
                    //DtrEnable = true,

                    WriteTimeout = 100,
                    ReadTimeout = 200
                };

                serial.Open();
                return;
            }
            catch (TimeoutException)
            {
                if (retryCount < 10 && retry)
                {
                    serial.Close();
                    Thread.Sleep(250);
                    StartSerial(true, ++retryCount);
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        public void Close()
        {
            serial.Close();
            serial = null;
        }

        public void Send(byte[] data)
        {
            try
            {
                serial.Write(data, 0, data.Length);
            }
            catch (TimeoutException e)
            {
                Log.Exception(e);

                Log.Info("Attempting to reopen port");
                try
                {
                    Close();
                    Thread.Sleep(250);
                    StartSerial(true);
                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        public byte[] Receive(bool wait = false)
        {
            try
            {
                if (wait)
                {
                    while (serial.BytesToRead == 0) Thread.Sleep(10);
                }

                if (serial.ReadByte() == 252)
                {
                    var len = serial.ReadByte();
                    byte[] data = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        data[i] = (byte)serial.ReadByte();
                    }
                    return data;
                }
                else
                {
                    serial.DiscardInBuffer();
                    return new byte[0];
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
