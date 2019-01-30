using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexDates
{
    public partial class Form1 : Form
    {

        private MyTextBox[,] m_textBoxes;
        private Label[] m_Labels;
        private MyCheckBox[] m_Checkboxes;
        private MyComboBox[] m_ComboBoxes;
        private MyTextBox m_prevTxtBox = null;

        public Form1()
        {
            InitializeComponent();

            CreateDateGraphics();
        }

        private void CreateDateGraphics()
        {

            //Clear Layout.
            dates_table_layout.Controls.Clear();
            dates_table_layout.ColumnStyles.Clear();
            dates_table_layout.RowStyles.Clear();

            //-----------------Add the styles-----------------//
            dates_table_layout.RowCount = 30;
            for (int x = 0; x < dates_table_layout.RowCount; x++)
                dates_table_layout.RowStyles.Add(new RowStyle() { Height = 100, SizeType = SizeType.Percent });

            dates_table_layout.ColumnCount = 6;
            for (int x = 0; x < dates_table_layout.ColumnCount; x++)
                dates_table_layout.ColumnStyles.Add(new ColumnStyle() { Width = 100, SizeType = SizeType.Percent });
            //-----------------Add the styles-----------------//

            //Add the layout.
            dates_group_box.Controls.Add(dates_table_layout);


            //------------Add the Column Names------------//
            Label label = new Label();
            label.Text = "Ώρα";
            dates_table_layout.Controls.Add(label, 0, 0);

            label = new Label();
            label.Text  = "Όνομα";
            dates_table_layout.Controls.Add(label, 1, 0);

            label = new Label();
            label.Text = "Επώνυμο";
            dates_table_layout.Controls.Add(label, 2, 0);

            label = new Label();
            label.Text = "Τηλέφωνο";
            dates_table_layout.Controls.Add(label, 3, 0);

            label = new Label();
            label.Text = "Υπηρεσία";
            dates_table_layout.Controls.Add(label, 4, 0);
            //------------Add the Column Names------------//


            string[] times = {"", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
                              "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30",
                              "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:30", "21:30", "22:00"};

            m_textBoxes  = new MyTextBox[29,3];
            m_Labels     = new Label[29];
            m_Checkboxes = new MyCheckBox[29];
            m_ComboBoxes = new MyComboBox[29];


            //Add Rows.
            for (int i = 1; i < 30; i++)
            {

                //Create the row time.
                label      = new Label();
                label.Text = times[i];
                label.Font = new Font("Arial", 12, FontStyle.Bold);
                dates_table_layout.Controls.Add(label, 0, i);

                //Store all the Label objects in an array.
                m_Labels[i - 1] = label;

                //Create a CheckBox and add it to the layout.
                MyCheckBox box = new MyCheckBox();
                box.Text       = "Πληρωμή";
                dates_table_layout.Controls.Add(box, 5, i);

                //Store it in the array.
                m_Checkboxes[i - 1] = box;
                box.row = i - 1;


                //Create a ComboBox and add it to the layout.
                MyComboBox comb    = new MyComboBox();
                comb.DropDownStyle = ComboBoxStyle.DropDownList;
                dates_table_layout.Controls.Add(comb, 4, i);

                //Store it into the array.
                m_ComboBoxes[i - 1] = comb;
                comb.row = i - 1;



                //Go through each column.
                for (int j = 1; j < 4; j++)
                {

                    //Create the TextBox and add it to the layout.
                    MyTextBox textBox = new MyTextBox();
                    textBox.Size      = new Size(150, textBox.Size.Height);
                    textBox.Font      = new Font("Arial", 8, FontStyle.Bold);
                    dates_table_layout.Controls.Add(textBox, j, i);

                    //Add Event Handlers.
                    textBox.Enter       += new EventHandler(this.TextBoxFocuseGained);
                    textBox.Leave       += new EventHandler(this.TextBoxFocuseLost);
                    textBox.KeyPress    += new KeyPressEventHandler(this.TextBoxKeyEnterEvent);
                    textBox.TextChanged += new EventHandler(this.TextBoxTextChangedEvent);

                    //Store all the TextBox object in a 2D array.
                    m_textBoxes[i - 1, j - 1] = textBox;

                    //Set The Textbox Properties.
                    textBox.row    = i - 1;
                    textBox.column = j - 1;
                }
            }
        }





        //TextBox Focus Gain.
        private void TextBoxFocuseGained(object sender, EventArgs e)
        {
            MyTextBox txt = (MyTextBox)sender;

            txt.focuse_gained = true;

            m_Labels[txt.row].ForeColor = Color.Green;

            //Restore Previous Text.
            if (m_prevTxtBox != null && m_prevTxtBox.row != txt.row)
                SubmitedTextRestored(m_prevTxtBox);


        }





        //TextBox Focus Lost.
        private void TextBoxFocuseLost(object sender, EventArgs e)
        {
            MyTextBox txt = (MyTextBox)sender;

            m_Labels[txt.row].ForeColor = Color.Black;

            txt.focuse_gained = false;

            m_prevTxtBox = txt;

        }





        //TextBox Key Pressed.
        private void TextBoxKeyEnterEvent(object sender, KeyPressEventArgs e)
        {

            //Return (ENTER) key Pressed.
            if (e.KeyChar == (char)Keys.Return)
            {
                SubmitedTextChanged((MyTextBox)sender);
            }
        }





        //TextBox Text Changed.
        private void TextBoxTextChangedEvent(object sender, EventArgs e)
        {
            MyTextBox txt = (MyTextBox)sender;

            if (!txt.disableTextChanged)
            {
                m_textBoxes[txt.row, 0].ForeColor = Color.Black;
                m_textBoxes[txt.row, 1].ForeColor = Color.Black;
                m_textBoxes[txt.row, 2].ForeColor = Color.Black;
            }
        }




        //TextBox Submited Text Chagned.
        private void SubmitedTextChanged(MyTextBox txt)
        {
            m_textBoxes[txt.row, 0].ForeColor    = Color.Green;
            m_textBoxes[txt.row, 0].submited     = true;
            m_textBoxes[txt.row, 0].submitedText = m_textBoxes[txt.row, 0].Text;

            m_textBoxes[txt.row, 1].ForeColor    = Color.Green;
            m_textBoxes[txt.row, 1].submited     = true;
            m_textBoxes[txt.row, 1].submitedText = m_textBoxes[txt.row, 1].Text;

            m_textBoxes[txt.row, 2].ForeColor    = Color.Green;
            m_textBoxes[txt.row, 2].submited     = true;
            m_textBoxes[txt.row, 2].submitedText = m_textBoxes[txt.row, 2].Text;
        }





        //TextBox Restore Text.
        private void SubmitedTextRestored(MyTextBox txt)
        {

            //Disable Text Changed Event.
            m_textBoxes[txt.row, 0].disableTextChanged = true;
            m_textBoxes[txt.row, 1].disableTextChanged = true;
            m_textBoxes[txt.row, 2].disableTextChanged = true;

            m_textBoxes[txt.row, 0].ForeColor = Color.Green;
            m_textBoxes[txt.row, 0].Text      = m_textBoxes[txt.row, 0].submitedText;

            m_textBoxes[txt.row, 1].ForeColor = Color.Green;
            m_textBoxes[txt.row, 1].Text      = m_textBoxes[txt.row, 1].submitedText;

            m_textBoxes[txt.row, 2].ForeColor = Color.Green;
            m_textBoxes[txt.row, 2].Text      = m_textBoxes[txt.row, 2].submitedText;

            //Enable Text Changed Event.
            m_textBoxes[txt.row, 0].disableTextChanged = false;
            m_textBoxes[txt.row, 1].disableTextChanged = false;
            m_textBoxes[txt.row, 2].disableTextChanged = false;
        }
    }
}
