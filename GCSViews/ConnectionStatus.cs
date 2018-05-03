﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MissionPlanner.GCSViews
{
    public partial class ConnectionStatus : MyUserControl
    {
        internal MAVLinkInterface conn1 = null;
        public ConnectionStatus()
        {
            InitializeComponent();
        }
        private void PrintBat()
        {
            DateTime updateTime = DateTime.Now;
            //curs = new CurrentState();
            while (true)
            {
                if (updateTime.AddSeconds(0.5) < DateTime.Now)
                {
                    if (MainV2.comPort.BaseStream.IsOpen)
                    {
                        this.Invoke(new Action(delegate ()
                        {
                            if (conn1 != null)
                            {
                                label_PortName.Text = conn1.BaseStream.PortName;
                                label_linkquality.Text = conn1.MAV.cs.linkqualitygcs.ToString() + "% ";
                                label_GroundSpeed.Text = conn1.MAV.cs.groundspeed.ToString("f1") + "m/s";
                                label_yaw.Text = conn1.MAV.cs.yaw.ToString("f1") + "deg";
                                label_alt.Text = conn1.MAV.cs.alt.ToString("f1") + "m";
                                label_mode.Text = conn1.MAV.cs.mode.ToString(); label_mode.ForeColor = Color.Cyan;
                                label_battery.Text = conn1.MAV.cs.battery_voltage.ToString("f1") + "V   " + conn1.MAV.cs.current.ToString("f1") + "A ";
                                label_GPS.Text = conn1.MAV.cs.satcount.ToString() + "   (" + conn1.MAV.cs.gpshdop.ToString() + "m)  ";
                                label_latlng.Text = "( " + conn1.MAV.cs.lat.ToString("f6") + " , " + conn1.MAV.cs.lng.ToString("f6") + " )";

                                if (conn1.MAV.cs.armed)
                                {//arm
                                label_armedstatus.Text = "Armed";
                                    label_armedstatus.ForeColor = Color.Red;
                                }
                                else
                                {
                                    label_armedstatus.Text = "Disarmed";
                                    label_armedstatus.ForeColor = Color.Lime;
                                }

                                if (conn1.MAV.cs.ekfstatus > 0.5)
                                {//EKF
                                if (conn1.MAV.cs.ekfstatus > 0.8)
                                    {
                                        label_ekf.Text = "EKF";
                                        label_ekf.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        label_ekf.Text = "EKF";
                                        label_ekf.ForeColor = Color.Orange;
                                    }
                                }
                                else
                                {
                                    label_ekf.Text = "EKF";
                                    label_ekf.ForeColor = Color.Lime;
                                }

                            //label_12.Text =  ;
                        }


                        }));

                    }
                    updateTime = DateTime.Now;
                }

            }
        }

        /*private void label1_Click(object sender, EventArgs e)
        {
            Thread ThreadA = new Thread(new ThreadStart(PrintBat));
            ThreadA.IsBackground = true;
            ThreadA.Start();
        }*/

        private void ConnectionStatus_Load(object sender, EventArgs e)
        {
            Thread ThreadA = new Thread(new ThreadStart(PrintBat));
            ThreadA.IsBackground = true;
            ThreadA.Start();
        }
    }
}
