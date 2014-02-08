using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Max_Decoder
{
    public partial class SetTemperatureForm : Form
    {
        public string RoomName
        {
            set
            { txtRoom.Text = value; }
            get
            { return txtRoom.Text; }
        }
        public string DeviceName
        {
            set
            { txtThermostat.Text = value; }
            get
            { return txtThermostat.Text; }
        }
        public int CurrentTemperature
        {
            set
            {
                txtCurrentTemp.Text = (value / 2.0).ToString("#.#°C");
                nNewTemperature.Value = Convert.ToDecimal(value / 2.0);
            }
            get
            { return Convert.ToInt16(Convert.ToDecimal(txtCurrentTemp.Text) *2); }
        }
        public int NewTemperature
        {
            set
            { nNewTemperature.Value = Convert.ToDecimal(value / 2.0); }
            get
            { return Convert.ToInt16(nNewTemperature.Value * 2); }
        }
        public bool Permanent
        {
            get
            {
                return rbPermanent.Checked;
            }
        }
        public SetTemperatureForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
