using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAvi.Output;

namespace animationTry2
{
    public class ControlRecorder : IDisposable
    {
        private AviWriter _writer;
        private RecordArguments _args;
        private IAviVideoStream _videoStream;
        private Thread _thread;
        private ManualResetEvent _stopThread = new ManualResetEvent(false);
        private Point _location;

        public ControlRecorder(RecordArguments args, Point location)
        {
            this._args = args;
            this._location = location;

            this._writer = args.CreateAviWriter();
            this._videoStream = args.CreateVideoStream(_writer);
            this._videoStream.Name = "ScreenRecorder";

            this._thread = new Thread(Record)
            {
                Name = typeof(ControlRecorder).Name + ".Record",
                IsBackground = true
            };
            this._thread.Start();
        }

        public void Record()
        {
            var frameInterval = TimeSpan.FromSeconds(1 / (double)_writer.FramesPerSecond);
            var buffer = new byte[_args.Width * _args.Height * 4];
            Task videoWriteTask = null;
            var timeTillNextFrame = TimeSpan.Zero;

            while (!_stopThread.WaitOne(timeTillNextFrame)) //todo: same as thread.sleep?
            {
                var timestamp = DateTime.Now;

                Screenshot(buffer);

                videoWriteTask?.Wait();

                videoWriteTask = _videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);

                timeTillNextFrame = timestamp + frameInterval - DateTime.Now;
                if (timeTillNextFrame < TimeSpan.Zero)
                    timeTillNextFrame = TimeSpan.Zero;
            }

            videoWriteTask?.Wait();
        }

        public void Screenshot(byte[] buffer)
        {
            using (var bm = new Bitmap(_args.Width, _args.Height))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    var r = _args.ClientRectangle;
                    g.CopyFromScreen(_location, _location, new Size(r.Width, r.Height));
                    g.Flush();

                    var bits = bm.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                    bm.UnlockBits(bits);
                }
                
            }
        }

        public void Dispose()
        {
            this._stopThread.Set();
            this._thread.Join();

            this._writer.Close();
            _stopThread.Dispose();
        }
    }
}