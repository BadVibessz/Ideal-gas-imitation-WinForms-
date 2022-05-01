using System.Drawing;
using System.Windows.Forms;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;

namespace animationTry2
{
    public class RecordArguments
    {
        private string _fileName;
        private int _frameRate;
        private FourCC _codec;
        private int _quality;
        private Rectangle _rectangleToScreen;

        private Rectangle _clientRectangle;
        //private Point _location;

        public RecordArguments(string filename, int framerate, FourCC codec, int quality,
            Rectangle rectangleToScreen, Rectangle clientRectangle)
        {
            this._fileName = filename;
            this._frameRate = framerate;
            this._codec = codec;
            this._quality = quality;
            this._rectangleToScreen = rectangleToScreen;
            this._clientRectangle = clientRectangle;
            //this._location = location;
        }
        
        public int Width =>this._rectangleToScreen.Width;
        public int Height => this._rectangleToScreen.Height;
        public Rectangle RectangleToScreen => this._rectangleToScreen;

        public Rectangle ClientRectangle => this._clientRectangle;
        //public Point Location => this.Location;
        

        public AviWriter CreateAviWriter()
        {
            return new AviWriter(_fileName)
            {
                FramesPerSecond = _frameRate,
                EmitIndex1 = true //todo: what is it?
            };
        }

        public IAviVideoStream CreateVideoStream(AviWriter writer)
        {
            if (_codec == CodecIds.Uncompressed)
                return writer.AddUncompressedVideoStream(_rectangleToScreen.Width, _rectangleToScreen.Height);
            if (_codec == CodecIds.MotionJpeg)
                return writer.AddMJpegWpfVideoStream(_rectangleToScreen.Width, _rectangleToScreen.Height);

            return writer.AddMpeg4VcmVideoStream(_rectangleToScreen.Width, _rectangleToScreen.Height,
                _frameRate, quality: _quality, codec: _codec, forceSingleThreadedAccess: true);
        }
    }
}