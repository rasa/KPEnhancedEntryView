using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightIdeasSoftware;
using System.Drawing;
using System.Windows.Forms;
using KeePassLib.Collections;
using KeePassLib.Security;

namespace KPEnhancedEntryView
{
	internal class AttachmentsListView : FastObjectListView
	{
		public AttachmentsListView()
		{
			SmallImageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };

			View = System.Windows.Forms.View.SmallIcon;
			Scrollable = true;

			var overlay = (TextOverlay)EmptyListMsgOverlay;
			overlay.Alignment = ContentAlignment.TopLeft;
			overlay.InsetX = 0;
			overlay.InsetY = 0;
			overlay.BackColor = Color.Empty;
			overlay.BorderColor = Color.Empty;
			overlay.TextColor = SystemColors.GrayText;

			var column = new OLVColumn { FillsFreeSpace = true };
			Columns.Add(column);

			column.AspectGetter = NameGetter;
			column.ImageGetter += IconImageGetter;
		}

		private object NameGetter(object model)
		{
			var kvp = (KeyValuePair<string, ProtectedBinary>)model;
			return kvp.Key;
		}

		private object IconImageGetter(object model)
		{
			var kvp = (KeyValuePair<string, ProtectedBinary>)model;
			var path = kvp.Key;
			var extension = System.IO.Path.GetExtension(path);

			if (!SmallImageList.Images.ContainsKey(extension))
			{
				var icon = IconHelper.GetIconForFileName(path);
				SmallImageList.Images.Add(icon);
			}

			return SmallImageList.Images.Count - 1;
		}

		private ProtectedBinaryDictionary mBinaries;
		public ProtectedBinaryDictionary Binaries
		{
			get { return mBinaries; }
			set
			{
				mBinaries = value;
				OnBinariesChanged(EventArgs.Empty);
			}
		}

		protected virtual void OnBinariesChanged(EventArgs e)
		{
			if (Binaries == null)
			{
				ClearObjects();
			}
			else
			{
				SetObjects(Binaries);
			}
		}
	}
}
