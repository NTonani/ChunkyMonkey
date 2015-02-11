/*
 * CHUNKY MONKEY 
 * 
 * See Readme.docx for instructions
 * @Authors: Nathan Tonani and Leland Burlingame
 * 
 * 
 */



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Media;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Permissions;
using System.Collections;



using WMPLib;

namespace ChunkyMonkey
{
    public partial class Form1 : Form
    {
        //Timer
        double rem = 15.0;

        private bool allowclick = false;
        public static StreamWriter qwrite = new StreamWriter("quadrants.txt", true);
        public static StreamWriter birdx = new StreamWriter("birdy.txt", true);

        public static StreamWriter highscoreWrite;
        public static StreamReader highscoreRead = new StreamReader("highscore.txt");
        public int hs = 0;
        private int bcount = 0;
        /*
         * AUDIO
         */
        private WMPLib.WindowsMediaPlayer mainscreenmusic;  //  main screen menu music
        private WMPLib.WindowsMediaPlayer gamemusic1; //  // starting part of game music song
        private WMPLib.WindowsMediaPlayer gamemusic2;  // looping part of game music song

        private SoundPlayer[] screeches = new SoundPlayer[4]; // monkey screech audio

        private bool mutedsound = false;           // true if sound is muted, initialized as opposite of whatever is here, as the MuteSound() method call in the form_load will change it

        private Hitbox[] hitboxes = new Hitbox[13];

        /*
         * background
         */

        private Bitmap hillbase = new Bitmap(@"Data\\images\\background\\newhills.png");
        private Bitmap hill;

        private Bitmap riverbase = new Bitmap(@"Data\\objects\\background\\rivernew.png");
        private Bitmap river1 = new Bitmap(@"Data\\objects\\background\\rivernew.png");
        private Bitmap river2 = new Bitmap(@"Data\\objects\\background\\rivernew.png");
        int river1x = 0;
        int river2x;


        private Bitmap leaf1 = new Bitmap(@"Data\\objects\\background\\leaf.png");
        private Bitmap leaf2 = new Bitmap(@"Data\\objects\\background\\leaf2.png");
        private Bitmap leaf3 = new Bitmap(@"Data\\objects\\background\\leaf3.png");
        private Bitmap leaf4 = new Bitmap(@"Data\\objects\\background\\leaf4.png");



        /*
         * BANANA
         */
        private Banana banana1;
        private static Random rnd;
        private static Random rand;
        private Bitmap bananabase = new Bitmap("Data\\images\\banana.bmp");
        private Bitmap banana = new Bitmap("Data\\images\\banana.bmp");
        private Hitbox bananahb;


        /*
         * MONKEY
         */

        private Monkey monkey1;
        private Bitmap monkeybase = new Bitmap(@"Data\\monkeyswing.png");
        private Bitmap monkeybase2 = new Bitmap(@"Data\\monkeysurf.png");
        private Bitmap monkey;              // the resized monkey image based on monkeybase
        private Bitmap monkeyfall;              // the resized monkey image based on monkeybase
        private int monkey_ix = 0;

        private bool allowhoot = true;

        

        /*
         * BIRD
         */

        private Bird bird1, bird2, bird11, bird22;
        private Bitmap birdbase = new Bitmap(@"Data\\birdhit.png");
        private Bitmap bird, bird1bm, bird11bm, bird22bm;

        private int set = 0;
        private int first = 0;
        private int second = 0;

        /*
         * CROCODILE 
         */

        private Crocodile croc1, croc2;
        private Bitmap crocopenbase = new Bitmap("Data\\objects\\creatures\\crocopen.png");
        private Bitmap crocopen;
        private Bitmap crocclosedbase = new Bitmap("Data\\objects\\creatures\\crocclosed.png");
        private Bitmap crocclosed;
        private int croc_ix = 0;  //  for animating the crocodile, will display from the bitmaps, croc[0] and croc[1]


        /*
         * MENU VARIABLES AND IMAGES
         */
        private bool printed = false;
        private bool pause = false;
        private bool menupause = false;
        private bool mainpause = false;
        private bool gameOver = false;


        private Bitmap mutebase = new Bitmap(@"Data\\images\\buttons\\muted.png");
        private Bitmap muteb;

        private Bitmap unmutebase = new Bitmap(@"Data\\images\\buttons\\unmuted.png");
        private Bitmap unmuteb;

        private Bitmap playbbase = new Bitmap(@"Data\\images\\buttons\\play.png");
        private Bitmap playb;

        private Bitmap pausebbase = new Bitmap(@"Data\\images\\buttons\\pause.png");
        private Bitmap pauseb;

        private Bitmap readmebase = new Bitmap(@"Data\\images\\buttons\\readme.png");
        private Bitmap readmeb;

        private Bitmap exitbbase = new Bitmap(@"Data\\images\\buttons\\exit.png");
        private Bitmap exitb;

        /*
        * SLIDING MENU
        */

        int menux;
        int menudx = 0;
        int faded_x = 0;


        int min_X;
        int min_Y;
        int max_X;
        int max_Y;

        /*
         * VINE VALUES
         */ 

        double vinex1 = -30;
        double vinex2 = -30;
        double vinex3 = -30;
        double vinex4 = -30;
        double vinex5 = -30;

        double vinedx1 = -10;
        double vinedx2 = -5;
        double vinedx3 = -0;
        double vinedx4 = -5;
        double vinedx5 = -10;

        int viney1 = 0;
        int viney2 = 300;
        int viney3 = 450;
        int viney4 = 100;
        int viney5 = 100;

        double vinedy1 = 0;
        double vinedy2 = 0;
        double vinedy4 = 0;
        double vinedy5 = 0;

        private int quadrant;
        private int lastQuad =  -1;

        private bool start;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hs = Convert.ToInt32(highscoreRead.ReadLine());
            highscoreRead.Close();
            highscoreWrite = new StreamWriter("highscore.txt", false);

            river2x = 0 + river1.Width;
            start = false;
            rnd = new Random();
            rand = new Random();
            banana1 = new Banana(this.Width + 20, 0, -10, 0, bananabase.Width, bananabase.Height, this.Width, this.Height);

            monkey1 = new Monkey((this.Width / 5 * 2 - 50/2), this.Height/10, 0, 0, .4, 100, 133, this.Width, this.Height);

            
            croc2 = new Crocodile(this.Width, this.Height-350, 0, 0, crocopenbase.Width, crocopenbase.Height, this.Width, this.Height,false);
            croc1 = new Crocodile(this.Width, this.Height - 350, -6, 0, crocopenbase.Width, crocopenbase.Height, this.Width, this.Height,false);

