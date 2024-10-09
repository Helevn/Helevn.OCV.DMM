namespace Helevn.OCV.DMM
{
    public class DefaultCmd
    {
        /// <summary>
        /// DMM7510 读取命令字
        /// </summary>
        public static byte[] ReadOcvCmd => [0x52, 0x65, 0x61, 0x64, 0x3F, 0x0A];
    }
}
