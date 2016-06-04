using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KeePass.UI;

namespace KPEnhancedEntryView
{
	public class CollapsibleSplitContainer : SplitContainer
	{
		private const int sDefaultButtonSize = 73;
		private const int DotSeparation = 5;
		private const int DotSize = 2;
		private const int DotDropOffset = 1;

		private const long SplitRatioMax = 10000000;

		private int? mSnapPosition;
		private int mMinimumSplitSize;
		private bool mMouseDownInButton;

		public CollapsibleSplitContainer()
		{
			SplitterMoving += OnSplitterMoving;
			SplitterMoved += OnSplitterMoved;
		}

		public int MinimumSplitSize
		{
			get { return mMinimumSplitSize; }
			set
			{
				if (mMinimumSplitSize != value)
				{
					mMinimumSplitSize = value;
					if (mMinimumSplitSize > SplitterDistance)
					{
						SplitterDistance = mMinimumSplitSize;
					}
				}
			}
		}

		private long mSplitRatioAsSet;

		public long SplitRatio
		{
			get
			{
				var maxSplit = GetMaxSplit();

				if (SplitterDistance < MinimumSplitSize)
				{
					return 0;
				}

				if (SplitterDistance > maxSplit - MinimumSplitSize)
				{
					return SplitRatioMax;
				}

				// If there's a set value, and it produces the same splitter value, return that to avoid rounding errors
				if ((int)Math.Round(((double)mSplitRatioAsSet / SplitRatioMax) * maxSplit) == SplitterDistance)
				{

					return mSplitRatioAsSet;
				}


				var ratio = (SplitterDistance / (double)maxSplit) * SplitRatioMax;
				return (long)ratio;
			}
			set
			{
				if (value >= 0)
				{
					mSplitRatioAsSet = value;
					SplitterDistance = (int)Math.Round(((double)value / SplitRatioMax) * GetMaxSplit());
				}
			}
		}

		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				// Don't adjust the split ratio as a result of font changes
				var splitRatio = SplitRatio;
				base.Font = value;
				SplitRatio = splitRatio;
			}
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			// Don't adjust the split ratio as a result of size changes
			var splitRatio = SplitRatio;
			base.SetBoundsCore(x, y, width, height, specified);

