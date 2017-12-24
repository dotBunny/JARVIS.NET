using System;
using Eto.Forms;
using Eto.Drawing;

namespace JARVIS.Client
{
    public class MainForm :  Form
    {
        public MainForm()
        {
            Title = "JARVIS.Shard";
            ClientSize = new Size(200, 200);
            Content = new Label { Text = "Hello World!" };

            BringToFront();
        }
    }
}
