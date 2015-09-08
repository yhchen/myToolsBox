using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data;
using System.Diagnostics;

using Point = System.Drawing.Point;

namespace NDiffDiff
{
	public partial class Display : ScrollableControl
	{
		static readonly Color cBack = Color.Black;
		static readonly Color cFore = Color.White;
		static readonly Color cDeleted = Color.FromArgb( 0xFF, 0, 0 );
		static readonly Color cDeletedLight = Color.FromArgb( 0xFF, 0xCC, 0xCC );
		static readonly Color cInserted = Color.FromArgb( 0, 0xFF, 0 );
		static readonly Color cInsertedLight = Color.FromArgb( 0xCC, 0xFF, 0xCC );

		string _Result = null;
		Size _Size = Size.Empty;

		object PaintKey = new object();

		public Display()
		{
			InitializeComponent();

			DoubleBuffered = true;
			BackColor = cBack;
			AutoScroll = true;
			Scroll += ( s, e ) => Invalidate();
		}

		public string Result
		{
			get { return _Result; }
			set
			{
				_Result = value;
				//Debug.WriteLine( _Result );

				using ( var g = CreateGraphics() ) DoPaint( g );

				BeginInvoke( ( Action ) ( () => Invalidate() ) ); // update the scroll bars
			}
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			DoPaint( e.Graphics );

			AutoScrollMinSize = _Size;
		}

		void DoPaint( Graphics g )
		{
			lock ( PaintKey ) DoPaintUnsafe( g );
		}

		void DoPaintUnsafe( Graphics g )
		{
			if ( _Result == null ) return;

			int maxX = 0, maxY = 0;

			using ( var f = new Font( "Verdana", 10 ) )
			{
				var flags = TextFormatFlags.NoPadding;

				int lineY = f.Height;

				int x = 0, y = 0, maxLen = 0;

				int start = 1;
				for ( int i = 1 ; i < _Result.Length - 1 ; i++ )
				{
					if ( _Result[ i ] == '\n' )
					{
						if ( i > start )
						{
							Color back = cBack, fore = cFore;
							switch ( _Result[ start++ ] )
							{
								case 'D': back = cDeleted; break;
								case 'E': back = cDeletedLight; fore = cBack; break;
								case 'I': back = cInserted; fore = cBack; break;
								case 'J': back = cInsertedLight; fore = cBack; break;
							}

							var s = _Result.Substring( start, i - start );
							var pt = new Point( x + 10 + AutoScrollPosition.X, y + 10 + AutoScrollPosition.Y );

							if ( x != 0 || _Result[ i + 1 ] != '\n' || s.Length > maxLen ) // segmented and long lines are measured
							{
								var size = TextRenderer.MeasureText( g, s, f, Size.Empty, flags );
								var rect = new Rectangle( pt, size );
								TextRenderer.DrawText( g, s, f, rect, fore, back, flags );
								x += size.Width;

								if ( s.Length > maxLen ) maxLen = s.Length;
								if ( x > maxX ) maxX = x;
							}
							else
							{
								TextRenderer.DrawText( g, s, f, pt, fore, back, flags );
							}
						}

						if ( _Result[ i + 1 ] == '\n' ) { maxY = y += lineY; x = 0; i++; }

						start = i + 1;
					}
				}
			}

			_Size = new Size( maxX + 20, maxY + 20 );
		}
	}

	static class D
	{
		public const string Normal = "\nN";
		public const string Deleted = "\nD";
		public const string DeletedLight = "\nE";
		public const string Inserted = "\nI";
		public const string InsertedLight = "\nJ";
	}
}
