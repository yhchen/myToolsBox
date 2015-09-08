using System;
using System.Windows.Forms;

namespace publishUpdateList.Common
{
	class WinFormsKickstart
	{
		public event Action Loaded = delegate { };

		Form _Form;
		Timer _Timer;

		public WinFormsKickstart( Form form )
		{
			_Form = form;
			_Form.Load += StartTimer; // this reference keeps this instance alive
		}

		void StartTimer( object sender, EventArgs args )
		{
			_Timer = new Timer { Interval = 1 };
			_Timer.Tick += Tick;
			_Timer.Start();
		}

		void Tick( object sender, EventArgs args )
		{
			if ( _Timer == null ) return;

			_Timer.Dispose();
			_Timer = null;

			Loaded();

			_Form.Load -= StartTimer; // this instance can now be collected
		}
	}
}