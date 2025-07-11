using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.Project.sharp
{
    public partial class Form3 : Form
    {
        private int boardSize = 8; // Размер поля по умолчанию

        public Form3()
        {
            InitializeComponent();
            checkBox8.CheckedChanged += checkBox8_CheckedChanged;
            checkBox9.CheckedChanged += checkBox9_CheckedChanged;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged_1;
            checkBox5.CheckedChanged += checkBox5_CheckedChanged;
            checkBox7.CheckedChanged += checkBox7_CheckedChanged;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBoardSize();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBoardSize();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            // Обработчик для Землетрясения
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            // Обработчик для Извержения вулкана
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            // Обработчик для Торнадо
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            // Обработчик для Заморозки
        }

        private void UpdateBoardSize()
        {
            if (checkBox9.Checked) // если выбран шпион (или обе галочки)
            {
                boardSize = 10;
            }
            else
            {
                boardSize = 8;
            }
            // Здесь в будущем можно вызвать перерисовку поля или передать размер дальше
        }

        public bool IsGuardianSelected => checkBox8.Checked;
        public bool IsSpySelected => checkBox9.Checked;
        public bool IsEarthquakeSelected => checkBox3.Checked;
        public bool IsVolcanoSelected => checkBox4.Checked;
        public bool IsRedZoneSelected => checkBox1.Checked;
        public bool IsTornadoSelected => checkBox5.Checked;
        public bool IsFreezingSelected => checkBox7.Checked;

        private void button1_Click(object sender, EventArgs e)
        {
            CustomGame form4 = new CustomGame(boardSize, IsGuardianSelected, IsSpySelected, IsEarthquakeSelected, IsVolcanoSelected, IsRedZoneSelected, IsTornadoSelected, IsFreezingSelected);
            form4.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
