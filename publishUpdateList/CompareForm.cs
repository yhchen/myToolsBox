#define MEMORYX

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using publishUpdateList.Common;
using Data;
using System.Web;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NDiffDiff
{
	public partial class CompareForm : Form
	{
		bool DISPLAY_ALL = ( 1 == 0 );

		public int DisplayA { set { lblA.Text = String.Format( "File A: {0:N0} lines", value ); } }
		public int DisplayB { set { lblB.Text = String.Format( "File B: {0:N0} lines", value ); } }
		public int DisplayLines { set { lblLines.Text = String.Format( "Diff: {0:N0} lines", value ); } }

		public double DisplayRead { set { lblRead.Text = String.Format( "Read: {0:N2} ms", value ); } }
		public double DisplayDiff { set { lblDiff.Text = String.Format( "Diff: {0:N2} ms", value ); } }
		public double DisplayOutput { set { lblOutput.Text = String.Format( "Output: {0:N2} ms", value ); } }
		public double DisplayTotal { set { lblTotal.Text = String.Format( "Time: {0:N2} ms", value ); } }
		public double DisplayJit { set { lblJit.Text = String.Format( "Jit: {0:N1} ms", value ); } }
		public long DisplaySpace { set { lblSpace.Text = String.Format( "Space: {0:N0} bytes", value ); } }

        private string m_sFileOld;
        private string m_sFileNew;
        private string m_sCommitURL;
        public CompareForm()
        {
            InitializeComponent();

            new WinFormsKickstart(this).Loaded += HandlesLoaded;
        }

		public CompareForm(string sFileOld, string sFileNew, string sCommitURL)
		{
            m_sFileOld = sFileOld;
            m_sFileNew = sFileNew;
            m_sCommitURL = sCommitURL;
			InitializeComponent();

			new WinFormsKickstart( this ).Loaded += HandlesLoaded;
		}

		void HandlesLoaded()
		{
			new Thread( Run ) { IsBackground = true, Priority = ThreadPriority.Highest }.Start();
		}

		void Run()
		{
			try
			{
				RunCore();
			}
			catch ( Exception x )
			{
				BeginInvoke( ( Action ) ( () => MessageBox.Show( x.ToString(), "Exception" ) ) );
			}
		}

		void RunCore()
		{
			var args = Environment.GetCommandLineArgs();

			string[] m_fileNew = "s-sas".Split( '-' );
			string[] m_fileOld = "s-sbs".Split( '-' );

			var sw = Stopwatch.StartNew();

			DiffLinear.Diff.Compare( m_fileNew, m_fileOld, true ); // JIT

			var tJit = sw.Elapsed;

#if MEMORY
			var p = Process.GetCurrentProcess();
			var pws = p.PeakWorkingSet64;
#endif

			if ( args.Length == 3 )
			{
				m_fileNew = File.ReadAllLines( args[ 1 ] );
				m_fileOld = File.ReadAllLines( args[ 2 ] );
			}
			else
			{
                m_fileNew = File.ReadAllLines(m_sFileOld);
				m_fileOld = File.ReadAllLines(m_sFileNew);
			}

			var tRead = sw.Elapsed;

#if MEMORY
			GC.Collect();
#endif

			Results r = DiffLinear.Diff.Compare( m_fileNew, m_fileOld, true );

			var tDiff = sw.Elapsed;

#if MEMORY
			GC.Collect();
#endif

			if ( r != null )
			{
				var sb = new StringBuilder();

				if ( !DISPLAY_ALL ) { sb.Append( D.Normal ); sb.Append( "---START---\n" ); }

				foreach ( var snakeWithChars in r.Snakes )
				{
					var snake = snakeWithChars.Snake;

					if ( snakeWithChars.CharSnakes == null )
					{
						for ( int i = snake.XStart ; i < snake.XMid ; i++ ) { sb.Append( D.Deleted ); sb.Append( m_fileNew[ i ] ); sb.Append( '\n' ); }
						for ( int i = snake.YStart ; i < snake.YMid ; i++ ) { sb.Append( D.Inserted ); sb.Append( m_fileOld[ i ] ); sb.Append( '\n' ); }
					}
					else
					{
						foreach ( var s in snakeWithChars.CharSnakes )
						{
							if ( s.IsDeleted )
							{
								if ( s.Diff > 0 ) { sb.Append( D.Deleted ); sb.Append( m_fileNew[ s.Line ].Substring( s.CharStart, s.Diff ) ); }
								if ( s.Same > 0 ) { sb.Append( D.DeletedLight ); sb.Append( m_fileNew[ s.Line ].Substring( s.CharStart, s.Same ) ); }
							}
							else
							{
								if ( s.Diff > 0 ) { sb.Append( D.Inserted ); sb.Append( m_fileOld[ s.Line ].Substring( s.CharStart, s.Diff ) ); }
								if ( s.Same > 0 ) { sb.Append( D.InsertedLight ); sb.Append( m_fileOld[ s.Line ].Substring( s.CharStart, s.Same ) ); }
							}

							if ( s.IsEOL ) sb.Append( '\n' );
						}
					}

					if ( snake != null )
						if ( DISPLAY_ALL )
						{
							for ( int i = snake.XMid ; i < snake.XEnd ; i++ ) { sb.Append( D.Normal ); sb.Append( m_fileNew[ i ] ); sb.Append( '\n' ); }
						}
						else
						{
							if ( snake.DiagonalLength > 0 )
							{
								sb.Append( D.Normal );
								sb.Append(
									String.Format( "=== A [ {0:N0} .. {1:N0} ] === {2:N0} === B [ {3:N0} .. {4:N0} ] ===\n",
									snake.XMid + 1, snake.XEnd, snake.DiagonalLength, snake.YMid + 1, snake.YEnd ) );
							}
						}
				}

				if ( !DISPLAY_ALL ) { sb.Append( D.Normal ); sb.Append( "---END---\n" ); }

				sb.Append( '\n' );

				_Display.Result = sb.ToString();
			}

#if MEMORY
			p.Refresh();
			Text += String.Format( " - {0:N0} KB", ( p.PeakWorkingSet64 - pws ) / 1024f );
#endif

			sw.Stop();

			BeginInvoke( ( Action ) ( () =>
				{
					//wb.DocumentText = html;

					DisplayA = m_fileNew.Length;
					DisplayB = m_fileOld.Length;

					if ( r != null )
					{
						DisplayLines = r.Snakes.Sum( s => ( s.Snake == null ? 0 : s.Snake.ADeleted + s.Snake.BInserted ) );
						DisplaySpace = r.Space;
					}

					DisplayRead = tRead.TotalMilliseconds - tJit.TotalMilliseconds;
					DisplayDiff = tDiff.TotalMilliseconds - tRead.TotalMilliseconds;
					DisplayOutput = sw.Elapsed.TotalMilliseconds - tDiff.TotalMilliseconds;
					DisplayTotal = sw.Elapsed.TotalMilliseconds - tJit.TotalMilliseconds;
					DisplayJit = tJit.TotalMilliseconds;
					//Close();
				} ) );
		}

		//static string HtmlEncode( string s )
		//{
		//    return HttpUtility.HtmlEncode( s ).Replace( " ", "&nbsp;" ).Replace( "\t", "&nbsp;&nbsp;&nbsp;" );
		//}

		static Random r = new Random();
		string[] Gen()
		{
			var l = new List<string>();
			var lines = r.Next( 1000 ) + 500;
			for ( int i = 0 ; i < lines ; i++ )
				l.Add( ( ( char ) ( 'a' + r.Next( 26 ) ) ).ToString() );

			return l.ToArray();
		}

		string[] GenLines()
		{
			var l = new List<string>();
			var sb = new StringBuilder();
			var lines = r.Next( 1000 ) + 500;
			for ( int i = 0 ; i < lines ; i++ )
			{
				sb.Length = 0;
				var chars = r.Next( 100 );
				for ( int c = 0 ; c < chars ; c++ )
					sb.Append( ( ( char ) ( 'a' + r.Next( 26 ) ) ).ToString() );
				l.Add( sb.ToString() );
			}

			return l.ToArray();
		}

		string[] Alter( string[] a )
		{
			var len = a.Length;
			var b = new string[ len ];
			a.CopyTo( b, 0 );

			for ( int line = 0 ; line < 100 ; line++ )
			{
				var l = r.Next( len );
				var c = r.Next( a[ l ].Length );

				switch ( r.Next( 3 ) )
				{
					case 0: b[ l ] = a[ l ].Substring( 0, c ) + "INSERTED" + a[ l ].Substring( c ); break;
					case 1: b[ l ] = a[ l ].Substring( 0, c ); break;
					case 2: b[ l ] = a[ l ].Substring( 0, c ) + a[ l ].Substring( a[ l ].Length - c ); break;
					default: break;
				}
			}

			return b;
		}

		string[] GetResource( string name )
		{
			using ( var s = Assembly.GetEntryAssembly().GetManifestResourceStream( name ) ) return SplitStream( s );
		}

		string[] SplitStream( Stream s )
		{
			var list = new List<string>();

			using ( var r = new StreamReader( s ) )
				while ( !r.EndOfStream )
					list.Add( r.ReadLine() );

			return list.ToArray();
		}

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                HttpPost poster = new HttpPost();
                string strNewFiles = File.ReadAllText(m_sFileNew, GlobalsConfig.defaultEncoder);
                poster.addPostValues("content", strNewFiles, false);
                string strFeedback = poster.PostWebRequest(m_sCommitURL, GlobalsConfig.defaultEncoder);
                if (strFeedback == "OK")
                {
                    string strsuccess = "======================================\r\n"
                        + "=\r\n= 提交成功\r\n=\r\n======================================";
                    MessageBox.Show(this, strsuccess);
                    // 打开检查用网页
                    System.Diagnostics.Process.Start(ConfigureManager.getCheckUrl());
                    Close();
                }
                else
                {
                    MessageBox.Show(this, strFeedback);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
	}
}


// old html builder

//string html = String.Empty;
//if ( r != null )
//{
//var sb = new StringBuilder();

//    sb.Append( "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n" );
//    sb.Append( "<html>\n<head>\n<style>\n" );
//    sb.Append( "BODY { font-family: Verdana; font-size: 10pt }\n" );
//    sb.Append( ".deleted { background: #F00; color: white; }\n" );
//    sb.Append( ".inserted { background: #0F0; }\n" );
//    sb.Append( ".deletedLight { background: #FCC; }\n" );
//    sb.Append( ".insertedLight { background: #CFC; }\n" );
//    sb.Append( "</style>\n</head>\n<body>\n" );
//    if ( !DISPLAY_ALL ) sb.Append( "---START---<br />\n" );
//    foreach ( var snakeWithChars in r.Snakes )
//    {
//        var snake = snakeWithChars.Snake;

//        if ( snakeWithChars.CharSnakes == null )
//        {
//            for ( int i = snake.XStart ; i < snake.XMid ; i++ ) { sb.Append( "<span class='deleted'>" ); sb.Append( HtmlEncode( a[ i ] ) ); sb.Append( "</span><br />\n" ); }
//            for ( int i = snake.YStart ; i < snake.YMid ; i++ ) { sb.Append( "<span class='inserted'>" ); sb.Append( HtmlEncode( b[ i ] ) ); sb.Append( "</span><br />\n" ); }
//        }
//        else
//        {
//            foreach ( var s in snakeWithChars.CharSnakes )
//            {
//                if ( s.IsDeleted )
//                {
//                    if ( s.Diff > 0 ) sb.Append( "<span class='deleted'>" + HtmlEncode( a[ s.Line ].Substring( s.CharStart, s.Diff ) ) + "</span>" );
//                    if ( s.Same > 0 ) sb.Append( "<span class='deletedLight'>" + HtmlEncode( a[ s.Line ].Substring( s.CharStart, s.Same ) ) + "</span>" );
//                }
//                else
//                {
//                    if ( s.Diff > 0 ) sb.Append( "<span class='inserted'>" + HtmlEncode( b[ s.Line ].Substring( s.CharStart, s.Diff ) ) + "</span>" );
//                    if ( s.Same > 0 ) sb.Append( "<span class='insertedLight'>" + HtmlEncode( b[ s.Line ].Substring( s.CharStart, s.Same ) ) + "</span>" );
//                }

//                if ( s.IsEOL ) sb.Append( "<br />\n" );
//            }
//        }

//        if ( snake != null )
//            if ( DISPLAY_ALL )
//            {
//                for ( int i = snake.XMid ; i < snake.XEnd ; i++ ) { sb.Append( HttpUtility.HtmlEncode( a[ i ] ) ); sb.Append( "<br />\n" ); }
//            }
//            else
//            {
//                if ( snake.DiagonalLength > 0 )
//                    sb.Append( String.Format( "=== A [ {0:N0} .. {1:N0} ] === {2:N0} === B [ {3:N0} .. {4:N0} ] ===<br />\n", snake.XMid + 1, snake.XEnd, snake.DiagonalLength, snake.YMid + 1, snake.YEnd ) );
//            }
//    }
//    if ( !DISPLAY_ALL ) sb.Append( "---END---\n" );

//    sb.Append( "</body>\n</html>\n" );

//    html = sb.ToString();
//}