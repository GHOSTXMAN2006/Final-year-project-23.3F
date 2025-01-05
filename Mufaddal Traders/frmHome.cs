using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public partial class frmHome : Form
    {
        // DLL imports to allow dragging
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Slideshow variables
        private List<string> imagePaths = new List<string>();  // List to store image file paths
        private int currentImageIndex = 0;  // Track the current image index
        private Timer slideshowTimer = new Timer();  // Timer for slideshow

        public frmHome()
        {
            InitializeComponent();
            InitializeSlideshow();  // Initialize slideshow when form loads
        }

        private void InitializeSlideshow()
        {
            // Add image file paths (Update these paths to actual image locations)
            imagePaths.Add(@"D:\=------NIBM WORK-----=\DSE - Niviru - Acer\Final Year Project\UI\WhatsApp-Image-2022-06-03-at-12.59.38-PM 1.png");
            imagePaths.Add(@"D:\=------NIBM WORK-----=\DSE - Niviru - Acer\Final Year Project\UI\desiccated-coconut-blog-1-pjo02v6tun9z38f2g6d3n7dq8b0tjvk1bktxb4nz0w.jpg");
            imagePaths.Add(@"D:\=------NIBM WORK-----=\DSE - Niviru - Acer\Final Year Project\UI\rens-d-Xvjs1G812Yo-unsplash-1-scaled-pprhrsq37muiaz1leh9ccz9txhar9uqiqgtuwxluz4.jpg");
            imagePaths.Add(@"D:\=------NIBM WORK-----=\DSE - Niviru - Acer\Final Year Project\UI\petr-sidorov-T9yWxecVV2Q-unsplash-scaled-pjo0pbk1460kfvsvbvs97je53ochfkp72pvi13deds.jpg");
            imagePaths.Add(@"D:\=------NIBM WORK-----=\DSE - Niviru - Acer\Final Year Project\UI\virgin-coconut-oil-500x500-1-300x300.jpg");
            imagePaths.Add(@"D:\=------NIBM WORK-----=\DSE - Niviru - Acer\Final Year Project\UI\alice-pasqual-59Kh3TAajg0-unsplash-scaled-pjwl9it7kr7qufdtpyki0li9fqiaozdrhi3j3fduqg.jpg");

            // Configure PictureBox
            picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
            if (imagePaths.Count > 0)
            {
                picDisplay.ImageLocation = imagePaths[0];  // Display the first image
            }

            // Configure Timer
            slideshowTimer.Interval = 3000;  // 3 seconds interval
            slideshowTimer.Tick += new EventHandler(OnSlideshowTimerTick);
            slideshowTimer.Start();  // Start automatic slideshow
        }

        private void OnSlideshowTimerTick(object sender, EventArgs e)
        {
            // Change to the next image in the list
            currentImageIndex++;
            if (currentImageIndex >= imagePaths.Count)
            {
                currentImageIndex = 0;  // Loop back to the first image
            }
            picDisplay.ImageLocation = imagePaths[currentImageIndex];
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // Navigate to the previous image manually
            currentImageIndex--;
            if (currentImageIndex < 0)
            {
                currentImageIndex = imagePaths.Count - 1;  // Loop to the last image
            }
            picDisplay.ImageLocation = imagePaths[currentImageIndex];
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Navigate to the next image manually
            currentImageIndex++;
            if (currentImageIndex >= imagePaths.Count)
            {
                currentImageIndex = 0;  // Loop to the first image
            }
            picDisplay.ImageLocation = imagePaths[currentImageIndex];
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            // Any additional form load logic
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Create an instance of frmLogin
            frmLogin loginForm = new frmLogin();

            // Show the frmLogin
            loginForm.Show();

            // Close the current form (frmHome)
            this.Hide();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
