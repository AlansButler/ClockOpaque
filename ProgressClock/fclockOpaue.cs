using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SpeechLib;
using System.Resources;
using System.Threading;
using System.Net;
using System.Net.Cache;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Configuration;

namespace ClockOpaque
{



/// <summary>
/// Summary description for Form1.
/// </summary>
public class formClockOpaque : System.Windows.Forms.Form
	{
	private System.Windows.Forms.Timer timerDrawClock;
	private System.ComponentModel.IContainer components;
	private float x;
	private float y;
	private float lastX;
	private float lastY;
	//private Single angle;
	//private double angleSeconds;
	//private double oldAngleSeconds;
	private PointF rotatePoint;
	private StringFormat strFormat;
	//private StringAlignment strAlignment;
	private SolidBrush drawBrush;
	private SolidBrush drawBrushOutline;
    private SolidBrush drawBrushAl;
	private SolidBrush drawBrushNext;
	private SolidBrush drawBrushColor;
	private SolidBrush drawBrushHeadline;
	private HatchBrush drawBrushHatch;
	//private TextureBrush drawBrushMinHand;
	//private LinearGradientBrush drawBrushGradient;
	private List<String> headlineDirt;

	private Font drawFont;
	private Font drawFontDate;
	private Font drawFontHeadlineTitle;
	private Font drawFontHeadlineDesc;
    private Font drawFontHours;
    private Font drawFontMinutes;
	private Font drawFontSeconds;
	private Font drawFontNumerator;
	private Font drawFontDenominator;
	private Font drawFontTiny;
	private Matrix myMatrix;
	private Matrix myMatrixSeconds;
	private int red;
	private int green;
	private int blue;
	private Pen pen;
    private string cbValue = String.Empty;

	public int countDown;
	//public Graphics g;
	public Random rnd;
	public Rectangle rc;
	public Thread thread;
    public Thread threadBaromDiff;
    public Thread threadDrawClock;
	public Thread threadHide;
	public Thread threadTemp;
	public Thread threadNews;
    public Thread threadStopApp;
	
	float RPx;
	int transparentCount;
	const int invisibleInterval = 400;
	const double opacityStd = 0.5;  //0.3;

	//LinearGradientBrush lgBrush;
	Point p1;
	Point p2;
	Point p3;
	int sec;
	//int alpha;
	//int antiAlpha;
	SpVoice speech;

	string lastDrawStringFrac = "*";
	public List<LocationInfo> LocationInfos = new List<LocationInfo>();
	NewsSources newsSources = new NewsSources();

	public DayTimeInfo dtInfo;
	private Panel pnlGraph;
    private Panel pnlWeather;
    private formClockOpaque.pnlTimeAl pnlTime;
    private System.Windows.Forms.Timer timerGeneral;
	private System.Windows.Forms.Timer timerHide;
	private BackgroundWorker BW;
	public string currentRSSURL;
    public Boolean silent = false;
	

	public static Object synchronizeVariable = "locking variable";


    public ConvertSafe cnvSafe;


	public formClockOpaque()
	{
	//
	// Required for Windows Form Designer support
	//
	InitializeComponent();
	//this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
	//this.SetStyle(ControlStyles.UserPaint, true);
	//this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		//ControlStyles.OptimizedDoubleBuffer
	//this.SetStyle(ControlStyles.Opaque, true);
	//
	// TODO: Add any constructor code after InitializeComponent call
	//
    cnvSafe = new ConvertSafe();
    this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
	}


    private class pnlTimeAl: Panel
    {
        public pnlTimeAl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint  | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
    
    

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
	    if (disposing)
	    {
	        if (components != null)
	        {
	        components.Dispose();
	        }
	    }

