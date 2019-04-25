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
using System.IO;
using System.Collections.Generic;


namespace ClockOpaque
{

  /// <summary>
  /// Summary description for Form1.
  /// </summary>
	public class formClockOpaque : System.Windows.Forms.Form
	{
	private System.Windows.Forms.Timer timer;
	private System.ComponentModel.IContainer components;
	private float x;
	private float y;
	//private Single angle;
	private double angleSeconds;
	private double oldAngleSeconds;
	private PointF rotatePoint;
	private StringFormat strFormat;
	private SolidBrush drawBrush;
	private SolidBrush drawBrushOutline;
	private Font drawFont;
	private Font drawFontDate;
	private Font drawFontSeconds;
	private Font drawFontNumerator;
	private Font drawFontDenominator;
	private Matrix myMatrix;
	private Matrix myMatrixSeconds;
	private int red;
	private int green;
	private int blue;
	private Pen pen;
	public Graphics g;
	public Random rnd;
	public Thread thread;
	public Thread threadTemp;
	public Thread threadBaromDiff;
	float RPx;
	int transparentCount;
	const int invisibleInterval = 10;
	//LinearGradientBrush lgBrush;
	Point p1;
	Point p2;
	Point p3;
	int sec;
	SpVoice speech;

	string lastDrawStringFrac = "*";
	public List<LocationInfo> LocationInfos = new List<LocationInfo>();
	public DayTimeInfo dtInfo;

	public static Object synchronizeVariable = "locking variable";


	public formClockOpaque()
	{
	//
	// Required for Windows Form Designer support
	//
	InitializeComponent();

	//
	// TODO: Add any constructor code after InitializeComponent call
	//
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
	this.timer = new System.Windows.Forms.Timer(this.components);
	this.SuspendLayout();
	// 
	// timer
	// 
	this.timer.Enabled = true;
	this.timer.Interval = 950;
	this.timer.Tick += new System.EventHandler(this.timer_Tick);
	// 
	// formClockOpaque
	// 
	this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
	this.ClientSize = new System.Drawing.Size(438, 438);
	this.ForeColor = System.Drawing.SystemColors.Control;
	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
	this.Location = new System.Drawing.Point(700, 500);
	this.Name = "formClockOpaque";
	this.Opacity = 0.2;
	this.ShowInTaskbar = false;
	this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
	this.TopMost = true;
	this.TransparencyKey = System.Drawing.SystemColors.Control;
	this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
	this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClockOpaque_FormClosing);
	this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
	this.MouseHover += new System.EventHandler(this.Form1_MouseHover);
	this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formClockOpaque_MouseDown);
	this.Load += new System.EventHandler(this.Form1_Load);
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

    private void timer_Tick(object sender, System.EventArgs e)
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



