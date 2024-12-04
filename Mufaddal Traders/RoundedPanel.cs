using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

public class RoundedPanel : Panel
{
    public int BorderRadius { get; set; } = 10; // Default radius of 10
    public Color BorderColor { get; set; } = Color.Black;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Create a rounded rectangle
        using (GraphicsPath path = new GraphicsPath())
        {
            path.AddArc(0, 0, BorderRadius, BorderRadius, 180, 90);
            path.AddArc(Width - BorderRadius, 0, BorderRadius, BorderRadius, 270, 90);
            path.AddArc(Width - BorderRadius, Height - BorderRadius, BorderRadius, BorderRadius, 0, 90);
            path.AddArc(0, Height - BorderRadius, BorderRadius, BorderRadius, 90, 90);
            path.CloseFigure();

            // Clip the panel to the rounded rectangle
            this.Region = new Region(path);

            // Draw the border
            using (Pen pen = new Pen(BorderColor, 2)) // Adjust border width if needed
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
