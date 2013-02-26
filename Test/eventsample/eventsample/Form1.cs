using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LogonEventLib;

namespace eventsample
{
	public partial class Form1 : Form
	{
		private LogonEventDispatcher m_dispatcher = new LogonEventDispatcher();

		public Form1()
		{
			InitializeComponent();

			m_dispatcher.SampleEvent += m_dispatcher_SampleEvent;
		}

		private delegate void _dispatcher_SampleEvent(string sEvent);

		void m_dispatcher_SampleEvent(string sEvent)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke((_dispatcher_SampleEvent)m_dispatcher_SampleEvent,new object[]{sEvent});
			}
			else
			{
				listBoxEvents.Items.Add(sEvent);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_dispatcher.ClosePipeListener();
		}
	}
}
