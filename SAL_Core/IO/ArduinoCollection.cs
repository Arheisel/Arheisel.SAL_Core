using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arheisel.Log;
using SAL_Core.Extensions;
using SAL_Core.RGB;

namespace SAL_Core.IO
{
    public class ArduinoCollection : IDisposable, IList<Arduino>
    {
        private readonly List<Arduino> collection = new List<Arduino>();
        private readonly ConcurrentQueue<ChColor> queue;
        private readonly Thread thread;

        public int ChannelCount { get; private set; } = 0;

        public bool Enabled { get; set; } = true;

        public event EventHandler<ArduinoExceptionArgs> OnError;

        public ArduinoCollection()
        {
            try
            {
                thread = new Thread(new ThreadStart(Worker));
                queue = new ConcurrentQueue<ChColor>();
                thread.Start();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void Worker()
        {
            while (true)
            {
                try
                {
                    while (queue.TryDequeue(out ChColor chColor))
                    {
                        if (chColor.Colors != null)
                        {
                            int i = 0;
                            foreach (Arduino arduino in collection)
                            {
                                if (arduino.Online)
                                {
                                    arduino.SetColor(chColor.Colors.Splice(i, arduino.Channels));
                                }
                                i += arduino.Channels;
                            }
                        }
                        else
                        {
                            int channel = chColor.Channel;
                            if (channel == 0)
                            {
                                foreach (Arduino arduino in collection)
                                {
                                    if (arduino.Online)
                                    {
                                        arduino.SetColor(chColor.Color);
                                    }
                                }
                            }
                            else
                            {
                                foreach (Arduino arduino in collection)
                                {
                                    if (channel - arduino.Channels <= 0)
                                    {
                                        if (arduino.Online)
                                        {
                                            arduino.SetColor(channel, chColor.Color);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        channel -= arduino.Channels;
                                    }
                                }
                            }
                        }
                    }
                    Parallel.ForEach(collection, arduino =>
                    {
                        try
                        {
                            arduino.Show();
                        }
                        catch (Exception e)
                        {
                            OnError?.Invoke(this, new ArduinoExceptionArgs(arduino, e));
                            throw;
                        }
                    });
                    Thread.Sleep(20);
                }
                catch (Exception e)
                {
                    Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
                }
            }
        }


        public void TurnOff()
        {
            Enabled = false;
            while (!queue.IsEmpty) queue.TryDequeue(out _);

            foreach (Arduino arduino in collection)
            {
                if (arduino.Online)
                {
                    try
                    {
                        arduino.SetColor(Colors.OFF);
                        arduino.Show();
                    }
                    catch (Exception e)
                    {
                        Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
                    }
                }
            }

            Thread.Sleep(50);
            Enabled = true;
        }

        public Arduino this[int index]
        {
            get
            {
                return collection[index];
            }
            set
            {
                collection[index] = value;
            }
        }

        public Arduino IndexOf(string name)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Name == name) return collection[i];
            }
            return null;
        }

        public int IndexOf(Arduino arduino)
        {
            return collection.IndexOf(arduino);
        }

        public bool Contains(string name)
        {
            return IndexOf(name) != null;
        }

        public bool Contains(Arduino arduino)
        {
            return collection.Contains(arduino);
        }

        public void Add(Arduino arduino)
        {
            try
            {
                if (Contains(arduino)) return;
                collection.Add(arduino);
                CalculateChannels();
                arduino.OnError += Arduino_OnError;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void Arduino_OnError(object sender, ArduinoExceptionArgs e)
        {
            CalculateChannels();
            OnError?.Invoke(this, new ArduinoExceptionArgs(e.Arduino, e.Exception));
        }

        public void Insert(int i, Arduino arduino)
        {
            try
            {
                if (Contains(arduino)) return;
                collection.Insert(i, arduino);
                ChannelCount += arduino.Channels;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public void ShiftUp(Arduino arduino)
        {
            collection.Shift(arduino, -1);
        }

        public void ShiftDown(Arduino arduino)
        {
            collection.Shift(arduino, 1);
        }

        public bool Remove(Arduino arduino)
        {
            if (Contains(arduino))
            {
                collection.Remove(arduino);
                arduino.Dispose();
                CalculateChannels();
                return true;
            }
            return false;
        }

        public void RemoveAt(int i)
        {
            if (i < collection.Count)
            {
                var arduino = collection[i];
                collection.RemoveAt(i);
                arduino.Dispose();
                CalculateChannels();
            }
        }

        public void Clear()
        {
            foreach (Arduino arduino in collection) Remove(arduino);
        }

        private void CalculateChannels()
        {
            var count = 0;
            foreach (var arduino in collection)
            {
                if (arduino.Online) count += arduino.Channels;
            }
            ChannelCount = count;
        }

        public int Count
        {
            get
            {
                return collection.Count;
            }
        }

        public void SetColor(Color color)
        {
            if (!Enabled) return;

            queue.Enqueue(new ChColor(0, color));
        }

        public void SetColor(int channel, Color color)
        {
            if (!Enabled) return;
            if (channel >= 0 && channel <= ChannelCount)
            {
                queue.Enqueue(new ChColor(channel, color));
            }
        }

        public void SetColor(Color[] colors)
        {
            if (!Enabled) return;
            if (colors.Length != ChannelCount) return;
            queue.Enqueue(new ChColor(colors));
        }

        public int Multiplier
        {
            get
            {
                if (ChannelCount <= 1) return 12;
                else if (ChannelCount == 2) return 6;
                else if (ChannelCount == 3) return 4;
                else if (ChannelCount == 4) return 3;
                else if (ChannelCount >= 5 && ChannelCount <= 8) return 2;
                else return 1;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<Arduino> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        public void CopyTo(Arduino[] array)
        {
            collection.CopyTo(array);
        }

        public void CopyTo(Arduino[] array, int i)
        {
            collection.CopyTo(array, i);
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                thread.Abort();
                foreach (Arduino arduino in collection) arduino.Dispose();
            }

            _disposed = true;
        }

    }
}