			if (splitRatio == 0 || splitRatio == SplitRatioMax)
			{
				SplitRatio = splitRatio;
			}
			else
			{
				var maxSplit = GetMaxSplit();

				var maxSplitRatio = (long)(((maxSplit - MinimumSplitSize) / (double)maxSplit) * SplitRatioMax);
				var minSplitRatio = (long)((MinimumSplitSize / (double)maxSplit) * SplitRatioMax);
				SplitRatio = Math.Min(Math.Max(splitRatio, minSplitRatio), maxSplitRatio);
			}
		}

		private void OnSplitterMoving(object sender, SplitterCancelEventArgs e)
		{
			mSnapPosition = SnapSplitter(e, MinimumSplitSize);
		}

		private void OnSplitterMoved(object sender, SplitterEventArgs splitterEventArgs)
		{
			mMouseDownInButton = false; // If it's a splitter move, it's not a click

			if (mSnapPosition.HasValue)
			{
				var snapPosition = mSnapPosition.Value;
				mSnapPosition = null;
				SplitterDistance = snapPosition;
			}

			// Repaint splitter handle area
			Invalidate(SplitterRectangle);
		}

		private int SnapSplitter(SplitterCancelEventArgs splitEventArgs, int minSplitSize)
		{
			var maxSplit = GetMaxSplit();
			var split = GetSplit(splitEventArgs);

			if (split < minSplitSize)
			{
				return (split < (minSplitSize / 2)) ? 0 : minSplitSize;
			}

			if (split > maxSplit - minSplitSize)
			{
				return (split > maxSplit - (minSplitSize / 2)) ? maxSplit : maxSplit - minSplitSize;
			}

			return split;
		}

		private int GetMaxSplit()
		{
			var splitBoundsMax = Orientation == Orientation.Horizontal ? Height : Width;
			var maxSplit = splitBoundsMax - SplitterWidth;
			return maxSplit;
		}

		private int GetSplit(SplitterCancelEventArgs splitEventArgs)
		{
			return Orientation == Orientation.Horizontal ? splitEventArgs.SplitY : splitEventArgs.SplitX;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			IsHot = GetButtonRectangle().Contains(e.Location);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			
			IsHot = false;
			mMouseDownInButton = false;
		}

		private bool mIsHot;
		private bool IsHot
		{
			get { return mIsHot; }
			set
			{
				if (value != mIsHot)
				{
					mIsHot = value;
					Cursor = mIsHot ? Cursors.Hand : null;
					Invalidate(SplitterRectangle);
				}
			}
		}

		private int mButtonSize = sDefaultButtonSize;

		[DefaultValue(sDefaultButtonSize)]
		public int ButtonSize
		{
			get { return mButtonSize; }
			set
			{
				mButtonSize = value;
				Invalidate(SplitterRectangle);
			}
		}

		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			base.ScaleControl(factor, specified);

			ButtonSize = (int)Math.Round((float)ButtonSize * (Orientation == Orientation.Horizontal ? factor.Width : factor.Height));
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (GetButtonRectangle().Contains(e.Location))
			{
				mMouseDownInButton = true;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (mMouseDownInButton)
			{
				mMouseDownInButton = false;

				if (SplitRatio == 0)
				{
					// Uncollapse it
					SplitRatio = (long)(SplitRatioMax * 0.45);
				}
				else if (SplitRatio == SplitRatioMax)
				{
					// Uncollapse it
					SplitRatio = (long)(SplitRatioMax * 0.55);
				}
				else
				{
					// Collapse it
					SplitRatio = SplitRatio < SplitRatioMax / 2 ? 0 : SplitRatioMax;
				}
			}
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);

			// Repaint splitter handle area
			Invalidate(SplitterRectangle);
		}

		private Rectangle GetButtonRectangle()
		{
			var buttonRectangle = SplitterRectangle;

			if (Orientation == Orientation.Horizontal)
			{
				buttonRectangle.X = (buttonRectangle.Width - ButtonSize) / 2;
				buttonRectangle.Width = ButtonSize;
			}
			else
			{
				buttonRectangle.Y = (buttonRectangle.Height - ButtonSize) / 2;
				buttonRectangle.Height = ButtonSize;
			}

			return buttonRectangle;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var g = e.Graphics;

			var buttonRectangle = GetButtonRectangle();
			var backgroundBrush = IsHot ? new SolidBrush(Blend(SystemColors.Highlight, SystemColors.Control, 0.2)) : SystemBrushes.Control;
			g.FillRectangle(backgroundBrush, buttonRectangle);
			
			var isCollapsed = SplitRatio == 0 || SplitRatio == SplitRatioMax;
			if (Orientation == Orientation.Horizontal)
			{
				ArrowDirection arrowDirection;
				if (SplitRatio < SplitRatioMax / 2)
				{
					arrowDirection = isCollapsed ? ArrowDirection.Down : ArrowDirection.Up;
				}
				else
				{
					arrowDirection = isCollapsed ? ArrowDirection.Up : ArrowDirection.Down;
				}


				// Draw the arrows
				g.FillPolygon(SystemBrushes.ControlDark, GetArrowPointArray(buttonRectangle.X + DpiUtil.ScaleIntX(5), buttonRectangle.Y, arrowDirection));
				g.FillPolygon(SystemBrushes.ControlDark, GetArrowPointArray(buttonRectangle.Right - DpiUtil.ScaleIntX(12), buttonRectangle.Y, arrowDirection));

				// Draw the dots
				var dotY = buttonRectangle.Y + DpiUtil.ScaleIntY(1);
				for (var dotX = buttonRectangle.X + DpiUtil.ScaleIntX(15); dotX < buttonRectangle.Right - DpiUtil.ScaleIntX(16); dotX += DpiUtil.ScaleIntX(DotSeparation))
				{
					g.FillRectangle(SystemBrushes.ControlLightLight, dotX + DpiUtil.ScaleIntX(DotDropOffset), dotY + DpiUtil.ScaleIntY(DotDropOffset), DpiUtil.ScaleIntX(DotSize), DpiUtil.ScaleIntY(DotSize));
					g.FillRectangle(SystemBrushes.ControlDark, dotX, dotY, DpiUtil.ScaleIntX(DotSize), DpiUtil.ScaleIntY(DotSize));
				}
			}
			else
			{
				
				ArrowDirection arrowDirection;
				if (SplitRatio < SplitRatioMax / 2)
				{
					arrowDirection = isCollapsed ? ArrowDirection.Right : ArrowDirection.Left;
				}
				else
				{
					arrowDirection = isCollapsed ? ArrowDirection.Left : ArrowDirection.Right;
				}

				// Draw the arrows
				g.FillPolygon(SystemBrushes.ControlDark, GetArrowPointArray(buttonRectangle.X, buttonRectangle.Y + DpiUtil.ScaleIntY(5), arrowDirection));
				g.FillPolygon(SystemBrushes.ControlDark, GetArrowPointArray(buttonRectangle.X, buttonRectangle.Bottom - DpiUtil.ScaleIntY(12), arrowDirection));

				// Draw the dots
				var dotX = buttonRectangle.X + DpiUtil.ScaleIntX(1);
				for (var dotY = buttonRectangle.Y + DpiUtil.ScaleIntY(15); dotY < buttonRectangle.Bottom - DpiUtil.ScaleIntY(16); dotY += DpiUtil.ScaleIntY(DotSeparation))
				{
					g.FillRectangle(SystemBrushes.ControlLightLight, dotX + DpiUtil.ScaleIntX(DotDropOffset), dotY + DpiUtil.ScaleIntY(DotDropOffset), DpiUtil.ScaleIntX(DotSize), DpiUtil.ScaleIntY(DotSize));
					g.FillRectangle(SystemBrushes.ControlDark, dotX, dotY, DpiUtil.ScaleIntX(DotSize), DpiUtil.ScaleIntY(DotSize));
				}
			}

			if (IsHot)
			{
				backgroundBrush.Dispose();
			}
		}

		// Create an arrow suitable for sitting in a 4px gap
		private Point[] GetArrowPointArray(int x, int y, ArrowDirection direction)
		{
			switch (direction)
			{
				case ArrowDirection.Up:
					return new[]
					{
						new Point(x - DpiUtil.ScaleIntX(1), y + DpiUtil.ScaleIntY(3)),
						new Point(x + DpiUtil.ScaleIntX(3), y - DpiUtil.ScaleIntY(1)),
						new Point(x + DpiUtil.ScaleIntX(6), y + DpiUtil.ScaleIntY(3)),
					};
				case ArrowDirection.Down:
					return new[]
					{
						new Point(x, y + DpiUtil.ScaleIntY(1)),
						new Point(x + DpiUtil.ScaleIntX(6), y + DpiUtil.ScaleIntY(1)),
						new Point(x + DpiUtil.ScaleIntX(3), y + DpiUtil.ScaleIntY(4)),
					};
				case ArrowDirection.Left:
					return new[]
					{
						new Point(x + DpiUtil.ScaleIntX(3), y + DpiUtil.ScaleIntY(6)),
						new Point(x, y + DpiUtil.ScaleIntY(3)),
						new Point(x + DpiUtil.ScaleIntX(3), y),
					};
				case ArrowDirection.Right:
					return new[]
					{
						new Point(x + DpiUtil.ScaleIntX(1), y),
						new Point(x + DpiUtil.ScaleIntX(4), y + DpiUtil.ScaleIntY(3)),
						new Point(x + DpiUtil.ScaleIntX(1), y + DpiUtil.ScaleIntY(6)),
					};
				default:
					throw new ArgumentOutOfRangeException("direction");
			}
		}

		/// <summary>Blends the specified colors together.</summary>
		/// <param name="color">Color to blend onto the background color.</param>
		/// <param name="backColor">Color to blend the other color onto.</param>
		/// <param name="amount">How much of <paramref name="color"/> to keep,
		/// “on top of” <paramref name="backColor"/>.</param>
		/// <returns>The blended colors.</returns>
		/// ref: http://stackoverflow.com/a/3722337
		private static Color Blend(Color color, Color backColor, double amount)
		{
			var r = (byte)((color.R * amount) + backColor.R * (1 - amount));
			var g = (byte)((color.G * amount) + backColor.G * (1 - amount));
			var b = (byte)((color.B * amount) + backColor.B * (1 - amount));
			return Color.FromArgb(r, g, b);
		}
	}
}
