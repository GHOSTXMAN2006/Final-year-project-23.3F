using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mufaddal_Traders
{
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 20; // Radius for rounded corners
        public Color BorderColor { get; set; } = Color.DarkGray; // Color of the border
        public int BorderThickness { get; set; } = 3; // Thickness of the border

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Rectangle for the border
            Rectangle rect = new Rectangle(
                BorderThickness / 2,
                BorderThickness / 2,
                this.Width - BorderThickness,
                this.Height - BorderThickness
            );

            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90); // Top-left
            path.AddArc(rect.Right - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90); // Top-right
            path.AddArc(rect.Right - BorderRadius, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 0, 90); // Bottom-right
            path.AddArc(rect.X, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 90, 90); // Bottom-left
            path.CloseFigure();

            // Fill the background
            using (Brush backgroundBrush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(backgroundBrush, path);
            }

            // Draw the border
            using (Pen borderPen = new Pen(BorderColor, BorderThickness))
            {
                e.Graphics.DrawPath(borderPen, path);
            }

            // Set region for rounded corners
            this.Region = new Region(path);
        }
    }
}
