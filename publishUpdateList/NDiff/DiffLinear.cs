using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using System.Diagnostics;

namespace DiffLinear
{
	public static class Diff
	{
		//-----------------------------------------------------------------------------------------
		// public Compare

		public static Results Compare( string[] aa, string[] ab, bool diffChars )
		{
			Debug.WriteLine( String.Format( "\n\n*** STRING A:{0:N0} B:{1:N0} A+B:{2:N0} ***", aa.Length, ab.Length, aa.Length + ab.Length ) );

			var VForwardArray = new V( aa.Length, ab.Length, true, true );
			var VReverseArray = new V( aa.Length, ab.Length, false, true );

			var snakes = new List<Snake>();

			Compare( snakes, aa, aa.Length, ab, ab.Length, VForwardArray, VReverseArray );

			var lines = MergeSnakes( snakes, 0, aa.Length, 0, ab.Length );

			var chars = CompareChars( aa, ab, lines, diffChars, ref VForwardArray, ref VReverseArray );

			return new Results( VForwardArray.Memory + VReverseArray.Memory, chars );
		}

		//-----------------------------------------------------------------------------------------
		// CompareChars

		static IList<SnakeWithCharResults> CompareChars( string[] aa, string[] ab, IList<Snake> lines, bool diffChars, ref V VForward, ref V VReverse )
		{
			var list = new List<SnakeWithCharResults>();

			foreach ( var line in lines )
			{
				if ( !diffChars || line.ADeleted == 0 || line.BInserted == 0 ) list.Add( new SnakeWithCharResults( line, null ) );
				else
				{
					var ca = new CompareCharsData( aa, line.XStart, line.ADeleted );
					var cb = new CompareCharsData( ab, line.YStart, line.BInserted );

					var charSnakes = new List<Snake>();

					EnsureArraySize( ca.Chars.Length, cb.Chars.Length, ref VForward, ref VReverse );

					Compare( charSnakes, ca.Chars, ca.Chars.Length, cb.Chars, cb.Chars.Length, VForward, VReverse );

					var tmp = MergeSnakes( charSnakes, 0, ca.Chars.Length, 0, cb.Chars.Length );

					foreach ( var s in tmp )
						for ( int matchLength = 1 ; matchLength <= 3 ; matchLength++ )
							if ( s.DiagonalLength == matchLength && s.XStart > 0 && s.XEnd < ca.Chars.Length && s.YEnd < cb.Chars.Length )
							{
								s.ADeleted += matchLength;
								s.BInserted += matchLength;
								s.DiagonalLength = 0;
							}

					var chars = MergeSnakes( tmp, 0, ca.Chars.Length, 0, cb.Chars.Length );

					list.Add( ConvertToLines( ca, true, chars, null ) );
					list.Add( ConvertToLines( cb, false, chars, line ) );
				}
			}

			return list;
		}

		static void EnsureArraySize( int N, int M, ref V VForward, ref V VReverse )
		{
			Debug.Assert( VForward.N == VReverse.N );
			Debug.Assert( VForward.M == VReverse.M );

			if ( N + M <= VForward.N + VForward.M ) return;

			Debug.WriteLine( "Increasing V from [ " + VForward.N + ", " + VForward.M + " ] to [ " + N + ", " + M + " ]" );

			VForward = new V( N, M, true, true );
			VReverse = new V( N, M, false, true );
		}

