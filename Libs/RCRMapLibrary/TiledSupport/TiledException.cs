using System;

namespace RCRMapLibrary.TiledSupport
{
    public class TiledException : Exception
    {
        /// <summary>
        /// Returns an Instance of the TiledException
        /// </summary>
        /// <param name="message">the exception message</param>
        public TiledException(string message): base(message){}
        /// <summary>
        /// Returns an Instance of the TiledException
        /// </summary>
        /// <param name="message">the exception message</param>
        /// <param name="inner">The inner Exception</param>
        public TiledException(string message, Exception inner) : base(message, inner){}
    }
}