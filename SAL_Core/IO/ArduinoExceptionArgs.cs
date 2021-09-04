using System;


namespace SAL_Core.IO
{
    public class ArduinoExceptionArgs
    {
        public Arduino Arduino { get; }
        public Exception Exception { get; }
        public ArduinoExceptionArgs(Arduino arduino, Exception exception)
        {
            Arduino = arduino;
            Exception = exception;
        }
    }
}
