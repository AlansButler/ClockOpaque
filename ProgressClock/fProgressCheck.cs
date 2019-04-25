using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace WindowsApplication1
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer timer;
		private System.ComponentModel.IContainer components;
		private float x; 
		private float y;
		private double angle;
		private PointF rotatePoint;
		private StringFormat strFormat;
		private Font drawFont;
		private Font drawFontOutline;
		private Matrix myMatrix;
		private int red;
		private int green;
		private int blue;
		private Pen pen;
		public Graphics g;
		public Random rnd;
		float RPx;


		public Form1()
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 450);
			this.ForeColor = System.Drawing.SystemColors.Control;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Location = new System.Drawing.Point(500, 200);
			this.Name = "Form1";
			this.Opacity = 0.25;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.SystemColors.Control;
			this.Click += new System.EventHandler(this.Form1_Click);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.MouseHover += new System.EventHandler(this.Form1_MouseHover);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			
			Application.Run(new Form1());
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{
			int v;
			v = DateTime.Now.Hour;
			v = v - 6;
			//if (v < pbHour.Minimum)
			//	v = pbHour.Minimum;
			//if (v > pbHour.Maximum)
			//	v = pbHour.Maximum;
			if (v < 0) 
				v = 0;
			if (v > 10)
				v = 10;
			
			Text = DateTime.Now.ToShortDateString() +' '+ DateTime.Now.ToShortTimeString();
			//
			//label2.Text = DateTime.Now.ToShortDateString() +' '+ DateTime.Now.ToShortTimeString();
			//Opacity = pbSec.Value;

			if (Decimal.Remainder(DateTime.Now.Second, 5)==0)
			{
		//		Visible = true;
			}
			else
			{
			//	Visible = false;
			}

			// Retrieve the working rectangle from the Screen class
			// using the PrimaryScreen and the WorkingArea properties.
			//System.Drawing.Rectangle workingRectangle = 
			//	Screen.PrimaryScreen.WorkingArea;
    
			// Set the size of the form slightly less than size of 
			// working rectangle.
			//this.Size = new System.Drawing.Size(
		//		workingRectangle.Width-10, workingRectangle.Height-10);


			//if (e.Button.ToString()
			this.Invalidate();  //Calls onPaint to be called, i think.


		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			x = Width /2;
			y = Height/2; //Height / 3;
			myMatrix = new Matrix();
			System.Diagnostics.Process.GetCurrentProcess().PriorityClass =
				System.Diagnostics.ProcessPriorityClass.Idle;
			drawFont = new Font("Small Font", 50);
			drawFontOutline = new Font("Small Font", 50);
			pen = new Pen(Color.Red, 3);
			rnd = new Random(1000);
			
		}

		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			
			//Color c = new Color();
			g = e.Graphics;
			
			
			// Create string to draw.
			
			String drawString = DateTime.Now.ToString("h:mm"); //DateTime.Now.Hour +":"+ DateTime.Now.Minute;
			//String drawString = DateTime.Now.ToShortTimeString();
			// Create font and brush.
			//Font drawFont = new Font("Arial Narrow", 80);
			//Font drawFont = new Font("Small Font", DateTime.Now.Hour*5+1);
			//Font drawFont = new Font("Small Font", 50);
						
			//SolidBrush drawBrush = new SolidBrush(TransparencyKey);
			
		  red = Convert.ToInt32(DateTime.Now.DayOfWeek)*35;
			green = Math.Min(DateTime.Now.Hour*15, 255);
			blue = DateTime.Now.Minute*4;
			SolidBrush drawBrush = 
				new SolidBrush(Color.FromArgb(red, green, blue));
			SolidBrush drawBrushOutline = 
				new SolidBrush(Color.FromArgb(255-red, 255-green, 255-blue));
			
			//SolidBrush drawBrushOutline = 
			//	new SolidBrush(Color.FromArgb(255-Convert.ToInt32(DateTime.Now.DayOfWeek)*35, 255-DateTime.Now.Minute, 255-DateTime.Now.Second));
			
			//SolidBrush drawBrush = new SolidBrush(DateTime.Now.Second);
		
			/*
			if (Decimal.Remainder(DateTime.Now.Second, 4)==0)			
			{
				drawBrush = new SolidBrush(Color.FromArgb(10, 128, 10));
				//drawBrush = new SolidBrush(Color.Black);
			}
			else
				drawBrush = new SolidBrush(Color.FromArgb(10, 128, 10));
				//drawBrush = new SolidBrush(Color.White);		
			*/
			

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

			myMatrix = new Matrix();
			//DateTime.Now.TimeOfDay
			RPx = (DateTime.Now.Second) * (DateTime.Now.Hour/4);  // + (DateTime.Now.Millisecond/1000);
			//RPx = RPx * (DateTime.Now.Hour/3) - drawFont.Size; //+ DateTime.Now.Millisecond /20;
			//RPx = RPx + rnd.Next() /1000000000;
			rotatePoint = new PointF(x + RPx, 
				//x+((DateTime.Now.Second*DateTime.Now.Hour)/4)-drawFont.Size,
				y+drawFont.Height/2);
			//rotatePoint = new PointF(x,y);
			angle = (DateTime.Now.Second * 6.0) + (6 * (DateTime.Now.Millisecond/1000.0));
			myMatrix.RotateAt(Convert.ToInt32(angle), rotatePoint, MatrixOrder.Prepend);
			// Draw the rectangle to the screen again after applying the
			// transform.
			g.Transform = myMatrix;


			// Draw string to screen.
			//g.DrawString(drawString, drawFont, drawBrush, x, y, strFormat);

			//Draw outline first so it's underneath.
			g.DrawString(drawString, drawFontOutline, drawBrushOutline, x+3, y+3, strFormat);			
			g.DrawString(drawString, drawFont, drawBrush, x, y, strFormat);			
			//Draw small rectangle showing rotation point
			g.DrawRectangle(pen, rotatePoint.X, rotatePoint.Y, 1, 2);
			//opacity see\ms to have to be after drawstring to avoid flicker.
			//Math.DivRem(DateTime.Now.Second, 60, Opacity);
			Opacity = .25;
			//Opacity = DateTime.Now.Second / 360.0 + 0.18;
			//Opacity = Decimal.Remainder(DateTime.Now.Second, 60);
			

			
		
		}

		private void Form1_Click(object sender, System.EventArgs e)
		{
			Opacity = 1;  //DateTime.Now.Second / 360.0 + 0.20;
		}

		private void pbHour_Click(object sender, System.EventArgs e)
		{

		}

		private void pbHour_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			Close();
			
		}

		private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
		}

		private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Alt && e.KeyCode == Keys.E)
				Close();

		}

		private void Form1_DoubleClick(object sender, System.EventArgs e)
		{
			Close();
		}

		private void Form1_MouseHover(object sender, System.EventArgs e)
		{
			Opacity = 1;  //DateTime.Now.Second / 360.0 + 0.20;
		}
	}
}
