using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Data
{

	//-----------------------------------------------------------------------------------------
	// Results

	public class Results
	{
		public long Space { get; protected set; }

		public IList<SnakeWithCharResults> Snakes { get; protected set; }

		public Results( long space, IList<SnakeWithCharResults> snakes )
		{
			Space = space;
			Snakes = snakes;
		}
	}

	//-----------------------------------------------------------------------------------------
	// SnakeWithCharResults

	public class SnakeWithCharResults
	{
		public Snake Snake { get; protected set; }

		public List<CharSnake> CharSnakes { get; protected set; }

		public SnakeWithCharResults( Snake snake, List<CharSnake> charSnakes )
		{
			Snake = snake;
			CharSnakes = charSnakes;
		}
	}

	//-----------------------------------------------------------------------------------------
	// CharSnake

	public class CharSnake
	{
		public int Line;
		public bool IsDeleted;

		public int CharStart;
		public int Diff;
		public int Same;
		public bool IsEOL;

		public override string ToString()
		{
			return String.Format( "CharSnake: Line:{0:N0} IsDeleted:{1} CharStart:{2:N0} Diff:{3:N0} Same:{4:N0} IsEOL:{5}",
				Line, IsDeleted, CharStart, Diff, Same, IsEOL );
		}
	}

	//-----------------------------------------------------------------------------------------
	// SnakePair

	struct SnakePair
	{
		public int D;
		public Snake Forward;
		public Snake Reverse;
	}

	//-----------------------------------------------------------------------------------------
	// Point

	public struct Point
	{
		public int X, Y;

		public Point( int x, int y ) { X = x; Y = y; }

		public static bool operator ==( Point left, Point right ) { return ( ( left.X == right.X ) && ( left.Y == right.Y ) ); }
		public static bool operator !=( Point left, Point right ) { return !( left == right ); }

		public override int GetHashCode() { return ( X ^ Y ); }

		public override bool Equals( object obj )
		{
			if ( !( obj is Point ) ) return false;
			Point point = ( Point ) obj;
			return ( ( point.X == this.X ) && ( point.Y == this.Y ) );
		}
	}

	//-----------------------------------------------------------------------------------------
	// Snake

	public class Snake
	{
		public int XStart;
		public int YStart;
		public int ADeleted;
		public int BInserted;
		public int DiagonalLength;
		public bool IsForward = true;
		public int DELTA = 0;

		public Point StartPoint { get { return new Point( XStart, YStart ); } }

		public int XMid { get { return ( IsForward ? XStart + ADeleted : XStart - ADeleted ); } }
		public int YMid { get { return ( IsForward ? YStart + BInserted : YStart - BInserted ); } }
		public Point MidPoint { get { return new Point( XMid, YMid ); } }

		public int XEnd { get { return ( IsForward ? XStart + ADeleted + DiagonalLength : XStart - ADeleted - DiagonalLength ); } }
		public int YEnd { get { return ( IsForward ? YStart + BInserted + DiagonalLength : YStart - BInserted - DiagonalLength ); } }
		public Point EndPoint { get { return new Point( XEnd, YEnd ); } }

		public override string ToString()
		{
			return "Snake " + ( IsForward ? "F" : "R" ) + ": ( " + XStart + ", " + YStart + " ) + ( " +
				( ADeleted > 0 ? "D" + ADeleted : "" ) +
				"," +
				( BInserted > 0 ? " I" + BInserted : "" ) +
				" ) + " + DiagonalLength + " -> ( " + XEnd + ", " + YEnd + " )" +
				" k=" + ( XMid - YMid );
		}

		Snake( bool isForward, int delta )
		{
			Debug.Assert( !( isForward && delta != 0 ) );

			IsForward = isForward;

			if ( !IsForward ) DELTA = delta;
		}

		void RemoveStubs( int a0, int N, int b0, int M )
		{
			if ( IsForward )
			{
				if ( XStart == a0 && YStart == b0 - 1 && BInserted == 1 )
				{
					//Debug.WriteLine( "Stub before:" + this );

					YStart = b0;
					BInserted = 0;

					//Debug.WriteLine( "Stub after:" + this );
				}
			}
			else
			{
				if ( XStart == a0 + N && YStart == b0 + M + 1 && BInserted == 1 )
				{
					//Debug.WriteLine( "Stub before: " + this );

					YStart = b0 + M;
					BInserted = 0;

					//Debug.WriteLine( "Stub after:" + this );
				}
			}

			Debug.Assert( a0 <= XStart && XStart <= a0 + N );
			Debug.Assert( a0 <= XMid && XMid <= a0 + N );
			Debug.Assert( a0 <= XEnd && XEnd <= a0 + N );

			Debug.Assert( b0 <= YStart && YStart <= b0 + M );
			Debug.Assert( b0 <= YMid && YMid <= b0 + M );
			Debug.Assert( b0 <= YEnd && YEnd <= b0 + M );

			Debug.Assert( XMid - YMid == XEnd - YEnd ); // k
		}

		public Snake( int a0, int N, int b0, int M, bool isForward, int xStart, int yStart, int aDeleted, int bInserted, int diagonal )
		{
			XStart = xStart;
			YStart = yStart;
			ADeleted = aDeleted;
			BInserted = bInserted;
			DiagonalLength = diagonal;
			IsForward = isForward;

			RemoveStubs( a0, N, b0, M );
		}

		public Snake( int a0, int N, int b0, int M, bool isForward, int xStart, int yStart, bool down, int diagonal )
		{
			XStart = xStart;
			YStart = yStart;
			ADeleted = down ? 0 : 1;
			BInserted = down ? 1 : 0;
			DiagonalLength = diagonal;
			IsForward = isForward;

			RemoveStubs( a0, N, b0, M );
		}

		//-----------------------------------------------------------------------------------------
		// Calculate string

		internal Snake( int a0, int N, int b0, int M, bool forward, int delta, V V, int k, int d, string[] pa, string[] pb )
			: this( forward, delta )
		{
			Calculate( V, k, d, pa, a0, N, pb, b0, M );
		}

		internal Snake Calculate( V V, int k, int d, string[] pa, int a0, int N, string[] pb, int b0, int M )
		{
			if ( IsForward ) return CalculateForward( V, k, d, pa, a0, N, pb, b0, M );

			return CalculateBackward( V, k, d, pa, a0, N, pb, b0, M );
		}

		Snake CalculateForward( V V, int k, int d, string[] pa, int a0, int N, string[] pb, int b0, int M )
		{
			bool down = ( k == -d || ( k != d && V[ k - 1 ] < V[ k + 1 ] ) );

			int xStart = down ? V[ k + 1 ] : V[ k - 1 ];
			int yStart = xStart - ( down ? k + 1 : k - 1 );

			int xEnd = down ? xStart : xStart + 1;
			int yEnd = xEnd - k;

			int snake = 0;
			while ( xEnd < N && yEnd < M && pa[ xEnd + a0 ] == pb[ yEnd + b0 ] ) { xEnd++; yEnd++; snake++; }

			XStart = xStart + a0;
			YStart = yStart + b0;
			ADeleted = down ? 0 : 1;
			BInserted = down ? 1 : 0;
			DiagonalLength = snake;

			RemoveStubs( a0, N, b0, M );

			return this;
		}

		Snake CalculateBackward( V V, int k, int d, string[] pa, int a0, int N, string[] pb, int b0, int M )
		{
			bool up = ( k == d + DELTA || ( k != -d + DELTA && V[ k - 1 ] < V[ k + 1 ] ) );

			int xStart = up ? V[ k - 1 ] : V[ k + 1 ];
			int yStart = xStart - ( up ? k - 1 : k + 1 );

			int xEnd = up ? xStart : xStart - 1;
			int yEnd = xEnd - k;

			int snake = 0;
			while ( xEnd > 0 && yEnd > 0 && pa[ xEnd + a0 - 1 ] == pb[ yEnd + b0 - 1 ] ) { xEnd--; yEnd--; snake++; }

			XStart = xStart + a0;
			YStart = yStart + b0;
			ADeleted = up ? 0 : 1;
			BInserted = up ? 1 : 0;
			DiagonalLength = snake;

			RemoveStubs( a0, N, b0, M );

			return this;
		}
		//-----------------------------------------------------------------------------------------
		// Calculate char

		internal Snake( int a0, int N, int b0, int M, bool forward, int delta, V V, int k, int d, char[] pa, char[] pb )
			: this( forward, delta )
		{
			Calculate( V, k, d, pa, a0, N, pb, b0, M );
		}

		internal Snake Calculate( V V, int k, int d, char[] pa, int a0, int N, char[] pb, int b0, int M )
		{
			if ( IsForward ) return CalculateForward( V, k, d, pa, a0, N, pb, b0, M );

			return CalculateBackward( V, k, d, pa, a0, N, pb, b0, M );
		}

		Snake CalculateForward( V V, int k, int d, char[] pa, int a0, int N, char[] pb, int b0, int M )
		{
			bool down = ( k == -d || ( k != d && V[ k - 1 ] < V[ k + 1 ] ) );

			int xStart = down ? V[ k + 1 ] : V[ k - 1 ];
			int yStart = xStart - ( down ? k + 1 : k - 1 );

			int xEnd = down ? xStart : xStart + 1;
			int yEnd = xEnd - k;

			int snake = 0;
			while ( xEnd < N && yEnd < M && pa[ xEnd + a0 ] == pb[ yEnd + b0 ] ) { xEnd++; yEnd++; snake++; }

			XStart = xStart + a0;
			YStart = yStart + b0;
			ADeleted = down ? 0 : 1;
			BInserted = down ? 1 : 0;
			DiagonalLength = snake;

			RemoveStubs( a0, N, b0, M );

			return this;
		}

		Snake CalculateBackward( V V, int k, int d, char[] pa, int a0, int N, char[] pb, int b0, int M )
		{
			bool up = ( k == d + DELTA || ( k != -d + DELTA && V[ k - 1 ] < V[ k + 1 ] ) );

			int xStart = up ? V[ k - 1 ] : V[ k + 1 ];
			int yStart = xStart - ( up ? k - 1 : k + 1 );

			int xEnd = up ? xStart : xStart - 1;
			int yEnd = xEnd - k;

			int snake = 0;
			while ( xEnd > 0 && yEnd > 0 && pa[ xEnd + a0 - 1 ] == pb[ yEnd + b0 - 1 ] ) { xEnd--; yEnd--; snake++; }

			XStart = xStart + a0;
			YStart = yStart + b0;
			ADeleted = up ? 0 : 1;
			BInserted = up ? 1 : 0;
			DiagonalLength = snake;

			RemoveStubs( a0, N, b0, M );

			return this;
		}

		//-----------------------------------------------------------------------------------------

	}

	//-----------------------------------------------------------------------------------------
	// V

	class V
	{
		public bool IsForward { get; private set; }
		public int N { get; private set; }
		public int M { get; private set; }

		public int _Max;
		public int _Delta;
		public int[] _Array;

		public int this[ int k ]
		{
			get
			{
				return _Array[ k - _Delta + _Max ];
			}

			set
			{
				_Array[ k - _Delta + _Max ] = value;
			}
		}

		public int Y( int x, int k ) { return x - k; }

		public long Memory { get { return sizeof( bool ) + sizeof( int ) * ( 4 + _Array.Length ); } }

		V() { }

		public V( int n, int m, bool forward, bool linear )
		{
			Debug.Assert( n >= 0 && m >= 0, "V.ctor N:" + n + " M:" + m );

			IsForward = forward;
			N = n;
			M = m;

			_Max = ( linear ? ( n + m ) / 2 + 1 : n + m );
			if ( _Max <= 0 ) _Max = 1;

			_Array = new int[ 2 * _Max + 1 ];

			InitStub( n, m, _Max );
		}

		public void InitStub( int n, int m, int max )
		{
			if ( IsForward ) this[ 1 ] = 0; // stub for forward
			else
			{
				_Delta = n - m;
				this[ n - m - 1 ] = n; // stub for backward
			}

			CheckBounds( max );
		}

		[Conditional( "DEBUG" )]
		void CheckBounds( int max )
		{
			var m = String.Format(
					"max: {0:N0}, delta: {1:N0} => [ {2:N0} .. {3:N0} ] {4}",
					max, _Delta, _Delta - max, _Delta + max, ToString() );

			if ( _Max - max < 0 || _Max + max >= _Array.Length )
				Debug.Assert( false, "CheckBounds failed: " + m );
			else
			{
				//Debug.WriteLine( "CheckBounds good: " + m );
			}
		}

		public override string ToString()
		{
			return "V " + _Array.Length + " [ " + ( _Delta - _Max ) + " .. " + _Delta + " .. " + ( _Delta + _Max ) + " ]";
		}
	}

	//-----------------------------------------------------------------------------------------
}
