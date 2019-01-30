using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexDates
{
    class MyTextBox : TextBox
    {
        public int row, column;
        public bool submited = false;
        public bool disableTextChanged = false;
        public bool focuse_gained = false;
        public string submitedText;
    }
}
