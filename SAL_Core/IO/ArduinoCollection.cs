using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arheisel.Log;
using SAL_Core.Extensions;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.IO
{
    public class ArduinoCollection : IDisposable, IList<Arduino>, IChannelGroup
    {
        private readonly List<Arduino> collection = new List<Arduino>();
        private readonly ConcurrentQueue<ChColor> queue;
        private readonly Thread thread;

        public readonly ArduinoCollectionSettings Settings;
        public int ChannelCount { get; private set; } = 0;
        public int Multiplier { get; private set; } = 1;
        public bool Enabled { get; private set; } = false;
        public ArduinoGroups Groups { get; }

        public event EventHandler<ArduinoExceptionArgs> OnError;
        public event EventHandler OnChannelCountChange;

        public ArduinoCollection(ArduinoCollectionSettings settings)
        {
            try
            {
                Settings = settings;
                thread = new Thread(new ThreadStart(Worker)) { IsBackground = true };
                queue = new ConcurrentQueue<ChColor>();
                Groups = new ArduinoGroups(this);
                thread.Start();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public void InitializeArduinos()
        {
            foreach(var settings in Settings.Arduinos)
            {
                var arduino = new Arduino(settings, true);
                if (Contains(arduino))
                    Settings.Arduinos.Remove(settings);
                else
                    collection.Add(arduino);
            }
            CalculateChannels();
            Enabled = true;
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
                            if(chColor.Channel == 0)
                            {
                                int i = 0;
                                foreach (Arduino arduino in collection)
                                {
                                    if (arduino.Online) arduino.SetColor(chColor.Colors.Splice(i, arduino.Channels));
                                    i += arduino.Channels;
                                }
                            }
                            else
                            {
                                int channel = chColor.Channel;
                                int i = 0;
                                foreach (Arduino arduino in collection)
                                {
                                    if (channel - arduino.Channels <= 0)
                                    {
                                        int len = chColor.Colors.Length - i < arduino.Channels - channel + 1 ? chColor.Colors.Length - i : arduino.Channels - channel + 1;
                                        if (arduino.Online) arduino.SetColor(chColor.Colors.Splice(i, len), channel);
                                        channel = 1;
                                        i += len;
                                        if (i >= chColor.Colors.Length) break;
                                    }
                                    else
                                    {
                                        channel -= arduino.Channels;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int channel = chColor.Channel;
                            if (channel == 0)
                            {
                                foreach (Arduino arduino in collection)
                                {
                                    if (arduino.Online) arduino.SetColor(chColor.Color);
                                }
                            }
                            else
                            {
                                foreach (Arduino arduino in collection)
                                {
                                    if (channel - arduino.Channels <= 0)
                                    {
                                        if (arduino.Online) arduino.SetColor(channel, chColor.Color);
                                        break;
                                    }
                                    channel -= arduino.Channels;
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
                            CalculateChannels();
                            OnError?.Invoke(this, new ArduinoExceptionArgs(arduino, e));
                            Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + arduino.Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
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

            Task.Delay(50).Wait();
            Enabled = true;
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

        public void SetColor(Color[] colors, int start)
        {
            if (!Enabled) return;
            if (start < 1 || start + colors.Length - 1 > ChannelCount) return;
            queue.Enqueue(new ChColor(colors, start));
        }

        public void Add(Arduino arduino)
        {
            try
            {
                if (Contains(arduino)) return;
                collection.Add(arduino);
                Settings.Arduinos.Add(arduino.Settings);
                CalculateChannels();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
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
                CalculateChannels();
            }
        }

        public int IndexOf(Arduino arduino)
        {
            return collection.IndexOf(arduino);
        }

        public bool Contains(Arduino arduino)
        {
            return collection.Contains(arduino);
        }

        public void Insert(int i, Arduino arduino)
        {
            try
            {
                if (Contains(arduino)) return;
                collection.Insert(i, arduino);
                Settings.Arduinos.Insert(i, arduino.Settings);
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
            Settings.Arduinos.Shift(arduino.Settings, -1);
        }

        public void ShiftDown(Arduino arduino)
        {
            collection.Shift(arduino, 1);
            Settings.Arduinos.Shift(arduino.Settings, 1);
        }

        public bool Remove(Arduino arduino)
        {
            if (Contains(arduino))
            {
                collection.Remove(arduino);
                Settings.Arduinos.Remove(arduino.Settings);
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
                Remove(arduino);
                Settings.Arduinos.Remove(arduino.Settings);
                arduino.Dispose();
                CalculateChannels();
            }
        }

        public void Clear()
        {
            foreach (Arduino arduino in collection)
            {
                arduino.Dispose();
                Remove(arduino);
            }
            Settings.Arduinos.Clear();
            ChannelCount = 0;
        }

        private void CalculateChannels()
        {
            var count = 0;
            foreach (var arduino in collection)
            {
                if (arduino.Online) count += arduino.Channels;
            }
            ChannelCount = count;

            if (ChannelCount <= 1) Multiplier = 12;
            else if (ChannelCount == 2) Multiplier = 6;
            else if (ChannelCount == 3) Multiplier = 4;
            else if (ChannelCount == 4) Multiplier = 3;
            else if (ChannelCount >= 5 && ChannelCount <= 8) Multiplier = 2;
            else Multiplier = 1;

            OnChannelCountChange?.Invoke(this, EventArgs.Empty);
        }

        public int Count
        {
            get
            {
                return collection.Count;
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

        public List<Arduino> ToList()
        {
            return new List<Arduino>(collection);
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
