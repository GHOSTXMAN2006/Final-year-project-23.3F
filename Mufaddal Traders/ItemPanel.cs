using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public class ItemPanel : Panel
    {
        public int BorderRadius { get; set; } = 20; // Corner radius for the panel

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Define the rectangle for the panel
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90); // Top-left corner
            path.AddArc(rect.Right - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90); // Top-right corner
            path.AddArc(rect.Right - BorderRadius, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 0, 90); // Bottom-right corner
            path.AddArc(rect.X, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 90, 90); // Bottom-left corner
            path.CloseFigure();

            // Fill the background
            using (Brush backgroundBrush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(backgroundBrush, path);
            }

            // Apply the region for rounded corners
            this.Region = new Region(path);
        }
    }
}
