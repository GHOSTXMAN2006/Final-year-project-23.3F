using System.Windows.Forms;

public static class ControlExtensions
{
    public static void EnableDoubleBuffering(this Control control)
    {
        if (SystemInformation.TerminalServerSession) return;

        var property = typeof(Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        property?.SetValue(control, true, null);
    }
}