            bird1 = new Bird(this.Width, 0, -6,0, birdbase.Width, birdbase.Height, this.Width, this.Height,false);
            bird2 = new Bird(this.Width, this.Height/3, -8,0, birdbase.Width - 50, birdbase.Height - 50, this.Width, this.Height,false);
            bird11 = new Bird(this.Width, 0, -0, 0, birdbase.Width, birdbase.Height-50, this.Width, this.Height,false);
            bird22 = new Bird(this.Width, this.Height / 3, 0, 0, birdbase.Width - 50, birdbase.Height - 50, this.Width, this.Height,false);
            

            bananahb = new Hitbox(banana1.X, banana1.Y, banana1.Width, banana1.Height);
            hitboxes[0] = new Hitbox(monkey1.X, monkey1.Y, monkey1.Width, monkey1.Height);

            hitboxes[1] = new Hitbox(bird1.X + bird1.Width / 8, bird1.Y + bird1.Height / 2 - bird1.Height / 8, bird1.Width / 2, bird1.Height / 5);
            hitboxes[2] = new Hitbox(bird1.X + bird1.Width / 4, bird1.Y + bird1.Height / 8, bird1.Width - bird1.Width / 2 - bird1.Width / 8, bird1.Height - bird1.Height / 3);

            hitboxes[3] = new Hitbox(bird2.X + bird2.Width / 8, bird2.Y + bird2.Height / 2 - bird2.Height / 8, bird2.Width / 2, bird2.Height / 5);
            hitboxes[4] = new Hitbox(bird2.X + bird2.Width / 4, bird2.Y + bird2.Height / 8, bird2.Width - bird2.Width / 2 - bird2.Width / 8, bird2.Height - bird2.Height / 3);

            hitboxes[5] = new Hitbox(bird11.X + bird11.Width / 8, bird11.Y + bird11.Height / 2 - bird11.Height / 8, bird11.Width / 2, bird11.Height / 5);
            hitboxes[6] = new Hitbox(bird11.X + bird11.Width / 4, bird11.Y + bird11.Height / 8, bird11.Width - bird11.Width / 2 - bird11.Width / 8, bird11.Height - bird11.Height / 3);

            hitboxes[7] = new Hitbox(bird22.X + bird22.Width / 8, bird22.Y + bird22.Height / 2 - bird22.Height / 8, bird22.Width / 2, bird22.Height / 5);
            hitboxes[8] = new Hitbox(bird22.X + bird22.Width / 4, bird22.Y + bird22.Height / 8, bird22.Width - bird22.Width / 2 - bird22.Width / 8, bird22.Height - bird22.Height / 3);

            hitboxes[9] = new Hitbox(croc1.X + croc1.Width / 14 * 2, croc1.Y + croc1.Height / 14 * 5, croc1.Width / 14 * 14, croc1.Height);
            hitboxes[10] = new Hitbox(croc1.X + croc1.Width / 14 * 4, croc1.Y, croc1.Width / 14 * 9, croc1.Height);

            hitboxes[11] = new Hitbox(croc2.X + croc2.Width / 14 * 2, croc2.Y + croc2.Height / 14 * 5, croc2.Width / 14 * 14, croc2.Height);
            hitboxes[12] = new Hitbox(croc2.X + croc2.Width / 14 * 4, croc2.Y, croc2.Width / 14 * 9, croc2.Height);



            CalculateBounds();

            this.KeyDown += new KeyEventHandler(Form_KeyDownp);


            this.SetStyle(ControlStyles.StandardDoubleClick, false);


            mainscreenmusic = new WMPLib.WindowsMediaPlayer();
            mainscreenmusic.URL = "dkr.mp3";
            mainscreenmusic.controls.stop();

            gamemusic1 = new WMPLib.WindowsMediaPlayer();
            gamemusic1.URL = "Data\\sounds\\dkrcs1.mp3";
            gamemusic1.controls.play();

            gamemusic2 = new WMPLib.WindowsMediaPlayer();
            gamemusic2.URL = "Data\\sounds\\dkrcs2.mp3";
            gamemusic2.controls.stop();

            screeches[0] = new SoundPlayer("Data\\sounds\\screech1.wav");
            screeches[1] = new SoundPlayer("Data\\sounds\\screech2.wav");
            screeches[2] = new SoundPlayer("Data\\sounds\\screech3.wav");
            screeches[3] = new SoundPlayer("Data\\sounds\\screech4.wav");

            MuteSound();

            banana.MakeTransparent();
            bananabase.MakeTransparent();


            crocopen.MakeTransparent();
            crocclosed.MakeTransparent();

            birdbase.MakeTransparent();

            timer1.Start();
            timer3.Start();
            timer4.Start();

            menux = 0;
            
        }
        
        private static double CalculateDistance(int x, int y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CalculateBounds();

            if (WindowState == FormWindowState.Minimized)
            {
                pause = true;
            }
        }

