using Companion.Sender.Abstractions;

using Device.Net;

using Hid.Net;
using Hid.Net.Windows;

namespace Companion.Sender.Usb
{
    internal enum Commands : byte
    {
        PutData = 0x01,
        Display = 0x02,
        RebootToBootloader = 0xFF
    }

    public class UsbSender : ISender
    {
        private readonly IDeviceFactory _hidFactory;

        private readonly object _writeLock = new();

        public UsbSender()
        {
            _hidFactory = new FilterDeviceDefinition(vendorId: 0xCAFE, productId: 0x4004).CreateWindowsHidDeviceFactory();
        }

        public async Task SendBitmapAsync(IBitmap bitmap)
        {
            IHidDevice? hidDevice = null;
            try
            {
                var device = await _hidFactory.GetFirstDeviceAsync().ConfigureAwait(false);
                if (device == null)
                {
                    return;
                }

                hidDevice = (IHidDevice)device;
                await hidDevice.InitializeAsync().ConfigureAwait(false);
                await InnerSendBitmapAsync(hidDevice, bitmap.GetBits()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (hidDevice != null && hidDevice.IsInitialized)
                {
                    hidDevice.Dispose();
                }
            }
        }

        private static async Task InnerSendBitmapAsync(IHidDevice device, byte[] image)
        {
            var buffer = new byte[65];

            var currentOffset = (short)0;
            var imageBuffer = buffer.AsMemory(4, 48).Slice(0);

            while (currentOffset < image.Length)
            {
                var reportOffset = currentOffset;
                var i = (byte)0;
                for (i = 0; i < 48; i++)
                {
                    if (currentOffset == image.Length)
                    {
                        break;
                    }
                    imageBuffer.Span[i] = image[currentOffset];
                    currentOffset++;
                }

                var indexArr = BitConverter.GetBytes(reportOffset);
                buffer[0] = (byte)Commands.PutData;
                buffer[1] = indexArr[0];
                buffer[2] = indexArr[1];
                buffer[3] = i;

                await device.WriteReportAsync(buffer, 0x00);
            }

            buffer[0] = (byte)Commands.Display;

            await device.WriteReportAsync(buffer, 0x00);
        }
    }
}