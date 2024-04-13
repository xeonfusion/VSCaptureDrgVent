/*
 * This file is part of VitalSignsCaptureDraegerVent v1.003.
 * Copyright (C) 2017-24 John George K., xeonfusion@users.sourceforge.net

    VitalSignsCaptureDraegerVent is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VitalSignsCaptureDraegerVent is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with VitalSignsCaptureDraegerVent.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;


namespace VSCaptureDrgVent
{
    class MainClass
    {
        static EventHandler dataEvent;
        public static string DeviceID;
        public static string JSONPostUrl;
        public static string MQTTUrl;
        public static string MQTTtopic;
        public static string MQTTuser;
        public static string MQTTpassw;


        public static void Main(string[] args)
        {
            Console.WriteLine("VitalSignsCaptureDraegerVent (C)2017-24 John George K.");
            Console.WriteLine();
            // Create a new SerialPort object with default settings.
            DSerialPort _serialPort = DSerialPort.getInstance;

            string portName;
            string sIntervalset;
            string sWaveformSet;

            var parser = new CommandLineParser();
            parser.Parse(args);

            if (parser.Arguments.ContainsKey("help"))
            {
                Console.WriteLine("VSCaptureDrgVent.exe -port [portname] -interval [number] -waveset [number]");
                Console.WriteLine(" -waveset[number] -export[number] -devid[name] -url [name]");
                Console.WriteLine("-port <Set serial port name>");
                Console.WriteLine("-interval <Set numeric transmission interval>");
                Console.WriteLine("-waveset <Set waveform transmission set option>");
                Console.WriteLine("-export <Set data export CSV, MQTT or JSON option>");
                Console.WriteLine("-devid <Set device ID for MQTT or JSON export>");
                Console.WriteLine("-url <Set MQTT or JSON export url>");
                Console.WriteLine("-topic <Set topic for MQTT export>");
                Console.WriteLine("-user <Set username for MQTT export>");
                Console.WriteLine("-passw <Set password for MQTT export>");
                Console.WriteLine();
                return;
            }

            if (parser.Arguments.ContainsKey("port"))
            {
                portName = parser.Arguments["port"][0];
            }
            else
            {
                Console.WriteLine("Select the Port to which Draeger Ventilator (Medibus.X protocol) is to be connected, Available Ports:");
                foreach (string s in SerialPort.GetPortNames())
                {
                    Console.WriteLine(" {0}", s);
                }

                string PortName;
                if (OSIsUnix())
                    PortName = "/dev/ttyUSB0"; //default Unix port
                else PortName = "COM1"; //default Windows port

                Console.Write("COM port({0}): ", PortName.ToString());
                portName = Console.ReadLine();
                if (portName == "") portName = PortName;

            }
            
            if (portName != "")
            {
                // Allow the user to set the appropriate properties.
                _serialPort.PortName = portName;
            }

            if (parser.Arguments.ContainsKey("interval"))
            {
                sIntervalset = parser.Arguments["interval"][0];
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Numeric Data Transmission sets:");
                Console.WriteLine("1. 5 second");
                Console.WriteLine("2. 10 second");
                Console.WriteLine("3. 1 minute");
                Console.WriteLine("4. 5 minute");
                Console.WriteLine("5. Single poll");
                Console.WriteLine();
                Console.Write("Choose Data Transmission interval (1-5):");

                sIntervalset = Console.ReadLine();

            }

            int[] setarray = { 5, 10, 60, 300, 0 };
            short nIntervalset = 2;
            int nInterval = 10;
            if (sIntervalset != "") nIntervalset = Convert.ToInt16(sIntervalset);
            if (nIntervalset > 0 && nIntervalset < 6) nInterval = setarray[nIntervalset - 1];

            string sDataExportset;
            if (parser.Arguments.ContainsKey("export"))
            {
                sDataExportset = parser.Arguments["export"][0];
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Data export options:");
                Console.WriteLine("1. Export as CSV files");
                Console.WriteLine("2. Export as CSV files and JSON to URL");
                Console.WriteLine("3. Export as MQTT to URL");
                Console.WriteLine();
                Console.Write("Choose data export option (1-3):");

                sDataExportset = Console.ReadLine();

            }
          
            int nDataExportset = 1;
            if (sDataExportset != "") nDataExportset = Convert.ToInt32(sDataExportset);

            if (nDataExportset == 2)
            {
                if (parser.Arguments.ContainsKey("devid"))
                {
                    DeviceID = parser.Arguments["devid"][0];
                }
                else
                {
                    Console.Write("Enter Device ID/Name:");
                    DeviceID = Console.ReadLine();

                }
              

                if (parser.Arguments.ContainsKey("url"))
                {
                    JSONPostUrl = parser.Arguments["url"][0];
                }
                else
                {
                    Console.Write("Enter JSON Data Export URL(http://):");
                    JSONPostUrl = Console.ReadLine();

                }
               

            }

            if (nDataExportset == 3)
            {
                if (parser.Arguments.ContainsKey("devid"))
                {
                    DeviceID = parser.Arguments["devid"][0];
                }
                else
                {
                    Console.Write("Enter Device ID/Name:");
                    DeviceID = Console.ReadLine();

                }
               

                if (parser.Arguments.ContainsKey("url"))
                {
                    MQTTUrl = parser.Arguments["url"][0];
                }
                else
                {
                    Console.Write("Enter MQTT WebSocket Server URL(ws://):");
                    MQTTUrl = Console.ReadLine();

                }
                

                if (parser.Arguments.ContainsKey("topic"))
                {
                    MQTTtopic = parser.Arguments["topic"][0];
                }
                else
                {
                    Console.Write("Enter MQTT Topic:");
                    MQTTtopic = Console.ReadLine();

                }
               

                if (parser.Arguments.ContainsKey("user"))
                {
                    MQTTuser = parser.Arguments["user"][0];
                }
                else
                {
                    Console.Write("Enter MQTT Username:");
                    MQTTuser = Console.ReadLine();

                }
              

                if (parser.Arguments.ContainsKey("passw"))
                {
                    MQTTpassw = parser.Arguments["passw"][0];
                }
                else
                {
                    Console.Write("Enter MQTT Password:");
                    MQTTpassw = Console.ReadLine();

                }
              

            }

            _serialPort.m_DeviceID = DeviceID;
            _serialPort.m_jsonposturl = JSONPostUrl;
            _serialPort.m_MQTTUrl = MQTTUrl;
            _serialPort.m_MQTTtopic = MQTTtopic;
            _serialPort.m_MQTTuser = MQTTuser;
            _serialPort.m_MQTTpassw = MQTTpassw;

            if (nDataExportset > 0 && nDataExportset < 4) _serialPort.m_dataexportset = nDataExportset;


            if (parser.Arguments.ContainsKey("waveset"))
            {
                sWaveformSet = parser.Arguments["waveset"][0];
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Waveform data export options:");
                Console.WriteLine("0. None");
                Console.WriteLine("1. Airway Pressure, Flow, Resp Volume");
                Console.WriteLine("2. Pleth, O2 conc.(%), CO2 conc.(mmHg), Primary agent conc.(%)");
                Console.WriteLine("3. Tracheal pressure, Inspiratory Flow");
                Console.WriteLine("4. All");
                Console.WriteLine();
                Console.Write("Choose Waveform data export priority option (0-4):");

                sWaveformSet = Console.ReadLine();

            }

            short nWaveformSet = 0;
            if (sWaveformSet != "") nWaveformSet = Convert.ToInt16(sWaveformSet);


            try
            {
                _serialPort.Open();
                _serialPort.m_nWaveformSet = nWaveformSet;

                if (_serialPort.OSIsUnix())
                {
                    dataEvent += new EventHandler((object sender, EventArgs e) => ReadData(sender));
                }

                if (!_serialPort.OSIsUnix())
                {
                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(p_DataReceived);
                }

                Console.WriteLine("You may now connect the serial cable to the Draeger Ventilator");
                //Console.WriteLine("Press any key to continue..");
                //Console.ReadKey(true);

                Console.WriteLine();
                //Console.WriteLine("Requesting Transmission from monitor");
                Console.WriteLine("Requesting Transmission set {0} from monitor", nIntervalset);


                Console.WriteLine();
                Console.WriteLine("Data will be written to CSV file DrgVentExportData.csv in same folder");

                _serialPort.RequestICC();
                WaitForMilliSeconds(200);

                Task.Run(() => _serialPort.SendCycledRequests(nInterval));
                
                //RequestRealtimeData after DevID response
                
                Task.Run(() => _serialPort.KeepConnectionAlive(2));

                Console.WriteLine("Press Escape button to Stop");

                if (_serialPort.OSIsUnix())
                {
                    do
                    {
                        if (_serialPort.BytesToRead != 0)
                        {
                            dataEvent.Invoke(_serialPort, new EventArgs());
                        }

                        if (Console.KeyAvailable == true)
                        {
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape) break;
                        }
                    }
                    while (Console.KeyAvailable == false);

                }

                if (!_serialPort.OSIsUnix())
                {
                    ConsoleKeyInfo cki;

                    do
                    {
                        cki = Console.ReadKey(true);
                    }
                    while (cki.Key != ConsoleKey.Escape);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }
            finally
            {
                _serialPort.StopTransfer();

                _serialPort.Close();

            }

        }

        static void p_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            ReadData(sender);

        }

        public static void ReadData(object sender)
        {
            try
            {
                (sender as DSerialPort).ReadBuffer();

            }
            catch (TimeoutException) { }
        }

        public static void WaitForSeconds(int nsec)
        {
            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddSeconds(nsec);
            do
            {
                dt = DateTime.Now;
            }
            while (dt2 > dt);

        }

        public static void WaitForMilliSeconds(int nmillisec)
        {
            DateTime dt = DateTime.Now;
            DateTime dt2 = dt.AddMilliseconds(nmillisec);
            do
            {
                dt = DateTime.Now;
            }
            while (dt2 > dt);

        }

        public static bool OSIsUnix()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128)) return true;
            else return false;

        }

        public class CommandLineParser
        {
            public CommandLineParser()
            {
                Arguments = new Dictionary<string, string[]>();
            }

            public IDictionary<string, string[]> Arguments { get; private set; }

            public void Parse(string[] args)
            {
                string currentName = "";
                var values = new List<string>();
                foreach (string arg in args)
                {
                    if (arg.StartsWith("-", StringComparison.InvariantCulture))
                    {
                        if (currentName != "" && values.Count != 0)
                            Arguments[currentName] = values.ToArray();

                        else
                        {
                            values.Add("");
                            Arguments[currentName] = values.ToArray();
                        }
                        values.Clear();
                        currentName = arg.Substring(1);
                    }
                    else if (currentName == "")
                        Arguments[arg] = new string[0];
                    else
                        values.Add(arg);
                }

                if (currentName != "")
                    Arguments[currentName] = values.ToArray();
            }

            public bool Contains(string name)
            {
                return Arguments.ContainsKey(name);
            }
        }

    }
}
