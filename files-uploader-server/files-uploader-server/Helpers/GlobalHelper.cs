using System;

namespace files_uploader_server.Helpers
{
    public static class GlobalHelper
    {
        /// <summary>
        /// File size limit constant
        /// </summary>
        public const long RequestLimit = 10L * 1024L * 1024L * 1024L;

        /// <summary>
        /// Convert MByte to Byte
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double ToBytes(this int bytes)
        {
            return bytes * 1024 * 1024;
        }

        /// <summary>
        /// Convert Byte to MByte
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double ToMBytes(this long bytes)
        {
            return Convert.ToDouble(bytes) / 1024 / 1024;
        }
    }
}
