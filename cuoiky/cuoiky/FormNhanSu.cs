using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuoiky
{
    public partial class FormNhanSu : Form
    {
        private int maNV;
        public FormNhanSu(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
        }
        public FormNhanSu()
        {
            InitializeComponent();
        }
    }
}
