using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using KeePass.UI;
using KeePassLib;

namespace KPEnhancedEntryView
{
	internal static class IconHelper
	{
		public static Icon GetIconForFileName(string filename)
		{
			try
			{
				var shFileInfo = new NativeMethods.SHFILEINFO();
				var result = NativeMethods.SHGetFileInfo(filename, NativeMethods.FILE_ATTRIBUTE_NORMAL, ref shFileInfo, (uint)Marshal.SizeOf(shFileInfo), NativeMethods.SHGFI.Icon | NativeMethods.SHGFI.SmallIcon | NativeMethods.SHGFI.UseFileAttributes);
				if (shFileInfo.hIcon != IntPtr.Zero)
				{
					var icon = (Icon)Icon.FromHandle(shFileInfo.hIcon).Clone();
					NativeMethods.DestroyIcon(shFileInfo.hIcon);
					return icon;
				}
			}
			catch (Exception)
			{
				// TODO: Non-windows platforms?
			}
			
			return null;
		}

        public static Image GetCustomIcon(PwDatabase database, PwUuid customIconId)
        {
	        return database.GetCustomIcon(customIconId, DpiUtil.ScaleIntX(16), DpiUtil.ScaleIntY(16));
		}
	}
}