		static SnakeWithCharResults ConvertToLines( CompareCharsData data, bool deleted, IList<Snake> chars, Snake lineSnake )
		{
			//Debug.WriteLine( "\nConvertToLines " + ( deleted ? "DELETED" : "INSERTED" ) );
			//Debug.WriteLine( data.ToString() );

			var list = new List<CharSnake>();

			foreach ( var s in chars )
			{
				//Debug.WriteLine( "Char " + s );

				int lStart, cStart, lMid, cMid, lEnd, cEnd;
				data.LineFromChar( deleted ? s.XStart : s.YStart, out lStart, out cStart );
				data.LineFromChar( deleted ? s.XMid : s.YMid, out lMid, out cMid );
				data.LineFromChar( deleted ? s.XEnd : s.YEnd, out lEnd, out cEnd );

				//Debug.WriteLine( "LINE:CHAR Start: " + lStart + ":" + cStart + " Mid: " + lMid + ":" + cMid + " End:" + lEnd + ":" + cEnd );

				if ( deleted ? s.ADeleted > 0 : s.BInserted > 0 )
				{
					for ( int l = lStart ; l <= lMid ; l++ )
					{
						var cs = new CharSnake();
						cs.Line = l;
						cs.IsDeleted = deleted;
						cs.CharStart = ( l == lStart ? cStart : 0 );
						var end = ( l == lMid ? cMid : data.LineLength( l ) );
						cs.Diff = end - cs.CharStart;
						cs.IsEOL = data.IsEOL( l, end );

						//Debug.WriteLine( "Diff " + cs + " = '" + data.Substring( cs.CharStart, cs.Diff ) + "'" );

						if ( cs.Diff > 0 || data.LineLength( l ) == 0 ) list.Add( cs );
					}
				}

				if ( s.DiagonalLength > 0 )
				{
					for ( int l = lMid ; l <= lEnd ; l++ )
					{
						var cs = new CharSnake();
						cs.Line = l;
						cs.IsDeleted = deleted;
						cs.CharStart = ( l == lMid ? cMid : 0 );
						var end = ( l == lEnd ? cEnd : data.LineLength( l ) );
						cs.Same = end - cs.CharStart;
						cs.IsEOL = data.IsEOL( l, end );

						//Debug.WriteLine( "Same " + cs + " = '" + data.Substring( cs.CharStart, cs.Same ) + "'" );

						if ( cs.Same > 0 || data.LineLength( l ) == 0 ) list.Add( cs );
					}
				}
			}

			return new SnakeWithCharResults( lineSnake, list );
		}

		class CompareCharsData
		{
			public int StartLine;
			public int LineCount;
			public char[] Chars { get; protected set; }

			List<LineData> Lines = new List<LineData>();

			public CompareCharsData( string[] sa, int x, int len )
			{
				StartLine = x;
				LineCount = len;

				var sb = new StringBuilder();

				int xc = 0;

				for ( int line = x ; line < x + len ; line++ )
				{
					Lines.Add( new LineData { Line = line, CharStart = xc, CharLength = sa[ line ].Length } );

					sb.Append( sa[ line ] );
					xc += sa[ line ].Length;
				}

				Chars = sb.ToString().ToCharArray();
			}

			class LineData
			{
				public int Line;
				public int CharStart;
				public int CharLength;

				public int CharEnd { get { return CharStart + CharLength; } }
			}

			public int LineLength( int line )
			{
				foreach ( var l in Lines )
					if ( l.Line == line )
						return l.CharLength;

				throw new ApplicationException( "LineLength: Not in line array" );
			}

			public int LineEnd( int line )
			{
				foreach ( var l in Lines )
					if ( l.Line == line )
						return l.CharEnd;

				throw new ApplicationException( "LineEnd: Not in line array" );
			}

			public bool IsEOL( int line, int c )
			{
				//Debug.WriteLine( "IsEOL( " + line + ", " + c + " )" );

				foreach ( var l in Lines )
					if ( l.Line == line )
						return ( c == l.CharLength );

				throw new ApplicationException( "IsEOL: Not in line array" );
			}

			public void LineFromChar( int c, out int line, out int @char )
			{
				foreach ( var l in Lines )
					if ( c <= l.CharEnd )
					{
						line = l.Line;
						@char = c - l.CharStart;

						//Debug.WriteLine( "LineFromChar( " + c + " ) => " + line + ":" + @char );

						return;
					}

				throw new ApplicationException( "LineFromChar( " + c + " ) Not in line array" );
			}

			public override string ToString()
			{
				return String.Format( "CompareCharsData: Lines: {0:N0} + {1:N0}\n{2}", StartLine, LineCount,
					String.Join( "\n", Lines.Select( ld => "CompareCharsData: Line: " + ld.Line + " Char: " + ld.CharStart + " + " + ld.CharLength + " = " + ld.CharEnd ).ToArray() ) );
			}

			public string Substring( int c, int len )
			{
				return new String( Chars, c, len );
			}
		}

		//-----------------------------------------------------------------------------------------
		// Compare string

		static void Compare( List<Snake> snakes,
			string[] pa, int N, string[] pb, int M,
			V VForward, V VReverse )
		{
			Debug.WriteLine( String.Format( "Compare STRING[] N: {0:N0}, M: {1:N0}", N, M ) );

			Compare( 0, snakes, pa, 0, N, pb, 0, M, VForward, VReverse );
		}

