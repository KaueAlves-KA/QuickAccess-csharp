using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuickAccess
{
    public static class RoundForm
    {
        public static void RoundFormMethod(Form form, int radius)
        {
            //remove a borda do Form
            form.FormBorderStyle = FormBorderStyle.None;

            GraphicsPath formPath = new GraphicsPath();
            Rectangle formRectangle = new Rectangle(0, 0, form.Width, form.Height);

            //desenha as bordas arredondas
            formPath.AddArc(formRectangle.X, formRectangle.Y, radius, radius, 180, 90);
            formPath.AddArc(formRectangle.Right - radius, formRectangle.Y, radius, radius, 270, 90);
            formPath.AddArc(formRectangle.Right - radius, formRectangle.Bottom - radius, radius, radius, 0, 90);
            formPath.AddArc(formRectangle.X, formRectangle.Bottom - radius, radius, radius, 90, 90);
            formPath.CloseFigure();

            form.Region = new Region(formPath);
        }
    }
}
