using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace Max_Decoder
{
    public partial class MainForm : Form
    {
        EventDrivenTCPClient client;
        static MAX max = new MAX();
        static Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        static Dictionary<string, Device> Devices = new Dictionary<string, Device>();
        static string[] TypeNames = { "Thermostat", "Thermostat+", "Controller", "Window Sensor", "Comfort Switch" };

        class MAX
        {
            public string SerialNumber;
            public string RFAddress;
            public string Firmware;
            public override string ToString()
            {
                string retval = "Serial Number: " + SerialNumber + Environment.NewLine;
                retval += "RFAddress: " + RFAddress + Environment.NewLine;
                retval += "Firmware: " + Firmware + Environment.NewLine;
                return retval;
            }
        }

        class Room
        {
            public int ID;
            public string Name;
            public string RFAddress;
            public bool HasWindows
            {
                get
                {
                    foreach (Device d in Devices.Values)
                    {
                        if (d.RoomID == ID && d.Type == 4)
                            return true;
                    }
                    return false;
                }
 
            }
            public bool IsWindowOpen
            {
                get
                {
                    foreach (Device d in Devices.Values)
                    {
                        if (d.RoomID == ID && d.Type == 4)
                            if (d.IsWindowOpen)
                                return true;
                    }
                    return false;
                }
            }
            public override string ToString()
            {
                string retval = "RoomID: " + ID.ToString() + Environment.NewLine;
                retval += "Name: " + Name + Environment.NewLine;
                retval += "RFAddress: " + RFAddress + Environment.NewLine;
                if (HasWindows)
                    retval += "Window(s): " + (IsWindowOpen ? "Open" : "Closed") + Environment.NewLine;
                if (Devices.ContainsKey("D:" + RFAddress))
                {
                    Device d1 = GetDeviceOfRoom(this);
                    if (d1 != null && (d1.Type == 1 || d1.Type == 2))
                    {
                        retval += "Comfort Temperature: " + (d1.ComfortTemperature / 2.0).ToString("#.#°C") + Environment.NewLine;
                        retval += "Eco Temperature: " + (d1.EcoTemperature / 2.0).ToString("#.#°C") + Environment.NewLine;
                        if (HasWindows)
                            retval += "Window Open Temperature: " + (d1.WindowOpenTemperature / 2.0).ToString("#.#°C") + Environment.NewLine;
                        retval += "Set Temperature: " + (d1.TemperatureSetpoint / 2.0).ToString("#.#°C") + Environment.NewLine;
                    }
                }
                return retval;
            }
        }

        class Device
        {
            public int Type;
            public int RoomID;
            public string RFAddress;
            public string Name;
            public string Serial;
            public int ComfortTemperature;
            public int EcoTemperature;
            public int WindowOpenTemperature;
            public byte Status1;
            public byte Status2;
            public int ValvePosition;
            public int TemperatureSetpoint;
            public bool StatusValid
            {
                get { return IsBitSet(Status1, 4); }
            }
            public bool IsWindowOpen
            {
                get { return IsBitSet(Status2, 1); }
            }
            public bool IsBatteryOK
            {
                get { return IsBitSet(Status2, 7); }
            }
            public override string ToString()
            {
                string retval = "RoomID: " + RoomID.ToString() + Environment.NewLine;
                retval += "Name: " + Name + Environment.NewLine;
                retval += "Type: " + TypeNames[Type - 1] + Environment.NewLine;
                retval += "RFAddress: " + RFAddress + Environment.NewLine;
                retval += "Serial: " + Serial + Environment.NewLine;
                retval += "Battery: " + (IsBatteryOK ? "Low" : "OK") + Environment.NewLine;
                retval += "Status: " + (StatusValid ? "OK" : "Unknown") + Environment.NewLine;
                switch (Type)
                {
                    case 1:
                    case 2:
                        retval += "Comfort Temperature: " + (ComfortTemperature / 2.0).ToString("#.#°C") + Environment.NewLine;
                        retval += "Eco Temperature: " + (EcoTemperature / 2.0).ToString("#.#°C") + Environment.NewLine;
                        if(GetRoomOfDevice(this).HasWindows)
                            retval += "Window Open Temperature: " + (WindowOpenTemperature / 2.0).ToString("#.#°C") + Environment.NewLine;
                        retval += "Set Temperature: " + (TemperatureSetpoint / 2.0).ToString("#.#°C") + Environment.NewLine;
                        retval += "Valve position: " + (ValvePosition * 100 / 255).ToString() + "%" + Environment.NewLine;
                        break;
                    case 4:
                        retval += "Window: " + (IsWindowOpen ? "Open" : "Closed") + Environment.NewLine;
                        break;

                }
                return retval;
            }
        }

        public MainForm()
        {
            InitializeComponent();    
        }

        static private string EncodeTo64(byte[] toEncode)
        {
            string returnValue = System.Convert.ToBase64String(toEncode);
            return returnValue;
        }
        static private byte[] DecodeFrom64(string encodedData)
        {
            byte[] returnValue = null;
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                // returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
                returnValue = encodedDataAsBytes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return returnValue;
        }
        static private bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
        static private Device GetDeviceOfRoom(Room room)
        {
            foreach (Device device in Devices.Values)
            {
                if (room.RFAddress == device.RFAddress)
                    return device;
            }
            return null;
        }
        static private Room GetRoomOfDevice(Device device)
        {
            foreach (Room room in Rooms.Values)
            {
                if (room.ID == device.RoomID)
                    return room;
            }
            return null;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(txtIPAddress.Text);
                int port = int.Parse(txtPort.Text);
                client = new EventDrivenTCPClient(ip, port);
                client.DataReceived += client_DataReceived;
                client.ConnectionStatusChanged += client_ConnectionStatusChanged;
                client.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void client_ConnectionStatusChanged(EventDrivenTCPClient sender, EventDrivenTCPClient.ConnectionStatus status)
        {
            this.Invoke((MethodInvoker)delegate
            {
                switch (status)
                {
                    case EventDrivenTCPClient.ConnectionStatus.Connected:
                        btnClose.Enabled = true;
                        btnUpdate.Enabled = true;
                        btnConnect.Enabled = false;
                        timer1.Enabled = true;
                        break;
                    case EventDrivenTCPClient.ConnectionStatus.DisconnectedByHost:
                        client.Disconnect();
                        break;
                    case EventDrivenTCPClient.ConnectionStatus.DisconnectedByUser:
                        timer1.Enabled = false;
                        treeView1.Nodes.Clear();
                        txtIncoming.Text = "";
                        btnConnect.Enabled = true;
                        btnUpdate.Enabled = false;
                        btnSet.Enabled = false;
                        btnClose.Enabled = false;
                        break;
                }
            });
        }

        private void client_DataReceived(EventDrivenTCPClient sender, object data)
        {
            string text = data as string;
            string[] lines = Regex.Split(text, "\r\n");
            foreach (string line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                    ProcessLine(line);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Disconnect();
                client = null;
            }
        }

        private void ProcessLine(string line)
        {
            switch (line.Substring(0, 1).ToUpper())
            {
                case "H":
                    ProcessUnit(line);
                    break;
                case "M":
                    ProcessMetadata(line);
                    break;
                case "C":
                    ProcessConfiguration(line);
                    break;
                case "L":
                    ProcessDevice(line);
                    break;
                case "S":
                    ProcessResponse(line);
                    break;
            }
        }
        private void ProcessResponse(string line)
        {
            line = line.Substring(2);
            string[] element = line.Split(',');
            if (element[1] == "1")
            {
                MessageBox.Show("Update Failed!");
            }
        }
        private void ProcessMetadata(string line)
        {
            Rooms = new Dictionary<string, Room>();
            Devices = new Dictionary<string, Device>();
            line = line.Substring(2);
            string[] element = line.Split(',');
            byte[] metadata = DecodeFrom64(element[2]);

            //Process Rooms
            int noOfRooms = metadata[2];
            int pos = 3;
            for (int r = 0; r < noOfRooms; r++)
            {
                Room ro = new Room();
                ro.ID = metadata[pos++];
                int noOfCharsofRoomName = metadata[pos++];
                string roomName = "";
                for (int c = 0; c < noOfCharsofRoomName; c++)
                {
                    roomName += (char)metadata[pos++];
                }
                ro.Name = roomName;
                string rfAddress = "";
                for (int c = 0; c < 3; c++)
                {
                    if (metadata[pos] <= 0x0f)
                        rfAddress += "0";
                    rfAddress += metadata[pos++].ToString("X");
                }
                ro.RFAddress = rfAddress;
                Rooms.Add("R:" + ro.RFAddress, ro);
            }

            // Process Devices
            int noOfDevices = metadata[pos++];
            for (int u = 0; u < noOfDevices; u++)
            {
                Device dev = new Device();
                dev.Type = metadata[pos++];
                string rfAddressDevice = "";
                for (int c = 0; c < 3; c++)
                {
                    if (metadata[pos] <= 0x0f)
                        rfAddressDevice += "0";
                    rfAddressDevice += metadata[pos++].ToString("X");
                }
                dev.RFAddress = rfAddressDevice;
                string serialDevice = "";
                for (int c = 0; c < 10; c++)
                {
                    serialDevice += (char)metadata[pos++];
                }
                dev.Serial = serialDevice;
                int noOfCharsofDeviceName = metadata[pos++];
                string DeviceName = "";
                for (int c = 0; c < noOfCharsofDeviceName; c++)
                {
                    DeviceName += (char)metadata[pos++];
                }
                dev.Name = DeviceName;
                dev.RoomID = metadata[pos++];
                Devices.Add("D:" + dev.RFAddress, dev);
            }
            ShowMetadata();
        }
        private void ProcessConfiguration(string line)
        {
            line = line.Substring(2);
            string[] element = line.Split(',');
            byte[] metadata = DecodeFrom64(element[1] + (element[1].Length % 4 == 0 ? "" : "==" ));
            if (metadata == null)
                return;
            int pos = 1;
            string rfAddressDevice = "";
            for (int c = 0; c < 3; c++)
            {
                if (metadata[pos] <= 0x0f)
                    rfAddressDevice += "0";
                rfAddressDevice += metadata[pos++].ToString("X");
            }
            Device dev;
            switch (metadata[pos++])
            {
                case 1: // Thermostat
                case 2: // Termostat+
                    dev = Devices["D:" + rfAddressDevice];
                    dev.ComfortTemperature = metadata[0x12];
                    dev.EcoTemperature = metadata[0x13];
                    dev.WindowOpenTemperature = metadata[0x17];
                    break;

                case 4: // Window Sensor
                    dev = Devices["D:" + rfAddressDevice];
                    break;
            }
        }
        private void ProcessDevice(string line)
        {
            line = line.Substring(2);
            byte[] metadata = DecodeFrom64(line);
            int pos = 0;
            do
            {
                int l = metadata[pos];
                string rfAddressDevice = "";
                for (int c = 1; c < 4; c++)
                {
                    if (metadata[pos + c] <= 0x0f)
                        rfAddressDevice += "0";
                    rfAddressDevice += metadata[pos + c].ToString("X");
                }
                if (Devices.ContainsKey("D:" + rfAddressDevice))
                {
                    Device dev = Devices["D:" + rfAddressDevice];
                    switch (dev.Type)
                    {
                        case 1:
                        case 2:
                            dev.Status1 = metadata[pos + 0x05];
                            dev.Status2 = metadata[pos + 0x06];
                            dev.ValvePosition = metadata[pos + 0x07];
                            dev.TemperatureSetpoint = metadata[pos + 0x08];
                            break;
                        case 4:
                            dev.Status1 = metadata[pos + 0x05];
                            dev.Status2 = metadata[pos + 0x06];
                            break;
                    }
                }
                pos += l + 1;
            }
            while (pos < metadata.Length);
        }
        private void ProcessUnit(string line)
        {
            line = line.Substring(2);
            string[] element = line.Split(',');
            max.SerialNumber = element[0];
            max.RFAddress = element[1];
            max.Firmware = element[2];
        }

        private void ShowMetadata()
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblUnitInfo.Text = max.ToString();
                treeView1.Nodes.Clear();
                txtIncoming.Text = "";
                foreach (Room r in Rooms.Values)
                {
                    TreeNode tnr = new TreeNode(r.Name);
                    tnr.Tag = "R:" + r.RFAddress;
                    foreach (Device u in Devices.Values)
                    {
                        if (u.RoomID == r.ID)
                        {
                            TreeNode tnu = new TreeNode(u.Name);
                            tnu.Tag = "D:" + u.RFAddress;
                            tnr.Nodes.Add(tnu);
                        }
                    }
                    treeView1.Nodes.Add(tnr);
                }
            });

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                switch (treeView1.SelectedNode.Tag.ToString().Substring(0, 1))
                {
                    case "D":
                        if((Devices[treeView1.SelectedNode.Tag.ToString()].Type == 1 || Devices[treeView1.SelectedNode.Tag.ToString()].Type == 2) && !GetRoomOfDevice(Devices[treeView1.SelectedNode.Tag.ToString()]).IsWindowOpen)
                            btnSet.Enabled = true;
                        else
                            btnSet.Enabled = false;
                        txtIncoming.Text = Devices[treeView1.SelectedNode.Tag.ToString()].ToString();
                        break;
                    case "R":
                        if (!Rooms[treeView1.SelectedNode.Tag.ToString()].IsWindowOpen)
                            btnSet.Enabled = true;
                        else
                            btnSet.Enabled = false;
                        txtIncoming.Text = Rooms[treeView1.SelectedNode.Tag.ToString()].ToString();
                        break;
                }
            }
            else
            {
                btnSet.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            client.Send("l:\r\n");
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                SetTemperatureForm stf = new SetTemperatureForm();

                switch (treeView1.SelectedNode.Tag.ToString().Substring(0, 1))
                {
                    case "D":
                        Device d = Devices[treeView1.SelectedNode.Tag.ToString()];
                        stf.DeviceName = d.Name;
                        stf.CurrentTemperature = d.TemperatureSetpoint;
                        stf.RoomName = GetRoomOfDevice(d).Name;
                        break;
                    case "R":
                        Room r = Rooms[treeView1.SelectedNode.Tag.ToString()];
                        stf.RoomName = r.Name;
                        Device dr = GetDeviceOfRoom(r);
                        stf.DeviceName = "All";
                        stf.CurrentTemperature = dr.TemperatureSetpoint;
                        break;
                }
                if (stf.ShowDialog(this) == DialogResult.OK)
                {
                    switch (treeView1.SelectedNode.Tag.ToString().Substring(0, 1))
                    {
                        case "D":
                            Device d = Devices[treeView1.SelectedNode.Tag.ToString()];
                            SetDeviceTemperature(d, stf.NewTemperature, stf.Permanent);
                            break;
                        case "R":
                            Room r = Rooms[treeView1.SelectedNode.Tag.ToString()];
                            Device device = GetDeviceOfRoom(r);
                            if (device != null && (device.Type == 1 || device.Type == 2))
                            {
                                SetDeviceTemperature(device, stf.NewTemperature, stf.Permanent);
                            }
                            break;
                    }
                }
            }
        }
        private void SetDeviceTemperature(Device device, int newTemp, bool permanent)
        {
            byte[] command = new byte[14];
            command[0] = 0x00;
            command[1] = 0x04;
            command[2] = 0x40;
            command[3] = 0x00;
            command[4] = 0x00;
            command[5] = 0x00;
            command[6] = byte.Parse(device.RFAddress.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            command[7] = byte.Parse(device.RFAddress.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            command[8] = byte.Parse(device.RFAddress.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            command[9] = Convert.ToByte(device.RoomID);
            command[10] = Convert.ToByte(newTemp + (permanent ? 64 : 128));
            command[11] = 0;
            command[12] = 0;
            command[13] = 0;
            string base64Command = System.Convert.ToBase64String(command);
            client.Send("s:" + base64Command + "\r\n");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            client.Send("l:\r\n");
            treeView1_AfterSelect(this, null);
        }
    }
}
