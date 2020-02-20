/*
 * This file is part of VitalSignsCaptureDraegerVent v1.003.
 * Copyright (C) 2017-20 John George K., xeonfusion@users.sourceforge.net

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

        public static void Main(string[] args)
        {
            Console.WriteLine("VitalSignsCaptureDraegerVent v1.003 (C)2017-20 John George K.");
            // Create a new SerialPort object with default settings.
            DSerialPort _serialPort = DSerialPort.getInstance;

            Console.WriteLine("Select the Port to which Draeger Ventilator (Medibus.X protocol) is to be connected, Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine(" {0}", s);
            }


            Console.Write("COM port({0}): ", _serialPort.PortName.ToString());
            string portName = Console.ReadLine();

            if (portName != "")
            {
                // Allow the user to set the appropriate properties.
                _serialPort.PortName = portName;
            }

            Console.WriteLine();
            Console.WriteLine("Numeric Data Transmission sets:");
            Console.WriteLine("1. 1 second");
            Console.WriteLine("2. 5 second");
            Console.WriteLine("3. 10 second");
            Console.WriteLine("4. 1 minute");
            Console.WriteLine("5. 5 minute");
            Console.WriteLine("6. Single poll");
            Console.WriteLine();
            Console.Write("Choose Data Transmission interval (1-6):");

            string sIntervalset = Console.ReadLine();
            int[] setarray = { 1, 5, 10, 60, 300, 0 };
            short nIntervalset = 2;
            int nInterval = 10;
            if (sIntervalset != "") nIntervalset = Convert.ToInt16(sIntervalset);
            if (nIntervalset > 0 && nIntervalset < 7) nInterval = setarray[nIntervalset - 1];

            Console.WriteLine();
            Console.WriteLine("Waveform data export options:");
            Console.WriteLine("0. None");
            Console.WriteLine("1. Airway Pressure, Flow, Resp Volume");
            Console.WriteLine("2. Pleth, O2 conc.(%), CO2 conc.(mmHg), Primary agent conc.(%)");
            Console.WriteLine("3. Tracheal pressure, Inspiratory Flow");
            Console.WriteLine("4. All");
            Console.WriteLine();
            Console.Write("Choose Waveform data export priority option (0-4):");

            string sWaveformSet = Console.ReadLine();
            short nWaveformSet = 0;
            if (sWaveformSet != "") nWaveformSet = Convert.ToInt16(sWaveformSet);

            Console.WriteLine("\nYou may now connect the serial cable to the Draeger Ventilator");
            Console.WriteLine("Press any key to continue..");

            Console.ReadKey(true);

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

                Console.WriteLine();
                Console.WriteLine("Requesting Transmission set {0} from monitor", nIntervalset);


                Console.WriteLine();
                Console.WriteLine("Data will be written to CSV file DrgVentExportData.csv in same folder");

                _serialPort.RequestICC();
                //_serialPort.RequestDevID();

                //Task.Run(() => _serialPort.SendCycledICCRequest(nInterval));
                WaitForMilliSeconds(200);

                Task.Run(() => _serialPort.SendCycledRequests(nInterval));
                /*WaitForMilliSeconds(200);
                Task.Run(() => _serialPort.SendCycledPollDataRequestCP2(nInterval));
                WaitForMilliSeconds(200);
                Task.Run(() => _serialPort.SendCycledPollDeviceSettings(nInterval));
                WaitForMilliSeconds(200);
                Task.Run(() => _serialPort.SendCycledPollTextMessages(nInterval));
                WaitForMilliSeconds(200);*/


                //RequestRealtimeData after DevID response
                //_serialPort.RequestRealtimeDataConfiguration();

                //Task.Run(() => _serialPort.KeepConnectionAlive(2));

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

    }
}
