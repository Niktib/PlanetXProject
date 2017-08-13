using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanetXTest
{
    public partial class Form2 : Form
    {
        IList<IList<Object>> TaskCollection;
        CheckBox[,] _checkBoxes;


        public Form2(IList<IList<Object>> values)
        {
            TaskCollection = values;
            InitializeComponent();
            _checkBoxes = new CheckBox[,] { { }, };
        }
        
        
    }
}
