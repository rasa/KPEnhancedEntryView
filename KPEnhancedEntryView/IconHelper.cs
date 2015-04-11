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

		private static Func<PwDatabase, PwUuid, Image> sGetCustomIconInternal;
        public static Image GetCustomIcon(PwDatabase database, PwUuid customIconId)
		{
	        if (sGetCustomIconInternal == null)
	        {
				// Attempt to use the new (2.29 and above) methods to get a natively larger icon, rather than rescaling
		        var getCustomIconMethod = database.GetType().GetMethod("GetCustomIcon", new[] { typeof(PwUuid), typeof(int), typeof(int) });
		        if (getCustomIconMethod != null)
		        {
			        sGetCustomIconInternal = (db, id) => getCustomIconMethod.Invoke(db, new object[] { id, DpiUtil.ScaleIntX(16), DpiUtil.ScaleIntY(16) }) as Image;
		        }
		        else
		        {
					// Pre- 2.29 method
					sGetCustomIconInternal = (db, id) => DpiUtil.ScaleImage(db.GetCustomIcon(id), false);
				}
			}
	        return sGetCustomIconInternal(database, customIconId);
		}
	}
}
