using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Delay;
using KeePass.Forms;
using KeePass.Resources;
using KeePass.UI;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Security;
using KeePassLib.Utility;

namespace KPEnhancedEntryView
{
	internal class AttachmentsListView : ObjectListView
	{
		public AttachmentsListView()
		{
			SmallImageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(DpiUtil.ScaleIntX(16), DpiUtil.ScaleIntY(16)) };

			View = System.Windows.Forms.View.SmallIcon;
			HeaderStyle = ColumnHeaderStyle.None;
			Scrollable = true;

			CellEditActivation = CellEditActivateMode.F2Only;

			CopySelectionOnControlC = false;

			DragSource = new VirtualFileDragSource();
			AllowDrop = true;

			var overlay = (TextOverlay)EmptyListMsgOverlay;
			overlay.Alignment = ContentAlignment.TopLeft;
			overlay.InsetX = 0;
			overlay.InsetY = 0;
			overlay.BackColor = Color.Empty;
			overlay.BorderColor = Color.Empty;
			overlay.TextColor = SystemColors.GrayText;

			var column = new OLVColumn
			{
				FillsFreeSpace = true,
				AutoCompleteEditor = false,
				AspectGetter = model => ((RowObject)model).Name,
				AspectPutter = SetAttachmentName,
				ImageGetter = IconImageGetter
			};
			Columns.Add(column);

			OnFontChanged(EventArgs.Empty); // Set initial font
		}

