namespace MriBase.Models.EventArgs
{
    public class BytesReceivedEventArgs
    {
        public byte[] Bytes { get; }

        public BytesReceivedEventArgs(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}
