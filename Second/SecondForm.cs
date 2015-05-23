using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Second
{
    public partial class SecondForm : Form
    {
        private readonly Dictionary<string, object> oldValues = new Dictionary<string, object>(); 

        public SecondForm()
        {
            InitializeComponent();
            Redraw();
        }

        #region Drawing

        void Redraw()
        {
            //TODO: Implement
        }

        #endregion


        #region Event Handlers

        private void FirstForm_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(Size.Width - 203, Size.Height - 63);
            panel1.Location = new Point(pictureBox1.Location.X + pictureBox1.Size.Width + 6, 12);
            Redraw();
        }

        private void NumberBoxOnGetFocus(object sender, EventArgs e)
        {
            ExecuteFor<TextBox>(sender, textBox =>
            {
                oldValues[textBox.Name] = textBox.Text;
            });
        }

        private void NumberBoxOnLostFocus(object sender, EventArgs e)
        {
            ExecuteFor<TextBox>(sender, textBox =>
            {
                int value;
                if (!int.TryParse(textBox.Text, out value))
                    textBox.Text = (string) oldValues[textBox.Name];
                Redraw();
            });
        }

        private void ExecuteFor<TSender>(object sender, Action<TSender> action)
            where TSender : class
        {
            var senderTyped = sender as TSender;
            if (senderTyped == null)
                return;
            action(senderTyped);
        }

        #endregion

        #region Drawing Ulitity

        #endregion
    }
}