		// Disallow setting of IsSimpleDragSource (as it breaks the file dragging, and is sometimes automatically set by the designer for some reason)
		public override bool IsSimpleDragSource
		{
			get { return false; }
			set { }
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mItalicFont != null)
				{
					mItalicFont.Dispose();
					mItalicFont = null;
				}
			}
			base.Dispose(disposing);
		}

		#region Font
		private Font mItalicFont = null;

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			if (mItalicFont != null) mItalicFont.Dispose();

			mItalicFont = new Font(Font, FontStyle.Italic);

			EmptyListMsgFont = mItalicFont;
		}
		#endregion

		#region Getters
		private object IconImageGetter(object model)
		{
			var rowObject = (RowObject)model;
			var path = rowObject.Name;
			var extension = System.IO.Path.GetExtension(path);

			/*
			if (!SmallImageList.Images.ContainsKey(extension))
			{
				using(var icon = IconHelper.GetIconForFileName(path))
				{
					var iconBitmap = icon.ToBitmap();
					var scaledIconBitmap = DpiUtil.ScaleImage(iconBitmap, false);
					if (iconBitmap != scaledIconBitmap)
					{
						iconBitmap.Dispose();
					}

					SmallImageList.Images.Add(scaledIconBitmap);
				}				
			}
			 */
			SmallImageList.Images.Add(IconHelper.GetIconForFileName(path));

			return SmallImageList.Images.Count - 1;
		}
		#endregion

		/// <summary>
		/// Database must be set in order to perform history maintenance when modifying the value
		/// </summary>
		public PwDatabase Database { get; set; }

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
				RefreshObjectsFromEntry();
			}
		}

		private void RefreshObjectsFromEntry()
		{
			SetObjects(from kvp in Entry.Binaries select new RowObject(kvp));
		}
		#endregion

		#region EntryModified event
		public event EventHandler EntryModified;
		protected virtual void OnEntryModified(EventArgs e)
		{
			var temp = EntryModified;
			if (temp != null)
			{
				temp(this, e);
			}
		}
		#endregion

		#region Commands
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (!IsCellEditing)
			{
				if (SelectedIndices.Count > 0)
				{
					if (keyData == Keys.Delete)
					{
						DeleteSelected();
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		public void AttachFiles()
		{
			// Copied from PwEntryForm.OnCtxBinImport (PwEntryForm.cs)
			var ofd = UIUtil.CreateOpenFileDialog(KPRes.AttachFiles,
				UIUtil.CreateFileTypeFilter(null, null, true), 1, null, true,
				KeePass.App.AppDefs.FileDialogContext.Attachments);

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				Entry.CreateBackup(Database);

				BinImportFiles(ofd.FileNames);

				RefreshObjectsFromEntry();
				OnEntryModified(EventArgs.Empty);
			}
		}

		public void ViewSelected()
		{
			var rowObject = SelectedObject as RowObject;
			if (rowObject != null)
			{
				ShowBinaryWindow(rowObject.Name, rowObject.Binary);
			}
		}

		public void RenameSelected()
		{
			if (SelectedItem != null)
			{
				StartCellEdit(SelectedItem, 0);
			}
		}

		public void DeleteSelected()
		{
			bool singleItem = (SelectedIndices.Count == 1);

			var prompt = new VistaTaskDialog
			{
				CommandLinks = false,
				Content = CreateSummaryList(from RowObject rowObject in SelectedObjects select rowObject.Name),
				MainInstruction = singleItem ? Properties.Resources.DeleteAttachmentQuestion :
												Properties.Resources.DeleteAttachmentsQuestion,
				WindowTitle = PwDefs.ShortProductName
			};

			prompt.SetIcon(VtdCustomIcon.Question);
			prompt.AddButton((int)DialogResult.OK, KPRes.DeleteCmd, null);
			prompt.AddButton((int)DialogResult.Cancel, KPRes.Cancel, null);

			if (prompt.ShowDialog())
			{
				if (prompt.Result == (int)DialogResult.Cancel) return;
			}
			else
			{
				if (!MessageService.AskYesNo(singleItem ? Properties.Resources.DeleteAttachmentQuestion : Properties.Resources.DeleteAttachmentsQuestion,
											 singleItem ? Properties.Resources.DeleteAttachmentTitle : Properties.Resources.DeleteAttachmentsTitle))
				{
					return;
				}
			}

			Entry.CreateBackup(Database);

			foreach (RowObject rowObject in SelectedObjects)
			{
				Entry.Binaries.Remove(rowObject.Name);
			}
			RemoveObjects(SelectedObjects);
			
			OnEntryModified(EventArgs.Empty);
		}

		private string CreateSummaryList(IEnumerable<String> attachmentNames)
		{
			var maxItemsToShow = 10;
			// Consistent behaviour with EntryUtil.CreateSummaryList (EntryUtil.cs)
			var summary = String.Join(MessageService.NewLine, (from attachmentName in attachmentNames.Take(maxItemsToShow) select "- " + StrUtil.CompactString3Dots(attachmentName, 39)).ToArray());

			var count = attachmentNames.Count();
			if (count > maxItemsToShow)
			{
				summary += MessageService.NewLine + "- " + String.Format(Properties.Resources.MoreAttachments, count - maxItemsToShow);
			}

			return summary;
		}

		public void SaveSelected()
		{
			// Copied from PwEntryForm.OnBtnBinSave (PwEntryForm.cs)
			var lvsc = SelectedObjects.Cast<RowObject>().ToList();

			int nSelCount = lvsc.Count;
			if (nSelCount == 0) { Debug.Assert(false); return; }

			if (nSelCount == 1)
			{
				SaveFileDialogEx sfd = UIUtil.CreateSaveFileDialog(KPRes.AttachmentSave,
					lvsc[0].Name, UIUtil.CreateFileTypeFilter(null, null, true), 1, null,
					KeePass.App.AppDefs.FileDialogContext.Attachments);

				if (sfd.ShowDialog() == DialogResult.OK)
					SaveAttachmentTo(lvsc[0].Binary, sfd.FileName, false);
			}
			else // nSelCount > 1
			{
				FolderBrowserDialog fbd = UIUtil.CreateFolderBrowserDialog(KPRes.AttachmentsSave);

				if (fbd.ShowDialog() == DialogResult.OK)
				{
					string strRootPath = UrlUtil.EnsureTerminatingSeparator(fbd.SelectedPath, false);

					foreach (var lvi in lvsc)
						SaveAttachmentTo(lvi.Binary, strRootPath + lvi.Name, true);
				}
				fbd.Dispose();
			}
		}

		// Copied from PwEntryForm.SaveAttachmentTo (PwEntryForm.cs)
		private void SaveAttachmentTo(ProtectedBinary pb, string strFileName,
			bool bConfirmOverwrite)
		{
			Debug.Assert(pb != null); if (pb == null) throw new ArgumentNullException("pb");
			Debug.Assert(strFileName != null); if (strFileName == null) throw new ArgumentNullException("strFileName");

			if (bConfirmOverwrite && File.Exists(strFileName))
			{
				string strMsg = KPRes.FileExistsAlready + MessageService.NewLine +
					strFileName + MessageService.NewParagraph +
					KPRes.OverwriteExistingFileQuestion;

				if (MessageService.AskYesNo(strMsg) == false)
					return;
			}

			Debug.Assert(pb != null); if (pb == null) throw new ArgumentException();

			byte[] pbData = pb.ReadData();
			try { File.WriteAllBytes(strFileName, pbData); }
			catch (Exception exWrite)
			{
				MessageService.ShowWarning(strFileName, exWrite);
			}
			MemUtil.ZeroByteArray(pbData);
		}
		#endregion

		#region Renaming
		protected override void OnCellEditorValidating(CellEditEventArgs e)
		{
			var rowObject = (RowObject)e.RowObject;
			var newName = (string)e.NewValue;

			if (newName != rowObject.Name)
			{
				// Logic copied from PwEntryForm.OnBinAfterLabelEdit (PwEntryForm.cs)
				if (String.IsNullOrEmpty(newName))
				{
					ReportValidationFailure(e.Control, KPRes.FieldNamePrompt);
					e.Cancel = true;
					return;
				}
				if (Entry.Binaries.Get(newName) != null)
				{
					ReportValidationFailure(e.Control, KPRes.FieldNameExistsAlready);
					e.Cancel = true;
					return;
				}
			}

			base.OnCellEditorValidating(e);
		}

		#region Validation Failure Reporting
		public ValidationFailureReporter ValidationFailureReporter { get; set; }

		private void ReportValidationFailure(Control control, string message)
		{
			if (ValidationFailureReporter != null)
			{
				ValidationFailureReporter.ReportValidationFailure(control, message);
			}
		}
		#endregion

		private void SetAttachmentName(Object model, Object newValue)
		{
			var rowObject = (RowObject)model;
			var newName = (string)newValue;

			if (!String.IsNullOrEmpty(newName) && newName != rowObject.Name)
			{
				var binary = Entry.Binaries.Get(rowObject.Name);

				Entry.CreateBackup(Database);

				Entry.Binaries.Remove(rowObject.Name);
				Entry.Binaries.Set(newName, binary);

				rowObject.Name = newName;
				OnEntryModified(EventArgs.Empty);
			}
		}
		#endregion

		#region Edit/View
		protected override void OnItemActivate(EventArgs e)
		{
			base.OnItemActivate(e);

			ViewSelected();
		}

		private void ShowBinaryWindow(string name, ProtectedBinary binary)
		{
			var modifiedData = BinaryDataUtil.Open(name, binary, null);
			if (modifiedData != null)
			{
				Entry.CreateBackup(Database);

				Entry.Binaries.Set(name, modifiedData);

				RefreshObjectsFromEntry();
				OnEntryModified(EventArgs.Empty);
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

				Entry.CreateBackup(Database);
				
				BinImportFiles(files);

				RefreshObjectsFromEntry();
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
				dataObject.SetData(from rowObject in olv.SelectedObjects.Cast<RowObject>()
								   select CreateFileDescriptor(rowObject.Name, rowObject.Binary));

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

		private class RowObject
		{
			public RowObject(KeyValuePair<string, ProtectedBinary> keyValuePair)
			{
				Name = keyValuePair.Key;
				Binary = keyValuePair.Value;
			}

			public string Name { get; set; }
			public ProtectedBinary Binary { get; set; }
		}
	}
}
