using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightIdeasSoftware;
using System.Drawing;
using System.Windows.Forms;
using KeePassLib.Collections;
using KeePassLib.Security;
using KeePass.Util;
using KeePass.Forms;
using KeePassLib;
using KeePass.UI;
using System.Diagnostics;
using KeePassLib.Utility;
using KeePass.Resources;
using System.IO;
using System.Reflection;
using Delay;

namespace KPEnhancedEntryView
{
	internal class AttachmentsListView : FastObjectListView
	{
		public AttachmentsListView()
		{
			SmallImageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };

			View = System.Windows.Forms.View.SmallIcon;
			Scrollable = true;

			DragSource = new VirtualFileDragSource();
			AllowDrop = true;

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

		#region Getters
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
		#endregion

		#region Entry
		private PwEntry mEntry;
		public PwEntry Entry 
		{
			get { return mEntry; }
			set
			{
				mEntry = value;
				OnEntryChanged(EventArgs.Empty);
			}
		}

		protected virtual void OnEntryChanged(EventArgs e)
		{
			if (Entry == null)
			{
				ClearObjects();
			}
			else
			{
				SetObjects(Entry.Binaries);
			}
		}
		#endregion

		#region EntryModified event
		public event EventHandler EntryModified;
		protected virtual void OnEntryModified(EventArgs e)
		{
			if (Entry != null)
			{
				Entry.Touch(true, false);
			}

			var temp = EntryModified;
			if (temp != null)
			{
				temp(this, e);
			}
		}
		#endregion

		#region Edit/View
		protected override void OnItemActivate(EventArgs e)
		{
			base.OnItemActivate(e);

			var kvp = (KeyValuePair<string, ProtectedBinary>)SelectedObject;

			ShowBinaryWindow(kvp.Key, kvp.Value);
		}

		private void ShowBinaryWindow(string name, ProtectedBinary binary)
		{
			var data = binary.ReadData();
			var dataClass = BinaryDataClassifier.Classify(name, data);

			if (DataEditorForm.SupportsDataType(dataClass))
			{
				var editor = new DataEditorForm();
				editor.InitEx(name, data);
				editor.ShowDialog();

				if (editor.EditedBinaryData != null)
				{
					Entry.Binaries.Set(name, new ProtectedBinary(binary.IsProtected, editor.EditedBinaryData));
					OnEntryModified(EventArgs.Empty);
				}

				UIUtil.DestroyForm(editor);
			}
			else
			{
				var viewer = new DataViewerForm();
				viewer.InitEx(name, data);
				UIUtil.ShowDialogAndDestroy(viewer);
			}
		}
		#endregion

		#region Drop
		protected override void OnDragOver(DragEventArgs args)
		{
			base.OnDragOver(args);

			if (Entry != null && args.Data.GetDataPresent(DataFormats.FileDrop))
			{
				args.Effect = DragDropEffects.Copy;
			}
			else
			{
				args.Effect = DragDropEffects.None;
			}
		}
		protected override void OnDragDrop(DragEventArgs args)
		{
			base.OnDragDrop(args);

			if (args.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);

				FindForm().Activate(); // Activate the parent form so that the message boxes and popup dialogs show up in the right places
				BinImportFiles(files);
				OnEntryModified(EventArgs.Empty);
			}
		}

		// Copied from PwEntryForm.BinImportFiles (PwEntryForm.cs), as the functionality isn't otherwise exposed.
		private void BinImportFiles(string[] vPaths)
		{
			var m_vBinaries = Entry.Binaries; // Allow copied code to refer directly to entry binaries

			if (vPaths == null) { Debug.Assert(false); return; }

			//UpdateEntryBinaries(true, false);

			foreach (string strFile in vPaths)
			{
				if (string.IsNullOrEmpty(strFile)) { Debug.Assert(false); continue; }

				byte[] vBytes = null;
				string strMsg, strItem = UrlUtil.GetFileName(strFile);

				if (m_vBinaries.Get(strItem) != null)
				{
					strMsg = KPRes.AttachedExistsAlready + MessageService.NewLine +
						strItem + MessageService.NewParagraph + KPRes.AttachNewRename +
						MessageService.NewParagraph + KPRes.AttachNewRenameRemarks0 +
						MessageService.NewLine + KPRes.AttachNewRenameRemarks1 +
						MessageService.NewLine + KPRes.AttachNewRenameRemarks2;

					DialogResult dr = MessageService.Ask(strMsg, null,
						MessageBoxButtons.YesNoCancel);

					if (dr == DialogResult.Cancel) continue;
					else if (dr == DialogResult.Yes)
					{
						string strFileName = UrlUtil.StripExtension(strItem);
						string strExtension = "." + UrlUtil.GetExtension(strItem);

						int nTry = 0;
						while (true)
						{
							string strNewName = strFileName + nTry.ToString() + strExtension;
							if (m_vBinaries.Get(strNewName) == null)
							{
								strItem = strNewName;
								break;
							}

							++nTry;
						}
					}
				}

				try
				{
					vBytes = File.ReadAllBytes(strFile);
					//vBytes = DataEditorForm.ConvertAttachment(strItem, vBytes);
					vBytes = ConvertAttachment(strItem, vBytes);

					if (vBytes != null)
					{
						ProtectedBinary pb = new ProtectedBinary(false, vBytes);
						m_vBinaries.Set(strItem, pb);
					}
				}
				catch (Exception exAttach)
				{
					MessageService.ShowWarning(KPRes.AttachFailed, strFile, exAttach);
				}
			}

			//UpdateEntryBinaries(false, true);
			//ResizeColumnHeaders();
		}

		// Wrapper around internal method on DataEditorForm that can't otherwise be called from here
		private static MethodInfo sConvertAttachmentInternal;
		internal static byte[] ConvertAttachment(string strDesc, byte[] pbData)
		{
			if (sConvertAttachmentInternal == null)
			{
				sConvertAttachmentInternal = typeof(DataEditorForm).GetMethod("ConvertAttachment", BindingFlags.Static | BindingFlags.NonPublic);

				if (sConvertAttachmentInternal == null)
				{
					Debug.Fail("Couldn't find DataEditorForm.ConvertAttachment");
					// Fall back on no conversion
					return pbData;
				}
			}

			return (byte[])sConvertAttachmentInternal.Invoke(null, new object[] { strDesc, pbData });
		}
		#endregion

		#region Drag
		private class VirtualFileDragSource : IDragSource
		{
			public object StartDrag(ObjectListView olv, System.Windows.Forms.MouseButtons button, OLVListItem item)
			{
				var dataObject = new VirtualFileDataObject();
				dataObject.SetData(from kvp in olv.SelectedObjects.Cast<KeyValuePair<string, ProtectedBinary>>()
								   select CreateFileDescriptor(kvp.Key, kvp.Value));

				return dataObject;
			}

			private VirtualFileDataObject.FileDescriptor CreateFileDescriptor(string name, ProtectedBinary binary)
			{
				var descriptor = new VirtualFileDataObject.FileDescriptor();
				descriptor.Name = "\\" + name;
				descriptor.Length = binary.Length;
				descriptor.StreamContents = stream =>
					{
						var data = binary.ReadData();
						stream.Write(data, 0, data.Length);
					};

				return descriptor;
			}

			public DragDropEffects GetAllowedEffects(object dragObject)
			{
				return DragDropEffects.Copy;
			}

			public void EndDrag(object dragObject, DragDropEffects effect)
			{
			}
		}
		#endregion
	}
}
