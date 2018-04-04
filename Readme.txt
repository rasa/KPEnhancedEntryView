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
 * Multiple selected entries can be edited together to perform mass changes


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
To view an attachment, double click it. When using KeePass 2.25 or above, if it's a
file type that is not supported by the internal editor, it will be extracted and an
external editor used.

To edit the Notes for an item, click in the Notes pane and start editing. It will
enter edit mode automatically. Edit mode is indicated by a sunken border. To exit
edit mode, press escape, or click anywhere outside the Notes pane. If you need to
re-enter edit mode and the Notes pane still has the focus, double-click it.

Other non-field information is available from the Properties tab, where the icon,
tags, and URL override properties are also editable.

To get back to the old textual entry view, select the All Text tab

All other operations are available through the context (right-click) menus.


Auto-Type field values
----------------------

On the context (right click) menu for fields, there is a command to Auto-Type that
field. This will perform an auto-type into the previous window to be active, in the
same way as the KeePass "Perform Auto-Type" command for an entry does. The difference
is that it will not auto-type the whole sequence of the entry, but instead auto-type
just the value of that field.

If the default auto-type sequence for an entry starts with special control
placeholders like {DELAY} or {APPACTIVATE} then these will be honoured for the field
value auto-typing, but the rest of the sequence is ignored.


Mass editing of multiple selected entries
-----------------------------------------

Multiple entries can be edited simultaneously. If you make a multiple selection then
the entry view will show a combined fields list. Notes and Attachments can not be
edited on a multiple selection, but other fields can be. Where fields have differing
values, or are not present on all entries in a selection, they will be displayed
greyed with the text "Multiple Values". These cannot be copied or dragged and dropped.
You can still delete or edit them, but a warning will be given first as this will be
making a modification to all the selected entries, overwriting any value they already
had for that field. Adding new fields works normally, and will add the new field and
value to all the selected entries.

Where all entries have the same value for a field, that is displayed normally and
can be edited and deleted without further warning.

Note: Where a field, such as Password, is set to show hidden (as asterisks) then its
values are always considered different to each other. A hidden field will always be
shown as ******* and treated as having multiple values. This avoids the need to read
the values out of protected memory to compare them, and avoids information about the
hidden passwords from being revealed to an observer. 

For example, if an entry with a known password was in a multiple selection with an 
entry with an unknown password then having the field show differently depending on
whether the passwords were the same or not could leak the information to an observer
that the unknown password was the same as the known one, even though both passwords
remained hidden.


URL recognition
---------------

Most common URLs will automatically be recognised correctly, however if using a custom
scheme or protocol, or spaces within the URL, it may be necessary to wrap it in <> to
make it treated as a link.

The value of the URL standard field will always be treated
as a URL. To force the value of any other field to be treated as a link, enclose the
whole value in <>. It is not possible to treat only part of a field value as a link - 
only the Notes for an entry can contain multiple links.

For the Notes, any text enclosed within <> is treated as a link.

When creating links with spaces in them, be aware that KeePass treats spaces as the
delimeters between the program name and arguments for links starting with "cmd://" or 
"\\" (network paths). So, to create a link to a network share with a space in it, you
would need to enclose it in quotes too: <"\\server\share name">


Placeholders
------------

When displaying field values, only a limited set of placeholders are evaluated. This matches
the substitutions displayed in the original textual entry view. When Copying a field value, all
placeholders are evaluated, including active ones such as {NEWPASSWORD} and {HMACOTP} that may
alter the entry.

When performing a drag and drop, most placeholders are evaluated in the same way as Copying,
however placeholders which require UI interaction (like PickChars) are not evaluated, as it's
impossible to interact with them while continuing the drag operation.


Checking for updates
--------------------
If you want to use the KeePass Check for Updates function to check for updates to this plugin
then it requires the SourceForgeUpdateChecker plugin to be installed too:
http://sourceforge.net/projects/kpsfupdatechecker


Bug Reporting, Questions, Comments, Feedback
--------------------------------------------
Please use the SourceForge project page: <http://sourceforge.net/projects/kpenhentryview>
Bugs can be reported using the issue tracker, for anything else, a discussion forum is available.


Changelog
---------
v1.11
 Fixed bug with Open With menu for URLs

v1.10
 Added Ctrl+V shortcut for auto-typing selected field.
 Fixed bugs with shortcuts on (Add New) row

v1.9.1
 Fixed further bug with splitter in Stacked window layout

v1.9
 Fixed bug where fields list would reset scroll position and selection when copying or dragging

v1.8.1
 Fixed bug with splitter causing a crash when entry view size is zero (possible in Stacked window
  layout)

v1.8
 Fixed bug with splitter snapping not persisting

v1.7
 Compatibility with KeePass 2.33 Escape key behaviour - while editing a field, Escape will only
  cancel the edit. Pressing Escape a second time will lock the workspace

v1.6
 Fixed bug with High DPI support

v1.5
 Attempt to correct gradual splitter position drifting
 Horizontal scrollbar added to properties tab, when required
 Added menu command and keyboard shortcut (F9) to toggle revealing of all values

v1.4
 Match KeePass 2.30 behaviour for case-insensitivity of field names. Multiple custom fields on the
  same entry with names which differ only by case are no longer allowed.
 Password (and other protected fields) are now shown using the selected Password font

v1.3
 Support for multi-line field values. Press Shift+Enter to insert a new line when editing

v1.2
 Added compatibility with KeePass 2.29 high resolution custom icons

v1.1
 Added checkboxes to enable and diable Auto-Type and Two-Channel Auto-Type to the properties tab
 Fixed compatibility with latest KeePass development snapshots