        private void CalculateBounds()
        {
            //quadText.AppendText(((int)(((double)bird2.Y / bird2.oldsH) * this.Height)).ToString());
            //quadText.AppendText(" Cur height width" + this.Height.ToString() + " "  + this.Width.ToString() + "Bird X,Y: " + bird2.X.ToString() + " " + bird2.Y.ToString());
            banana1.Resize(this.Width, this.Height);

            monkey1.Resize(this.Width, this.Height);

            croc1.Resize(this.Width, this.Height);
            croc2.Resize(this.Width, this.Height);

            bird1.Resize(this.Width, this.Height);
            bird2.Resize(this.Width, this.Height);
            bird11.Resize(this.Width, this.Height);
            bird22.Resize(this.Width, this.Height);

            
           // compon.AppendText("Cur height width" + this.Height.ToString() + " " + this.Width.ToString() + "Bird X,Y: " + bird2.X.ToString() + " " + bird2.Y.ToString());

            //monkey bitmap
            monkey = new Bitmap(monkeybase, new Size(monkey1.Width, monkey1.Height));
            monkeyfall = new Bitmap(monkeybase2, new Size(monkey1.Width, monkey1.Height));
            hill = new Bitmap(hillbase, new Size(this.Width, this.Height));

            //croc bitmaps
            crocopen = new Bitmap(crocopenbase, new Size(croc1.Width, croc1.Height));
            crocclosed = new Bitmap(crocclosedbase, new Size(croc1.Width, croc1.Height));
            
            //bird bitmaps
            bird22bm = new Bitmap(birdbase, new Size(bird22.Width, bird22.Height));
            bird11bm = new Bitmap(birdbase, new Size(bird11.Width, bird11.Height));
            bird1bm = new Bitmap(birdbase, new Size(bird2.Width, bird2.Height));
            bird = new Bitmap(birdbase, new Size(bird1.Width, bird1.Height));

            banana = new Bitmap(bananabase, new Size(banana1.Width, banana1.Height));

            min_X = -16;
            min_Y = 0;
            max_X = this.Width - monkey.Width;
            max_Y = this.Height-monkey.Height-38;

            menux = 0 - this.Width / 9;

            unmuteb = new Bitmap(unmutebase, new Size(this.Width / 20, this.Width / 20));
            muteb = new Bitmap(mutebase, new Size(this.Width / 19, this.Width / 20));

            playb = new Bitmap(playbbase, new Size(this.Width / 20, this.Width / 20));
            pauseb = new Bitmap(pausebbase, new Size(this.Width / 20, this.Width / 20));

            readmeb = new Bitmap(readmebase, new Size(this.Width / 20, this.Width / 20));
            exitb = new Bitmap(exitbbase, new Size(this.Width / 20, this.Width / 20));

            //croc1.Resize(this.Width, this.Height);
            
            /*
            hitmonkey = new Hitbox(monkey1.X, monkey1.Y, monkey1.Width, monkey1.Height);
            hitbird1 = new Hitbox(bird1.X, bird1.Y, bird1.Width, bird1.Height);
            hitbird2 = new Hitbox(bird2.X, bird2.Y, bird2.Width, bird2.Height);
           // hitcroc = new Hitbox(croc1.X, croc1.Y, croc1.Width, croc1.Height);
             */
            bananahb = new Hitbox(banana1.X, banana1.Y, banana1.Width, banana1.Height);
            hitboxes[0] = new Hitbox(monkey1.X, monkey1.Y, monkey1.Width, monkey1.Height);

            hitboxes[1] = new Hitbox(bird1.X + bird1.Width / 8, bird1.Y + bird1.Height / 2 - bird1.Height / 8, bird1.Width / 2, bird1.Height / 5);
            hitboxes[2] = new Hitbox(bird1.X + bird1.Width / 4, bird1.Y + bird1.Height / 8, bird1.Width - bird1.Width / 2 - bird1.Width / 8, bird1.Height - bird1.Height / 3);

            hitboxes[3] = new Hitbox(bird2.X + bird2.Width / 8, bird2.Y + bird2.Height / 2 - bird2.Height / 8, bird2.Width / 2, bird2.Height / 5);
            hitboxes[4] = new Hitbox(bird2.X + bird2.Width / 4, bird2.Y + bird2.Height / 8, bird2.Width - bird2.Width / 2 - bird2.Width / 8, bird2.Height - bird2.Height / 3);

            hitboxes[5] = new Hitbox(bird11.X + bird11.Width / 8, bird11.Y + bird11.Height / 2 - bird11.Height / 8, bird11.Width / 2, bird11.Height / 5);
            hitboxes[6] = new Hitbox(bird11.X + bird11.Width / 4, bird11.Y + bird11.Height / 8, bird11.Width - bird11.Width / 2 - bird11.Width / 8, bird11.Height - bird11.Height / 3);

            hitboxes[7] = new Hitbox(bird22.X + bird22.Width / 8, bird22.Y + bird22.Height / 2 - bird22.Height / 8, bird22.Width / 2, bird22.Height / 5);
            hitboxes[8] = new Hitbox(bird22.X + bird22.Width / 4, bird22.Y + bird22.Height / 8, bird22.Width - bird22.Width / 2 - bird22.Width / 8, bird22.Height - bird22.Height / 3);

            hitboxes[9] = new Hitbox(croc1.X + croc1.Width / 14 * 2, croc1.Y + croc1.Height / 14 * 5, croc1.Width / 14 * 14, croc1.Height);
            hitboxes[10] = new Hitbox(croc1.X + croc1.Width / 14 * 4, croc1.Y, croc1.Width / 14 * 9, croc1.Height);

            hitboxes[11] = new Hitbox(croc2.X + croc2.Width / 14 * 2, croc2.Y + croc2.Height / 14 * 5, croc2.Width / 14 * 14, croc2.Height);
            hitboxes[12] = new Hitbox(croc2.X + croc2.Width / 14 * 4, croc2.Y, croc2.Width / 14 * 9, croc2.Height);
           // hitboxes[5] = new Hitbox(croc1.X + croc1.Width / 4, croc1.Y, croc1.Width - croc1.Width / 2, croc1.Height);

        }


