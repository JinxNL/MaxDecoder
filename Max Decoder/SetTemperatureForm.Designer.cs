namespace Max_Decoder
{
    partial class SetTemperatureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblRoom = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.Label();
            this.txtThermostat = new System.Windows.Forms.Label();
            this.lblCurrentTemp = new System.Windows.Forms.Label();
            this.txtCurrentTemp = new System.Windows.Forms.Label();
            this.lblNewTemp = new System.Windows.Forms.Label();
            this.nNewTemperature = new System.Windows.Forms.NumericUpDown();
            this.rbPermanent = new System.Windows.Forms.RadioButton();
            this.rbTemporarely = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nNewTemperature)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(255, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(174, 148);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblRoom
            // 
            this.lblRoom.AutoSize = true;
            this.lblRoom.Location = new System.Drawing.Point(13, 13);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(38, 13);
            this.lblRoom.TabIndex = 2;
            this.lblRoom.Text = "Room:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Thermostat:";
            // 
            // txtRoom
            // 
            this.txtRoom.AutoSize = true;
            this.txtRoom.Location = new System.Drawing.Point(128, 13);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(66, 13);
            this.txtRoom.TabIndex = 4;
            this.txtRoom.Text = "Room Name";
            // 
            // txtThermostat
            // 
            this.txtThermostat.AutoSize = true;
            this.txtThermostat.Location = new System.Drawing.Point(128, 38);
            this.txtThermostat.Name = "txtThermostat";
            this.txtThermostat.Size = new System.Drawing.Size(91, 13);
            this.txtThermostat.TabIndex = 5;
            this.txtThermostat.Text = "Thermostat Name";
            // 
            // lblCurrentTemp
            // 
            this.lblCurrentTemp.AutoSize = true;
            this.lblCurrentTemp.Location = new System.Drawing.Point(13, 63);
            this.lblCurrentTemp.Name = "lblCurrentTemp";
            this.lblCurrentTemp.Size = new System.Drawing.Size(107, 13);
            this.lblCurrentTemp.TabIndex = 6;
            this.lblCurrentTemp.Text = "Current Temperature:";
            // 
            // txtCurrentTemp
            // 
            this.txtCurrentTemp.AutoSize = true;
            this.txtCurrentTemp.Location = new System.Drawing.Point(128, 63);
            this.txtCurrentTemp.Name = "txtCurrentTemp";
            this.txtCurrentTemp.Size = new System.Drawing.Size(26, 13);
            this.txtCurrentTemp.TabIndex = 7;
            this.txtCurrentTemp.Text = "0 ⁰C";
            // 
            // lblNewTemp
            // 
            this.lblNewTemp.AutoSize = true;
            this.lblNewTemp.Location = new System.Drawing.Point(13, 88);
            this.lblNewTemp.Name = "lblNewTemp";
            this.lblNewTemp.Size = new System.Drawing.Size(95, 13);
            this.lblNewTemp.TabIndex = 8;
            this.lblNewTemp.Text = "New Temperature:";
            // 
            // nNewTemperature
            // 
            this.nNewTemperature.DecimalPlaces = 1;
            this.nNewTemperature.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nNewTemperature.Location = new System.Drawing.Point(131, 88);
            this.nNewTemperature.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nNewTemperature.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nNewTemperature.Name = "nNewTemperature";
            this.nNewTemperature.Size = new System.Drawing.Size(76, 20);
            this.nNewTemperature.TabIndex = 9;
            this.nNewTemperature.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // rbPermanent
            // 
            this.rbPermanent.AutoSize = true;
            this.rbPermanent.Checked = true;
            this.rbPermanent.Location = new System.Drawing.Point(131, 115);
            this.rbPermanent.Name = "rbPermanent";
            this.rbPermanent.Size = new System.Drawing.Size(76, 17);
            this.rbPermanent.TabIndex = 10;
            this.rbPermanent.TabStop = true;
            this.rbPermanent.Text = "Permanent";
            this.rbPermanent.UseVisualStyleBackColor = true;
            // 
            // rbTemporarely
            // 
            this.rbTemporarely.AutoSize = true;
            this.rbTemporarely.Location = new System.Drawing.Point(223, 115);
            this.rbTemporarely.Name = "rbTemporarely";
            this.rbTemporarely.Size = new System.Drawing.Size(83, 17);
            this.rbTemporarely.TabIndex = 11;
            this.rbTemporarely.Text = "Temporarely";
            this.rbTemporarely.UseVisualStyleBackColor = true;
            // 
            // SetTemperatureForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(342, 183);
            this.Controls.Add(this.rbTemporarely);
            this.Controls.Add(this.rbPermanent);
            this.Controls.Add(this.nNewTemperature);
            this.Controls.Add(this.lblNewTemp);
            this.Controls.Add(this.txtCurrentTemp);
            this.Controls.Add(this.lblCurrentTemp);
            this.Controls.Add(this.txtThermostat);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblRoom);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetTemperatureForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Temperature";
            ((System.ComponentModel.ISupportInitialize)(this.nNewTemperature)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label txtRoom;
        private System.Windows.Forms.Label txtThermostat;
        private System.Windows.Forms.Label lblCurrentTemp;
        private System.Windows.Forms.Label txtCurrentTemp;
        private System.Windows.Forms.Label lblNewTemp;
        private System.Windows.Forms.NumericUpDown nNewTemperature;
        private System.Windows.Forms.RadioButton rbPermanent;
        private System.Windows.Forms.RadioButton rbTemporarely;
    }
}