		static void Compare( int recursion, List<Snake> snakes,
			string[] pa, int a0, int N, string[] pb, int b0, int M,
			V VForward, V VReverse )
		{
			//Debug.WriteLine( new String( '-', recursion ) + recursion + "> Compare( " + a0 + ", " + b0 + " ) + ( " + N + ", " + M + " ) = ( " + ( a0 + N ) + ", " + ( b0 + M ) + " )" );

			if ( N == 0 && M > 0 )
			{
				var down = new Snake( a0, N, b0, M, true, a0, b0, 0, M, 0 );
				//Debug.WriteLine( "down: " + down );
				snakes.Add( down );
			}

			if ( M == 0 && N > 0 )
			{
				var right = new Snake( a0, N, b0, M, true, a0, b0, N, 0, 0 );
				//Debug.WriteLine( "right: " + right );
				snakes.Add( right );
			}

			if ( N <= 0 || M <= 0 ) return;

			SnakePair? middle = null;

			VForward[ 1 ] = 0;
			//VReverse[ N - M - 1 ] = N;

			middle = DiffCommon.CalcForD.MiddleSnake( pa, a0, N, pb, b0, M, VForward, VReverse );

			if ( middle == null ) throw new ApplicationException( "No middle snake" );

			var m = middle.Value;
			int d = middle.Value.D;

			//Debug.WriteLine( "d:" + d + " " + m.Forward + " " + m.Reverse );

			if ( d > 1 )
			{
				var xy = ( m.Forward != null ? m.Forward.StartPoint : m.Reverse.EndPoint );
				var uv = ( m.Reverse != null ? m.Reverse.StartPoint : m.Forward.EndPoint );

				Compare( recursion + 1, snakes, pa, a0, xy.X - a0, pb, b0, xy.Y - b0, VForward, VReverse );

				if ( m.Forward != null ) snakes.Add( m.Forward );
				if ( m.Reverse != null ) snakes.Add( m.Reverse );

				Compare( recursion + 1, snakes, pa, uv.X, a0 + N - uv.X, pb, uv.Y, b0 + M - uv.Y, VForward, VReverse );

			}
			else
			{
				if ( m.Forward != null && m.Reverse != null ) // check for overlapping diagonal
					if ( m.Forward.XMid - m.Forward.YMid == m.Reverse.XMid - m.Reverse.YMid )
					{
						m.Forward.DiagonalLength = m.Reverse.XMid - m.Forward.XMid;
						m.Reverse.DiagonalLength = 0;
					}

				if ( m.Forward != null )
				{
					// D0
					if ( m.Forward.XStart > a0 )
					{
						if ( m.Forward.XStart - a0 != m.Forward.YStart - b0 ) throw new ApplicationException( "Missed D0 forward" );
						snakes.Add( new Snake( a0, N, b0, M, true, a0, b0, 0, 0, m.Forward.XStart - a0 ) );
					}

					snakes.Add( m.Forward );
				}

				if ( m.Reverse != null )
				{
					snakes.Add( m.Reverse );

					// D0
					if ( m.Reverse.XStart < a0 + N )
					{
						if ( a0 + N - m.Reverse.XStart != b0 + M - m.Reverse.YStart ) throw new ApplicationException( "Missed D0 reverse" );
						snakes.Add( new Snake( a0, N, b0, M, true, m.Reverse.XStart, m.Reverse.YStart, 0, 0, a0 + N - m.Reverse.XStart ) );
					}
				}
			}
		}

		//-----------------------------------------------------------------------------------------
		// Compare char

		static void Compare( List<Snake> snakes,
			char[] pa, int N, char[] pb, int M,
			V VForward, V VReverse )
		{
			Debug.WriteLine( String.Format( "Compare CHAR[] N: {0:N0}, M: {1:N0}", N, M ) );

			Compare( 0, snakes, pa, 0, N, pb, 0, M, VForward, VReverse );
		}