        /*PAINT*/
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!mainpause)
            {
                try
                {
                    Graphics g = e.Graphics;

                    g.DrawImage(hill, 0, this.Height / 15);
                    
                    Pen greenPen = new Pen(Color.Green, 7);
                    Pen redPen = new Pen(Color.Red);

                    // quadText.Clear();
                    //compon.Clear();
                    // quadText.AppendText(croc1.X.ToString() + "," + croc1.Y.ToString());
                    // compon.AppendText(croc2.X.ToString() + "," + croc2.Y.ToString());
                    if (croc_ix == 0)
                    {
                        g.DrawImage(crocopen, croc1.X, croc1.Y);
                        g.DrawImage(crocclosed, croc2.X, croc2.Y);
                    }
                    else
                    {
                        g.DrawImage(crocclosed, croc1.X, croc1.Y);
                        g.DrawImage(crocopen, croc2.X, croc2.Y);
                    }
                    //g.DrawRectangle(redPen, new Rectangle(croc1.X, croc1.Y, croc1.Width, croc1.Height));
                    // e.Graphics.DrawRectangle(greenPen, new Rectangle(croc1.X, croc1.Y, croc1.Width, croc1.Height));

                    g.DrawImage(bird, bird1.X, bird1.Y);
                    g.DrawImage(bird1bm, bird2.X, bird2.Y);
                    g.DrawImage(bird11bm, bird11.X, bird11.Y);
                    g.DrawImage(bird22bm, bird22.X, bird22.Y);
                    birdx.WriteLine(bird1.Y + " " + bird2.Y);

                    //e.Graphics.DrawRectangle(greenPen, new Rectangle(bird1.X, bird1.Y, bird1.Width, bird1.Height));
                    if (!banana1.hit)
                    {
                        g.DrawImage(banana, banana1.X, banana1.Y);
                      //  g.DrawRectangle(redPen, new Rectangle(bananahb.X, bananahb.Y, bananahb.Width, bananahb.Height));
                    }


                    //hitbox visuals (temp)
                    redPen.Width = 3;
                    /*
                    g.DrawRectangle(redPen, new Rectangle(hitboxes[0].X, hitboxes[0].Y, hitboxes[0].Width, hitboxes[0].Height));
                    g.DrawRectangle(redPen, new Rectangle(hitboxes[1].X, hitboxes[1].Y, hitboxes[1].Width, hitboxes[1].Height));
                    g.DrawRectangle(redPen, new Rectangle(hitboxes[2].X, hitboxes[2].Y, hitboxes[2].Width, hitboxes[2].Height));
                    g.DrawRectangle(greenPen, new Rectangle(hitboxes[3].X, hitboxes[3].Y, hitboxes[3].Width, hitboxes[3].Height));
                    g.DrawRectangle(greenPen, new Rectangle(hitboxes[4].X, hitboxes[4].Y, hitboxes[4].Width, hitboxes[4].Height));
                    g.DrawRectangle(redPen, new Rectangle(hitboxes[5].X, hitboxes[5].Y, hitboxes[5].Width, hitboxes[5].Height));
                    g.DrawRectangle(redPen, new Rectangle(hitboxes[6].X, hitboxes[6].Y, hitboxes[6].Width, hitboxes[6].Height));
                    g.DrawRectangle(greenPen, new Rectangle(hitboxes[7].X, hitboxes[7].Y, hitboxes[7].Width, hitboxes[7].Height));
                    g.DrawRectangle(greenPen, new Rectangle(hitboxes[8].X, hitboxes[8].Y, hitboxes[8].Width, hitboxes[8].Height));
                    g.DrawRectangle(greenPen, new Rectangle(hitboxes[9].X, hitboxes[9].Y, hitboxes[9].Width, hitboxes[9].Height));
                    g.DrawRectangle(greenPen, new Rectangle(hitboxes[10].X, hitboxes[10].Y, hitboxes[10].Width, hitboxes[10].Height));
                    */
                    // g.DrawRectangle(redPen, new Rectangle(hitboxes[5].X, hitboxes[5].Y, hitboxes[5].Width, hitboxes[5].Height));

                   


                    greenPen.Width = 1;
                   // e.Graphics.DrawRectangle(greenPen, new Rectangle(monkey1.X, monkey1.Y, monkey1.Width, monkey1.Height));
                    //e.Graphics.DrawRectangle(redPen, new Rectangle(0, 0, this.Width, this.Height / 2));
                    //e.Graphics.DrawRectangle(redPen, new Rectangle(0, this.Height / 2, this.Width, this.Height / 2));

                    //Collision detection
                    /*
                    if (DetectCollision(croc1) || DetectCollision(bird1))
                    {
                        g.DrawImage(bananabase, monkey1.X - 20, monkey1.Y - 20);
                    }*/

                    //if(hitboxes[0].Hit(bird1) || hitboxes[0].Hit(bird2) || hitboxes[0].Hit(croc1))
                    // g.DrawImage(bananabase, monkey1.X - 20, monkey1.Y - 20);
                    if (!pause && !menupause && !gameOver && !mainpause && start)
                    {
                        for (int i = 1; i < hitboxes.Length; i++)
                        {
                            if (hitboxes[0].Hit(hitboxes[i]))
                            {
                                // g.DrawImage(bananabase, monkey1.X - 20, monkey1.Y - 20);
                                gameOver = true;
                                timer5.Start();
                                MonkeyHoot();
                            }
                        }
                    }
                    //.DrawRectangle(greenPen, new Rectangle(0, quadrant, this.Width, 2* monkey1.Height));

                    greenPen.Width = 7;

                    greenPen.Color = Color.Green;
                    // Create points that define curve.
                    Point point1 = new Point((int)vinex1, (int)viney1);
                    Point point2 = new Point((int)vinex2, (int)viney2);
                    Point point3 = new Point((int)vinex3, (int)viney3);
                    Point point4 = new Point((int)vinex4, (int)viney4);
                    Point point5 = new Point((int)vinex5, (int)viney5);
                    Point[] curvePoints = { point1, point2, point3, point4, point5 };

                    // Draw lines between original points to screen.

                    // Draw curve to screen.
                    e.Graphics.DrawCurve(greenPen, curvePoints);
                    greenPen.Color = Color.DarkGreen;
                    greenPen.Width = 3;

                    point1 = new Point((int)vinex1 - 3, (int)viney1);
                    point2 = new Point((int)vinex2 - 3, (int)viney2);
                    point3 = new Point((int)vinex3 - 3, (int)viney3);
                    point4 = new Point((int)vinex4 - 3, (int)viney4);
                    point5 = new Point((int)vinex5 - 3, (int)viney5);
                    Point[] cps = { point1, point2, point3, point4, point5 };
                    e.Graphics.DrawCurve(greenPen, curvePoints);

                    if (monkey_ix == 1)
                    {
                        g.DrawImage(monkey, monkey1.X, monkey1.Y);
                    }
                    else
                    {
                        g.DrawImage(monkeyfall, monkey1.X, monkey1.Y);
                    }


                    greenPen.Width = 12;
                    greenPen.Color = Color.LimeGreen;

                    g.DrawImage(leaf3, point4.X, point4.Y,leaf3.Width/3*2, leaf3.Height/3*2);
                    g.DrawImage(leaf1, point5.X - leaf1.Width/3, point5.Y-5, leaf1.Width/3*2, leaf1.Height/3*2);
                    g.DrawImage(leaf4, point2.X, point2.Y);


                    g.DrawImage(river1, river1x, this.Height / 8*7, river1.Width, river1.Height);

                    g.DrawImage(river2, river2x, this.Height/8*7, river1.Width, river1.Height);

                    SolidBrush b = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                    if (pause || menupause || gameOver)
                    {

                        e.Graphics.FillRectangle(b, new Rectangle(faded_x, -20, this.Width + 20, this.Height + 20));
                    }

                    b.Color = Color.White;

                    g.FillRectangle(b, new Rectangle(menux, -10, (this.Width / 9), this.Height + 30));

                    if (!mutedsound)
                    {
                        g.DrawImage(unmuteb, menux + this.Width / 35, 50);
                    }
                    else
                    {
                        g.DrawImage(muteb, menux + this.Width / 35, 50);
                    }

                    if (pause)
                    {
                        g.DrawImage(playb, menux + this.Width / 35, 175);
                    }
                    else
                    {
                        g.DrawImage(pauseb, menux + this.Width / 35, 175);
                    }

                    g.DrawImage(readmeb, menux + this.Width / 35, 300);
                    g.DrawImage(exitb, menux + this.Width / 35, this.Height/5*4);

                    greenPen.Dispose();
                    b.Dispose();

                    g.DrawString(rem.ToString("0.0"), new Font(FontFamily.GenericSansSerif, 23, FontStyle.Regular), new SolidBrush(Color.Black), new Rectangle(new Point(this.Width - this.Width / 10, this.Height / 10), new Size(this.Width / 10, this.Height / 10)));
                    g.DrawString("Bananas: "+bcount.ToString(), new Font(FontFamily.GenericSansSerif, 23, FontStyle.Regular), new SolidBrush(Color.White), new Rectangle(new Point(this.Width - this.Width / 8, this.Height - this.Height/ 6), new Size(this.Width / 8, this.Height / 10)));
                    if (gameOver)
                    {
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        if (bcount <= hs && !printed)
                        {
                            g.DrawString("Game Over\nScore: " + bcount.ToString(), new Font(FontFamily.GenericSansSerif, 60, FontStyle.Regular), new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height), sf);
                            g.DrawString("High Score: " + hs.ToString() + " Bananas", new Font(FontFamily.GenericSansSerif, 20, FontStyle.Regular), new SolidBrush(Color.White), new Rectangle(this.Width - this.Width / 8, 0, this.Width / 8, this.Height / 8), sf);
                        }
                        else
                        {
                            g.DrawString("New High Score!\n" + bcount.ToString() + " Bananas", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Regular), new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height), sf);
                            hs = bcount;
                            printed = true;
                        }
                    }

                }
                catch (Exception except)
                {
                    qwrite.WriteLine(except.Data.ToString());
                    qwrite.Close();
                    Application.Exit();
                }
            }
            else
            {
                Rectangle main = new Rectangle(this.Width / 10, (this.Height-38) / 10, this.Width/10 * 8, (this.Height-38)/10 *8);
                Graphics g = e.Graphics;
                Pen greenPen = new Pen(Color.LightGreen, 7);
                Pen bluePen = new Pen(Color.Blue,5);
                Pen lbluePen = new Pen(Color.LightBlue,5);
                g.DrawRectangle(greenPen, main);
                g.DrawRectangle(bluePen, new Rectangle(main.X+main.Width/3, main.Y+main.Height/4, main.Width/3,main.Height/10));
                g.DrawRectangle(lbluePen, new Rectangle(main.X + main.Width / 3, main.Y + main.Height / 2, main.Width / 3, main.Height / 10));
                //MessageBox.Show(bird1.X.ToString() + " " + monkey1.X.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
                try{
                    
                    menux += menudx;

                    if (gamemusic1.playState == WMPLib.WMPPlayState.wmppsStopped)
                    {
                        gamemusic2.controls.play();
                    }

                    /*
                     * SLIDING MENU CODE
                     * 
                     */
                    Point m1 = this.PointToClient(Cursor.Position);

                    if ((m1.X > this.Width / 10 && menux > 0 - this.Width / 9) && !pause)
                    {
                        menudx = -20;
                        menupause = false;
                    }
                    else if (menux < 0 - this.Width / 9 || menux > -5)
                    {
                        menudx = 0;
                        if (menux < 0 - this.Width / 9)
                            menux = 0 - this.Width / 9;
                        else if (menux > -5)
                        {
                            menux = -5;
                        }
                    }
                    else if ((m1.X < this.Width / 10 && menux < -20) || pause)
                    {
                        menudx = 20;
                        menupause = true;
                    }

                    if (!pause && !menupause && !gameOver && !mainpause && start)
                    {
                        if (hitboxes[0].Hit(bananahb) && !banana1.hit)
                        {
                            rem += 5.0;
                            banana1.hit = true;
                            bcount++;
                        }

                        viney3 = monkey1.Y;

                        vinex1 += vinedx1;
                        vinex2 += vinedx2;
                        vinex3 += vinedx3;
                        vinex4 += vinedx4;
                        vinex5 += vinedx5;

                        vinedy1 = 0;
                        if (viney2 < viney1 + 200)
                        {
                            vinedy1 = monkey1.dY / 7.5;
                            vinedx1 = -9.5;
                        }

                        vinedy2 = monkey1.dY / 1.75;
                        vinedy4 = monkey1.dY / 1.1;
                        vinedy5 = monkey1.dY;

                        viney1 += (int)(vinedy1);
                        viney2 += (int)(vinedy2);
                        viney4 += (int)(vinedy4);
                        viney5 += (int)(vinedy5);


                        /*
                         * MOVEMENT CALLS
                         */
                        //Two crocs. Once croc reaches monkey, the next croc goes.
                    
                        if (croc1.X < monkey1.X - croc1.X / 2 && !croc2.changed)
                        {
                            croc2.dX = rnd.Next(-5, -3);
                            croc2.changed = true;
                            croc1.changed = false;
                        }
                        else if (croc2.X < monkey1.X - croc2.X / 2 && !croc1.changed)
                        {
                            croc1.dX = rnd.Next(-5, -3);
                            croc1.changed = true;
                            croc2.changed = false;
                        }
                    






















                            //Four birds, two sets. Once one set has reached the monkey, the next set goes. 
                            if ((bird11.X < monkey1.X - bird11.Width / 2 || bird22.X < monkey1.X - bird22.Width / 2) && first < 2)
                            {
                                if (bird11.X < monkey1.X - bird11.Width / 2 && !bird1.changed)
                                {
                                    first++;
                                    // bird1.dX = -5;
                                    bird1.changed = true;
                                    bird11.changed = false;
                                    bird1.dX = rnd.Next(-14, -6);
                                    bird1.X = bird1.oldsW + rnd.Next(10, 100);
                                    bird1.dY = 0;
                                    bird1.Y = rnd.Next(0, this.Height / 3 - bird1.Height);
                                    if (rnd.Next(1, 6) != 1)
                                    {
                                        bird1.low = false;
                                        if (bird2.changed)
                                        {
                                            while (!bird1.ProperY(bird2, monkey1))
                                            {
                                                bird1.Y = rnd.Next(0, this.Height / 3 - bird1.Height);
                                            }
                                        }
                                        else if (bird22.changed)
                                        {
                                            while (!bird1.ProperY(bird22, monkey1))
                                            {
                                                bird1.Y = rnd.Next(0, this.Height / 3 - bird1.Height);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bird1.low = true;
                                        bird1.Y = rnd.Next(2*monkey.Height, this.Height / 3);
                                    }
                                    //quadrant = SmallestQuad();
                                }

                                if (bird22.X < monkey1.X - bird22.Width / 2 && !bird2.changed)
                                {
                                    first++;
                                    // bird2.dX = -5;
                                    bird2.changed = true;
                                    bird22.changed = false;
                                    bird2.dX = rnd.Next(-12, -8);
                                    bird2.X = bird2.oldsW + rnd.Next(10, 100);
                                    bird2.dY = 0;
                                    bird2.Y = rnd.Next(this.Height / 3, this.Height / 3 + this.Height / 3 - bird2.Height);

                                    if (bird1.changed && !bird1.low)
                                    {
                                        while (!bird2.ProperY(bird1, monkey1))
                                        {
                                            bird2.Y = rnd.Next(this.Height / 3, this.Height / 3 + this.Height / 3 - bird2.Height);
                                        }
                                    }
                                    else if (bird11.changed && !bird11.low)
                                    {
                                        while (!bird2.ProperY(bird11, monkey1))
                                        {
                                            bird2.Y = rnd.Next(this.Height / 3, this.Height / 3 + this.Height / 3 - bird2.Height);
                                        }
                                    }


                                    //quadrant = SmallestQuad();
                                }

                                if (first == 2)
                                {
                                    second = 0;
                                }
                                //bird11.X = this.Width + bird11.Width;
                                //bird22.X = this.Width + bird22.Width;
                            }

                            else if ((bird1.X < monkey1.X - bird1.Width / 2 || bird2.X < monkey1.X - bird2.Width / 2) & second < 2)
                            {

                                set = 2;
                                if (bird1.X < monkey1.X - bird1.Width / 2 && !bird11.changed)
                                {
                                    second++;
                                    // bird1.dX = -5;
                                    bird11.changed = true;
                                    bird1.changed = false;
                                    bird11.dX = rnd.Next(-12, -8);
                                    bird11.X = bird1.oldsW + rnd.Next(10, 100);
                                    bird11.dY = 0;
                                    bird11.Y = rnd.Next(0, this.Height / 3 - bird11.Height);
                                    if (rnd.Next(1, 6) != 1)
                                    {
                                        bird11.low = false;
                                        if (bird22.changed)
                                        {

                                            while (!bird11.ProperY(bird22, monkey1))
                                            {
                                                bird11.Y = rnd.Next(0, this.Height / 3 - bird11.Height);
                                            }
                                        }
                                        else if (bird2.changed)
                                        {

                                            while (!bird11.ProperY(bird2, monkey1))
                                            {
                                                bird11.Y = rnd.Next(0, this.Height / 3 - bird11.Height);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bird11.low = true;
                                        bird11.Y = rnd.Next(2*monkey.Height + monkey.Height / 2, this.Height / 3);
                                    }
                                    


                                    //quadrant = SmallestQuad();
                                }

                                if (bird2.X < monkey1.X - bird2.Width / 2 && !bird22.changed)
                                {
                                    second++;
                                    // bird2.dX = -5;
                                    bird2.changed = false;
                                    bird22.changed = true;
                                    bird22.dX = rnd.Next(-10, -9);
                                    bird22.X = bird2.oldsW + rnd.Next(10, 100);
                                    bird22.dY = 0;
                                    bird22.Y = rnd.Next(this.Height / 3, this.Height / 3 + this.Height / 3 - bird22.Height);

                                    if (bird11.changed && !bird11.low)
                                    {
                                        while (!bird22.ProperY(bird11, monkey1))
                                        {
                                            bird22.Y = rnd.Next(this.Height / 3, this.Height / 3 + this.Height / 3 - bird22.Height);
                                        }
                                    }
                                    else if (bird1.changed && !bird1.low)
                                    {
                                        while (!bird22.ProperY(bird1, monkey1))
                                        {
                                            bird22.Y = rnd.Next(this.Height / 3, this.Height / 3 + this.Height / 3 - bird22.Height);
                                        }

                                        //quadrant = SmallestQuad();
                                    }
                                }
                                //bird1.X = this.Width + bird1.Width;
                                //bird2.X = this.Width + bird2.Width;

                                if (second == 2)
                                {
                                    first = 0;
                                }
                            }
                    

















                        if (banana1.X <= -300)
                        {
                            banana1.X = this.Width + 20;
                            banana1.Y = RandomQuad();
                            banana1.dX = rnd.Next(-8, -5);
                            banana1.hit = false;
                        }

                        river1x += -20;
                        if (river1x <= 0 - river1.Width)
                            river1x = river2x + river1.Width - 20;
                        river2x += -20;
                        if (river2x <= 0 - river2.Width)
                            river2x = river1x + river2.Width;

                        monkey1.Move();

                        banana1.Move();
                    
                        croc1.Move();
                        croc2.Move();
                    
                        bird1.Move();
                        bird2.Move();
                        bird22.Move();
                        bird11.Move();
                    
                    

                        // ceiling and floor collision detection, collision = death
                        if (monkey1.Y < min_Y)
                        {
                            gameOver = true;
                            monkey1.Y = min_Y;
                            monkey1.dY = 0;
                            MonkeyHoot();
                            timer5.Start();
                        }
                        else if (monkey1.Y > max_Y)
                        {
                            gameOver = true;
                            monkey1.Y = max_Y;
                            monkey1.dY = 0;
                            MonkeyHoot();
                            timer5.Start();
                        }
                        //UPDATING HITBOXES
                        bananahb = new Hitbox(banana1.X, banana1.Y, banana1.Width, banana1.Height);
                        hitboxes[0] = new Hitbox(monkey1.X, monkey1.Y, monkey1.Width, monkey1.Height);

                        hitboxes[1] = new Hitbox(bird1.X + bird1.Width / 8, bird1.Y + bird1.Height / 2 - bird1.Height / 8, bird1.Width / 2, bird1.Height / 5);
                        hitboxes[2] = new Hitbox(bird1.X + bird1.Width / 4, bird1.Y + bird1.Height / 8, bird1.Width - bird1.Width / 2 - bird1.Width / 8, bird1.Height - bird1.Height / 3);

                        hitboxes[3] = new Hitbox(bird2.X + bird2.Width / 8, bird2.Y + bird2.Height / 2 - bird2.Height / 8, bird2.Width / 2, bird2.Height / 5);
                        hitboxes[4] = new Hitbox(bird2.X + bird2.Width / 4, bird2.Y + bird2.Height / 8, bird2.Width - bird2.Width / 2 - bird2.Width / 8, bird2.Height - bird2.Height / 3);

                        hitboxes[5] = new Hitbox(bird11.X + bird11.Width / 8, bird11.Y + bird11.Height / 2 - bird11.Height / 8, bird11.Width / 2, bird11.Height / 5);
                        hitboxes[6] = new Hitbox(bird11.X + bird11.Width / 4, bird11.Y + bird11.Height / 8, bird11.Width - bird11.Width / 2 - bird11.Width / 8, bird11.Height - bird11.Height / 3);

                        hitboxes[7] = new Hitbox(bird22.X + bird22.Width / 8, bird22.Y + bird22.Height / 2 - bird22.Height / 8, bird22.Width / 2, bird22.Height / 5);
                        hitboxes[8] = new Hitbox(bird22.X + bird22.Width / 4, bird22.Y + bird22.Height / 8, bird22.Width - bird22.Width / 2 - bird22.Width / 8, bird22.Height - bird22.Height / 3);

                        hitboxes[9] = new Hitbox(croc1.X + croc1.Width / 14 * 2, croc1.Y + croc1.Height / 14 * 5, croc1.Width / 14 * 14, croc1.Height);
                        hitboxes[10] = new Hitbox(croc1.X + croc1.Width / 14 * 4, croc1.Y, croc1.Width / 14 * 9, croc1.Height);

                        hitboxes[11] = new Hitbox(croc2.X + croc2.Width / 14 * 2, croc2.Y + croc2.Height / 14 * 5, croc2.Width / 14 * 14, croc2.Height);
                        hitboxes[12] = new Hitbox(croc2.X + croc2.Width / 14 * 4, croc2.Y, croc2.Width / 14 * 9, croc2.Height);
                        //hitboxes[5] = new Hitbox(croc1.X + croc1.Width / 4, croc1.Y, croc1.Width-croc1.Width/2, croc1.Height);
                    }
                    
                    this.Invalidate();
                }
                catch (Exception except)
                {
                    qwrite.WriteLine(except.Data.ToString());
                    qwrite.Close();
                    MessageBox.Show("EXITING");
                    Application.Exit();
                }
            
        }


        public int RandomQuad()
        {
            try
            {
                int quad = rand.Next(1, 5);
                qwrite.WriteLine(quad.ToString());
                
                //lastQuad = quad;
                if (quad == 1)
                {
                    return (rand.Next(0, this.Height / 4 - banana.Height/2));

                }
                else if (quad == 2)
                {
                    return (rand.Next(this.Height / 4, this.Height / 2 - banana.Height/2));
                }
                else if (quad == 3)
                {
                    return (rand.Next(this.Height / 2, this.Height / 2 + this.Height / 4 - banana.Height/2));
                }
                else
                {
                    return (rand.Next(this.Height / 2 + this.Height / 4, this.Height - banana.Height/2
                        ));
                }
            }
            catch (Exception except)
            {
                qwrite.WriteLine(except.Data.ToString());
                qwrite.Close();
                Application.Exit();
            }
            return -1;
        }

        private bool CheckQuad(int quad)
        {
            try
            {
                if (quad == 1)
                {
                    if (bird1.Y < this.Height / 4 && bird1.X > this.Width - bird1.Width - this.Width / 10 && bird1.X < this.Width)
                        return false;
                    if (bird11.Y < this.Height / 4 && bird11.X > this.Width - bird11.Width - this.Width / 10 && bird11.X < this.Width)
                        return false;

                }
                else if (quad == 2)
                {
                    if (bird1.Y > this.Height / 4 && bird1.Y < this.Height / 2 && bird1.X > this.Width - bird1.Width - this.Width / 10 && bird1.X < this.Width)
                        return false;
                    if (bird11.Y > this.Height / 4 && bird11.Y < this.Height / 2 && bird11.X > this.Width - bird11.Width - this.Width / 10 && bird11.X < this.Width)
                        return false;
                    if (bird2.Y > this.Height / 4 && bird2.Y < this.Height / 2 && bird2.X > this.Width - bird2.Width - this.Width / 10 && bird2.X < this.Width)
                        return false;
                    if (bird22.Y > this.Height / 4 && bird22.Y < this.Height / 2 && bird22.X > this.Width - bird22.Width - this.Width / 10 && bird22.X < this.Width)
                        return false;
                }
                else if (quad == 3)
                {
                    if (bird2.Y > this.Height / 2 && bird2.Y < this.Height / 2 + this.Height / 4 && bird2.X > this.Width - bird2.Width - this.Width / 10 && bird2.X < this.Width)
                        return false;
                    if (bird22.Y > this.Height / 2 && bird22.Y < this.Height / 2 + this.Height / 4 && bird22.X > this.Width - bird22.Width - this.Width / 10 && bird22.X < this.Width)
                        return false;
                }
                else
                {
                    if (croc1.X > this.Width - croc1.Width && croc1.X < this.Width)
                        return false;
                    if (croc2.X > this.Width - croc2.Width && croc2.X < this.Width)
                        return false;
                }
                return true;
            }
            catch (Exception except)
            {
                qwrite.WriteLine(except.Data.ToString());
                qwrite.Close();
                Application.Exit();
            }
            return false;
        }

        // plays monkey screeching sound effect 
        private void MonkeyHoot()
        {
            if (allowhoot)
            {
                Random rand = new Random();
                int hoot = rand.Next(4);
                screeches[hoot].Play();
                timer2.Start();
                allowhoot = false;
            }
        }
        private void Reset()
        {
            //monkey1.X = this.Width + monkey1.Width;
            banana1.X = this.Width + monkey1.X;
            monkey1.Y = this.Height / 9;
            monkey1.dY = 0;


            croc2 = new Crocodile(this.Width, this.Height - 350, 0, 0, croc2.Width, croc2.Height, this.Width, this.Height, false);
            croc1 = new Crocodile(this.Width, this.Height - 350, -6, 0, croc1.Width, croc1.Height, this.Width, this.Height, false);

            bird1 = new Bird(this.Width, 0, -6, 0, bird1.Width, bird1.Height, this.Width, this.Height, false);
            bird2 = new Bird(this.Width, this.Height / 3, -8, 0, bird2.Width, bird2.Height, this.Width, this.Height, false);
            bird11 = new Bird(this.Width, 0, -0, 0, bird11.Width, bird11.Height, this.Width, this.Height, false);
            bird22 = new Bird(this.Width, this.Height / 3, 0, 0, bird22.Width, bird22.Height, this.Width, this.Height, false);

            bananahb = new Hitbox(banana1.X, banana1.Y, banana1.Width, banana1.Height);
            hitboxes[0] = new Hitbox(monkey1.X, monkey1.Y, monkey1.Width, monkey1.Height);

            hitboxes[1] = new Hitbox(bird1.X + bird1.Width / 8, bird1.Y + bird1.Height / 2 - bird1.Height / 8, bird1.Width / 2, bird1.Height / 5);
            hitboxes[2] = new Hitbox(bird1.X + bird1.Width / 4, bird1.Y + bird1.Height / 8, bird1.Width - bird1.Width / 2 - bird1.Width / 8, bird1.Height - bird1.Height / 3);

            hitboxes[3] = new Hitbox(bird2.X + bird2.Width / 8, bird2.Y + bird2.Height / 2 - bird2.Height / 8, bird2.Width / 2, bird2.Height / 5);
            hitboxes[4] = new Hitbox(bird2.X + bird2.Width / 4, bird2.Y + bird2.Height / 8, bird2.Width - bird2.Width / 2 - bird2.Width / 8, bird2.Height - bird2.Height / 3);

            hitboxes[5] = new Hitbox(bird11.X + bird11.Width / 8, bird11.Y + bird11.Height / 2 - bird11.Height / 8, bird11.Width / 2, bird11.Height / 5);
            hitboxes[6] = new Hitbox(bird11.X + bird11.Width / 4, bird11.Y + bird11.Height / 8, bird11.Width - bird11.Width / 2 - bird11.Width / 8, bird11.Height - bird11.Height / 3);

            hitboxes[7] = new Hitbox(bird22.X + bird22.Width / 8, bird22.Y + bird22.Height / 2 - bird22.Height / 8, bird22.Width / 2, bird22.Height / 5);
            hitboxes[8] = new Hitbox(bird22.X + bird22.Width / 4, bird22.Y + bird22.Height / 8, bird22.Width - bird22.Width / 2 - bird22.Width / 8, bird22.Height - bird22.Height / 3);
            hitboxes[9] = new Hitbox(croc1.X + croc1.Width / 14 * 2, croc1.Y + croc1.Height / 14 * 5, croc1.Width / 14 * 14, croc1.Height);
            hitboxes[10] = new Hitbox(croc1.X + croc1.Width / 14 * 4, croc1.Y, croc1.Width / 14 * 9, croc1.Height);

            hitboxes[11] = new Hitbox(croc2.X + croc2.Width / 14 * 2, croc2.Y + croc2.Height / 14 * 5, croc2.Width / 14 * 14, croc2.Height);
            hitboxes[12] = new Hitbox(croc2.X + croc2.Width / 14 * 4, croc2.Y, croc2.Width / 14 * 9, croc2.Height);
            rem = 15.0;
            second = 0;
            first = 0;
            bcount = 0;
            printed = false;

        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            allowhoot = true;
            timer2.Stop();
        }

        private void MuteSound()
        {
            if (!mutedsound)
            {
                gamemusic1.settings.volume = 0;
                gamemusic2.settings.volume = 0;
                mainscreenmusic.settings.volume = 0;
                mutedsound = true;
            }
            else
            {
                gamemusic1.settings.volume = 35;
                gamemusic2.settings.volume = 35;
                mainscreenmusic.settings.volume = 35;
                mutedsound = false;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point m1 = this.PointToClient(Cursor.Position);
            if (CalculateDistance(m1.X - (menux + this.Width / 35 + muteb.Width / 2), m1.Y - (50 + muteb.Height / 2)) < 52) // if the distance returned from the center of the circle is less than the radius, the click was on the button
            {
                MuteSound();
                this.Invalidate();
            }
            else if (CalculateDistance(m1.X - (menux + this.Width / 35 + playb.Width / 2), m1.Y - (175 + playb.Height / 2)) < 38) // if the distance returned from the center of the circle is less than the radius, the click was on the button
            {
                if (pause)
                {
                    pause = false;
                }
                else
                {
                    pause = true;
                }
                this.Invalidate();
            }
            else if (CalculateDistance(m1.X - (menux + this.Width / 35 + readmeb.Width / 2), m1.Y - (300 + readmeb.Height / 2)) < 38) // if the distance returned from the center of the circle is less than the radius, the click was on the button
            {
                MessageBox.Show("Chunky Monkey is a 2d game revolving around a monkey trying to avoid obstacles and gather bananas to increase the time. There are two crocodiles and four birds you have to look out for, all the while a timer is counting down, forcing you to eat bananas which give back time. You are given 15 seconds to start and 5 seconds are added every time you eat a banana. Score is kept by bananas eaten. If you hit the walls, a bird, or a crocodile, you lose.\n\n" +
                                "To Play:\n" +
                                "When you load into the game, the monkey will be suspended waiting for you to start. Click the mouse button and the game will begin. Holding down the mouse button will cause the monkey to swing on the vine, while releasing will throw the monkey off.\n" +
                                "\nYou can pause at any point by hovering over the far left side of the game, or clicking the 'p' hotkey. If you move your mouse to the left side of the game to pause, simply move the mouse back to the right and the game will resume. If you hit the 'p' hokey, you'll have to hit it again to resume.\n" +
                                "\nYou'll notice a small pause menu pops up when you pause the game. On the menu exists a mute/unmute, pause/play, readme and exit. There is a song that is set to mute when you load in. Un-mute at any time to listen!\n"+
                                "When you die, to play again, simply click the mouse button to be redirected to the beginning of the game. Click again, and the game begins.\n" + 
                                "\nTo exit, hit the exit button in the top right or the X button in the slide out menu.\n" +
                                "\nHope you enjoy!\n" +
                                "VS 2013/.NET 4.5\n" +
                                "\nAuthors: Nathan Tonani and Leland Burlingame\nMusic used: Diddy Kong Racing Theme song by David Wise", "Readme");
            }
            else if (CalculateDistance(m1.X - (menux + this.Width / 35 + exitb.Width / 2), m1.Y - (this.Height/5*4 + exitb.Height / 2)) < 38) // if the distance returned from the center of the circle is less than the radius, the click was on the button
            {
                Application.Exit();
            }
            else if (!pause && !menupause)
            {
                monkey1.Swing();

                vinex1 = monkey1.X + monkey1.Width / 80 * 79;
                vinex2 = monkey1.X + monkey1.Width / 80 * 79;
                vinex3 = monkey1.X + monkey1.Width / 80 * 79;
                vinex4 = monkey1.X + monkey1.Width / 80 * 79;
                vinex5 = monkey1.X + monkey1.Width / 80 * 79;

                vinedx1 = -10;
                vinedx2 = -6.1;
                vinedx3 = 0;
                vinedx4 = -2.5;
                vinedx5 = -6;

                viney1 = 0;
                viney2 = monkey1.Y / 5 * 2;

                if (viney3 - viney2 < 200)
                {
                    viney2 = viney2-200;
                }

                if (viney2 < 0)
                {
                    viney1 = -600;
                }

                viney4 = monkey1.Y+200;
                viney5 = monkey1.Y+400;
                monkey_ix = 1;

                if (!start)
                {
                    start = true;
                }
                if (gameOver && allowclick)
                {
                    Reset();
                    gameOver = false;
                    start = false;
                    allowclick = false;
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            monkey1.Fall();
            vinedx1 = -10;
            vinedx2 = -11;
            vinedx3 = -14;
            vinedx4 = -11;
            vinedx4 = -10.5;
            vinedy2 = 0;
            monkey_ix = 0;
        }
        
        private void timer3_Tick(object sender, EventArgs e)
        {
            
            if (!pause && !menupause && !gameOver)
            {
                if (croc_ix == 0)
                {
                    croc_ix = 1;
                    timer3.Interval = 600;
                }
                else
                {
                    croc_ix = 0;
                    timer3.Interval = 400;
                }
            }
             
        }
        private bool DetectCollision(MovingEntity o)
        {
            if (((monkey1.X < o.X + o.Width & monkey1.X > o.X) || (monkey1.X + monkey.Width > o.X & monkey1.X + monkey.Width < o.X + o.Width)) &
                ((monkey1.Y < o.Y + o.Height & monkey1.Y > o.Y) || (monkey1.Y + monkey.Height > o.Y & monkey1.Y + monkey.Height < o.Y + o.Height)))
            {
                MonkeyHoot();
                return true;
            }
            return false;
        }

        void Form_KeyDownp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                if (pause)
                {
                    pause = false;
                }
                else
                {
                    pause = true;
                }

                e.SuppressKeyPress = true;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            
            if (!pause && !menupause && !gameOver && !mainpause && start)
            {
                if (rem < 0.1)
                {
                    gameOver = true;
                    MonkeyHoot();
                    timer5.Start();
                }
                else
                {
                    rem -= .1;
                }

            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            qwrite.Close();
            highscoreWrite.WriteLine(hs.ToString());
            highscoreWrite.Close();
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            allowclick = true;
            timer5.Stop();
        }
     }
}