      if (transparentCount > invisibleInterval)
      {
        if (Opacity != 0.20)
        {
          Opacity = 0.20;
        }
        this.Invalidate();  //Causes onPaint to be called, i think.

      }
      else
        transparentCount++;



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
    }

    private void Form1_Load(object sender, System.EventArgs e)
    {
		x = Width / 3;
		y = Height / 3; // 0; // Height / 2; //Height / 3;
		myMatrix = new Matrix();
		myMatrixSeconds = new Matrix();
		System.Diagnostics.Process.GetCurrentProcess().PriorityClass =
		System.Diagnostics.ProcessPriorityClass.Idle;

		drawFont = new Font("Small Font", 60);
		drawFontDate = new Font("Small Font", 21);
		drawFontSeconds = new Font("Small Font", 15);
		drawFontNumerator = new Font("Small Font", 15, FontStyle.Underline);
		drawFontDenominator = new Font("Small Font", 14);
		pen = new Pen(Color.Red, 3);
		rnd = new Random(1000);

		LocationInfos.Add(new LocationInfo("Pleasanton"));
		LocationInfos.Add(new LocationInfo("Union City"));
		LocationInfos.Add(new LocationInfo("Redding"));
		dtInfo = new DayTimeInfo();
		dtInfo.angle = 0;
		dtInfo.oldAngle = 0;
		angleSeconds = 0;
		oldAngleSeconds = 0;
		p1 = new Point(0, 0);
		p2 = new Point(1, 1);
		speech = new SpVoice();
		speech.Volume = 80;  //0 to 100.
		transparentCount = timer.Interval;
		thread = new Thread(new ThreadStart(ComputeFrac));
		threadTemp = new Thread(new ThreadStart(ComputeTemp));
		//threadBaromDiff = new Thread(new ThreadStart(ComputeBaromDiff));
		dtInfo.stop = false;
		foreach (LocationInfo locInfo in LocationInfos)
		{
			locInfo.stop = false;
		}

		thread.Start();
		threadTemp.Start();
		//threadBaromDiff.Start();
		/*
		lgBrush = new LinearGradientBrush(p1, p2,
		Color.FromArgb(0, 0, 0, 0), 
		Color.FromArgb(255, red, green , blue));			
		*/
		if (this.Left < 0)
		this.Left = 0;
		if ((Screen.AllScreens.Length) > 1)
		{
		this.Left =
			Screen.AllScreens[1].Bounds.Left +
			Screen.AllScreens[1].Bounds.Left - Width;

		this.Top = Screen.AllScreens[1].Bounds.Top; // Convert.ToInt32(Height);
		}

    }

    private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {

      //Color c = new Color();
      int hour = DateTime.Now.Hour;
      int min = DateTime.Now.Minute;
      sec = DateTime.Now.Second;
      int msec = DateTime.Now.Millisecond;
      /*
      lock (computeFracVars)
      //synchronized (synchronizeVariable)
      */


      //Monitor.TryEnter(computeFracVars);

      //Properties.Settings.Default.AlSetting 

      g = e.Graphics;
      // Create string to draw.
      String drawString = DateTime.Now.ToString("h:mm"); //DateTime.Now.Hour +":"+ DateTime.Now.Minute;
      String drawStringNext = (DateTime.Now.AddMinutes(1)).ToString("h:mm");
      String drawStringSeconds = DateTime.Now.ToString("ss");
      double displayPer = dtInfo.per * 100;
      displayPer = Math.Ceiling(displayPer) / 100;
      String drawStringPercent = Convert.ToString(displayPer);
	  String drawStringFraction = Convert.ToString(dtInfo.numerator) + "\r\n" + Convert.ToString(dtInfo.denominator);
      String drawStringTemp = Convert.ToString(locPleasanton.temp);
	  String drawStringWind = Convert.ToString(lvPleasanton.wind);
	  String drawStringHumid = Convert.ToString(lvPleasanton.humid);
	  String drawStringBaromCurrent = Convert.ToString(lvPleasanton.baromCurrent);
	  String drawStringBaromDiff = Convert.ToString(lvPleasanton.baromDiff);
	  String drawStringBaromDiffCD = Convert.ToString(lvPleasanton.baromDiffCD);
      //String drawStringBaromDir = Convert.ToString(computeFracVars.baromDir);
      String drawStringTempUC = Convert.ToString(lvUC.temp);
      String drawStringWindUC = Convert.ToString(lvUC.wind);
      String drawStringHumidUC = Convert.ToString(lvUC.humid);


      red = sec * 4;
      green = 255;
      blue = 255;
      /*
      if ((sec % 2) == 0)
      {
          red = 255;
          blue = 255;
          green = 255;
      }
      else
      {
          blue = 0;
          green = 0;
      }
      */

      //red = (255- (hour*15))*2;
      //red = (hour*15);
      /*
			green = (hour*15);
			green = (green > 255) ? 255 : green;  //if red > 255 then set red = 255 else set red to red.
			blue = (255-(hour*15));
			blue = (blue < 0) ? 0 : blue;
		
			red = ((hour*15));
			red = (red > 255) ? 255 : red;  //if red > 255 then set red = 255 else set red to red.
			red = (red < 0) ? 0 : red;
*/
      //if (hour >= 15)
      //				red = (min * 4) + 15;
      //green = 0;
      //blue = 0;
      //red = sec * 4 + 50;
      //red = (red > 255) ? 255 : red;  //if red > 255 then set red = 255 else set red to red.
      //red = (red < 0) ? 0 : red;

      //blue = DateTime.Now.Hour*15;   //Somehow it became -15, hour was -1?
      //green = (green > 255) ? 255 : green;
      //green = (green < 0) ? 0 : green;

      //blue = 255; //sec * 4 + 15;
      //blue = 255-red;
      //green = (green > 255) ? 255 : green;
      //green = (green < 0) ? 0 : green;

      //green=255;
      //int rem;
      //Math.DivRem(sec, 2, out rem);
      //if (rem > 0)
      //	{red = 0; blue=0; green=0;}
      //else
      //	{red = 0; blue = 0; green=0;}
      //Math.DivRem(sec, 10, out rem);


      //int alpha = (sec*sec*sec)/848;  //( sec) + 100;

      //int alpha = Convert.ToInt32(Math.Pow(Convert.ToDouble(sec), Convert.ToDouble(10)));
      //int alpha = Convert.ToInt32(Math.Pow(Convert.ToDouble(sec+10), 4.0)/3049412);
      SolidBrush drawBrushColor =
               new SolidBrush(Color.FromArgb(255, red, 127, 127));

      int alpha = 30 * 2 + 135;
      drawBrush =
        new SolidBrush(Color.FromArgb(alpha, 255, 255, 255));
      drawBrushOutline =
        new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));

      alpha = sec * 4;
      SolidBrush drawBrushNext =
          new SolidBrush(Color.FromArgb(alpha, red, green, blue));


      string drawStringColors = red.ToString() + " " + green.ToString() + " " + blue.ToString();
      string drawStringDate = DateTime.Now.ToLongDateString();

      // Create point for upper-left corner of drawing.
      /*	
      x++; //DateTime.Now.Hour;
      if (x > Width-200)
        x = 0;
			
      y = y + 1;
      if (y > Height -100)
        y = 0;
      */
      // Set format of string.
      strFormat = new StringFormat();
      strFormat.FormatFlags = StringFormatFlags.DisplayFormatControl;
      /*
      if ((sec==0) && (msec < 500))
      {
        myMatrix = new Matrix();  //to fix angle getting off
        myMatrixSeconds = new Matrix();
      }
      */
      //DateTime.Now.TimeOfDay
      RPx = /*(DateTime.Now.Second) * */(drawFont.Size * 2);  // + (DateTime.Now.Millisecond/1000);
      //RPx = RPx * (DateTime.Now.Hour/3) - drawFont.Size; //+ DateTime.Now.Millisecond /20;
      //RPx = RPx + rnd.Next() /1000000000;
      rotatePoint = new PointF(x + RPx,
        //x+((DateTime.Now.Second*DateTime.Now.Hour)/4)-drawFont.Size,
      y + drawFont.Height / 2);
      //rotatePoint = new PointF(x,y);
      //angle = (sec * 6.0) + (6*(msec/1000.0));
      //angle = (per * (360/100)); //math per * 3.60;
      //angle = angle / (1000/timer.Interval);

      // Draw the rectangle to the screen again after applying the transform.
      p1.X = Convert.ToInt32(x);
      p1.Y = Convert.ToInt32(y);
      //Point p2 = new Point(Convert.ToInt32(x+50/*drawString.Length*drawFont.Size*/), Convert.ToInt32(y+50/*drawFont.Height*/));


      p2.X = 0;
      p2.Y = 0;
      p3.X = (p1.X - Convert.ToInt32(drawFont.Size / 2)) + (sec + 1); //(hour+min+sec)*sec; //drawFont.Height;
      p3.Y = (p1.Y - (drawFont.Height / 2)) + sec; //p2.X + sec;



      /*lgBrush = new LinearGradientBrush(p1, p3, 
                                        Color.FromArgb(0, 0, 0), 
                                        Color.FromArgb(0, green , blue));
      */

      //Color[] = Color.FromArgb(0, 0, 0),	Color.FromArgb(red, green , blue);
      //lgbrush.LinearColors = new Color [] {Color.FromArgb(0, 0, 0),	Color.FromArgb(red, green , blue)};

      //new Color [] { captionBrush.LinearColors[0], 
      //						 captionBrush.LinearColors[0],captionBrush.LinearColors[1] } ;

      //lgBrush.WrapMode = WrapMode.Clamp;
      
      DrawWithShadow(g, drawStringWind, drawFontSeconds, drawBrush, drawBrushOutline, 0, y, strFormat);
      DrawWithShadow(g, drawStringTemp, drawFontSeconds, drawBrush, drawBrushOutline, 0, y+20 , strFormat);
      DrawWithShadow(g, drawStringHumid, drawFontSeconds, drawBrush, drawBrushOutline, 0, y+40, strFormat);
      DrawWithShadow(g, drawStringBaromCurrent, drawFontSeconds, drawBrush, drawBrushOutline, 0, y+60, strFormat);
      //DrawWithShadow(g, drawStringBaromDiff, drawFontSeconds, drawBrush, drawBrushOutline, 80, y + 60, strFormat);
      //DrawWithShadow(g, drawStringBaromDiffCD, drawFontSeconds, drawBrush, drawBrushOutline, 180, y + 60, strFormat);
      //DrawWithShadow(g, drawStringBaromDir, drawFontSeconds, drawBrush, drawBrushOutline, 160, y + 60, strFormat);
      DrawBaromGraph();
      
      DrawWithShadow(g, drawStringWindUC, drawFontSeconds, drawBrush, drawBrushOutline, 0, y+100, strFormat);
      DrawWithShadow(g, drawStringTempUC, drawFontSeconds, drawBrush, drawBrushOutline, 0, y+120 , strFormat);
      DrawWithShadow(g, drawStringHumidUC, drawFontSeconds, drawBrush, drawBrushOutline, 0, y+140, strFormat);
      
      g.Transform = myMatrix;

	  myMatrix.RotateAt(dvPleasanton.angle - dvPleasanton.oldAngle, rotatePoint, MatrixOrder.Prepend); //, rotatePoint);
	  dvPleasanton.oldAngle = dvPleasanton.angle;

      int offset = Convert.ToInt32(drawFont.Size) + 25;
      //Draw Date    

      DrawWithShadow(g, drawStringDate, drawFontDate, drawBrush, drawBrushOutline, x + 1 - offset, y + 1 + offset, strFormat);

      DrawWithShadow(g, drawString, drawFont, drawBrush, drawBrushOutline, x + 4, y + 4, strFormat);

      if (sec > 55)
      {
        g.DrawString(drawStringNext, drawFont, drawBrushNext, x, y, strFormat);
      }

      //Opacity = sec/180.0 + 0.10;

      angleSeconds = sec * 6 + 90; // (DateTime.Now.Millisecond / 1000.0);
      g.Transform = myMatrixSeconds;

      myMatrixSeconds.RotateAt(Convert.ToInt64(angleSeconds - oldAngleSeconds), rotatePoint, MatrixOrder.Prepend);
      oldAngleSeconds = angleSeconds;

      //int xoff = 25;
      int yoff = 20;
      DrawWithShadow(g, drawStringPercent, drawFontSeconds, drawBrush, drawBrushOutline, x + (sec * min / 8), y + yoff - 25, strFormat);


      if (/*(computeFracVars.denominator < 1000) && */(lastDrawStringFrac != drawStringFraction))
      {
        //DrawWithShadow(g, drawStringFraction, drawFontSeconds, drawBrush, drawBrushOutline, x - xoff, y + yoff, strFormat);

        SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
		speech.Speak(dvPleasanton.numerator + "over" + dvPleasanton.denominator, SpFlags);
      }
      //else
      //    DrawWithShadow(g, lastDrawStringFrac, drawFontSeconds, drawBrush, drawBrushOutline, x - xoff, y + yoff, strFormat);


      
      //DrawWithShadow(g, drawStringFraction, drawFontSeconds, drawBrush, drawBrushOutline, x - xoff, y + yoff, strFormat);
      lastDrawStringFrac = drawStringFraction;

      
      /*
      g.DrawString(drawStringSeconds, drawFontSeconds, drawBrush, 
                   rotatePoint.X, rotatePoint.Y, strFormat);	
      g.DrawString(drawStringSeconds, drawFontSeconds, drawBrushOutline, 
                   rotatePoint.X, rotatePoint.Y, strFormat);
      */

      //lgBrush.Dispose();
      //lgBrush = null;  //So it will garbage collect. 
      drawBrush.Dispose();
      drawBrushOutline.Dispose();
      drawBrushColor.Dispose();
      g.Dispose();

      //Draw small rectangle showing rotation point
      //g.DrawRectangle(pen, rotatePoint.X, rotatePoint.Y, 1, 1);  //rotatePoint.X, rotatePoint.Y, 1, 2);
      //opacity seems to have to be after drawstring to avoid flicker.
      //Math.DivRem(DateTime.Now.Second, 60, Opacity);
      //Opacity = Decimal.Remainder(DateTime.Now.Second, 60);
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
      TopMost = true;
    }


    private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.Alt && e.KeyCode == Keys.E)
        Close();

    }

    private void Form1_DoubleClick(object sender, System.EventArgs e)
    {
  		dvPleasanton.stop = true;
		Close();
    }

    private void Form1_MouseHover(object sender, System.EventArgs e)
    {
		//Opacity = 1;  //DateTime.Now.Second / 360.0 + 0.20;
		Opacity = 0.00;  //So pass thru will occur.
		transparentCount = 0;
		SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
		speech.Speak("mr. inviso", SpFlags);

		DateTime dt = DateTime.Now;
		Clipboard.SetText(dt.ToLongDateString() + " " + dt.ToShortTimeString());
		//Clipboard.SetText(dt.DayOfWeek +", "+ dt. +" "+ dt.Day +", " + dt.Year);

    }



    private void formClockOpaque_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        Close();
      else
      {
        Opacity = 0.00;
        transparentCount = 0;
      }

    }

    private void formClockOpaque_FormClosing(object sender, FormClosingEventArgs e)
    {
	  dvPleasanton.stop = true;
      SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFDefault;
      speech.Speak("ah-de-ohes amigoos", SpFlags);
    }



    private void DrawWithShadow(Graphics g, string drawString, Font drawFont, Brush drawBrush, Brush drawBrushOutline,
                                float x, float y, StringFormat strFormat)
    {
      g.DrawString(drawString, drawFont, drawBrush, x - 1, y - 1, strFormat);
      g.DrawString(drawString, drawFont, drawBrush, x - 1, y + 1, strFormat);
      g.DrawString(drawString, drawFont, drawBrush, x + 1, y - 1, strFormat);
      g.DrawString(drawString, drawFont, drawBrush, x + 1, y + 1, strFormat);
      g.DrawString(drawString, drawFont, drawBrushOutline, x, y, strFormat);
    }

	private void DrawWithShadowRev(Graphics g, string drawString, Font drawFont, Brush drawBrush, Brush drawBrushOutline,
							float x, float y, StringFormat strFormat)
	{
		g.DrawString(drawString, drawFont, drawBrushOutline, x - 1, y - 1, strFormat);
		g.DrawString(drawString, drawFont, drawBrushOutline, x - 1, y + 1, strFormat);
		g.DrawString(drawString, drawFont, drawBrushOutline, x + 1, y - 1, strFormat);
		g.DrawString(drawString, drawFont, drawBrushOutline, x + 1, y + 1, strFormat);
		g.DrawString(drawString, drawFont, drawBrush, x, y, strFormat);
	}

    private void DrawBaromGraph()
    {
        lock (computeFracVars)
        {
            float x = 0;
            float y = 0;
            Decimal b;

            for (int i=0; i<computeFracVars.baroms.Length-1; i++)
            //foreach (Decimal b in computeFracVars.baroms)
            {
                b = computeFracVars.baroms[i];
                if (b > 0)
                {
                    y = (Height)-(Convert.ToInt64(b * 100)-2900);
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
        }
    }


    private void ComputeFrac()
    {

      while (!computeFracVars.stop)
      {
        lock (computeFracVars)
        {
          //Calculate percentage of day that's passed.
          //Calculate number of seconds since
          DateTime dt7 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 07, 00, 00);
          Single ticksElapsed = DateTime.Now.Ticks - dt7.Ticks;
          Single ticksInSpan = TimeSpan.TicksPerHour * 9;
          computeFracVars.perDay = ticksElapsed / ticksInSpan;
          computeFracVars.angle = computeFracVars.perDay * (360);

          //(per, out numerator, ref denominator)
          switch (DateTime.Now.DayOfWeek.GetHashCode())
          {
            case 1:  //Monday i hope
              computeFracVars.per = 0;
              break;
            case 2:
              computeFracVars.per = 20;
              break;
            case 3:
              computeFracVars.per = 40;
              break;
            case 4:
              computeFracVars.per = 60;
              break;
            case 5:  //Friday I hope
              computeFracVars.per = 80;
              break;
            default:
              computeFracVars.per = 0;
              break;

          }

          computeFracVars.per = computeFracVars.per + (computeFracVars.perDay * 20);

          /*
          computeFracVars.denominator = 5*9*60; // Convert.ToInt64(Math.Pow(12, 3));  // 12 * 8(DateTime.Now.Minute + 1);
          Single i = computeFracVars.denominator / 2;
          computeFracVars.numerator = Convert.ToInt64(computeFracVars.per * computeFracVars.denominator / 100);
          while ( (i > 1) && (!computeFracVars.stop) )
          {
            if ((computeFracVars.numerator % i == 0) && (computeFracVars.denominator % i == 0))
            {
              computeFracVars.numerator = Convert.ToInt64(computeFracVars.numerator / i);
              computeFracVars.denominator = computeFracVars.denominator / i;
            }
            i--;
          }
          */
        }
        Thread.Sleep(1000);
      }
    }

    
    private void ComputeTemp()
    {
      //WebRequest request = WebRequest.Create("http://www.weather.com/outlook/recreation/boatandbeach/local/94588?from=hp_promolocator&lswe=94544&lwsa=Weather36HourBoatAndBeachCommand");
	  const int delay = 900; 
      int i = delay;
      while (!computeFracVars.stop)
      {
          if (i == delay)
          {
              lock (computeFracVars)
              {
                  try
                  {
                      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                            "http://www.weather.com/outlook/recreation/boatandbeach/local/94588");
                          
                      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                      Stream stream = response.GetResponseStream();
                      StreamReader sr = new StreamReader(stream);
                      try
                      {
                          string str = sr.ReadToEnd();
                          int idxTemp = str.IndexOf("temp=");
                          int idxWind = str.IndexOf("wind=");
                          int idxHumid = str.IndexOf("humid=");
                          //barometric pressure find &nbsp;in. and go back 5 chars.
                          int idxBarom = str.IndexOf("&nbsp;in.") - 5;
                          char[] arr = new char[] { '&' };
                          computeFracVars.temp = (str.Substring(idxTemp, 7)).TrimEnd(arr);
                          computeFracVars.wind = (str.Substring(idxWind, 7)).TrimEnd(arr);
                          computeFracVars.humid = (str.Substring(idxHumid, 8)).TrimEnd(arr);
                          computeFracVars.baromCurrent = (str.Substring(idxBarom, 5)).TrimEnd(arr);
                          //Shift all baroms to left by one and put current at end.
                          for (int j= 0; j < computeFracVars.baroms.Length-1; j++)
                          {
                            computeFracVars.baroms[j] = computeFracVars.baroms[j+1];

                          }
						  computeFracVars.baroms[computeFracVars.baroms.Length-1] = Convert.ToDecimal(computeFracVars.baromCurrent);
                          

                      }
                      finally
                      {
                          sr.Close();
                          stream.Close();
                          response.Close();
                      }


                      //Get Hayward data.
                      request = (HttpWebRequest)WebRequest.Create(
                            "http://www.weather.com/outlook/recreation/boatandbeach/local/94587");
                      response = (HttpWebResponse)request.GetResponse();
                      stream = response.GetResponseStream();
                      sr = new StreamReader(stream);
                      try
                      {
                          string str = sr.ReadToEnd();
                          int idxTemp = str.IndexOf("temp=");
                          int idxWind = str.IndexOf("wind=");
                          int idxHumid = str.IndexOf("humid=");
                          char[] arr = new char[] { '&' };
                          computeFracVars.tempUC = (str.Substring(idxTemp, 7)).TrimEnd(arr);
                          computeFracVars.windUC = (str.Substring(idxWind, 7)).TrimEnd(arr);
                          computeFracVars.humidUC = (str.Substring(idxHumid, 8)).TrimEnd(arr);
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
                      computeFracVars.temp = e.Message;
                  }
              }  //lock
              i = 0;
          }
          Thread.Sleep(1000);
          i++;
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
		

	}

	

	//Shared between threads
	public class LocationInfo
	{
		public string name;
		public bool stop;
		public string temp;
		public string wind;
		public string humid;
		public string baromCurrent;
		public Decimal[] baroms = new decimal[194];  //Half of current form width
		public string baromDiff;
		public string baromDiffCD;

		public LocationInfo(string aName)
		{
			name = aName;
		}
	}

    

  }
}