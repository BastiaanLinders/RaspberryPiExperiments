using System;
using System.Device.I2c;
using System.Threading;

namespace DeviceServices.Drivers
{
	public class LcdDisplay : IDisposable
	{
		private readonly I2cDevice _i2CDevice;

		public LcdDisplay(int i2cBusId, int i2cDeviceAddress)
		{
			_i2CDevice = I2cDevice.Create(new I2cConnectionSettings(i2cBusId, i2cDeviceAddress));
		}

		public void Init()
		{
			Write(0x03);
			Write(0x03);
			Write(0x03);
			Write(0x02);

			Write((byte) (AllFlags.LCD_FUNCTIONSET | AllFlags.LCD_2LINE | AllFlags.LCD_5x8DOTS | AllFlags.LCD_4BITMODE));
			Write((byte) (AllFlags.LCD_DISPLAYCONTROL | AllFlags.LCD_DISPLAYON));
			Write((byte) AllFlags.LCD_CLEARDISPLAY);
			Write((byte) (AllFlags.LCD_ENTRYMODESET | AllFlags.LCD_ENTRYLEFT));

			Thread.Sleep(200);
		}

		private void Write(byte data, byte mode = 0)
		{
			WriteFourBites((byte) (mode | (data & 0xF0)));
			WriteFourBites((byte) (mode | ((data << 4) & 0xF0)));
		}


		private void WriteFourBites(byte bits)
		{
			_i2CDevice.WriteByte((byte) (bits | (byte) AllFlags.LCD_BACKLIGHT));
			Strobe(bits);
		}

		private void Strobe(byte bits)
		{
			_i2CDevice.WriteByte((byte) (bits | EnableBit | (byte) AllFlags.LCD_BACKLIGHT));
			Thread.Sleep(new TimeSpan(5000));
			_i2CDevice.WriteByte((byte) ((bits & ~EnableBit) | (byte) AllFlags.LCD_BACKLIGHT));
			Thread.Sleep(new TimeSpan(10000));
		}

		public void DisplayText(string text, int line)
		{
			switch (line)
			{
				case 1:
					Write(0x80);
					break;
				case 2:
					Write(0xC0);
					break;
				case 3:
					Write(0x94);
					break;
				case 4:
					Write(0xD4);
					break;
				default: throw new ArgumentOutOfRangeException();
			}

			foreach (char c in text)
			{
				Write((byte) c, RegisterSelectBit);
			}
		}

		public void Clear()
		{
			Write((byte) AllFlags.LCD_CLEARDISPLAY);
			Write((byte) AllFlags.LCD_RETURNHOME);
		}

		public void Dispose()
		{
			Clear();
			_i2CDevice.Dispose();
		}

		private const byte EnableBit = 1 << 2;
		private const byte ReadWriteBit = 1 << 1;
		private const byte RegisterSelectBit = 1;

		//[Flags]
		//private enum Commands : byte
		//{
		//	LCD_CLEARDISPLAY = 0x01,
		//	LCD_RETURNHOME = 0x02,
		//	LCD_ENTRYMODESET = 0x04,
		//	LCD_DISPLAYCONTROL = 0x08,
		//	LCD_CURSORSHIFT = 0x10,
		//	LCD_FUNCTIONSET = 0x20,
		//	LCD_SETCGRAMADDR = 0x40,
		//	LCD_SETDDRAMADDR = 0x80
		//}


		//[Flags]
		//private enum DisplayEntryMode : byte
		//{
		//	LCD_ENTRYRIGHT = 0x00,
		//	LCD_ENTRYLEFT = 0x02,
		//	LCD_ENTRYSHIFTINCREMENT = 0x01,
		//	LCD_ENTRYSHIFTDECREMENT = 0x00
		//}


		//[Flags]
		//private enum DisployControlFlags : byte
		//{
		//	LCD_DISPLAYON = 0x04,
		//	LCD_DISPLAYOFF = 0x00,
		//	LCD_CURSORON = 0x02,
		//	LCD_CURSOROFF = 0x00,
		//	LCD_BLINKON = 0x01,
		//	LCD_BLINKOFF = 0x00
		//}


		//[Flags]
		//private enum CursorShiftFlags : byte
		//{
		//	LCD_DISPLAYMOVE = 0x08,
		//	LCD_CURSORMOVE = 0x00,
		//	LCD_MOVERIGHT = 0x04,
		//	LCD_MOVELEFT = 0x00
		//}


		//[Flags]
		//private enum SetFlags : byte
		//{
		//	LCD_8BITMODE = 0x10,
		//	LCD_4BITMODE = 0x00,
		//	LCD_2LINE = 0x08,
		//	LCD_1LINE = 0x00,
		//	LCD_5x10DOTS = 0x04,
		//	LCD_5x8DOTS = 0x00
		//}


		//[Flags]
		//private enum BacklightControlFlages : byte
		//{
		//	LCD_BACKLIGHT = 0x08,
		//	LCD_NOBACKLIGHT = 0x00
		//}

		[Flags]
		private enum AllFlags : byte
		{
			// Commands
			LCD_CLEARDISPLAY = 0x01,
			LCD_RETURNHOME = 0x02,
			LCD_ENTRYMODESET = 0x04,
			LCD_DISPLAYCONTROL = 0x08,
			LCD_CURSORSHIFT = 0x10,
			LCD_FUNCTIONSET = 0x20,
			LCD_SETCGRAMADDR = 0x40,
			LCD_SETDDRAMADDR = 0x80,
			// DisplayEntryMode
			LCD_ENTRYRIGHT = 0x00,
			LCD_ENTRYLEFT = 0x02,
			LCD_ENTRYSHIFTINCREMENT = 0x01,
			LCD_ENTRYSHIFTDECREMENT = 0x00,
			// DisployControl
			LCD_DISPLAYON = 0x04,
			LCD_DISPLAYOFF = 0x00,
			LCD_CURSORON = 0x02,
			LCD_CURSOROFF = 0x00,
			LCD_BLINKON = 0x01,
			LCD_BLINKOFF = 0x00,
			// CursorShift
			CD_DISPLAYMOVE = 0x08,
			LCD_CURSORMOVE = 0x00,
			LCD_MOVERIGHT = 0x04,
			LCD_MOVELEFT = 0x00,
			// Set
			LCD_8BITMODE = 0x10,
			LCD_4BITMODE = 0x00,
			LCD_2LINE = 0x08,
			LCD_1LINE = 0x00,
			LCD_5x10DOTS = 0x04,
			LCD_5x8DOTS = 0x00,
			// BacklightControl
			LCD_BACKLIGHT = 0x08,
			LCD_NOBACKLIGHT = 0x00
		}
	}
}