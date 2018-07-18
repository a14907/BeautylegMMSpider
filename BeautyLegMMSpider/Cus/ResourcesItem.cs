using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeautyLegMMSpider.Cus
{
    public partial class ResourcesItem : UserControl
    {
        public ResourcesItem(string chboxText ,Tuple<string,string> tagValue,int index)
        {
            InitializeComponent();
            chboxUrl.Text = chboxText;
            this.Tag = tagValue;
            
            this.Location = new Point { X = 10, Y = 20 * (index + 1) };
        }

        public bool IsCheck()
        {
            return chboxUrl.Checked;
        }
    }
}