	base.Dispose(disposing);
	}

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
			this.components = new System.ComponentModel.Container();
			this.timerDrawClock = new System.Windows.Forms.Timer(this.components);
			this.pnlGraph = new System.Windows.Forms.Panel();
			this.timerGeneral = new System.Windows.Forms.Timer(this.components);
			this.pnlWeather = new System.Windows.Forms.Panel();
			this.timerHide = new System.Windows.Forms.Timer(this.components);
			this.BW = new System.ComponentModel.BackgroundWorker();
			this.pnlTime = new ClockOpaque.formClockOpaque.pnlTimeAl();
			this.SuspendLayout();
			// 
			// timerDrawClock
			// 
			this.timerDrawClock.Interval = 900;
			this.timerDrawClock.Tick += new System.EventHandler(this.timerDrawClock_Tick);
			// 
			// pnlGraph
			// 
			this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlGraph.Location = new System.Drawing.Point(0, 0);
			this.pnlGraph.Name = "pnlGraph";
			this.pnlGraph.Size = new System.Drawing.Size(1136, 100);
			this.pnlGraph.TabIndex = 1;
			this.pnlGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlGraph_Paint);
			this.pnlGraph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlGraph_MouseClick);
			this.pnlGraph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formClockOpaque_MouseDown);
			// 
			// timerGeneral
			// 
			this.timerGeneral.Enabled = true;
			this.timerGeneral.Interval = 7000;
			this.timerGeneral.Tick += new System.EventHandler(this.timerGeneral_Tick);
			// 
			// pnlWeather
			// 
			this.pnlWeather.Location = new System.Drawing.Point(0, 100);
			this.pnlWeather.MaximumSize = new System.Drawing.Size(137, 556);
			this.pnlWeather.Name = "pnlWeather";
			this.pnlWeather.Size = new System.Drawing.Size(137, 556);
			this.pnlWeather.TabIndex = 5;
			this.pnlWeather.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlWeather_Paint);
			this.pnlWeather.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formClockOpaque_MouseDown);
			this.pnlWeather.MouseHover += new System.EventHandler(this.Form1_MouseHover);
			// 
			// timerHide
			// 
			this.timerHide.Interval = 400;
			this.timerHide.Tick += new System.EventHandler(this.timerHide_Tick);
			// 
			// BW
			// 
			this.BW.WorkerSupportsCancellation = true;
			this.BW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BW_DoWork);
			this.BW.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BW_ProgressChanged);
			this.BW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BW_RunWorkerCompleted);
			// 
			// pnlTime
			// 
			this.pnlTime.Location = new System.Drawing.Point(137, 100);
			this.pnlTime.Name = "pnlTime";
			this.pnlTime.Size = new System.Drawing.Size(987, 626);
			this.pnlTime.TabIndex = 6;
			this.pnlTime.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTime_Paint);
			this.pnlTime.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlTime_MouseClick);
			this.pnlTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formClockOpaque_MouseDown);
			this.pnlTime.Validated += new System.EventHandler(this.pnlTime_Validated);
			// 
			// formClockOpaque
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(1136, 716);
			this.Controls.Add(this.pnlTime);
			this.Controls.Add(this.pnlWeather);
			this.Controls.Add(this.pnlGraph);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.SystemColors.Control;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "formClockOpaque";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.SystemColors.Control;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClockOpaque_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formClockOpaque_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formClockOpaque_MouseDown);
			this.MouseHover += new System.EventHandler(this.Form1_MouseHover);
			this.ResumeLayout(false);

	}
	#endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {

      Application.Run(new formClockOpaque());
    }

    private void DrawClock()
    {
        
        while (!dtInfo.stop)
        {
            //pnlTime.Paint += pnlTime_Paint;
            
            pnlTime.Invalidate();  //Causes onPaint to be called, i think.
            //pnlTime.Update(); //Forces synchronous paint.
            //pnlTime.Refresh();

            Thread.Sleep(100);
        }
    }

	private void timerDrawClock_Tick(object sender, System.EventArgs e)
	{
		//Timer interval should be 167 to turn rotate back on.
		//int v;
		//v = DateTime.Now.Hour;
		//v = v - 6;
		//if (v < pbHour.Minimum)
		//	v = pbHour.Minimum;
		//if (v > pbHour.Maximum)
		//	v = pbHour.Maximum;
		//	if (v < 0) 
		//		v = 0;
		//		if (v > 10)
		//		v = 10;

		Text = DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToShortTimeString();
		//
		//label2.Text = DateTime.Now.ToShortDateString() +' '+ DateTime.Now.ToShortTimeString();

        //(sender as System.Windows.Forms.Timer).Interval = 5000; //was 32;// 241 - (DateTime.Now.Hour * 10);

        //pnlTime.Refresh();
        //pnlTime.Update();
        pnlTime.Paint += pnlTime_Paint;
		pnlTime.Invalidate();  //Causes onPaint to be called, i think.
		

		/*
		if (transparentCount > invisibleInterval)
		{

			if (Opacity != 0.2)
			{
				Opacity = 0.2;
			}


			if (Opacity < 0.4)
			{
				Opacity = Opacity + 0.02;
			}
			if (Opacity < 0.1)
			{
				Opacity = 0.1;
			}




			if (alpha < 255)
			{
				alpha++;
			}

			//Opacity = 1.0;

			//only works if timer interval is odd.


			if ((DateTime.Now.Second % 2) == 0)
			{
				alpha = 254;
				antiAlpha = 1;
			}
			else
			{
				alpha = 254;
				antiAlpha = 1;
			}


			//drawBrush.Color = Color.FromArgb(alpha, 255, 255, 255);
			//drawBrushOutline.Color = Color.FromArgb(alpha, 0, 0, 0);
			//drawBrushHeadline.Color = Color.FromArgb(alpha, 255, 255, 255);
			//(sender as System.Windows.Forms.Timer).Interval = 241 - (DateTime.Now.Hour * 10);
			//pnlTime.Invalidate();  //Causes onPaint to be called, i think.

			//this.Invalidate(rc);
			//this.Update();

			//	protected void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)

		}
		else
		{
			transparentCount++;
		}

		//pnlGraph.Invalidate();

		//if (Decimal.Remainder(DateTime.Now.Second, 5)==0)
		//{
		//		Visible = true;
		//}
		//else
		//{
		//	Visible = false;
		//}

		// Retrieve the working rectangle from the Screen class
		// using the PrimaryScreen and the WorkingArea properties.
		//System.Drawing.Rectangle workingRectangle = 
		//	Screen.PrimaryScreen.WorkingArea;

		// Set the size of the form slightly less than size of 
		// working rectangle.
		//this.Size = new System.Drawing.Size(
		//		workingRectangle.Width-10, workingRectangle.Height-10);


		//if (e.Button.ToString()
		 */
	}
	
	private void timerGeneral_Tick(object sender, EventArgs e)
	{
		//pnlGraph.Invalidate();
    if ((System.DateTime.Now.Second >25) && (DateTime.Now.Second < 35))
    {
        pnlWeather.Visible = true;
        pnlWeather.Visible = false;
        pnlWeather.Invalidate();
    }
    else
    {
        pnlWeather.Visible = false;
    }



		pnlGraph.Invalidate();


	}

	private void timerHide_Tick(object sender, EventArgs e)
	{
     /*   
		if (!BW.IsBusy)
			BW.RunWorkerAsync();
      */ 
        

	}

    private void Form1_Load(object sender, System.EventArgs e)
    {
		myMatrix = new Matrix();
		myMatrixSeconds = new Matrix();
		System.Diagnostics.Process.GetCurrentProcess().PriorityClass =
		System.Diagnostics.ProcessPriorityClass.Idle;

		drawFont = new Font("Small Font", 25);
		drawFontDate = new Font("Small Font", 25);
		drawFontHeadlineDesc = new Font("Small Font", 19);
		drawFontHeadlineTitle = new Font("Arial", 17);
		drawFontHours = new Font("Century", 55);
		drawFontMinutes = new Font("Small Font", 42);
		drawFontSeconds = new Font("Small Font", 24);
		drawFontNumerator = new Font("Small Font", 14, FontStyle.Underline);
		drawFontDenominator = new Font("Small Font", 13);
		drawFontTiny = new Font("SmallFont", 12);

		drawBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
		drawBrushOutline = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
		drawBrushAl = new SolidBrush(Color.FromArgb(255, 255, 255));
		drawBrushColor = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
		drawBrushNext = new SolidBrush(Color.FromArgb(255, red, green, blue));
		drawBrushHeadline = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
		drawBrushHatch = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.FromArgb(255, 255, 255, 255));

		//drawBrush.Color = Color.FromArgb(alpha, 255, 255, 255);
		//drawBrushOutline.Color = Color.FromArgb(alpha, 0, 0, 0);
		//drawBrushHeadline.Color = Color.FromArgb(alpha, 255, 255, 255);
		
		pen = new Pen(Color.Green, 3);
		rnd = new Random(1000);

		LocationInfos.Add(new LocationInfo("Pleasanton", "94588"));
		//LocationInfos.Add(new LocationInfo("Union City", "94587"));
		//LocationInfos.Add(new LocationInfo("Redding", "96001"));

		dtInfo = new DayTimeInfo();
		dtInfo.angle = 0;
		dtInfo.oldAngle = 0;
		//angleSeconds = 0;
		//oldAngleSeconds = 0;
		p1 = new Point(0, 0);
		p2 = new Point(1, 1);
		speech = new SpVoice();
		speech.Volume = 40;  //0 to 100.
		transparentCount = timerDrawClock.Interval;
		thread = new Thread(new ThreadStart(ComputeFrac));
        threadDrawClock = new Thread(new ThreadStart(DrawClock));
        threadHide = new Thread(new ThreadStart(ComputeHideNeed));
        threadNews = new Thread(new ThreadStart(ComputeHeadlines));
        threadStopApp = new Thread(new ThreadStart(ComputeStopApp));
        threadTemp = new Thread(new ThreadStart(ComputeTemp));
				
		//threadBaromDiff = new Thread(new ThreadStart(ComputeBaromDiff));
		thread.Priority = ThreadPriority.Lowest;
		threadTemp.Priority = ThreadPriority.Lowest;
		threadNews.Priority = ThreadPriority.Lowest;
        threadStopApp.Priority = ThreadPriority.Lowest;

		//alpha = 254;
		//antiAlpha = 1;
		
		dtInfo.stop = false;
		rc = new Rectangle(0, 0, Width, Height);
		
		//thread.Start();
		threadDrawClock.Start();
		threadHide.Start();
		threadTemp.Start();
		threadNews.Start();
		threadStopApp.Start();
		//threadBaromDiff.Start();

		/*
		lgBrush = new LinearGradientBrush(p1, p2, Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, red, green , blue));			
		*/

		

		int highestScreen = Screen.AllScreens.Length - 1;
		//highestScreen = 0;
		int l = Math.Abs(Screen.AllScreens[highestScreen].Bounds.Left);
		//// l = Screen.AllScreens[highestScreen].Bounds.Left;
		int w = Math.Abs(Screen.AllScreens[highestScreen].Bounds.Width);
		int h = Math.Abs(Screen.AllScreens[highestScreen].Bounds.Height);


		this.Left = 0;
		this.Top = 0;


		this.Left = l + (2 * w / 3);
		if (this.Left > w)
			this.Left = (2 * w / 3);
		if (this.Left < 0)
			this.Left = 0;
		if (this.Left > 1920)
			this.Left = 0;


		this.Top = Screen.AllScreens[highestScreen].Bounds.Top; // Convert.ToInt32(Height);
		  if ((this.Top) < 0)
			this.Top = 0;
		if ((this.Top) > 1920)
			this.Top = 1920;

		//this.Left = l;
		//this.Width = w / 2;
		//this.Height = h;

		//this.x = this.Left;  // Width / 3;
		//this.y = this.Top; // Height / 3; // 0; // Height / 2; //Height / 3;

		this.Left = 500;
		this.Top = -500;
		this.Opacity = 1;
		
		//BW.RunWorkerAsync();
		//this.TransparencyKey = Color.Red;
		AddHeadlineDirt();
        
        

    }

	void AddHeadlineDirt()
	{
		headlineDirt = new List<string>();
		headlineDirt.Add("r />\n");
		headlineDirt.Add("<br />");
		headlineDirt.Add("<br />");
		headlineDirt.Add("\n");
		headlineDirt.Add("\n");  
		headlineDirt.Add("</b>");
		headlineDirt.Add("<BR />");
		headlineDirt.Add("BR />");
		headlineDirt.Add(">");
		headlineDirt.Add("<");
		headlineDirt.Add("b ");
		headlineDirt.Add("href");
		headlineDirt.Add("&QUOT");
		headlineDirt.Add("&apos");
		headlineDirt.Add("&amp");
		headlineDirt.Add("&#39");
		headlineDirt.Add("&#039");
		headlineDirt.Add("&#036");
		headlineDirt.Add("&GT");
		headlineDirt.Add("![CDATA");
		headlineDirt.Add("[CDATA");
		headlineDirt.Add("CDATA");
		headlineDirt.Add(@"\u000A");
		headlineDirt.Add("\u000D");
		headlineDirt.Add("\r\n");
		headlineDirt.Add("\r");
		headlineDirt.Add("\n");
		headlineDirt.Add("<BR");
		headlineDirt.Add("<!");
		headlineDirt.Add("[[");
		headlineDirt.Add("]]");
}

	/// <summary>
	/// Draw Clock and Date stuff first then loop thru locationInfos and draw each location Info.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
		//Draw small rectangle showing rotation point
		/*
		if ((sec == 0) && (msec < timer.Interval))
		{

			//SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
			SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
			SpFlags = SpFlags & SpeechVoiceSpeakFlags.SVSFVoiceMask;
			//SpVoice speech = new SpVoice();
			speech.Rate = min / 3 - 10;  // -10 to 10.

			int houer = hour;
			//SpeechLib.ISpeechObjectTokens voices = speech.GetVoices("", "");
			//speech.Voice = (SpObjectToken)voices[1];
			if (hour > 12)
				houer = houer - 12;
			string msg = "The time is ";
			switch (min)
			{
				case 0: msg = msg + houer + "o clock "; break;
				case 1: msg = msg + min + "minutes after " + houer; break;
				case 2: msg = msg + min + "minutes after " + houer; break;
				case 3: msg = msg + min + "minutes after " + houer; break;
				case 4: msg = msg + min + "minutes after " + houer; break;
				case 5: msg = msg + min + "minutes after " + houer; break;
				case 6: msg = msg + min + "minutes after " + houer; break;
				case 7: msg = msg + min + "minutes after " + houer; break;
				case 8: msg = msg + min + "minutes after " + houer; break;
				case 9: msg = msg + min + "minutes after " + houer; break;
				case 15: msg = msg + "quarter past " + houer; break;
				case 30: msg = msg + "half past " + houer; break;
				case 45: msg = msg + "a quarter of " + (houer + 1); break;
				case 50: msg = msg + "ten minutes of " + (houer + 1); break;
				case 55: msg = msg + "five of " + (houer + 1); break;
				default: msg = msg + houer + " " + min; break;
			}
			//speech.Speak(msg, SpFlags);
		}
		*/


		//lgBrush.Dispose();
		//lgBrush = null;  //So it will garbage collect. 
		
    }


    private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.Alt && e.KeyCode == Keys.E)
        Close();

    }

    private void Form1_DoubleClick(object sender, System.EventArgs e)
    {
  		dtInfo.stop = true;
		BW.CancelAsync();
		Close();
    }

    private void Form1_MouseHover(object sender, System.EventArgs e)
    {
		//Opacity = 0.00;  //So pass thru will occur.
		//alpha = 0;
		//transparentCount = 0;
		//SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
		//speech.Speak("mr. inviizo", SpFlags);
    }

	

	

    private void pnlTime_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            if (silent == false)
            {
                silent = true;
                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                speech.Speak("Sound off", SpFlags);
            }
            else
            {
                silent = false;
                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                speech.Speak("Sound on", SpFlags);
            }

        }
        if (e.Button == MouseButtons.Right)
		{

			//SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
			//speech.Volume = 20;
			//speech.Speak("Bye", SpFlags);
            dtInfo.stop = true;
            BW.CancelAsync();

            dtInfo.stop = true;
            thread.Abort();
        	threadTemp.Abort();
            threadNews.Abort();
            threadDrawClock.Abort();
            threadStopApp.Abort();
            if (threadBaromDiff != null)
    	        threadBaromDiff.Abort();
            Application.ExitThread();
			Close();
		}
        
            

    }
    


    private void formClockOpaque_MouseDown(object sender, MouseEventArgs e)
    {
		
		if (e.Button == MouseButtons.Right)
		{
			try
			{
				//DateTime dt = DateTime.Now;
				//Clipboard.SetText(dt.ToLongDateString() + " " + dt.ToShortTimeString());
			}
			catch  //sometimes can't write to clipboard for some reason.
			{
			}
		}
    }

	

    private void formClockOpaque_FormClosing(object sender, FormClosingEventArgs e)
    {
	  dtInfo.stop = true;
	  BW.CancelAsync();

	  //todo find way to iterate thru threads and abort them all
	  threadTemp.Abort();
	  threadNews.Abort();
	  //threadBaromDiff.Abort();
      //SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFDefault;
      //speech.Speak("ah-de-ohes amigoos", SpFlags);
    }

	private void formClockOpaque_FormClosed(object sender, FormClosedEventArgs e)
	{
		drawBrush.Dispose();
		drawBrushNext.Dispose();
		drawBrushOutline.Dispose();
		//drawBrushColor.Dispose();

	}



    private void DrawWithShadow(Graphics g, string drawString, Font drawFont, Brush aDrawBrush, Brush aDrawBrushOutline,
                                float x, float y, StringFormat strFormat, int o)
    {
		//g.CompositingMode = CompositingMode.SourceOver;
		//g.CompositingQuality = CompositingQuality.GammaCorrected;

        /*
      PointF pf = new PointF();
        pf.X = x;
        pf.Y = y;
        g.DrawString("Hi", drawFont, aDrawBrush, pf); 
         */ 
    
      //g.DrawString(drawString, drawFont, aDrawBrush, x-o, y+o, strFormat);

//      g.DrawString(drawString, drawFont, Brushes.Black, x - o, y - o, strFormat);
      g.DrawString(drawString, drawFont, aDrawBrush, x - o, y + o, strFormat);
      g.DrawString(drawString, drawFont, aDrawBrush, x + o, y - o, strFormat);
      g.DrawString(drawString, drawFont, aDrawBrush, x + o, y + o, strFormat);
      g.DrawString(drawString, drawFont, aDrawBrushOutline, x, y, strFormat);

    }


	private void DrawWithShadowRev(Graphics g, string drawString, Font drawFont, Brush drawBrush, Brush drawBrushOutline,
							float x, float y, StringFormat strFormat, int o)
	{
		
        //g.CompositingMode = CompositingMode.SourceOver;
		//g.CompositingQuality = CompositingQuality.GammaCorrected;
		g.DrawString(drawString, drawFont, drawBrushOutline, x - o, y - o, strFormat);
		g.DrawString(drawString, drawFont, drawBrushOutline, x - o, y + o, strFormat);
		g.DrawString(drawString, drawFont, drawBrushOutline, x + o, y - o, strFormat);
		g.DrawString(drawString, drawFont, drawBrushOutline, x + o, y + o, strFormat);
		g.DrawString(drawString, drawFont, drawBrush, x, y, strFormat);
        
	}

    private void DrawWithRotation(Graphics g, string drawString, Font drawFont, Brush drawBrush, Brush drawBrushOutline,
                                float x, float y, StringFormat strFormat, int o, float angle)
    {
        //g.CompositingMode = CompositingMode.SourceOver;
        //g.CompositingQuality = CompositingQuality.GammaCorrected;
        g.TranslateTransform(x,y);
        g.RotateTransform(angle);
        g.DrawString(drawString, drawFont, drawBrush, x - o, y - o, strFormat);
        g.DrawString(drawString, drawFont, drawBrush, x - o, y + o, strFormat);
        g.DrawString(drawString, drawFont, drawBrush, x + o, y - o, strFormat);
        g.DrawString(drawString, drawFont, drawBrush, x + o, y + o, strFormat);
        g.DrawString(drawString, drawFont, drawBrushOutline, x, y, strFormat);
    }

	private void DrawWithHatch(Graphics g, string drawString, Font drawFont, Brush drawBrush, Brush drawBrushOutline,
							float x, float y, StringFormat strFormat, int o)
	{
		g.CompositingMode = CompositingMode.SourceOver;
		g.CompositingQuality = CompositingQuality.GammaCorrected;
		g.DrawString(drawString, drawFont, drawBrush, x, y, strFormat);

	}

    private void DrawBaromGraph(LocationInfo locInfo)
    {
		//float x = 0;
		//float y = 0;
		//Decimal b;

		/*
		for (int i = 0; i < locInfo.baroms.Length; i++)
		//foreach (Decimal b in computeFracVars.baroms)
		{
			b = locInfo.baroms[i];
			if (b > 0)
			{
				y = (Height) - (Convert.ToInt64(b * 100) - 2900);
				x = (i * 2); // (computeFracVars.baroms.Length - i * 2);
				if (DateTime.Now.Day % 2 == 0)
				{
					DrawWithShadowRev(g, "-", drawFontSeconds, drawBrush, drawBrushOutline, x, y, strFormat);
				}
				else
				{
					DrawWithShadow(g, "-", drawFontSeconds, drawBrush, drawBrushOutline, x, y, strFormat);
				}
				//g.DrawString("=", drawFontSeconds, drawBrush, x, y, strFormat);

			}
		}
		*/
    }


    private void ComputeFrac()
    {
		while (!dtInfo.stop)
		{
			lock (dtInfo)
			{
				//Calculate percentage of day that's passed.
				//Calculate number of seconds since
				DateTime dt7 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 07, 00, 00);
				long ticksElapsed = DateTime.Now.Ticks - dt7.Ticks;
				long ticksInSpan = TimeSpan.TicksPerHour * 9;
				dtInfo.perDay = ticksElapsed / ticksInSpan;
				dtInfo.angle = dtInfo.perDay * (360);

				//(per, out numerator, ref denominator)
				/*
				switch (DateTime.Now.DayOfWeek.GetHashCode())
				{
					case 1:  //Monday i hope
						dtInfo.per = 0;
						break;
					case 2:
						dtInfo.per = 20;
						break;
					case 3:
						dtInfo.per = 40;
						break;
					case 4:
						dtInfo.per = 60;
						break;
					case 5:  //Friday I hope
						dtInfo.per = 80;
						break;
					default:
						dtInfo.per = 0;
						break;

				}
				*/
				dtInfo.per = dtInfo.per + (dtInfo.perDay * 20);

				computeFracvars cfv = new computeFracvars();
				cfv.denominator = ticksInSpan/1000000000; // Convert.ToInt64(Math.Pow(12, 3));  // 12 * 8(DateTime.Now.Minute + 1);
				long i =(cfv.denominator / 2);
				cfv.numerator = ticksElapsed /1000000000; // (DateTime.Now.Hour - 7) * 60;  // Convert.ToInt64(cfv.per * computeFracVars.denominator / 100);
				//cfv.numerator = cfv.numerator + (DateTime.Now.Minute);
				while ( (i > 1) )
				{
				  if ((cfv.numerator % i == 0) && (cfv.denominator % i == 0))
				  {
					cfv.numerator = Convert.ToInt32(cfv.numerator / i);
					cfv.denominator = cfv.denominator / i;
				  }
				  i--;
				}
				dtInfo.numerator = cfv.numerator;
				dtInfo.denominator = cfv.denominator;
			}
			Thread.Sleep(1800000);
		}
	
    }


    private void ComputeHideNeed()
    {

        while (!dtInfo.stop)
        {
            if ((Math.Abs(lastX - MousePosition.X) > 1) || (Math.Abs(lastY - MousePosition.Y) > 1))
            {
                dtInfo.opasity = 0.00;
                Thread.Sleep(3000);
            }
            else
            {
                dtInfo.opasity = opacityStd;
            }

            lastX = MousePosition.X;
            lastY = MousePosition.Y;

            Thread.Sleep(200);

        }
    }

	private double ComputeHideNeed(BackgroundWorker bw)
	{
		double opasity = 1.00;
        while ((!bw.CancellationPending) && (!dtInfo.stop))
		{
			if ( (Math.Abs(lastX - MousePosition.X) > 1) || (Math.Abs(lastY - MousePosition.Y) > 1) )
			{
				opasity = 0.00;

			}
			else
			{
				opasity = opacityStd;
			}

			lastX = MousePosition.X;
			lastY = MousePosition.Y;

			Thread.Sleep(200);
			return opasity;
			
		}


		return opasity;
	}



    private void ComputeStopApp()
    {
        DateTime startTime = DateTime.Parse("7:00:00");
        DateTime stopTime = DateTime.Parse("18:10:00");
        dtInfo.stop = (DateTime.Now.CompareTo(startTime) < 0) || (DateTime.Now.CompareTo(stopTime) > 0);
        Thread.Sleep(5000);
    }

    
    private void ComputeTemp()
    {
		//WebRequest request = WebRequest.Create("http://www.weather.com/outlook/recreation/boatandbeach/local/94588?from=hp_promolocator&lswe=94544&lwsa=Weather36HourBoatAndBeachCommand");
		while (!dtInfo.stop)
		{
			SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
			speech.Speak("Computing Temp", SpFlags);          
			foreach (LocationInfo locInfo in LocationInfos)
			{
				lock (locInfo)
				{
					try
					{

						/*
							HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.rssweather.com/wx/us/ca/pleasanton/wx.php");
							*/
						HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.rssweather.com/zipcode/94588/wx.php");

						Thread.Sleep(1000);

						HttpWebResponse response = (HttpWebResponse)request.GetResponse();
						Thread.Sleep(1000);
						Stream stream = response.GetResponseStream();
						Thread.Sleep(1000);
						StreamReader sr = new StreamReader(stream, Encoding.UTF8);
						Regex rgx = new Regex("[^0-9/.]");  //Remove all but numeric and period.
						try
						{
							string str = sr.ReadToEnd();
							int idxTemp = str.IndexOf("&deg");
							int idxWind = str.IndexOf("Wind Speed:");
							int idxHumid = str.IndexOf("humidity");
							int idxBarom = str.IndexOf("in.");

							if (idxBarom > -1)
							{
								locInfo.barom = str.Substring(idxBarom-37, 5);
								locInfo.barom = rgx.Replace(locInfo.barom, string.Empty);  //replace anything that's not 0..9 or . with "".
								locInfo.baromDiff = Convert.ToString(Convert.ToDouble(locInfo.barom) - Convert.ToDouble(locInfo.baromOld));
							}
							//So difference is compared to 12Am daily.
							//if (DateTime.Now.Hour == 7)
							//{
							locInfo.baromOld = locInfo.barom;
							//}
							char[] arr = new char[] { '&', 'C', 'c' };

							locInfo.attribs.Clear();
						  
							string key = str.Substring(idxTemp-15, 4).TrimEnd(arr);
							string val = str.Substring(idxTemp-3, 3).TrimEnd(arr);
							locInfo.attribs.Add(key, val);
							locInfo.temp = rgx.Replace(val, string.Empty);  //replace anything that's not 0..9 or . with "".

							key = str.Substring(idxWind, 12).TrimEnd(arr);
							val = str.Substring(idxWind + 24, 9).TrimEnd(arr);
							locInfo.attribs.Add(key, val);

							key = str.Substring(idxHumid, 5).TrimEnd(arr);
							val = str.Substring(idxHumid + 10, 3).TrimEnd(arr);
							locInfo.attribs.Add(key, val);

							key = "B"; // str.Substring(idxBarom, 5).TrimEnd(arr);
							val = str.Substring(idxBarom-6, 5).TrimEnd(arr);
							locInfo.attribs.Add(key, val);

							/*
							key = "TempLastRead";
							val = DateTime.Now.Ticks.ToString();
							locInfo.attribs.Add(key, val);
							*/

						  
							//Shift all baroms to left by one and put current at end.
							/*
							for (int j = 0; j < locInfo.baroms.Length - 1; j++)
							{
								locInfo.baroms[j] = locInfo.baroms[j + 1];

							}
							locInfo.baroms[locInfo.baroms.Length - 1] = Convert.ToDecimal(locInfo.barom);
							locInfo.needsToBeDrawn = true;
								*/

						}
						finally
						{
							sr.Close();
							stream.Close();
							response.Close();
						}


					}
					catch (Exception e)
					{
						locInfo.attribs.Clear();
						locInfo.attribs.Add("Error", e.Message);
					}
				}  //lock
			}  //foreach
			speech.Speak("Computing Temp Complete", SpFlags);
			Thread.Sleep(MinsToMsecs(30));


		}  //while
	  
    }

	private string CleanHeadline(string headline)
	{
		foreach (string s in headlineDirt)
		{
			headline = headline.Replace(s, String.Empty);
		}
		return headline;

	}

	private void ComputeHeadlines()
	{
		string str = "";
		HttpWebRequest request;
		HttpWebResponse response;
		StreamReader sr;
		Stream stream;
		string pubDate = "";
		string headline = "";
        string JFPriority = String.Empty;

		SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;

		while (!dtInfo.stop)
		{
			speech.Speak("Computing Headlines", SpFlags);
			for (int j = 0; (j < newsSources.Count); j++)
			{
			  try
			  {
				  //Change cache policy to not use cache.
				  HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);
				  HttpWebRequest.DefaultCachePolicy = policy;

				  request = (HttpWebRequest)WebRequest.Create(newsSources[j].RSSURL);
				  
				  request.CachePolicy = policy;

				  response = (HttpWebResponse)request.GetResponse();

				  stream = response.GetResponseStream();
				  sr = new StreamReader(stream, Encoding.UTF8);
				  str = sr.ReadToEnd();
				  sr.Close();
				  stream.Close();
				  response.Close();
			  }
			  catch (Exception e)
			  {
				  newsSources[j].Add(new NewsItem("Prob", "Problem with feed " + e.Message, DateTime.Now.ToString()));
			  }
			  try
			  {
				  int idxTitle = str.IndexOf("<title");  //leave off '>' in case has attribute.
				  int idxEndTitle = str.IndexOf("</title>");
				  int idxPubDate = str.IndexOf("<pubDate>");
				  int idxEndPubDate = str.IndexOf("</pubDate>");
				  int idxDesc = str.IndexOf("<description>");
				  int idxEndDesc = str.IndexOf("</description>");
				  
				  lock (newsSources[j])
				  {

					  newsSources[j].Clear();

					  if (newsSources[j].name == "Weather")
					  {
						  idxTitle = str.IndexOf("Forecast:");
						  headline = str.Substring(idxTitle, 180);
						  int idx = headline.IndexOf("\n");
						  headline = headline.Substring(idx);

						  idx = headline.IndexOf("<a");
						  headline = headline.Substring(0, idx);
						  headline = CleanHeadline(headline);
						  headline = headline.Trim();

						  newsSources[j].Add(new NewsItem("Title", headline, pubDate));
					  }
                      if (newsSources[j].name.Contains("Traffic"))
                      {
                          /*
                          List<string> searchItems = new List<String>();
                          searchItems.Add("680");
                          searchItems.Add("84");
                          searchItems.Add("Mission");
                          searchItems.Add("Niles");
                          searchItems.Add("AmTrak");

                          idxTitle = -1;
                          int i = 0;
                          while ((idxTitle == -1) && (i < searchItems.Count))
                          {

                              idxTitle = str.IndexOf(searchItems[i]);
                              int JFLoc = str.IndexOf("JAMFACTOR", idxTitle);
                              if (JFLoc < str.Length - 5)
                              {
                                  JFPriority = str.Substring(JFLoc + 10, 2);

                                  JFPriority = JFPriority.Replace("\n", String.Empty);
                                  JFPriority = JFPriority.Replace("<", String.Empty);
                              }

                              i++;
                          }

                          headline = str.Substring(idxTitle, 40);
                          newsSources[j].Add(new NewsItem("Title", headline, pubDate));
                          */
                          

                          
                      }
					  else
					  {
						  while (idxEndTitle > idxTitle)
						  {
							  headline = str.Substring(idxTitle + 7, idxEndTitle - idxTitle - 7);
							  headline = CleanHeadline(headline);
							  headline = headline.Trim();
					
							  if (idxPubDate >= 0)
							  {
								  pubDate = str.Substring(idxPubDate + 9, idxEndPubDate - idxPubDate - 9);
								  pubDate = Remove("CDATA", pubDate);
								  pubDate = Remove("&QUOT", pubDate);
								  pubDate = Remove("&apos", pubDate);
								  pubDate = Remove(@"\u000A", pubDate);
							  }
							  else
							  {
								  pubDate = "";
							  }
							  
							  NewsItem ni = new NewsItem("Title", headline, pubDate);

							  newsSources[j].Add(ni);
	
							  str = str.Substring(idxEndTitle + 1);
							  idxTitle = str.IndexOf("<title>");
							  idxEndTitle = str.IndexOf("</title>");
							  idxPubDate = str.IndexOf("<pubDate>");
							  idxEndPubDate = str.IndexOf("</pubDate>");
						  }
					  }
					  	  
				  }
			  }
			  catch (Exception e)
			  {
				  
				  newsSources[j].Add(new NewsItem("Prob", "Problem with SNO feed " + e.Message +" "+ newsSources[j].name, DateTime.Now.ToString()));
			  }
				  

			}

			speech.Speak("Computing Headlines complete", SpFlags);
			Thread.Sleep(MinsToMsecs(30));

		}  //while


	}

	  /*
    private void ComputeBaromDiff()
    {
	  const int delay = 15; // 901;  //~15 mins.
      int i = delay;
      while (!computeFracVars.stop)
      {
          if (i == delay)
          {
              lock (computeFracVars)
              {
                  int j = 0;
                  while ((j < computeFracVars.baroms.Length-1) && (computeFracVars.baroms[j] != 0))
                  {
                    j++;
                  }
                  if (j > 0)
                  { j--; }  //move back to one with value.
                  Decimal baromDiff = Convert.ToDecimal(computeFracVars.baromCurrent) - Convert.ToDecimal(computeFracVars.baroms[j]);
                  computeFracVars.baromDiff = Convert.ToString(baromDiff);
              }
              i = 0;
          }
          i++;
          computeFracVars.baromDiffCD = (delay - i).ToString();
          Thread.Sleep(1000);
      }
    }
  */

	public class DayTimeInfo
	{
		public bool stop;
		public Single angle;
		public Single per;
		public Single denominator;
		public Single numerator;
		public Single oldAngle;
		public Single perDay;
		public double opasity;
	}


	//Shared between threads
	public class LocationInfo
	{
		public string barom;
    public string baromOld = "30.00";
		public string baromDiff;
		public string baromDiffCD;
		public Decimal[] baroms = new decimal[194];  //Half of current form width
    public int cpuPercent;
		public Boolean needsToBeDrawn = true;
		public string name;
		public string zip;
		public ListDictionary attribs;
		public ListDictionary attribsOld;
		public string temp;
    public string windSpeed;

		public LocationInfo(string aName, string aZip)
		{
			name = aName;
			zip = aZip;
			attribs = new ListDictionary();
			attribsOld = new ListDictionary();
			 
		}
	}

	public class NewsItem
	{
		public bool nu;
		public string title;
		public string description;
		public string itemDate;
		public NewsItem(string aTitle, string aDescription, string aItemDate)
		{
			title = aTitle;
			description = aDescription;
			itemDate = aItemDate;
			nu = false;
		}
	}

	public class NewsSource: List<NewsItem>
	{
		public string name;
		public string RSSURL;
		public int idx = 0;

		public NewsSource(string aName, string aRSSURL)
		{
			name = aName;
			RSSURL = aRSSURL;			
		}
	}

    public class NewsSources: List<NewsSource>
	{

		public NewsSources()
		{
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("RSSFeeds");
            foreach (string key in section.AllKeys)
            {
                    this.Add(new NewsSource(key, section[key]));
            }

            //this.Add(new NewsSource("MSNBC", "http://rss.msnbc.msn.com/id/3032091/device/rss/rss.xml"));
            //this.Add(new NewsSource("Daily Review", "http://www.eastbaytimes.com/feed/"));
            
			//this.Add(new NewsSource("A123 batthttp://www.cnet.com/rss/car-tech/ery 5 at 19.97", "http://www.quoterss.com/quote.php?symbol=AONE&frmt=0&Freq=0"));
      //this.Add(new NewsSource("GE 16.42", "http://www.quoterss.com/quote.php?symbol=ge&frmt=0&Freq=0"));
      //this.Add(new NewsSource("ABAT 4.29","http://www.quoterss.com/quote.php?symbol=abat&frmt=0&Freq=0"));
      //this.Add(new NewsSource("ABAT 10 at 4.15. sellat 4.20", "http://www.quoterss.com/quote.php?symbol=abat&frmt=0&Freq=0"));
      //this.Add(new NewsSource("A123 10 at 3.92. sell at 3.96", "http://www.quoterss.com/quote.php?symbol=aone&frmt=0&Freq=0"));
			//this.Add(new NewsSource("AOne A123 19.99", "http://www.quoterss.com/quote.php?symbol=AOne&frmt=0&Freq=0"));
			//this.Add(new NewsSource("Buckle 32.40 median", "http://www.quoterss.com/quote.php?symbol=Bke&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Guaranty Federal Bancshares 8 at 6.07", "http://www.quoterss.com/quote.php?symbol=GFED&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Meridian Bioscience 3 at 21.37", "http://www.quoterss.com/quote.php?symbol=VIVO&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Forward 45 at 2.09.  Sell 2.15", "http://www.quoterss.com/quote.php?symbol=FORD&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Ford 8 at 11.76.  Sell 12.36", "http://www.quoterss.com/quote.php?symbol=F&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Costco 2 at 59.07.  Sell 62.00", "http://www.quoterss.com/quote.php?symbol=COST&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Coke 2 at 56.11.  Sell 58.90", "http://www.quoterss.com/quote.php?symbol=KO&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Medtronic 2 at 43.03", "http://www.quoterss.com/quote.php?symbol=MDT&frmt=0&Freq=0"));
      //this.Add(new NewsSource("Wells Fargo 4 at 26.79", "http://www.quoterss.com/quote.php?symbol=WFC&frmt=0&Freq=0"));            
            
			//this.Add(new NewsSource("CraigsList-Hayward", http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=55&format=rss"));
      //this.Add(new NewsSource("CraigsList-Fremont", "http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=54&format=rss"));	
      //this.Add(new NewsSource("CraigsList-Concord", "http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=51&format=rss"));
			//this.Add(new NewsSource("CraigsList-Danville", "http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=52&format=rss"));
			//this.Add(new NewsSource("CraigsList-Dublin", "http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=53&format=rss"));
			//this.Add(new NewsSource("CraigsList-SanLeandro", "http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=67&format=rss"));
      //this.Add(new NewsSource("CraigsList-WalnutCreek", "http://sfbay.craigslist.org/search/boa/eby/52?query=&minAsk=3000&maxAsk=12000&neighborhood=69&format=rss"));
			//this.Add(new NewsSource("CraigsList ukulele", "http://sfbay.craigslist.org/search/sss?catAbb=sss&query=towable&sort=rel&format=rss"));
            
      //csz = zip code, mag = magnfication higher =covers more miles (1 -5), minsev = miniumu severity (1 - 5).
			//this.Add(new NewsSource("Traffic mag=2 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=2&minsev=1"));
			//this.Add(new NewsSource("Traffic mag=2 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=2&minsev=1"));
			//this.Add(new NewsSource("Traffic mag=2 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=2&minsev=1"));
			//this.Add(new NewsSource("Traffic mag=2 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=2&minsev=1"));
			//this.Add(new NewsSource("Traffic mag=3 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=3&minsev=1"));            
			//this.Add(new NewsSource("Traffic mag=3 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=3&minsev=1"));			
			//this.Add(new NewsSource("Traffic mag=3 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=3&minsev=1"));
			//this.Add(new NewsSource("Traffic mag=3 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=3&minsev=1"));
			//this.Add(new NewsSource("Traffic mag=3 minsev=1", "http://maps.yahoo.com/traffic.rss?csz=94586&mag=3&minsev=1"));
            
			
			
      //this.Add(new NewsSource("Athlethics", "http://sports.yahoo.com/mlb/teams/oak/rss.xml"));
			//this.Add(new NewsSource("MLB", "http://mlb.mlb.com/partnerxml/gen/news/rss/mlb.xml"));
			//this.Add(new NewsSource("40 something", "http://feeds.feedburner.com/fortysomething/etc"));
	    ////// this.Add(new NewsSource("Weather", "http://weather.yahooapis.com/forecastrss?p=94587"));
      //this.Add(new NewsSource("Cnet Main", "http://news.cnet.com/2547-1_3-0-20.xml?tag=txt"));

      //this.Add(new NewsSource("Cnet Main", "http://news.cnet.com/2547-1035_3-0-5.xml?tag=txt"));

			//this.Add(new NewsSource("SFGate Radiders", "http://feeds.sfgate.com/sfgate/rss/feeds/raiders"));  //bad
			////// this.Add(new NewsSource("SFGate Bay Area News", "http://www.sfgate.com/bayarea/feed/Bay-Area-News-429.php"));
			
      //////this.Add(new NewsSource("ABC News Top", "http://feeds.abcnews.com/abcnews/topstories"));
      //////this.Add(new NewsSource("ABC News Tech", "http://feeds.abcnews.com/abcnews/technologyheadlines"));
			//this.Add(new NewsSource("Google News", "http://news.google.com/?output=rss"))

			//this.Add(new NewsSource("Discover", "http://dsc.discovery.com/news/xml/top-stories.xml"));
	    //this.Add(new NewsSource("Scifi", "http://www.coolscifi.com/forums/external.php?forumids=2"));
	    //////this.Add(new NewsSource("Space", "http://feeds.space.com/spaceheadlines?format=xml	"));

	    //this.Add(new NewsSource("CNN", "http://rss.cnn.com/rss/cnn_topstories.rss"));
			//this.Add(new NewsSource("CNN2", "http://rss.cnn.com/rss/cnn_topstories.rss"));
			//this.Add(new NewsSource("Pop. Mech.", "http://feeds.popularmechanics.com/pm/automotive/new_cars/"));
			//this.Add(new NewsSource("RIGHTWing", "http://feeds.feedburner.com/grist/gristmill"));
			
			//this.Add(new NewsSource("NeoWin", "http://feedproxy.google.com/neowin-all")); 
	
	    //this.Add(new NewsSource("DRDobbs", "http://www.ddj.com/rss/developmentTools.xml;jsessionid=XDUOQDRNFS33QQSNDLRSKH0CJUNN2JVN"));
			//////this.Add(new NewsSource("NewStack Dobbs replacement hopefully", "http://thenewstack.io/blog/feed/"));

      //////this.Add(new NewsSource("Autobytel", "http://rss.autobytel.com/standard/articles.xml"));

			//////this.Add(new NewsSource("NPR Tech", "http://www.npr.org/rss/rss.php?id=1019"));
			//////this.Add(new NewsSource("NPR USNews", "http://www.npr.org/rss/rss.php?id=1003"));
	        
			//this.Add(new NewsSource("Roget", "http://dictionary.reference.com/wordoftheday/wotd.rss"));
			//this.Add(new NewsSource("Earthquakes", "http://earthquake.usgs.gov/eqcenter/catalogs/1day-M2.5.xml"));
      			
      //this.Add(new NewsSource("YahooTech", "http://rss.news.yahoo.com/rss/tech"));
			//this.Add(new NewsSource("YahooTechMicrosoft", "http://rss.news.yahoo.com/rss/microsoft"));
      //this.Add(new NewsSource("Bart", "http://www.bart.gov/news/rss/rss.xml"));
      //this.Add(new NewsSource("OneTipAday", "http://feeds2.feedburner.com/onetipaday"));
	    //this.Add(new NewsSource("Russian", @"E:\temp\index.htm")); 
			//this.Add(new NewsSource("Amazon Top Deals", "http://rssfeeds.s3.amazonaws.com/goldbox"));
			//////this.Add(new NewsSource("TechCrunch Main", "http://feeds.feedburner.com/TechCrunch/"));
			//////this.Add(new NewsSource("Guru Portfolio", "https://www.gurufocus.com/feed/guru_portfolio_feed.php/"));

			//Defunct
			//this.Add(new NewsSource("KTVU East bay", "http://www.ktvu.com/category/289821/east-bay-news?clienttype=rss")); off air
			//this.Add(new NewsSource("Throttle Blips", "http://throttleblips.dailyradar.com/index.xml"));  Gone.
			//this.Add(new NewsSource("AutoHeadlines", "http://www.pheedo.com/f/automotive_headlines")); no longer valid.
			//this.Add(new NewsSource("ABC Local", "http://cdn.abclocal.go.com/kgo/xml?id=7095531"));  Not updating as of 8/2014

			}

	}
	
	private string Remove (string aThis, string aFromThis)
	{
		int idx = aFromThis.IndexOf(aThis);
		if (idx >= 0)
		{
			int tailLen = aFromThis.Length-(idx+aThis.Length);
			return (aFromThis.Substring(0, idx) + aFromThis.Substring(aThis.Length + idx, tailLen));

		}
		else
		{
			return (aFromThis);
		}
	}

	private void pnlWeather_Paint(object sender, PaintEventArgs e)
	{
        //Graphics g = e.Graphics;
        //g = e.Graphics;

		red = sec * 4;
		green = 255;
		blue = 255;

		string drawStringColors = red.ToString() + " " + green.ToString() + " " + blue.ToString();

		// Set format of string.
		strFormat = new StringFormat();
		strFormat.FormatFlags = StringFormatFlags.NoWrap;
		strFormat.Alignment = StringAlignment.Near;

		y = Height / 7; // 0; // Height / 2; //Height / 3;
		
		//Show Forecast
		foreach (NewsSource ns in newsSources)
		{
			lock (ns)
			{
				if ((ns.name == "Weather") && (ns.Count > 0))
				{
					
					NewsItem ni = ns[0];
					//Find second dash, go back 3 spaces.
					//Left is 1st line, to right is 2nd line.
					int idxDash = ni.description.IndexOf(" - ");
					idxDash = ni.description.IndexOf(" - ", idxDash + 1);
					idxDash = idxDash - 3;
					if (idxDash > 0)
					{
						string today = ni.description.Substring(0, idxDash);
						string tomorrow = ni.description.Substring(idxDash);

						//DrawWithShadow(g, ns.name + " " + ni.itemDate, drawFontSeconds, drawBrush, drawBrushOutline, 2, 2, strFormat);
						DrawWithShadowRev(e.Graphics, today, drawFontDenominator, drawBrush, drawBrushOutline, 0, y, strFormat, 1);
						y = y + 20;
						DrawWithShadowRev(e.Graphics, tomorrow, drawFontDenominator, drawBrush, drawBrushOutline, 0, y, strFormat, 1);
						//y = y + 20;
						//DrawWithShadowRev(g, high, drawFontDenominator, drawBrush, drawBrushOutline, 0, y, strFormat, 1);
					}
				}

			}
		}
		
		//Calculate and display biorhythm
		/*
		physical: sin(2πt / 23), 
		emotional: sin(2πt / 28), 
		intellectual: sin(2πt / 33), 
		intuitive: sin(2πt / 38), 
		


		DateTime dtBirth = new DateTime(1964, 10, 19);
		System.TimeSpan diff = DateTime.Now - dtBirth;
		int daysSinceBirth = diff.Days;

		double bioPhysical = Math.Sin(2 * Math.PI * daysSinceBirth / 23);
		double bioEmotional = Math.Sin(2 * Math.PI * daysSinceBirth / 28);
		double bioIntellectual = Math.Sin(2 * Math.PI * daysSinceBirth / 33);
		double bioIntuitive = Math.Sin(2 * Math.PI * daysSinceBirth / 38);
		bioPhysical = Math.Round(bioPhysical * 100);
		bioEmotional = Math.Round(bioEmotional * 100);
		bioIntellectual = Math.Round(bioIntellectual * 100);
		bioIntuitive = Math.Round(bioIntuitive * 100);
		double bioTotal = bioPhysical + bioEmotional + bioIntellectual + bioIntuitive;
		string bio = bioPhysical.ToString() + " " + bioEmotional.ToString() + " " + bioIntellectual.ToString() + " " + bioIntuitive.ToString() +"   "+ bioTotal.ToString();
		y = y + 20;
		DrawWithShadowRev(g, bio.ToString(), drawFontDenominator, drawBrush, drawBrushOutline, 0, y, strFormat, 1);
		*/

		
        
		//g.Dispose();

		//			TopMost = true;

		

		
	}

		

	private void pnlTime_Paint(object sender, PaintEventArgs e)
	{

		//Graphics g = e.Graphics;
        //g = e.Graphics;
        //Graphics g = pnlTime.CreateGraphics();

        Opacity = dtInfo.opasity;
        if (Opacity == 0)
        {
            return;
        }

		//Color c = new Color();
		int hour = DateTime.Now.Hour;
		int min = DateTime.Now.Minute;
		sec = DateTime.Now.Second;
		int msec = DateTime.Now.Millisecond;


		//this.TransparencyKey = Color.FromArgb(0, 128, 128, sec*4);

		string drawStringDate = DateTime.Now.ToShortDateString();
		String drawString = DateTime.Now.ToString("h:mm"); //DateTime.Now.Hour +":"+ DateTime.Now.Minute;
		String drawStringNext = (DateTime.Now.AddMinutes(1)).ToString("h:mm");
		String drawStringSeconds = DateTime.Now.ToString("ss");
		double displayPer = dtInfo.per * 100;
		displayPer = Math.Ceiling(displayPer) / 100;
		String drawStringPercent = Convert.ToString(displayPer);
		String drawStringFraction = Convert.ToString(dtInfo.numerator) + "\r\n" + Convert.ToString(dtInfo.denominator);
		String drawStringCountDown = Convert.ToString(countDown);
        

		x = Width / 4;
		y = Height / 3; // 0; // Height / 2; //Height / 3;

		RPx = /*(DateTime.Now.Second) * */(drawFont.Size * 2);  // + (DateTime.Now.Millisecond/1000);
		rotatePoint = new PointF(x + RPx,
			//x+((DateTime.Now.Second*DateTime.Now.Hour)/4)-drawFont.Size,
		y + drawFont.Height / 2);

		// Draw the rectangle to the screen again after applying the transform.
		p1.X = Convert.ToInt32(x);
		p1.Y = Convert.ToInt32(y);
		//Point p2 = new Point(Convert.ToInt32(x+50/*drawString.Length*drawFont.Size*/), Convert.ToInt32(y+50/*drawFont.Height*/));

		p2.X = 0;
		p2.Y = 0;
		p3.X = (p1.X - Convert.ToInt32(drawFont.Size / 2)) + (sec + 1); //(hour+min+sec)*sec; //drawFont.Height;
		p3.Y = (p1.Y - (drawFont.Height / 2)) + sec; //p2.X + sec;

		//g.Transform = myMatrix;

		//myMatrix.RotateAt(dtInfo.angle - dtInfo.oldAngle, rotatePoint, MatrixOrder.Prepend); //, rotatePoint);
		//dtInfo.oldAngle = dtInfo.angle;

		int offset = Convert.ToInt32(drawFont.Size) + 25;
		//Draw Date    
		//DrawWithShadowRev(g, drawStringDate, drawFont, drawBrush, drawBrushOutline, x + 1 - offset, y + 1 + offset, strFormat, 1);
		//DrawWithShadowRev(g, drawString, drawFont, drawBrush, drawBrushOutline, x + 4, y + 4, strFormat, 1);
		//g.DrawString(drawStringDate, drawFontDate, drawBrush, x + 1 - offset, y + 1 + offset, strFormat);
		//g.DrawString(drawString, drawFont, drawBrush, x + 4, y + 2, strFormat);


		//Opacity = sec/180.0 + 0.10;

		/*
		angleSeconds = (sec * 6) + 1;  // (DateTime.Now.Millisecond / 1000.0);
		g.Transform = myMatrixSeconds;

		myMatrixSeconds.RotateAt(Convert.ToInt64(angleSeconds - oldAngleSeconds), rotatePoint, MatrixOrder.Prepend);
		oldAngleSeconds = angleSeconds;

		//int xoff = 25;
		int yoff = 5;
		//DrawWithShadow(g, drawStringPercent, drawFontSeconds, drawBrush, drawBrushOutline, 
		//	x + (sec * min / 8), y + yoff - 25, strFormat);
		
		//DrawWithShadow(g, drawStringPercent, drawFontSeconds, drawBrush, drawBrushOutline,
			//rotatePoint.X, rotatePoint.Y, strFormat);
		DrawWithShadow(g, drawStringCountDown, drawFontSeconds, drawBrush, drawBrushOutline,
			rotatePoint.X, rotatePoint.Y, strFormat);

		*/
		/*
		if ((computeFracVars.denominator < 1000) && (lastDrawStringFrac != drawStringFraction))
		{
			DrawWithShadow(g, drawStringFraction, drawFontSeconds, drawBrush, drawBrushOutline, x+offset, y+offset, strFormat, 1);

			//SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
			//speech.Speak(dtInfo.numerator + "over" + dtInfo.denominator, SpFlags);
		}
		else
		    DrawWithShadow(g, lastDrawStringFrac, drawFontSeconds, drawBrush, drawBrushOutline, x+offset, y+offset, strFormat, 1 );
		*/

		//All Hands
		//int clockSize = 160;
        int clockSize = 150;
		
		double toRads = Math.PI / 180.0;
		  
		Pen whitePen = new Pen(Color.White, clockSize / 40);
		Pen blackPen = new Pen(Color.Black, clockSize / 40);
		
		Pen pinkPen = new Pen(Color.Pink, 2);
		Pen hourHandPen = new Pen(Color.Red, clockSize / 30);
		Pen minHandPen = new Pen(Color.Black, clockSize / 40);
		Pen secHandPen = new Pen(Color.White, 2);
		  

		LocationInfo li = LocationInfos[0];
		int redTemp;
		int blueTemp;
		int greenTemp;
		int temp;
		double barom;
        double baromDiff;
        int cpuPercent;
        int windSpeed;
		lock (li)
		{
            
            temp = cnvSafe.ConvertToInt32(li.temp);
            barom = cnvSafe.ConvertToDouble(li.barom);
            baromDiff = cnvSafe.ConvertToDouble(li.baromDiff);
            cpuPercent = li.cpuPercent;
            windSpeed = cnvSafe.ConvertToInt16(li.windSpeed);
            
		}

		//to get more exaggerated color effect;
		int t2 = temp * 2;

		//redTemp = 255 - (Math.Abs(temp - 93) * 10);

		int skew = -136; // -68;  //22
		//temp = sec * 2;
		int sinValue = Convert.ToInt32(255 * Math.Sin((t2 + skew) * toRads));
		redTemp = 0;
		greenTemp = 0;
		blueTemp = 0;
		if (sinValue > 0)
		{
			redTemp = sinValue;
		}
		else
		{
			blueTemp = Math.Abs(sinValue);
		}

		greenTemp = 255 - Math.Abs(sinValue);

		
		//Keep between 0 and 255.
		greenTemp = (greenTemp > 255) ? 255 : greenTemp;
		greenTemp = (greenTemp < 0) ? 0 : greenTemp;

		//redTemp = Convert.ToInt32(255 * Math.Sin((temp + skew + 270) * toRads));
		redTemp = (redTemp > 255) ? 255 : redTemp;
		redTemp = (redTemp < 0) ? 0 : redTemp;


		//blueTemp = Convert.ToInt32(255 * Math.Cos((temp + skew) * toRads));
		//blueTemp = 255 - (Math.Abs(temp + skew) * 10);
		blueTemp = (blueTemp > 255) ? 255 : blueTemp;
		blueTemp = (blueTemp < 0) ? 0 : blueTemp;

		Color colTime = Color.FromArgb(redTemp, greenTemp, blueTemp);
		
		int redBarom;
		if (barom > 30)
		{
			redBarom = Convert.ToInt32((barom - 30) * 255);
		}
		else
		{
			redBarom = 0;
		}


		int blueBarom;
		try
		{
			blueBarom = Convert.ToInt32(Math.Abs(30-barom) * 255);
		}
		catch
		{
			blueBarom = 255;

		}
		if (blueBarom > 255)
			{ blueBarom = 255; }	

		
		Color colBarom = Color.FromArgb(redBarom, 0, blueBarom);
		//colTime = (Color.FromArgb(hour*5, hour*10, hour*10));

        /* Set radius to be used below */
        //double r = (clockSize * 0.8);
        double r = clockSize;
        double h = hour;

        //Draw dots around clock (draw them first so they can be overwritten 
        int dotWidth = 8;
        int dotHeight = 8;
    
        for (int h1 = 1; h1 <= 12; h1++)
        {
            int x1 = Convert.ToInt32(Math.Sin(h1 * 30 * toRads) * r) - (dotWidth / 2);
            int y1 = Convert.ToInt32(Math.Cos(h1 * 30 * toRads) * r) + (dotHeight / 2);
        
            Rectangle rect = new Rectangle(p1.X + x1-1, p1.Y - y1-1, dotWidth+2, dotHeight+2);
            //rect.X = 137; rect.Y = 100; rect.Width = 9; rect.Height = 8;
            e.Graphics.FillEllipse(drawBrush, rect); //Brushes.Black, rect);

            rect = new Rectangle(p1.X + x1, p1.Y - y1, dotWidth, dotHeight);
            e.Graphics.FillEllipse(drawBrushOutline, rect);
            //Rectangle rect
            //e.Graphics.FillEllipse(drawBrush, rect);
         
        }   

		int lx;
		int ly;

          //
          //Hour Hand. 
          //
          //Draw hour at current hour
          //Dot will move based on Hour-Minute
      
          if (h > 12)
              h = h - 12;
          //double denotion = Math.Sin(hour * 15);  //0 to 1 to -1.
          int L = h.ToString().Length;

          //DrawWithShadow(g, h.ToString(), drawFontHeadlineDesc, drawBrush, drawBrushOutline, minuteX, minuteY, strFormat, 1);
      
          //Draw Min at relative hour hand pos
          double hm = hour + (Convert.ToDouble(min) / 60);  
	        lx = Convert.ToInt32(Math.Sin(hm * 30 * toRads) * r);
	        ly = Convert.ToInt32(Math.Cos(hm * 30 * toRads) * r);
          int lxMid = Convert.ToInt32(Math.Sin(hm * 30 * toRads) * (1 * r / 2));
          int lyMid = Convert.ToInt32(Math.Cos(hm * 30 * toRads) * (1 * r / 2));
          int lxEnd = Convert.ToInt32(Math.Sin(hm * 30 * toRads) * (1 * r));
          int lyEnd = Convert.ToInt32(Math.Cos(hm * 30 * toRads) * (1 * r));    
          int lxEndHour = Convert.ToInt32(Math.Sin(h * 30 * toRads) * (1 * r));
          int lyEndHour = Convert.ToInt32(Math.Cos(h * 30 * toRads) * (1 * r));
          Point lpStart = new Point(p1.X, p1.Y);
          Point lpMid = new Point(p1.X + lxMid, p1.Y - lyMid);
          Point lpEnd = new Point(p1.X + lxEnd, p1.Y - ly);
          Point lpEndHour = new Point(p1.X + lxEndHour, p1.Y - lyEndHour);
          float endX = Convert.ToInt32(lpEnd.X) - (drawFontHours.Size * L / 2) - 6;
          float endY = Convert.ToInt32(lpEnd.Y) - (drawFontHours.Height / 2);

      

          //Draw Dot at number to be more exact
          Rectangle rectDot = new Rectangle(lpEnd.X - 6, lpEnd.Y - 6, 12, 12);  //dotWidth, dotHeight);


          //int remainder;
          //Math.DivRem(sec, 2, out remainder);
          //g.FillEllipse(drawBrush, rectDot);
          DrawWithShadow(e.Graphics, h.ToString(), drawFontHours, drawBrushOutline, drawBrush, endX, endY, strFormat, 1);            
       
      

        //g.FillEllipse(drawBrushOutline, new Rectangle(p1.X - 3, p1.Y - 3, 6, 6));

        //hourHandPen.Brush = new HatchBrush(HatchStyle.SolidDiamond, Color.White, Color.Black);
    
        //Draw minute on hour hand, minute hand not needed, move it in counterclockwise circle.
        //DrawWithShadowRev(g, min.ToString(), drawFontHeadlineTitle, drawBrush, drawBrushOutline, lpEnd.X, lpEnd.Y, strFormat, 1);
    
        //float minuteX = (Convert.ToInt32((lpEnd.X) + Math.Cos(sec) * 6)) - drawFontDate.Size;
        //float minuteY = (Convert.ToInt32((lpEnd.Y) + Math.Sin(sec) * 6)) - drawFontDate.Size;

        //Minute Hand.
        //Draw Min at minute
        double ms = min + (Convert.ToDouble(sec) / 60);
        lx = Convert.ToInt32(Math.Sin(ms * 6 * toRads) * r);
        ly = Convert.ToInt32(Math.Cos(ms * 6 * toRads) * r);
        lxMid = Convert.ToInt32(Math.Sin(ms * 6 * toRads) * (3 * r / 4));
        lyMid = Convert.ToInt32(Math.Cos(ms * 6 * toRads) * (3 * r / 4));
        lpStart.X = p1.X;  lpStart.Y = p1.Y;
        lpMid.X = p1.X + lxMid; lpMid.Y = p1.Y - lyMid;
        lpEnd.X = p1.X + lx; lpEnd.Y = p1.Y - ly;


        L = min.ToString().Length;
        float midX = Convert.ToInt32(lpMid.X) - (drawFontMinutes.Size * L / 2); // - drawFontDate.Size * denotion);  
        float midY = Convert.ToInt32(lpMid.Y) - (drawFontMinutes.Height / 2);  // * denotion);

        DrawWithShadow(e.Graphics, min.ToString(), drawFontMinutes, drawBrush, drawBrushOutline, midX, midY, strFormat, 1);
        /*
        if (remainder == 0)
        {
            DrawWithShadow(g, min.ToString(), drawFontMinutes, drawBrush, drawBrushOutline, midX, midY, strFormat, 1);
            //DrawWithRotation(g, min.ToString(), drawFontMinutes, drawBrush, drawBrushOutline, Width/2, Height/2, strFormat, 1, min*60);
        }
        else
        {

            DrawWithShadow(g, min.ToString(), drawFontMinutes, drawBrushOutline, drawBrush, midX, midY, strFormat, 1);
            //DrawWithRotation(g, min.ToString(), drawFontMinutes, drawBrushOutline, drawBrush, lx, ly, strFormat, 1, min*60);
        }
        */

        /*
        r = Convert.ToInt32(clockSize * 0.95);
		    lx = Convert.ToInt32(Math.Sin(min * 6 * toRads) * r);
		    ly = Convert.ToInt32(Math.Cos(min * 6 * toRads) * r);
		    lpStart = new Point(p1.X, p1.Y);
		    lpEnd = new Point(p1.X + lx, p1.Y - ly);
		    //g.FillEllipse(drawBrushOutline, new Rectangle(p1.X - 3, p1.Y - 3, 6, 6));
		    //secHandPen.Brush = new HatchBrush(HatchStyle.DiagonalBrick, Color.Black, Color.White);
		    minHandPen.Brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.White, Color.Black);
		    //minHandPen.Brush = new LinearGradientBrush(lpStart, lpEnd, colTime, Color.Green);
		    //g.DrawLine(minHandPen, lpStart, lpEnd);

		    //Draw number on tip of hand if not divisible by 5
        
        int rem5;
        Math.DivRem(min, 5, out rem5);
        if (rem5 != 0)
        {
            lx = Convert.ToInt32(Math.Sin(min * 6 * toRads) * (r + 1));
            ly = Convert.ToInt32(Math.Cos(min * 6 * toRads) * (r + 1));
            lpEnd = new Point(p1.X + lx, p1.Y - ly);
            //float off = drawFontHeadlineTitle.Size;
            DrawWithShadowRev(g, min.ToString(), drawFontHeadlineTitle, drawBrush, drawBrushOutline, lpEnd.X, lpEnd.Y, strFormat, 1);
        }
    
        //DrawWithShadowRev(g, min.ToString(), drawFontHeadlineTitle, drawBrush, drawBrushOutline, lpEnd.X, lpEnd.Y, strFormat, 1);
        */

	    //Second hand.
        r = Convert.ToInt32(clockSize);
	    double z = ((sec) * 6) + (msec / 166.66);
	    //double z30 = z - 30;
	    //double z90 = z-90;
	    //double z45 = z - 45.0;
	    double z180 = z - 180.0;
	    //double z2 = z + 2;
	    //double zsec = z + sec;
	    double zsin = z180 + Math.Sin(z) * hour; // (msec / 166.66);  //;(sec * 6) + Math.Sin(sec);  //msec / 166.66);
	    //double zcos = z180 + Math.Cos(z) * hour; // (msec / 166.66);  //;(sec * 6) + Math.Sin(sec);  //msec / 166.66);

	    /*
	    int lxmid =  Convert.ToInt32(Math.Sin(z) * (r / 40));
	    int lymid =  Convert.ToInt32(Math.Cos(z) * (r / 40));
	    int lxmid2 = Convert.ToInt32(Math.Cos((5*z) * toRads) * (r / 30));
	    int lymid2 = Convert.ToInt32(Math.Sin((4*z) * toRads) * (r / 30));
	    int lxmid3 = Convert.ToInt32(Math.Sin((10*z) * toRads) * (r/20));
	    int lymid3 = Convert.ToInt32(Math.Cos((8*z) * toRads) * (r/20));
	    int lxmid4 = Convert.ToInt32(Math.Sin((10 * z) * toRads) * (r / 10));
	    int lymid4 = Convert.ToInt32(Math.Cos((8 * z) * toRads) * (r / 10));
	    Point lpmid = new Point(p1.X + lxmid, p1.Y + lymid);
	    Point lpmid2 = new Point(p1.X + lxmid2, p1.Y + lymid2);
	    Point lpmid3 = new Point(p1.X + lxmid3, p1.Y + lymid3);
	    Point lpmid4 = new Point(p1.X + lxmid4, p1.Y + lymid4);
		    */
	    lx = Convert.ToInt32(Math.Sin(z180 * toRads) * r);
	    ly = Convert.ToInt32(Math.Cos(z180 * toRads) * r);

	    lxMid = Convert.ToInt32(Math.Sin(zsin * toRads) * (r/2));
	    lyMid = Convert.ToInt32(Math.Cos(zsin * toRads) * (r/2));
	    int lxmid2 = Convert.ToInt32(Math.Sin(zsin * toRads) * (r / 2));
	    int lymid2 = Convert.ToInt32(Math.Cos(zsin * toRads) * (r / 2));
	    int lxmid3 = Convert.ToInt32(Math.Sin(zsin * toRads) * (r / 1));
	    int lymid3 = Convert.ToInt32(Math.Cos(zsin * toRads) * (r / 1));

	    /*
	    int remainder;
	    Math.DivRem(sec, 2, out remainder);

		
	    if (remainder > 0)
	    {
		    lxMid = Convert.ToInt32(Math.Sin(z * toRads) * (r / 1.5) );
		    lyMid = Convert.ToInt32(Math.Sin(z * toRads) * (r / 1.5) ); 	
	    }
	    else
	    {
		    lxMid = Convert.ToInt32(Math.Sin(z * toRads) * (r / 2) );
		    lyMid = Convert.ToInt32(Math.Sin(z * toRads) * (r / 2) ); 
	    }
		    */

	    lpStart.X = p1.X;  lpStart.Y = p1.Y; 
	    lpMid.X = p1.X - lxMid; lpMid.Y = p1.Y + lyMid;
	    Point lpMid2 = new Point(p1.X - lxmid2, p1.Y + lymid2);
	    Point lpMid3 = new Point(p1.X - lxmid3, p1.Y + lymid3);
        lpEnd.X = p1.X - lx; lpEnd.Y = p1.Y + ly;

	    //PathPointType ppt = new PathPointType;
	
	    //byte[] types = new PathPointType;

	    //GraphicsPath gp = new GraphicsPath(new Point[2] {lpStart, lpEnd}, 
	    //secHandPen.Brush = new LinearGradientBrush(lpStart, lpmid2, Color.White, Color.Black);
	    //Point[] points = new Point[2] { lpStart, lpEnd };

		
	    //secHandPen.Brush = new HatchBrush(HatchStyle.SmallGrid, Color.Black, Color.White);
	    //secHandPen.Brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.Black, Color.White);

        //pinkPen.Brush = new PathGradientBrush(points, WrapMode.Tile);
        //g.DrawLine(pinkPen, lpStart, lpEnd);
        //Point[] curvePoints = { lpStart, lpmid,  lpEnd, lpmid2, lpStart, lpEnd };
        //Point[] curvePoints = { lpStart, lpmid, lpmid2, lpmid3, lpmid4, lpEnd };
        //Point[] curvePoints = { lpStart, lpMid, lpEnd };
    
    
        //PathGradientBrush pgb = new PathGradientBrush(curvePoints);
        //LinearGradientBrush    
        //(lpStart, lpEnd, Color.Black, Color.FromArgb(msecABS, msecABS, msecABS));
        //pgb.WrapMode = pgb.WrapMode = WrapMode.Clamp;
        //pgb.SurroundColors = new Color[] { Color.FromArgb(msecABS, msecABS, msecABS) };
        //pgb.CenterColor = Color.FromArgb(msecABS, msecABS, msecABS);
        //e.Graphics.FillRectangle(pgb, this.ClientRectangle);


        //double radians = (msec / 5.555556) * Math.PI / 180.0;
        //radians = (msec / 2.7) * Math.PI / 180.0;
        //double radians = (sec * (1000/60) / 5.55556) * Math.PI / 180.0;
        //int colorAdj = Convert.ToInt32(Math.Truncate(Math.Sin(radians) * 255));
        int colorAdj = 0;
        int colorAdjOpp = 255 - colorAdj;

        int colorAdjRed = Convert.ToInt32(Math.Min(255.0, Math.Abs(baromDiff * 2550.0)) );

        LinearGradientBrush lgb;

        int rem;
        Math.DivRem(sec, 2, out rem);
        if (rem == 0)
        {
            lgb = new LinearGradientBrush(lpStart, lpEnd,
                                                        Color.FromArgb(255, colorAdj, colorAdj, colorAdj),
                                                        Color.FromArgb(255, colorAdjOpp, colorAdjOpp, colorAdjOpp)); 
        }
        else
        {
            lgb = new LinearGradientBrush(lpStart, lpEnd,
                                                    Color.FromArgb(255, colorAdjOpp, colorAdjOpp, colorAdjOpp),
                                                    Color.FromArgb(255, colorAdj, colorAdj, colorAdj));
        }
        
        //LinearGradientBrush lgb = new LinearGradientBrush(lpStart, lpEnd, Color.FromArgb(colorAdj), Color.FromArgb(colorAdj));
        //LinearGradientBrush lgb = new LinearGradientBrush(lpStart, lpEnd, Color.FromArgb(0), Color.FromArgb(255));

        //Brush b = new SolidBrush(Color.FromArgb(Math.Abs(colorAdjRed-colorAdj), 0, 0));
        //Brush b = new SolidBrush(Color.Black);

        

        //secHandPen.Brush = b; 
        secHandPen.Brush = lgb;

        //double baromOffCenter = Math.Abs(30 - barom);
        //g.DrawCurve(secHandPen, curvePoints, Convert.ToSingle(baromOffCenter));
        
        e.Graphics.DrawLine(secHandPen, lpStart, lpEnd);

        //g.DrawCurve(secHandPen, curvePoints, Convert.ToSingle(baromDiff*100.0)+ 1);

        //g.DrawCurve(secHandPen, curvePoints, windSpeed);  
        //g.DrawCurve(secHandPen, curvePoints, Convert.ToSingle(cpuPercent) + 1);
    
		
	    //double b = ((barom * 100) - 2900) / 200;
	    //g.DrawCurve(pinkPen, curvePoints, Convert.ToSingle(b));

	    //Draw number on tip of second hand.
	    lx = Convert.ToInt32(Math.Sin(z * toRads) * (r + 1));
	    ly = Convert.ToInt32(Math.Cos(z * toRads) * (r + 1));
	    lpEnd = new Point(p1.X + lx, p1.Y - ly);
	    //off = drawFontHeadlineTitle.Size;
	    /*
	    if  ((sec > 0) && (sec <= 15))
	    {
		    DrawWithShadowRev(g, sec.ToString(), drawFontHeadlineTitle, drawBrush, 
							    drawBrushOutline, lpEnd.X, lpEnd.Y-off, strFormat, 1); 
	    }
	    else if ((sec >15) && (sec<=30))
	    {
		    DrawWithShadowRev(g, sec.ToString(), drawFontHeadlineTitle, drawBrush, 
									    drawBrushOutline, lpEnd.X-off, lpEnd.Y, strFormat, 1); 
	    }
	    else if ((sec >30) && (sec<=45))
	    {
		    DrawWithShadowRev(g, sec.ToString(), drawFontHeadlineTitle, drawBrush, 
							    drawBrushOutline, lpEnd.X-off, lpEnd.Y, strFormat, 1); 
	    }
	    else if ((sec >45) && (sec<=60))
	    {
		    DrawWithShadowRev(g, sec.ToString(), drawFontHeadlineTitle, drawBrush, 
							    drawBrushOutline, lpEnd.X-off, lpEnd.Y-off, strFormat, 1); 
	    }
		    */

        //Drawcenter dot (Draw center dot last so can click on it)
        //drawBrush.Color = colTime;

        e.Graphics.FillEllipse(drawBrushOutline, new Rectangle(p1.X - dotWidth, p1.Y - dotHeight, (dotWidth * 2), (dotHeight * 2)));
        e.Graphics.FillEllipse(drawBrush, new Rectangle((p1.X-(dotWidth/2)), (p1.Y-(dotHeight/2)), (dotWidth * 1), (dotHeight * 1)));

        //DrawWithShadowRev(g, sec.ToString(), drawFontHeadlineTitle, drawBrush, drawBrushOutline, lpEnd.X, lpEnd.Y, strFormat, 1); 

	    //Draw Temp and Barom
        string baromDirIndicator;
        if (baromDiff > 0)
        {
			int unicodeVal = 11014;	//up  arrow
			baromDirIndicator = Convert.ToString((char)unicodeVal);
            //baromDirIndicator = "^";
        }
        else
        if (baromDiff == 0)
        {
			int unicodeVal = 11020;	//left right arrow
			baromDirIndicator = Convert.ToString((char)unicodeVal);
			//baromDirIndicator = "=";

		}
        else
        {
			int unicodeVal = 11015; //down arrow
			baromDirIndicator = Convert.ToString((char)unicodeVal);
			//baromDirIndicator = "!";
        }

        DrawWithShadowRev(e.Graphics, temp.ToString() + "º " + barom.ToString() + baromDirIndicator,
                          drawFontHeadlineTitle, drawBrush, drawBrushOutline, p1.X - 125, p1.Y, strFormat, 1);

        //drawBrush.Color = colTime;
        //Draw Month and Day.
        DrawWithShadowRev(e.Graphics, DateTime.Now.DayOfWeek.ToString().Substring(0, 3) + "  " + DateTime.Now.Day.ToString(),
	        drawFontHeadlineTitle, drawBrush, drawBrushOutline, p1.X + 30, p1.Y, strFormat, 1);

        //Draw surrounding circle
        //g.DrawEllipse(pen, new Rectangle(p1.X-r, p1.Y-r, r*2, r*2));

        //DrawWithShadow(g, drawStringSeconds, drawFontSeconds, drawBrush, drawBrushOutline, x, y, strFormat, 1);
        lastDrawStringFrac = drawStringFraction;
        TopMost = true;
        int xTime = (sender as Panel).Location.X;
        int yTime = (sender as Panel).Location.Y;
        int widthTime = xTime + (sender as Panel).Width;
        int heightTime = yTime + (sender as Panel).Height;
        //e.Graphics.CopyFromScreen(new Point(xTime, yTime), new Point(xTime + widthTime, yTime + heightTime),
        //						  new Size(xTime, yTime));

    
        //DrawWithShadow(g, cbValue, drawFontSeconds, drawBrush, drawBrushOutline, p1.X - 70, p1.Y - 100, strFormat, 1);

            /* interrupts hide
        if (sec > 58)
        {
            Opacity = 1.0;
        }
        else
        {
            Opacity = opacityStd;
        }
                */ 

        //Thread.Sleep(1000);
        //Application.DoEvents();
        //g.Dispose();

	}

	
    private void pnlGraph_Paint(object sender, PaintEventArgs e)
	{
		int idx;
		int min;

		NewsSource ns;
		Random rand;
		//Graphics g = e.Graphics;
        //g = e.Graphics;

		/*
		foreach (LocationInfo li in LocationInfos)
		{
			{
				//DrawBaromGraph(li);
				lock (li)
				{
					li.needsToBeDrawn = false;
				}
			}
		}
		*/

		//Draw news items.  Randomly pick an item to display.
		//uses time as seed
		rand = new Random();

		//int rem;
		//Math.DivRem(DateTime.Now.Minute, 2, out rem);
		min = DateTime.Now.Minute;
		
		double frac = newsSources.Count / 60.0;
		idx = Convert.ToInt32(Math.Truncate(frac * min));

        if (newsSources[idx].name == "Weather")
            idx++;

        if ( (newsSources[idx].name.Contains("Traffic")) && (DateTime.Now.Hour < 14) )
        {
            idx++;  //Skip traffic until after 2pm.
        }
        

		if (idx >= newsSources.Count)
		{
			idx = 0;
		}
		ns = newsSources[idx];
		currentRSSURL = ns.RSSURL;

		lock (ns)
		{
			if (ns.Count > 0)
			{
				//Count can be changed externally
				if (ns.idx >= ns.Count)
					ns.idx = 0;

				NewsItem ni = ns[ns.idx];

				if ((ns.idx > 0) && (ns.idx < 5))
				{
					timerGeneral.Interval = 7000;
				}
				else
				{
					timerGeneral.Interval = 2500;
				}

				string idxDisplay = " " + (ns.idx +1).ToString() + " of " + ns.Count.ToString();

                DrawWithShadow(e.Graphics, ns.name + " " + ni.itemDate + " " + idxDisplay, drawFontHeadlineTitle, drawBrushHeadline, drawBrushOutline, 2, 32, strFormat, 1);
                //DrawWithShadowRev(e.Graphics, ni.description, drawFontHeadlineDesc, drawBrushHeadline, drawBrushOutline, 2, 0, strFormat, 3);
                DrawWithShadow(e.Graphics, ni.description, drawFontHeadlineDesc, drawBrushHeadline, drawBrushOutline, 2, 0, strFormat, 1);
                
				//DrawWithHatch(g, ns.name + " " + ni.itemDate + " " + idxDisplay, drawFontHeadlineTitle, drawBrushHatch, drawBrushHatch, 2, 32, strFormat, 1);

				//g.DrawString(ns.name, drawFontSeconds, drawBrush, x - 1, y - 1, strFormat);

				//g.DrawString(ns.name, drawFontHeadline, drawBrushHeadline, 2, 32, strFormat);
				//g.DrawString(ni.description, drawFontHeadline, drawBrushHeadline, 2, 2, strFormat);

				//g.DrawString(Opacity.ToString(), drawFontSeconds, drawBrush, 2, 60, strFormat);

				if ((ns.idx > 0) && (ns.idx <= 3))
				{

					if (!ni.description.Contains(ns.name.Substring(0,2)))
					{
                        if (silent == false)
                        {
                            SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                            if ( (!ni.description.ToUpper().Contains("QUOTERSS")) &&
                                 (!ni.description.Contains("No incident")) )
                            {

                                speech.Speak(ni.description, SpFlags);
                            }
                        }
					}
				}
				ns.idx++;
				if (ns.idx >= ns.Count)
					ns.idx = 0;
			}
			else
			{
				//NewsSource count was 0, so display only name of newsSource.
                DrawWithShadow(e.Graphics, ns.name, drawFontHeadlineTitle, drawBrushHeadline, drawBrushOutline, 2, 0, strFormat, 1);
			}
		}

        
        //g.Dispose();
		
		TopMost = true;
	}

	

	/// <summary>
	/// To set up for a background operation, add an event handler for the DoWork event. 
	///	Call your time-consuming operation in this event handler. 
	///	To start the operation, call RunWorkerAsync. 
	///	To receive notifications of progress updates, handle the ProgressChanged event. 
	///	To receive a notification when the operation is completed, handle the RunWorkerCompleted event.
	/// </summary>
	private void BW_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker bw = sender as BackgroundWorker;

		e.Result = ComputeHideNeed(bw);
		
		// If the operation was canceled by the user, 
		// set the DoWorkEventArgs.Cancel property to true.
		if (bw.CancellationPending)
		{
			e.Cancel = true;
		}
		

	}

	private void BW_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		


	}

	private void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			MessageBox.Show("Error in RunWorkerCompleted " + e.Error.Message);
		}
		Opacity = System.Convert.ToDouble(e.Result.ToString()); 

        /*
        //Here retrieve and display clipboard value.
        IDataObject iData = Clipboard.GetDataObject();
        if (iData.GetDataPresent(DataFormats.Text))
        {
            try
            {
                cbValue = (String)iData.GetData(DataFormats.Text);
                cbValue = Point lpmid = new Point(p1.X + lxmid, p1.Y + lymid);.Substring(0, 14);
            }
            catch
            {
                //Suppress error.
            }

        }
         */ 
	}

	private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
	{
		System.Diagnostics.Process.Start(currentRSSURL);


	}

    private int MinsToMsecs(int Mins)
    {
        return (Mins * 60 * 1000);
    }

    

    //this.pnlTime.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pnlTime.MouseWheel);    
    void pnlTime_MouseWheel(object sender, MouseEventArgs e)
    {
        if (e.Delta < 0)
        {

        }
    }

    private void pnlTime_Validated(object sender, EventArgs e)
    {
        this.Update();
    }
  }

	class computeFracvars
	{
		public long numerator;
		public long denominator;
		//public int per;
	}

    public class ConvertSafe
    {
        public double ConvertToDouble(string s)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch
            {
                return (0.00);
            }
        }

        public Int16 ConvertToInt16(string s)
        {
            try
            {
                return Convert.ToInt16(s);
            }
            catch
            {
                return (0);
            }
        }

        public Int32 ConvertToInt32(string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                return (0);
            }
        }




    }
}