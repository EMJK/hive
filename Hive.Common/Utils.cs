namespace Hive.Common
{
    public static class Utils
    {
        public static void WriteUInt32(byte[] array, int number, int offset)
        {
            WriteUInt32(array, (uint)number, offset);
        }

        public static void WriteUInt32(byte[] array, uint number, int offset)
        {
            array[offset + 0] = (byte)((number >> (8 * 3)) & 0xFF);
            array[offset + 1] = (byte)((number >> (8 * 2)) & 0xFF);
            array[offset + 2] = (byte)((number >> (8 * 1)) & 0xFF);
            array[offset + 3] = (byte)((number >> (8 * 0)) & 0xFF);
        }

        public static int ReadUInt32(byte[] array, int offset)
        {
            uint number = 0;
            number |= ((uint) array[offset + 0]) << 8*3;
            number |= ((uint) array[offset + 1]) << 8*2;
            number |= ((uint) array[offset + 2]) << 8*1;
            number |= ((uint) array[offset + 3]) << 8*0;
            return (int) number;
        }
    }
}