		static void Compare( int recursion, List<Snake> snakes,
			char[] pa, int a0, int N, char[] pb, int b0, int M,
			V VForward, V VReverse )
		{
			//Debug.WriteLine( new String( '-', recursion ) + recursion + "> Compare( " + a0 + ", " + b0 + " ) + ( " + N + ", " + M + " ) = ( " + ( a0 + N ) + ", " + ( b0 + M ) + " )" );

			if ( N == 0 && M > 0 )
			{
				var down = new Snake( a0, N, b0, M, true, a0, b0, 0, M, 0 );
				//Debug.WriteLine( "down: " + down );
				snakes.Add( down );
			}

			if ( M == 0 && N > 0 )
			{
				var right = new Snake( a0, N, b0, M, true, a0, b0, N, 0, 0 );
				//Debug.WriteLine( "right: " + right );
				snakes.Add( right );
			}

			if ( N <= 0 || M <= 0 ) return;

			SnakePair? middle = null;

			VForward[ 1 ] = 0;
			//VReverse[ N - M - 1 ] = N;

			middle = DiffCommon.CalcForD.MiddleSnake( pa, a0, N, pb, b0, M, VForward, VReverse );

			if ( middle == null ) throw new ApplicationException( "No middle snake" );

			var m = middle.Value;
			int d = middle.Value.D;

			//Debug.WriteLine( "d:" + d + " " + m.Forward + " " + m.Reverse );

			if ( d > 1 )
			{
				var xy = ( m.Forward != null ? m.Forward.StartPoint : m.Reverse.EndPoint );
				var uv = ( m.Reverse != null ? m.Reverse.StartPoint : m.Forward.EndPoint );

				Compare( recursion + 1, snakes, pa, a0, xy.X - a0, pb, b0, xy.Y - b0, VForward, VReverse );

				if ( m.Forward != null ) snakes.Add( m.Forward );
				if ( m.Reverse != null ) snakes.Add( m.Reverse );

				Compare( recursion + 1, snakes, pa, uv.X, a0 + N - uv.X, pb, uv.Y, b0 + M - uv.Y, VForward, VReverse );

			}
			else
			{
				if ( m.Forward != null && m.Reverse != null ) // check for overlapping diagonal
					if ( m.Forward.XMid - m.Forward.YMid == m.Reverse.XMid - m.Reverse.YMid )
					{
						m.Forward.DiagonalLength = m.Reverse.XMid - m.Forward.XMid;
						m.Reverse.DiagonalLength = 0;
					}

				if ( m.Forward != null )
				{
					// D0
					if ( m.Forward.XStart > a0 )
					{
						if ( m.Forward.XStart - a0 != m.Forward.YStart - b0 ) throw new ApplicationException( "Missed D0 forward" );
						snakes.Add( new Snake( a0, N, b0, M, true, a0, b0, 0, 0, m.Forward.XStart - a0 ) );
					}

					snakes.Add( m.Forward );
				}

				if ( m.Reverse != null )
				{
					snakes.Add( m.Reverse );

					// D0
					if ( m.Reverse.XStart < a0 + N )
					{
						if ( a0 + N - m.Reverse.XStart != b0 + M - m.Reverse.YStart ) throw new ApplicationException( "Missed D0 reverse" );
						snakes.Add( new Snake( a0, N, b0, M, true, m.Reverse.XStart, m.Reverse.YStart, 0, 0, a0 + N - m.Reverse.XStart ) );
					}
				}
			}
		}

		//-----------------------------------------------------------------------------------------
		// MergeSnakes

		static IList<Snake> MergeSnakes( IList<Snake> snakes, int a0, int N, int b0, int M )
		{
			var r = new List<Snake>( snakes.Count );

			var c = new Snake( a0, N, b0, M, true, 0, 0, 0, 0, 0 );

			foreach ( var s in snakes )
			{
				if ( s.IsForward )
				{
					if ( ( s.ADeleted > 0 || s.BInserted > 0 ) && c.DiagonalLength > 0 )
					{
						r.Add( c );
						c = new Snake( a0, N, b0, M, true, c.XEnd, c.YEnd, 0, 0, 0 );
					}

					c.ADeleted += s.ADeleted;
					c.BInserted += s.BInserted;
					c.DiagonalLength += s.DiagonalLength;

					Debug.Assert( c.EndPoint == s.EndPoint );
				}
				else
				{
					c.DiagonalLength += s.DiagonalLength;

					if ( ( s.ADeleted > 0 || s.BInserted > 0 ) && c.DiagonalLength > 0 )
					{
						r.Add( c );
						c = new Snake( a0, N, b0, M, true, c.XEnd, c.YEnd, 0, 0, 0 );
					}

					c.ADeleted += s.ADeleted;
					c.BInserted += s.BInserted;

					Debug.Assert( c.EndPoint == s.StartPoint );
				}
			}

			if ( c.StartPoint != c.EndPoint ) r.Add( c );

			return r;
		}


		//-----------------------------------------------------------------------------------------

	}
}
