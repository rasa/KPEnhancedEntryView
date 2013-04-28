KeePass Enhanced Entry View (KPEnhancedEntryView)
=================================================
http://sourceforge.net/projects/kpenhentryview


This is a plugin to KeePass <http://www.KeePass.info> to provide an enhanced entry view for the selected entry.

Features
 * All fields (standard and custom string) shown in a grid view
 * Field values and, for custom strings, names, are in place-editable
 * Easy to add new custom fields through in-place editing
 * Protected (password) fields can be edited, or their value shown through the in-place editor
 * Attachments can be dragged and dropped to and from files in Windows Explorer
 * Metadata properties shown on a separate tab to details field
 * Original textual entry view still available, through the "All Text" tab


Installation
------------
Place KPEnhancedEntryView.plgx in your KeePass Plugins folder.
It is recommended, though not essential, to use Side by Side window layout; to do this go to
 the View menu and choose Window Layout, then Side by Side.
If you are seeing no entry view at all, ensure that the "Show Entry View" item on the View
 menu is checked.	


Uninstallation
--------------
Delete KPEnhancedEntryView.plgx from your KeePass Plugins folder.


Usage
-----
To in-place edit a field value, double click on it, or select it and press Enter.
To rename a field, double click the field name, or select it and press F2.
To delete a field, select it and press Delete.

Drag and drop files from Windows Explorer into the Attachments pane to attach them.
Drag and drop files from the Attachments pane to Windows Explorer to extract them.
To rename an attachment, select it and press F2.
To delete attachments, select them and press Delete

To edit the Notes for an item, click in the Notes pane and start editing. It will
enter edit mode automatically. Edit mode is indicated by a sunken border. To exit
edit mode, press escape, or click anywhere outside the Notes pane. If you need to
re-enter edit mode and the Notes pane still has the focus, double-click it.

Other non-field information is available from the Properties tab, where the icon,
tags, and URL override properties are also editable.

To get back to the old textual entry view, select the All Text tab

All other operations are available through the context (right-click) menus.


Bug Reporting, Questions, Comments, Feedback
--------------------------------------------
Please use the SourceForge project page: <http://sourceforge.net/projects/kpenhentryview>
Bugs can be reported using the issue tracker, for anything else, a discussion forum is available.


Changelog
---------
v0.4
 Packaged as plgx instead of dlls.

v0.3
 Allow drag and drop of field values

 Added "Open URL" to the context menu for fields which are URLs

 Fixed bug where Protect Fields context menu command wouldn't actually make any change

 Does not include KeeFox's "KPRPC JSON" custom field (as this is not intended to be 
  directly user editable or visible)

v0.2
 Fixed bug where editing a field value would not notify KeePass that a modification
 had been made

v0.1 (2013-04-03)
 Initial release