v1.0
 Added "AutoType Field" command to the context menu for fields. This performs an auto-type of only
  that field, rather than using the full entry auto-type sequence
 Added the ability to edit the expiry for entries from the Properties tab

v0.32
 Fixed bug where uncomitted changes may not be comitted when auto-locking a database

v0.31
 Fixed bug where validation failure with a multiple selection may cause a crash when changing
  selection before attempting to comitt an invalid change
 Fixed bug where a mulitple selection change with uncommitted changes may not properly update
  the last modified timestamp

v0.30
 Fixed compatibility with older versions of the .NET Framework

v0.29
 Added support for High DPI (requires KeePass v2.28 or above)
 More reliable committing of in-progress editing when saving, closing or locking the workspace

v0.28
 Fixed bug with multiple editing where entries Last-Modified date would not be updated (this could
  also lead to synchronisation issues)
 Fixed bug where edits to a multiple selection would not be comitted if a single entry was then
  selected in the list before committing

v0.27
 Added support for URL override behaviour for the standard URL field. If an entry has an override
  URL, then this will apply if the URL field link is clicked. Other fields will behave as normal.
 Added the browser drop-down suggestions to the URL override field on the Properties tab.
 Fixed bug where changes to URL override and Tags properties would sometimes be ignored
 Added flag to PlgX to indicate that this plugin only works on Windows

v0.26
 Fixed bug with Protect Field command

v0.25
 Further performance enhancements when dealing with very large groups
 Placeholders substitution for display now matches KeePass behaviour, no active subsititutions
  will be made. (Substitution for Copy or Drag and Drop still perform active substitutions)

v0.24
 Performance enhancements when editing entries in large groups
 Fixed bug where entry view would steal focus switching between single and multiple selection

v0.23
 Notes field and Properties tab now participate in the History record creation reduction system
 Added custom colours and UUID to the Properties tab for entries
 Fixed issue with highlighting and clicking on URLs containing placeholders

v0.22
 Reduces creation of multiple History records when editing entries.
 In-progress cell edits will now be committed rather than discarded when selecting a different
  entry. To cancel an edit, click Escape.
 Added support for update checking, using SourceForgeUpdateChecker

v0.21
 Field values which reference the password field now hide the referenced value, in the same way
  as the old textual entry view does.
 URLs can now be wrapped in < > to force detection as a URL. For example, <cmd://notepad.exe> or
  <"\\server\share name"> will be made into clickable links.
 Collapse toggle button added to splitters

v0.20
 Added support for KeePass 2.25 feature for editing attachments with an external application

v0.19
 Fixed bug where attchements weren't updated immediately after editing

v0.18
 Fixed bug where clicks were registered on mouse up without corresponding initial mouse down
 Fixed bug where drag cursor was not displayed within the fields list itself
 Last Access Time visibility now respects the UIFlags setting for KeePass 2.24 and above

v0.17
 Fixed bug with saving while editing notes.

v0.16
 Improved splitter behaviour - it is now possible to move the splitters so that any of the three
  panels (fields, notes, attachments) is completely hidden. The positions of the splitters are
  now persisted after closing and re-opening KeePass.

v0.15
 If KeePass is version 2.24 or later, hides the Last Access Time field (which is deprecated)
 Added a password reveal button to the right hand edge of protected fields. Click this to
  temporarily display the password in that field. Click again to conceal it.

v0.14
 Fixed bug where all custom fields may be displayed as hidden data (asterisks)

v0.13
 Added support for dereferencing field references.
 In the rare case where standard fields are missing (usually from imported data), they are now
  treated as blank.

v0.12
 Fixed crash that could occur when multiply-selecting entries that had missing standard fields.

v0.11
 Added multiple selection mass entry editing functionality

v0.10
 Disabled non-functional multi-select capability of fields
 Added support for main window shortcut keys while the entry view has the focus
 Added support for obeying the column value hiding settings (so Ctrl+H and Ctrl+J will now hide
  and show the password and username, and custom fields will obey the hiding settings set in the
  Configure Columns window). Note that KeePass (as of v2.22) will only allow you to set hiding
  settings for columns that are set to be visible in the entry list.

v0.9
 Standard fields will not be immediately hidden when the value is edited to be blank. They will
  be hidden next time the entry is viewed. To hide immediately, use the Delete Field context menu
  command.
 Fixed crash when attempting to drag a blank value (blank values now simply can't be dragged).

v0.8
 Standard field hiding is now optional, see the menu item Tools, Entry View Options,
  Hide Empty Standard Fields. It's turned off (no hiding) by default.
 Fixed performance issue with large Notes fields
 Fixed cosmetic bug where fields were slow to repaint when deleted or switched to or
  from being protected
 Enter key (as well as F2) can now be used on insertion row to start insertion

v0.7
 Updates the view less aggressively - the view will only be updated if the entry last-modified
  timestamp has changed. This should reduce flickering, and prevent loss of edit mode on
  clipboard clearing.
 Standard fields (Title, Username, Password, URL) are now not shown when blank. They can be added
  using the insertion (add new) row in the same way as adding custom fields

v0.6
 Fixed support for cmd:// links (and any other non-standard links that were being mangled)
 Now assumes that the contents of the URL field should always be treaded as a link, even if 
 it doesn't look like one. Other fields will only be made links if they look like URLs.

v0.5
 Added support for writing history entries when changes are made to entries
 Added "Link" drop down menu to fields whose values are URL so that they can be opened in
  alternative browsers.

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