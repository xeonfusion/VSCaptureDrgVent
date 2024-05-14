﻿/*
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
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Net;
using System.Text.Json;

namespace VSCaptureDrgVent
{
    public class Crc
    {
        public byte ComputeChecksum(byte[] bytes)
        {
            byte sum = 0;
            byte crc = 0;

            for (int i = 0; i < bytes.Length; ++i)
            {
                sum += bytes[i];
            }

            //get 8-bit sum total checksum
            crc = (byte)(sum & 0xFF);                  //For Draeger
            return crc;
        }
    }

    public sealed class DSerialPort : SerialPort
    {
        private int DPortBufSize;
        public byte[] DPort_rxbuf;
        public List<byte[]> FrameList = new List<byte[]>();
        private bool m_storestartResp = false;
        private bool m_storestartCom = false;
        private bool m_storeend = false;
        private List<byte> m_bList = new List<byte>();
        private List<byte> m_bRespList = new List<byte>();
        private List<byte> m_bComList = new List<byte>();
        private bool m_runningCommand = false;
        private bool m_readingResponse = false;

        public List<NumericValResult> m_NumericValList = new List<NumericValResult>();
        public List<string> m_NumValHeaders = new List<string>();
        public StringBuilder m_strbuildvalues = new StringBuilder();
        public StringBuilder m_strbuildheaders = new StringBuilder();
        public string m_strTimestamp;
        public bool m_transmissionstart = true;
        public bool m_transmissionstart2 = true;
        public bool m_transmissionstart3 = true;
        public bool m_transmissionstart4 = true;

        public List<RealTimeConfigResponse> m_RtConfigRespList = new List<RealTimeConfigResponse>();
        public List<byte> m_RealTimeByteList = new List<byte>();
        public List<RealTimeData> m_RealTimeDataList = new List<RealTimeData>();
        public List<byte> m_RealTimeReqWaveList = new List<byte>();
        public List<WaveValResult> m_WaveValResultList = new List<WaveValResult>();
        public StringBuilder m_strbuildwavevalues = new StringBuilder();
        public long m_RealtiveTimeCounter =0;
        public int m_nWaveformSet=0;
        public bool m_realtimestart = false;
        public bool m_MEDIBUSstart = false;

        public string m_DeviceID;
        public string m_jsonposturl;
        public int m_dataexportset = 1;

        public string m_MQTTUrl;
        public string m_MQTTtopic;
        public string m_MQTTuser;
        public string m_MQTTpassw;
        public string m_MQTTclientId = Guid.NewGuid().ToString();



        public class NumericValResult
        {
            public string Timestamp;
            public string PhysioID;
            public string Value;
        }

        public class WaveValResult
        {
            public string Timestamp;
            public string Relativetimestamp;
            public string PhysioID;
            public string Value;
            public string Datastreamindex;
            public long Relativetimecounter;
            public RealTimeConfigResponse RtConfigData = new RealTimeConfigResponse();
            public string RespiratoryCycleState;
        }

        public class RealTimeConfigResponse
        {
            public string datacode;
            public string interval;
            public string minvalue;
            public string maxvalue;
            public string maxbinvalue;

        }

        public class RealTimeData
        {
            public byte syncbyte;
            public List<byte[]> synccommand = new List<byte[]>();
            public List<byte[]> rtdatavalues = new List<byte[]>();
            public List<byte> datastreamlist = new List<byte>();
            public string respsyncstate;
        }

        //Create a singleton serialport subclass
        private static volatile DSerialPort DPort = null;

        public static DSerialPort getInstance
        {

            get
            {
                if (DPort == null)
                {
                    lock (typeof(DSerialPort))
                        if (DPort == null)
                        {
                            DPort = new DSerialPort();
                        }

                }
                return DPort;
            }

        }

        public DSerialPort()
        {
            DPort = this;

            DPortBufSize = 4096;
            DPort_rxbuf = new byte[DPortBufSize];

            if (OSIsUnix())
                DPort.PortName = "/dev/ttyUSB0"; //default Unix port
            else DPort.PortName = "COM1"; //default Windows port

            DPort.BaudRate = 19200;
            DPort.Parity = Parity.Even;
            DPort.DataBits = 8;
            DPort.StopBits = StopBits.One;

            DPort.Handshake = Handshake.None;
            //DPort.Handshake = Handshake.XOnXOff;
            DPort.RtsEnable = true;
            DPort.DtrEnable = true;

            // Set the read/write timeouts
            DPort.ReadTimeout = 600000;
            DPort.WriteTimeout = 600000;

            //ASCII Encoding in C# is only 7bit so
            DPort.Encoding = Encoding.GetEncoding("ISO-8859-1");

        }

        public void RequestICC()
        {
            DPort.WriteBuffer(DataConstants.poll_request_icc_msg);
            DebugLine("Send: Request ICC");
        }

        public void RequestDevID()
        {
            DPort.WriteBuffer(DataConstants.poll_request_deviceid);
            DebugLine("Send: Request DevID");
        }

		public void RequestMeasuredDataCP1()
		{
            DPort.WriteBuffer(DataConstants.poll_request_config_measured_data_codepage1);
            DebugLine("Send: Request Data CP1");
        }

        public void RequestMeasuredDataCP2()
        {
            DPort.WriteBuffer(DataConstants.poll_request_config_measured_data_codepage2);
            DebugLine("Send: Request Data CP2");
        }

        public void RequestDeviceSettings()
		{
            DPort.WriteBuffer(DataConstants.poll_request_device_settings);
            DebugLine("Send: Request Data Dev settings");
        }

        public void RequestTextMessages()
		{
            DPort.WriteBuffer(DataConstants.poll_request_text_messages);
            DebugLine("Send: Request Data TextMsgs");
        }

        public void RequestStopCommunication()
        {
            DPort.WriteBuffer(DataConstants.poll_request_stop_com);
            DebugLine("Send: Request Stop Communication");
        }

        public void RequestRealtimeDataConfiguration()
        {
            DPort.WriteBuffer(DataConstants.poll_request_real_time_data_config);
            DebugLine("Send: Request Realtime Config");
        }

        public void SendNOP()
        {
            DPort.WriteBuffer(DataConstants.poll_request_no_operation);
            DebugLine("Send: NOP");
        }

        public void SendDeviceID()
        {
            List<byte> temptxbufflist = new List<byte>();
            byte[] deviceidcommandresponse = { 0x52 };
            byte[] DevID = Encoding.ASCII.GetBytes("0161");
            byte[] DevName = Encoding.ASCII.GetBytes("'VSCaptureDrgVent'");
            byte[] DevRevision = Encoding.ASCII.GetBytes("01.03");
            byte[] MedibusVer = Encoding.ASCII.GetBytes(":06.00");

            temptxbufflist.AddRange(deviceidcommandresponse);
            temptxbufflist.AddRange(DevID);
            temptxbufflist.AddRange(DevName);
            temptxbufflist.AddRange(DevRevision);
            temptxbufflist.AddRange(MedibusVer);

            CommandEchoResponse(temptxbufflist.ToArray());
            DebugLine("Send: Device ID (response)");
        }

        public void ReadRealtimeConfigResponse(byte[] packetdata)
        {
            //Store configuration values
            MemoryStream memstream = new MemoryStream(packetdata);
            BinaryReader binreader = new BinaryReader(memstream);

            byte[] packetheader = binreader.ReadBytes(2);

            for (int i=2; i<packetdata.Length;i=i+23)
            {
                byte[] datacode = binreader.ReadBytes(2);
                byte[] interval = binreader.ReadBytes(8);
                byte[] minvalue = binreader.ReadBytes(5);
                byte[] maxvalue = binreader.ReadBytes(5);
                byte[] maxbinvalue = binreader.ReadBytes(3);

                RealTimeConfigResponse RtConfigResp = new RealTimeConfigResponse();
                RtConfigResp.datacode = Regex.Replace(Encoding.ASCII.GetString(datacode), @"\s+", "");
                RtConfigResp.interval = Regex.Replace(Encoding.ASCII.GetString(interval), @"\s+", "");
                RtConfigResp.minvalue = Regex.Replace(Encoding.ASCII.GetString(minvalue), @"\s+", "");
                RtConfigResp.maxvalue = Regex.Replace(Encoding.ASCII.GetString(maxvalue), @"\s+", "");
                RtConfigResp.maxbinvalue = Regex.Replace(Encoding.ASCII.GetString(maxbinvalue), @"\s+", "");

                m_RtConfigRespList.Add(RtConfigResp);

            }


        }

        public static void CreateWaveformSet(int nWaveSetType, List<byte> WaveTrtype)
        {
            switch (nWaveSetType)
            {
                case 0:
                    break;
                case 1:
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Airway_pressure"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Flow_inspiratory_expiratory"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Respiratory_volume_since_start_of_inspiration"));
                    break;
                case 2:
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "O2_saturation_pulse_Pleth"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "O2_concentration_inspiratory_expiratory"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "CO2_concentration_mmHg"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Concentration_of_primary_agent_inspiratory_expiratory_Percent"));
                    break;
                case 3:
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Tracheal_pressure"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Inspiratory_device_flow"));
                    break;
                case 4:
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Airway_pressure"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Flow_inspiratory_expiratory"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Respiratory_volume_since_start_of_inspiration"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "O2_saturation_pulse_Pleth"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "O2_concentration_inspiratory_expiratory"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "CO2_concentration_mmHg"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Concentration_of_primary_agent_inspiratory_expiratory_Percent"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Inspiratory_device_flow"));
                    WaveTrtype.Add((byte)Enum.Parse(typeof(DataConstants.MedibusXRealTimeData), "Tracheal_pressure"));
                    break;
            }

        }

        public void ConfigureRealtimeTransmission()
        {
            if (m_nWaveformSet == 0) return;

            List<byte> temptxbufflist = new List<byte>();
            List<byte> WaveTrType = new List<byte>();
            
            CreateWaveformSet(m_nWaveformSet, WaveTrType);
            byte[] rtdlistarray = WaveTrType.ToArray();

            temptxbufflist.AddRange(DataConstants.poll_configure_real_time_transmission);

            m_RealTimeReqWaveList.AddRange(rtdlistarray);

            for (int i=0; i < rtdlistarray.Count(); i++)
            {
                byte[] rtdarray = { rtdlistarray.ElementAt(i) };
                string rtdtoasciihex1 = BitConverter.ToString(rtdarray);
                byte[] rtdasciihexbytes1 = Encoding.ASCII.GetBytes(rtdtoasciihex1);
                string multiplier1 = "01";
                byte[] multiplierhexbytes1 = Encoding.ASCII.GetBytes(multiplier1);

                temptxbufflist.AddRange(rtdasciihexbytes1);
                temptxbufflist.AddRange(multiplierhexbytes1);
            }

            DPort.WriteBuffer(temptxbufflist.ToArray());
            DebugLine("Send: Configure realtime transmission (command)");
        }

        public void EnableDataStreams()
        {
            EnableDataStream1to4();
            if (m_nWaveformSet == 4)
            {
                EnableDataStream5to8();
                EnableDataStream9to12();
            }
        }

        public void DisableDataStreams()
        {
            DisableDataStream1to4();
            if (m_nWaveformSet == 4)
            {
                DisableDataStream5to8();
                DisableDataStream9to12();
            }
        }

        public void EnableDataStream1to4()
        {
            const byte SyncByte = 0xD0;
            const byte SyncCommand = DataConstants.SC_DATASTREAM_1_4; //0xC1
            const byte SyncArgument = 0xCF; //number of streams to enable
            const byte EndSyncByte = 0xC0;

            List<byte> temptxbufflist = new List<byte>();

            temptxbufflist.Add(SyncByte);
            temptxbufflist.Add(SyncCommand);
            temptxbufflist.Add(SyncArgument);
            temptxbufflist.Add(EndSyncByte);
            temptxbufflist.Add(EndSyncByte);

            byte[] finalbuff = temptxbufflist.ToArray();
            DPort.Write(finalbuff, 0, finalbuff.Length);
            DebugLine("Send: Enable Data Stream 1to4");
        }

        public void EnableDataStream5to8()
        {
            const byte SyncByte = 0xD0;
            const byte SyncCommand = DataConstants.SC_DATASTREAM_5_8; //0xC2
            const byte SyncArgument = 0xCF; //number of streams to enable
            const byte EndSyncByte = 0xC0;

            List<byte> temptxbufflist = new List<byte>();

            temptxbufflist.Add(SyncByte);
            temptxbufflist.Add(SyncCommand);
            temptxbufflist.Add(SyncArgument);
            temptxbufflist.Add(EndSyncByte);
            temptxbufflist.Add(EndSyncByte);

            byte[] finalbuff = temptxbufflist.ToArray();
            DPort.Write(finalbuff, 0, finalbuff.Length);
            DebugLine("Send: Enable Data Stream 5to8");
        }

        public void EnableDataStream9to12()
        {
            const byte SyncByte = 0xD0;
            const byte SyncCommand = DataConstants.SC_DATASTREAM_9_12; //0xC3
            const byte SyncArgument = 0xCF; //number of streams to enable
            const byte EndSyncByte = 0xC0;

            List<byte> temptxbufflist = new List<byte>();

            temptxbufflist.Add(SyncByte);
            temptxbufflist.Add(SyncCommand);
            temptxbufflist.Add(SyncArgument);
            temptxbufflist.Add(EndSyncByte);
            temptxbufflist.Add(EndSyncByte);

            byte[] finalbuff = temptxbufflist.ToArray();
            DPort.Write(finalbuff, 0, finalbuff.Length);
            DebugLine("Send: Enable Data Stream 9to12");
        }

        public void DisableDataStream1to4()
        {
            const byte SyncByte = 0xD0;
            const byte SyncCommand = DataConstants.SC_DATASTREAM_1_4; //0xC1
            const byte SyncArgument = 0xC0; //number of streams to disable
            const byte EndSyncByte = 0xC0;

            List<byte> temptxbufflist = new List<byte>();

            temptxbufflist.Add(SyncByte);
            temptxbufflist.Add(SyncCommand);
            temptxbufflist.Add(SyncArgument);
            temptxbufflist.Add(EndSyncByte);
            temptxbufflist.Add(EndSyncByte);

            byte[] finalbuff = temptxbufflist.ToArray();
            DPort.Write(finalbuff, 0, finalbuff.Length);
        }

        public void DisableDataStream5to8()
        {
            const byte SyncByte = 0xD0;
            const byte SyncCommand = DataConstants.SC_DATASTREAM_5_8; //0xC2
            const byte SyncArgument = 0xC0; //number of streams to disable
            const byte EndSyncByte = 0xC0;

            List<byte> temptxbufflist = new List<byte>();

            temptxbufflist.Add(SyncByte);
            temptxbufflist.Add(SyncCommand);
            temptxbufflist.Add(SyncArgument);
            temptxbufflist.Add(EndSyncByte);
            temptxbufflist.Add(EndSyncByte);

            byte[] finalbuff = temptxbufflist.ToArray();
            DPort.Write(finalbuff, 0, finalbuff.Length);
        }

        public void DisableDataStream9to12()
        {
            const byte SyncByte = 0xD0;
            const byte SyncCommand = DataConstants.SC_DATASTREAM_9_12; //0xC3
            const byte SyncArgument = 0xC0; //number of streams to disable
            const byte EndSyncByte = 0xC0;

            List<byte> temptxbufflist = new List<byte>();

            temptxbufflist.Add(SyncByte);
            temptxbufflist.Add(SyncCommand);
            temptxbufflist.Add(SyncArgument);
            temptxbufflist.Add(EndSyncByte);
            temptxbufflist.Add(EndSyncByte);

            byte[] finalbuff = temptxbufflist.ToArray();
            DPort.Write(finalbuff, 0, finalbuff.Length);
        }
        
        public void RequestRealtimeConfigChanged()
        {
            DPort.WriteBuffer(DataConstants.poll_request_real_time_config_changed);
        }

        public void ParseRealtimeDataResponse()
        {
            if(m_RealTimeByteList.Count()>2 && m_nWaveformSet!=0 && m_realtimestart ==true)
            {
                byte[] RealTimeByteArray = m_RealTimeByteList.ToArray();
                int bytesSuccessfullyRead = 0;

                for (int i = 0; i < RealTimeByteArray.Length; i++)
                {
                    byte bvalue = RealTimeByteArray.ElementAt(i);
                    
                    if ((bvalue & DataConstants.SYNC_BYTE) == DataConstants.SYNC_BYTE)
                    {
                        RealTimeData RTdata = new RealTimeData();

                        RTdata.syncbyte = bvalue;

                        for (int j = i+1; j < RealTimeByteArray.Length-1; j++)
                        {
                            byte bvaluenext = RealTimeByteArray.ElementAt(j);
                            if ((bvaluenext & DataConstants.SYNC_BYTE) == DataConstants.SYNC_BYTE) break;

                            if ((bvaluenext & DataConstants.SYNC_CMD_BYTE) == DataConstants.SYNC_CMD_BYTE)
                            {
                                byte[] buffer = new byte[2];
                                Buffer.BlockCopy(RealTimeByteArray, j, buffer, 0, 2);
                                RTdata.synccommand.Add(buffer);
                                j = j + 1;
                                i = j;
                            }
                            else
                            {
                                byte[] buffer = new byte[2];
                                Buffer.BlockCopy(RealTimeByteArray, j, buffer, 0, 2);
                                RTdata.rtdatavalues.Add(buffer);
                                j = j + 1;
                                i = j;
                            }
                        }
                        CreateDataStreamList(ref RTdata);
                        //if (RTdata.rtdatavalues.Count() != 0)
                        if (RTdata.rtdatavalues.Count() == RTdata.datastreamlist.Count())
                        {
                            m_RealTimeDataList.Add(RTdata);
                            bytesSuccessfullyRead = i + 1;
                        }
                    }

                }
                ReadRealTimeDataList();
                m_RealTimeByteList.RemoveRange(0, bytesSuccessfullyRead);
            }
            
        }

        public void CreateDataStreamList(ref RealTimeData RTdata)
        {
            //Read enabled Realtime curve numbers from sync byte & sync command byte
            //The bit-positions are related to the datastream-number in the same order as specified 
            //within the "Configure Realtime-Tranmission" command by the data-requesting device before
            byte syncbyte = RTdata.syncbyte;

            if ((syncbyte & 0x01) != 0) RTdata.datastreamlist.Add(0);
            if ((syncbyte & 0x02) != 0) RTdata.datastreamlist.Add(1);
            if ((syncbyte & 0x04) != 0) RTdata.datastreamlist.Add(2);
            if ((syncbyte & 0x08) != 0) RTdata.datastreamlist.Add(3);

            if (RTdata.synccommand.Count() != 0)
            {
                foreach (byte[] synccommandbytes in RTdata.synccommand)
                {
                    byte synccommand = synccommandbytes[0];
                    byte syncargument = synccommandbytes[1];
                    
                    switch (synccommand)
                    {
                        case DataConstants.SC_TX_DATASTREAM_5_8:
                            if ((syncargument & 0x01) != 0) RTdata.datastreamlist.Add(4);
                            if ((syncargument & 0x02) != 0) RTdata.datastreamlist.Add(5);
                            if ((syncargument & 0x04) != 0) RTdata.datastreamlist.Add(6);
                            if ((syncargument & 0x08) != 0) RTdata.datastreamlist.Add(7);
                            break;
                        case DataConstants.SC_TX_DATASTREAM_9_12:
                            if ((syncargument & 0x01) != 0) RTdata.datastreamlist.Add(8);
                            if ((syncargument & 0x02) != 0) RTdata.datastreamlist.Add(9);
                            if ((syncargument & 0x04) != 0) RTdata.datastreamlist.Add(10);
                            if ((syncargument & 0x08) != 0) RTdata.datastreamlist.Add(11);
                            break;
                        case DataConstants.SC_START_CYCLE:
                            if (syncargument == 0xC0) RTdata.respsyncstate = "InspStart";
                            if (syncargument == 0xC1) RTdata.respsyncstate = "ExpStart";
                            break;
                    }
                }
            }

        }
        
        public void ReadRealTimeDataList()
        {
            int RTdatacount = m_RealTimeDataList.Count();
            DateTime dtime = DateTime.Now;
            //Get unix timestamp
            m_RealtiveTimeCounter = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000);

            for (int i = 0; i < RTdatacount; i++)
            {
                RealTimeData RTdata = m_RealTimeDataList.ElementAt(i);

                for (int j=0;j<RTdata.datastreamlist.Count()&&j<RTdata.rtdatavalues.Count();j++)
                {
                    //Add realtime data values to datastreams
                    int datastreamindex = RTdata.datastreamlist.ElementAt(j);

                    WaveValResult WavValResult = new WaveValResult();

                    WavValResult.Datastreamindex = datastreamindex.ToString();
                    WavValResult.Timestamp = dtime.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);

                    if (datastreamindex < m_RealTimeReqWaveList.Count())
                    {
                        byte wavecode = m_RealTimeReqWaveList.ElementAt(datastreamindex);
                        
                        //byte wavecode = (byte)Int32.Parse(m_RtConfigRespList.ElementAt(datastreamindex).datacode);

                        byte[] wavecodearray = { wavecode };
                        string wavedatacode = BitConverter.ToString(wavecodearray);
                        WavValResult.PhysioID = Enum.GetName(typeof(DataConstants.MedibusXRealTimeData), wavecode);

                        WavValResult.RespiratoryCycleState = RTdata.respsyncstate;

                        WavValResult.RtConfigData = m_RtConfigRespList.Find(x => x.datacode == wavedatacode);
                        if (WavValResult.RtConfigData != null)
                        {
                            //int interval = Int32.Parse(WavValResult.RtConfigData.interval);
                            int minvalue = Int32.Parse(WavValResult.RtConfigData.minvalue);
                            int maxvalue = Int32.Parse(WavValResult.RtConfigData.maxvalue);
                            int maxbinvalue = Int32.Parse(WavValResult.RtConfigData.maxbinvalue, NumberStyles.HexNumber);

                            byte[] rtdatabytes = RTdata.rtdatavalues.ElementAt(j);

                            int firstbinvalue = (rtdatabytes[0] & 0x3F);
                            int secondbinvalue = (rtdatabytes[1] & 0x3F);

                            //A realtime value is transmitted at a resolution of 12 bits
                            int rtbinval = (firstbinvalue & 0x3F) | ((secondbinvalue & 0x3F) << 6);

                            double rtvalue = (((double)rtbinval / maxbinvalue) * (maxvalue - minvalue)) + minvalue;
                            double finalrtvalue = Math.Round(rtvalue, 4);

                            WavValResult.Value = finalrtvalue.ToString(CultureInfo.InvariantCulture);
                            WavValResult.Relativetimecounter = m_RealtiveTimeCounter;

                            m_WaveValResultList.Add(WavValResult);

                        }
                    }
                    
                    //byte wavecode = m_RealTimeReqWaveList.ElementAt(datastreamindex);
 
                }
                
            }
            
            m_RealTimeDataList.RemoveRange(0, RTdatacount);

            SaveWaveValues();
        }
        
        public void SaveWaveValues()
        {
            int wavevallistcount = m_WaveValResultList.Count;
            if (wavevallistcount != 0)
            {
                //Create list of physio ids available
                List<string> PhysioIDList = m_WaveValResultList.Select(x => x.PhysioID).Distinct().ToList();

                foreach (string physioid in PhysioIDList)
                {
                    m_strbuildwavevalues =  CreateCSVFromWavevalues(physioid);

                    string WavValID = string.Format("{0}DrgWaveExport.csv", physioid);

                    string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), WavValID);

                    ExportNumValListToCSVFile(pathcsv, m_strbuildwavevalues);

                    m_strbuildwavevalues.Clear();
                }
        
                m_WaveValResultList.RemoveRange(0, wavevallistcount);
            }

        }

        public StringBuilder CreateCSVFromWavevalues(string PhysioID)
        {

            List<WaveValResult> Wavevalues = m_WaveValResultList.FindAll(x => x.PhysioID == PhysioID);
            StringBuilder strbuildwavevalues = new StringBuilder();
            int i = 0;

            foreach (WaveValResult WavValResult in Wavevalues)
            {
                int interval = Int32.Parse(WavValResult.RtConfigData.interval);
                WavValResult.Relativetimestamp = (m_RealtiveTimeCounter+(i++*(interval/1000))).ToString();

                strbuildwavevalues.Append(WavValResult.Timestamp);
                strbuildwavevalues.Append(',');
                strbuildwavevalues.Append(WavValResult.Relativetimestamp);
                strbuildwavevalues.Append(',');
                strbuildwavevalues.Append(WavValResult.Value);
                strbuildwavevalues.Append(',');
                strbuildwavevalues.Append(WavValResult.RespiratoryCycleState);
                //strbuildwavevalues.Append(',');
                strbuildwavevalues.AppendLine();

            }

            return strbuildwavevalues;
        }

        public async Task SendCycledICCRequest(int nInterval)
        {
            int nmillisecond = nInterval * 1000;
            if (nmillisecond != 0)
            {
                do
                {
                    RequestICC();
                    await Task.Delay(nmillisecond);

                }
                while (true);
            }
            RequestICC();

        }

        public async Task SendCycledRequests(int nInterval)
        {
            int nmillisecond = nInterval * 1000;
            if (nmillisecond != 0)
            {
                do
                {
                    if (m_MEDIBUSstart == true)
                    {
                        RequestMeasuredDataCP1();
                    }
                    await Task.Delay(nmillisecond);
        
                }
                while (true);
            }
            if (m_MEDIBUSstart == true) RequestMeasuredDataCP1();

        }

        public async Task SendCycledPollDataRequestCP1(int nInterval)
        {
            int nmillisecond = nInterval * 1000;
            if (nmillisecond != 0)
            {
                do
                {
                    if(m_MEDIBUSstart == true)
                    {
                        RequestMeasuredDataCP1();
                    }
                    await Task.Delay(nmillisecond);

                }
                while (true);
            }
            if (m_MEDIBUSstart == true) RequestMeasuredDataCP1();

		}

        public async Task SendCycledPollDataRequestCP2(int nInterval)
        {
            int nmillisecond = nInterval * 1000;
            if (nmillisecond != 0)
            {
                do
                {
                    if (m_MEDIBUSstart == true)
                    {
                        RequestMeasuredDataCP2();
                    }
                    await Task.Delay(nmillisecond);

                }
                while (true);
            }
            if (m_MEDIBUSstart == true) RequestMeasuredDataCP2();

		}

		public async Task SendCycledPollDeviceSettings(int nInterval)
		{
			int nmillisecond = nInterval * 1000;
			if (nmillisecond != 0)
			{
				do
				{
                    if (m_MEDIBUSstart == true)
                    {
                        RequestDeviceSettings();
                    }
                    await Task.Delay(nmillisecond);

				}
				while (true);
			}
            if (m_MEDIBUSstart == true) RequestDeviceSettings();
		}

		public async Task SendCycledPollTextMessages(int nInterval)
		{
			int nmillisecond = nInterval * 1000;
			if (nmillisecond != 0)
			{
				do
				{
                    if (m_MEDIBUSstart == true)
                    {
                        RequestTextMessages();
                    }
					await Task.Delay(nmillisecond);

				}
				while (true);
			}
            if (m_MEDIBUSstart == true) RequestTextMessages();
		}

        public async Task KeepConnectionAlive(int nInterval)
        {
            int nmillisecond = nInterval * 1000;
            if (nmillisecond != 0)
            {
                do
                {
                    if(m_MEDIBUSstart == true && m_readingResponse == false && m_runningCommand == false)
                    {
                        SendNOP();
                    }
                    await Task.Delay(nmillisecond);


                }
                while (true);
            }
            if (m_MEDIBUSstart == true && m_readingResponse == false && m_runningCommand == false) SendNOP();
        }

        public void WriteBuffer(byte[] txbuf)
        {
            m_runningCommand = true;
            List<byte> temptxbufflist = new List<byte>();

            int framelen = txbuf.Length;
            if (framelen != 0)
            {
                //Add first command character 0x1B
                temptxbufflist.Add(DataConstants.BOFCOMCHAR);
                temptxbufflist.AddRange(txbuf);

                byte[] inputbuffer = temptxbufflist.ToArray();

                Crc crccheck = new Crc();
                byte checksumcomputed = crccheck.ComputeChecksum(inputbuffer);
               
                byte[] checksumarray = { checksumcomputed };
				string checksumtoasciihex = BitConverter.ToString(checksumarray);
				byte[] checksumasciihexbytes = Encoding.ASCII.GetBytes(checksumtoasciihex);

				temptxbufflist.AddRange(checksumasciihexbytes);
                temptxbufflist.Add(DataConstants.EOFCHAR);
                
                byte[] finaltxbuff = temptxbufflist.ToArray();

                try
                {
                    DPort.Write(finaltxbuff, 0, finaltxbuff.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
                }

            }
            m_runningCommand = false;
        }

        public void ClearReadBuffer()
        {
            //Clear the buffer
            for (int i = 0; i < DPortBufSize; i++)
            {
                DPort_rxbuf[i] = 0;
            }
        }

        public int ReadBuffer()
        {
            int bytesreadtotal = 0;

            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "DrgVentRawoutput.raw");

                int lenread = 0;

                do
                {
                    ClearReadBuffer();
                    lenread = DPort.Read(DPort_rxbuf, 0, DPortBufSize);

                    byte[] copyarray = new byte[lenread];

                    for (int i = 0; i < lenread; i++)
                    {
                        copyarray[i] = DPort_rxbuf[i];
                        CreateFrameListFromByte(copyarray[i]);
                    }

                    ByteArrayToFile(path, copyarray, copyarray.GetLength(0));
                    bytesreadtotal += lenread;


                }
                while (DPort.BytesToRead != 0);

                if (DPort.BytesToRead == 0)
                {
                    ParseRealtimeDataResponse();

                    if (FrameList.Count > 0)
                    {
                        ReadPacketFromFrame();

                        FrameList.RemoveRange(0, FrameList.Count);

                    }
   
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }


            return bytesreadtotal;

        }

        public void ReadPacketFromFrame()
        {
            if (FrameList.Count > 0)
            {
                foreach (byte[] fArray in FrameList)
                {
                    ProcessPacket(fArray);
                }

            }
        }

        public void ProcessPacket(byte[] packetbuffer)
        {
            string headerdataresponse = Encoding.ASCII.GetString(packetbuffer);

            string responsetype = headerdataresponse.Substring(0, 2);

            m_strTimestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);

            m_readingResponse = true;

            switch (responsetype)
            {
                case "\x1bQ": //ICC request
                    DebugLine("Received: ICC request");
                    byte[] icccommandresponse = {0x51};
                    CommandEchoResponse(icccommandresponse);
                    WaitForMilliSeconds(200);
                    RequestDevID();
                    break;
                case "\x01Q": //ICC response
                    DebugLine("Received: ICC response");
                    m_MEDIBUSstart = true;
                    RequestDevID();
                    break;
                case "\x1bR": //Device ID request
                    DebugLine("Received: Device ID request");
                    //Send empty or complete device id response
                    SendDeviceID();
                    m_MEDIBUSstart = true;
                    break;
                case "\x01R": //Device id response
                    DebugLine("Received: Device ID response");
                    m_MEDIBUSstart = true;
                    if (m_realtimestart == false)
                    {
                        RequestRealtimeDataConfiguration();
                    }
                    break;
                case "\x01S": //Realtime config respone
                    DebugLine("Received: Realtime Config Response");
                    ReadRealtimeConfigResponse(packetbuffer);
                    ConfigureRealtimeTransmission();
                    m_realtimestart = true;
                    break;
                case "\x01T": //Realtime configuration transmission response
                    DebugLine("Received: Realtime Config Transmission response");
                    EnableDataStreams();
                    break;
                case "\x1bV": //Realtime configuration changed command
                    DebugLine("Received: Realtime config changed command");
                    DisableDataStreams();
                    m_realtimestart = false;
                    //Request realtime configuration to reenable realtime data
                    RequestRealtimeDataConfiguration();
                    break;
                case "\x01$": //Data response cp1
                    DebugLine("Received: Data CP1 response");
                    ParseDataResponseMeasuredCP1(packetbuffer);
                    RequestMeasuredDataCP2();
                    break;
                case "\x01+": //Data response cp2
                    DebugLine("Received: Data CP2 response");
                    ParseDataResponseMeasuredCP2(packetbuffer);
                    RequestDeviceSettings();
                    break;
                case "\x01)": //Data response device settings
                    DebugLine("Received: Data device settings response");
                    ParseDataDeviceSettings(packetbuffer);
                    RequestTextMessages();
                    break;
                case "\x01*": //Data response text messages
                    DebugLine("Received: Data text messages response");
                    ParseDataTextMessages(packetbuffer);
                    break;
                case "\x01\x30": //NOP Response
                    DebugLine("Received: NOP response");
                    byte[] nopresponse = { 0x30 };
                    CommandEchoResponse(nopresponse);
                    break;
                case "\x1b\x30": //NOP request
                    DebugLine("Received: NOP request");
                    byte[] nopresponse2 = { 0x30 };
                    CommandEchoResponse(nopresponse2);
                    break;
                default: // Unknown message
                    DebugLine("Received: Unknown message");
                    if (responsetype.Substring(0,1) == "\x1b")
                    {
                        byte[] echoresponse = Encoding.ASCII.GetBytes(responsetype.Substring(1, 1));
                        CommandEchoResponse(echoresponse);
                    }
                    break;
            }

            m_readingResponse = false;

        }

        public void DebugLine(string msg)
        {
            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff") + " - " + msg);
        }

        public void CommandEchoResponse(byte[] commandbuffer)
        {
            string headerdataresponse = Encoding.ASCII.GetString(commandbuffer);

            List<byte> temprxbufflist = new List<byte>();

            int framelen = commandbuffer.Length;
            if (framelen != 0)
            {
                temprxbufflist.Add(DataConstants.BOFRESPCHAR);
                temprxbufflist.AddRange(commandbuffer);

                byte[] inputbuffer = temprxbufflist.ToArray();

                Crc crccheck = new Crc();
                byte checksumcomputed = crccheck.ComputeChecksum(inputbuffer);
                
                byte[] checksumarray = { checksumcomputed };
                string checksumtoasciihex = BitConverter.ToString(checksumarray);
                byte[] checksumasciihexbytes = Encoding.ASCII.GetBytes(checksumtoasciihex);

                temprxbufflist.AddRange(checksumasciihexbytes);
                temprxbufflist.Add(DataConstants.EOFCHAR);

                byte[] finaltxbuff = temprxbufflist.ToArray();

                try
                {
                    DPort.Write(finaltxbuff, 0, finaltxbuff.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to serial port :: " + ex.Message, "Error!");
                }

            }

        }

        public void ParseDataResponseMeasuredCP1(byte[] packetbuffer)
        {
            ParseDataResponseMeasured(packetbuffer, typeof(DataConstants.MedibusXMeasurementCP1));
            SaveNumericValueListRows("MedibusXMeasuredCP1");
        }

        public void ParseDataResponseMeasuredCP2(byte[] packetbuffer)
        {
            ParseDataResponseMeasured(packetbuffer, typeof(DataConstants.MedibusXMeasurementCP2));
            SaveNumericValueListRows("MedibusXMeasuredCP2");
        }

        public void ParseDataDeviceSettings(byte[] packetbuffer)
        {
            if (packetbuffer.Length != 0)
            {
                int dataarraylen = (packetbuffer.Length - 2);
                byte[] dataarray = new byte[dataarraylen];
                Array.Copy(packetbuffer, 2, dataarray, 0, dataarraylen);
                string response = Encoding.ASCII.GetString(dataarray);

                Console.WriteLine(response);
                Console.WriteLine();

                int responselen = response.Length;
                string DataCode;
                string DataValue;
                string physio_id;
                NumberStyles style;


                for (int i = 0; i < responselen; i = i + 7)
                {
                    DataCode = response.Substring(i, 2);
                    DataValue = response.Substring((i + 2), 5);
                    DataValue.Trim();

                    style = NumberStyles.HexNumber;
                    byte datacodebyte = Byte.Parse(DataCode, style);

                    physio_id = Enum.GetName(typeof(DataConstants.MedibusXDeviceSettings), datacodebyte);
                    if (physio_id == null) physio_id = "UnknownID";
                    
                    Console.WriteLine("{0}: {1}", physio_id, DataValue);
                    //Console.WriteLine();

                    NumericValResult NumVal = new NumericValResult();

                    NumVal.Timestamp = m_strTimestamp;
                    NumVal.PhysioID = physio_id;
                    NumVal.Value = DataValue;

                    m_NumericValList.Add(NumVal);
                    m_NumValHeaders.Add(NumVal.PhysioID);

                }

                SaveNumericValueListRows("MedibusXDeviceSettings");
            }

 
        }


        public void ParseDataResponseMeasured(byte [] packetbuffer, Type enumtype)
        {
            if (packetbuffer.Length != 0)
            {
                int dataarraylen = (packetbuffer.Length - 2);
                byte[] dataarray = new byte[dataarraylen];
                Array.Copy(packetbuffer,2,dataarray,0,dataarraylen);
                string response = Encoding.ASCII.GetString(dataarray);

                Console.WriteLine(response);
                Console.WriteLine();

                int responselen = response.Length;
                string DataCode;
                string DataValue;
                string physio_id;
                NumberStyles style;


                for (int i = 0; i < responselen; i = i + 6)
                {
                    DataCode = response.Substring(i, 2);
                    DataValue = response.Substring((i + 2), 4);
                    DataValue.Trim();

                    style = NumberStyles.HexNumber;
                    byte datacodebyte = Byte.Parse(DataCode, style);

                    physio_id = Enum.GetName(enumtype, datacodebyte);
                    if (physio_id == null) physio_id = "UnknownID";

                    Console.WriteLine("{0}: {1}", physio_id, DataValue);
                    //Console.WriteLine();

                    NumericValResult NumVal = new NumericValResult();

                    NumVal.Timestamp = m_strTimestamp;
                    NumVal.PhysioID = physio_id;
                    NumVal.Value = DataValue;

                    m_NumericValList.Add(NumVal);
                    m_NumValHeaders.Add(NumVal.PhysioID);

                }


            }

        }

        public void ParseDataTextMessages(byte[] packetbuffer)
        {
            if (packetbuffer.Length != 0)
            {
                int dataarraylen = (packetbuffer.Length - 2);
                byte[] dataarray = new byte[dataarraylen];
                Array.Copy(packetbuffer, 2, dataarray, 0, dataarraylen);
                string response = Encoding.ASCII.GetString(dataarray);

                Console.WriteLine(response);
                Console.WriteLine();

                int responselen = response.Length;
                int textitemlen = 0;
                string DataCode;
                string DataValue;
                string physio_id;
                NumberStyles style;

                for (int i = 0; i < responselen; i = (i + 4 + textitemlen))
                {
                    DataCode = response.Substring(i, 2);
                    string textitemlenstr = response.Substring(i + 2, 1);
                    byte textitemlenbyte = Encoding.ASCII.GetBytes(textitemlenstr).ElementAt(0);
                    textitemlen = (int)(textitemlenbyte - 0x30);
                    if(textitemlen!=0)
                    {
                        DataValue = response.Substring((i + 3), textitemlen);
                        DataValue.Trim();

                        style = NumberStyles.HexNumber;
                        byte datacodebyte = Byte.Parse(DataCode, style);

                        physio_id = Enum.GetName(typeof(DataConstants.MedibusXTextMessages), datacodebyte);
                        if (physio_id == null) physio_id = "UnknownID";

                        Console.WriteLine("{0}: {1}", physio_id, DataValue);
                        Console.WriteLine();

                        NumericValResult NumVal = new NumericValResult();

                        NumVal.Timestamp = m_strTimestamp;
                        NumVal.PhysioID = physio_id;
                        NumVal.Value = DataValue;

                        m_NumericValList.Add(NumVal);
                        m_NumValHeaders.Add(NumVal.PhysioID);

                    }

                }

                SaveNumericValueListRows("MedibusXTextMessages");
            }


        }

        public void NAKResponse()
        {
            byte[] nakresponse = { 0x15 };
            CommandEchoResponse(nakresponse);
        }

        public void CreateFrameListFromByte(byte bvalue)
        {
            switch (bvalue)
            {
                case DataConstants.BOFRESPCHAR:
                    m_storestartResp = true;
                    m_storeend = false;
                    m_bRespList.Add(bvalue);
                    break;
                case DataConstants.BOFCOMCHAR:
                    m_storestartCom = true;
                    m_storeend = false;
                    m_bComList.Add(bvalue);
                    break;
                case DataConstants.EOFCHAR:
                    // If both m_storestartResp and m_storestartCom are true, the Command (Com) is embedded.
                    // In this case EOF refers to Com.
                    if(m_storestartCom == true && m_storestartResp == true)
                    {
                        m_bList.AddRange(m_bComList);
                        m_bComList.RemoveRange(0, m_bComList.Count());
                        m_bRespList.RemoveRange(0, m_bRespList.Count());
                        m_storestartCom = false;
                        m_storestartResp = false;
                    }
                    else if (m_storestartCom == true)
                    {
                        m_bList.AddRange(m_bComList);
                        m_bComList.RemoveRange(0, m_bComList.Count());
                        m_storestartCom = false;
                    }
                    else if (m_storestartResp == true)
                    {
                        m_bList.AddRange(m_bRespList);
                        m_bRespList.RemoveRange(0, m_bRespList.Count());
                        m_storestartResp = false;
                    }
                    
                    m_storeend = true;
                    break;
                default:
                    // Other bytes should be added to either the realtime-byte list or the com-list or the resp-list to be parsed
                    if ((DataConstants.RT_BYTE & bvalue) == DataConstants.RT_BYTE)
                    {
                        //Realtime data is distinguished from slow data in that the most significant bit (realtime data flag) is set
                        m_RealTimeByteList.Add(bvalue);
                    }
                    else if (m_storestartCom == true && m_storeend == false) m_bComList.Add(bvalue);
                    else if (m_storestartResp == true && m_storeend == false) m_bRespList.Add(bvalue);
                    break;
            }

            if(m_storeend == true)
            {
                int framelen = m_bList.Count();
                if (framelen != 0)
                {
                    byte[] bArray = new byte[framelen];
                    bArray = m_bList.ToArray();

                    //serial data without checksum byte
                    int userdataframelen = (framelen - 2);
                    byte[] userdataArray = new byte[userdataframelen];

                    //Get user data without checksum
                    Array.Copy(bArray, 0, userdataArray, 0, userdataframelen);

                    //Read checksum
                    //byte checksumbyte = bArray[framelen - 1];
                    byte[] checksumarray = new byte[2];
                    Array.Copy(bArray, framelen-2, checksumarray, 0, 2);
                    string checksumstr = Encoding.ASCII.GetString(checksumarray);

                    //Calculate checksum
                    Crc crccheck = new Crc();
                    byte checksumcomputed = crccheck.ComputeChecksum(userdataArray);
                    byte[] checksumcomputedarray = { checksumcomputed };
                    string checksumcomputedstr = BitConverter.ToString(checksumcomputedarray);

                    if (checksumcomputedstr == checksumstr)
                    {
                        FrameList.Add(userdataArray);
                        //Console.WriteLine("Crc OK");
                    }
                    else
                    {
                        Console.WriteLine("Checksum Error");
                        DebugLine("Checksum Error");
                        NAKResponse();
                    }

                    m_bList.RemoveRange(0, m_bList.Count);
                    m_storeend = false;

                }

            }
        }

        bool WriteHeadersForDatatype(string datatype)
        {
            bool writeheader = true;
            switch (datatype)
            {
                case "MedibusXMeasuredCP1":
                    if (m_transmissionstart)
                    {
                        m_transmissionstart = false;
                        
                    }
                    else writeheader = false;
                    break;
                case "MedibusXMeasuredCP2":
                    if (m_transmissionstart2)
                    {
                        m_transmissionstart2 = false;
                       
                    }
                    else writeheader = false;
                    break;
                case "MedibusXDeviceSettings":
                    if (m_transmissionstart3)
                    {
                        m_transmissionstart3 = false;
                        
                    }
                    else writeheader = false;
                    break;
                case "MedibusXTextMessages":
                    if (m_transmissionstart4)
                    {
                        m_transmissionstart4 = false;
                        
                    }
                    else writeheader = false;
                    break;
            }

            return writeheader;
        }

        public void WriteNumericHeadersList(string datatype)
        {
            if (m_NumericValList.Count != 0 && (WriteHeadersForDatatype(datatype)))
            {
                string filename = String.Format("DrgVent{0}DataExport.csv", datatype);

                string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), filename);

                m_strbuildheaders.Append("Time");
                m_strbuildheaders.Append(',');


                foreach (NumericValResult NumValResult in m_NumericValList)
                {
                    m_strbuildheaders.Append(NumValResult.PhysioID);
                    m_strbuildheaders.Append(',');

                }

                m_strbuildheaders.Remove(m_strbuildheaders.Length - 1, 1);
                m_strbuildheaders.Replace(",,", ",");
                m_strbuildheaders.AppendLine();
                ExportNumValListToCSVFile(pathcsv, m_strbuildheaders);

                m_strbuildheaders.Clear();
                m_NumValHeaders.RemoveRange(0, m_NumValHeaders.Count);
             
            }
        }

        public void SaveNumericValueListRows(string datatype)
        {
            if (m_dataexportset == 2) ExportNumValListToJSON(datatype);
            if (m_dataexportset == 3) ExportNumValListToMQTT(datatype);
            if (m_dataexportset == 4) ExportNumValListToJSONFile(datatype);
            if (m_dataexportset != 3 && m_dataexportset != 4)
            {
                if (m_NumericValList.Count != 0)
                {
                    WriteNumericHeadersList(datatype);
                    string filename = String.Format("DrgVent{0}DataExport.csv", datatype);

                    string pathcsv = Path.Combine(Directory.GetCurrentDirectory(), filename);

                    m_strbuildvalues.Append(m_NumericValList.ElementAt(0).Timestamp);
                    m_strbuildvalues.Append(',');


                    foreach (NumericValResult NumValResult in m_NumericValList)
                    {
                        m_strbuildvalues.Append(NumValResult.Value);
                        m_strbuildvalues.Append(',');

                    }

                    m_strbuildvalues.Remove(m_strbuildvalues.Length - 1, 1);
                    m_strbuildvalues.Replace(",,", ",");
                    m_strbuildvalues.AppendLine();

                    ExportNumValListToCSVFile(pathcsv, m_strbuildvalues);
                    m_strbuildvalues.Clear();
                    m_NumericValList.RemoveRange(0, m_NumericValList.Count);
                }
            }

        }

        public void ExportNumValListToCSVFile(string _FileName, StringBuilder strbuildNumVal)
        {
            try
            {
                // Open file for reading. 
                using (StreamWriter wrStream = new StreamWriter(_FileName, true, Encoding.UTF8))
                {
                    wrStream.Write(strbuildNumVal);
                    strbuildNumVal.Clear();

                    // close file stream. 
                    wrStream.Close();
                }

            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

        }

        public void StopTransfer()
        {
            RequestStopCommunication();
        }

        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray, int nWriteLength)
        {
            try
            {
                // Open file for reading. 
                using (FileStream _FileStream = new FileStream(_FileName, FileMode.Append, FileAccess.Write))
                {
                    // Writes a block of bytes to this stream using data from a byte array
                    _FileStream.Write(_ByteArray, 0, nWriteLength);

                    // close file stream. 
                    _FileStream.Close();
                }
    
                return true;
            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            // error occured, return false. 
            return false;
        }

        public void ExportNumValListToJSON(string datatype)
        {
            string serializedJSON = JsonSerializer.Serialize(m_NumericValList, new JsonSerializerOptions { IncludeFields = true });

            try
            {
                // Open file for reading. 
                //using (StreamWriter wrStream = new StreamWriter(pathjson, true, Encoding.UTF8))
                //{
                // wrStream.Write(serializedJSON);
                //  wrStream.Close();
                //}

                Task.Run(() => PostJSONDataToServer(serializedJSON));

            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
        }

        public void ExportNumValListToJSONFile(string datatype)
        {
            string serializedJSON = JsonSerializer.Serialize(m_NumericValList, new JsonSerializerOptions { IncludeFields = true });

            m_NumericValList.RemoveRange(0, m_NumericValList.Count);

            string filename = String.Format("DataExportVSC.json");

            string pathjson = Path.Combine(Directory.GetCurrentDirectory(), filename);

            try
            {
                // Open file for reading. 
                using (StreamWriter wrStream = new StreamWriter(pathjson, true, Encoding.UTF8))
                {
                    wrStream.Write(serializedJSON);

                    wrStream.Close();
                }
            }

            catch (Exception _Exception)
            {
                // Error. 
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
        }

        public async Task PostJSONDataToServer(string postData)
        {
            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var data = new StringContent(postData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(m_jsonposturl, data);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();

                Console.WriteLine(result);
            }
        }

        public void ExportNumValListToMQTT(string datatype)
        {
            string serializedJSON = JsonSerializer.Serialize(m_NumericValList, new JsonSerializerOptions { IncludeFields = true });

            m_NumericValList.RemoveRange(0, m_NumericValList.Count);

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var mqttClient = new MqttFactory().CreateMqttClient();
            var logger = new MqttFactory().DefaultLogger;
            var managedClient = new ManagedMqttClient(mqttClient, logger);

            var topic = m_MQTTtopic + string.Format("/{0}", datatype);

            try
            {
                var task = Task.Run(async () =>
                {
                    var connected = GetConnectedTask(managedClient);
                    await ConnectMQTTAsync(managedClient, token, m_MQTTUrl, m_MQTTclientId, m_MQTTuser, m_MQTTpassw);
                    await connected;

                    //await PublishMQTTAsync(managedClient, token, topic, serializedJSON);
                    //await managedClient.StopAsync();
                });

                task.ContinueWith(antecedent =>
                {
                    if (antecedent.Status == TaskStatus.RanToCompletion)
                    {
                        Task.Run(async () =>
                        {
                            await PublishMQTTAsync(managedClient, token, topic, serializedJSON);
                            await managedClient.StopAsync();
                        });
                    }
                });

            }

            catch (Exception _Exception)
            {
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }

        }

        Task GetConnectedTask(ManagedMqttClient managedClient)
        {
            TaskCompletionSource<bool> connected = new TaskCompletionSource<bool>();
            managedClient.ConnectedAsync += (MqttClientConnectedEventArgs arg) =>
            {

                connected.SetResult(true);
                //Console.WriteLine("MQTT Client connected");
                return Task.CompletedTask;
            };

            return connected.Task;

        }

        public static async Task ConnectMQTTAsync(ManagedMqttClient mqttClient, CancellationToken token, string mqtturl, string clientId, string mqttuser, string mqttpassw)
        {
            bool mqttSecure = true;

            var messageBuilder = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithCredentials(mqttuser, mqttpassw)
            .WithCleanSession()
            .WithWebSocketServer((MqttClientWebSocketOptionsBuilder b) =>
            {
                b.WithUri(mqtturl);
            });

            var tlsOptions = new MqttClientTlsOptionsBuilder()
               .WithSslProtocols(System.Security.Authentication.SslProtocols.Tls12)
               .Build();

            var options = mqttSecure
            ? messageBuilder
            .WithTlsOptions(tlsOptions)
                .Build()
            : messageBuilder
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
              .WithAutoReconnectDelay(TimeSpan.FromSeconds(1))
              .WithClientOptions(options)
              .Build();

            await mqttClient.StartAsync(managedOptions);

        }

        public static async Task PublishMQTTAsync(ManagedMqttClient mqttClient, CancellationToken token, string topic, string payload, bool retainFlag = true, int qos = 1)
        {
            await mqttClient.EnqueueAsync(topic, payload, (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos, retainFlag);
            //Console.WriteLine("The managed MQTT client is connected, publishing data.");

            // Wait until the queue is fully processed.
            SpinWait.SpinUntil(() => mqttClient.PendingApplicationMessagesCount == 0, 5000);
            //Console.WriteLine($"Pending messages = {mqttClient.PendingApplicationMessagesCount}");

        }

        public bool OSIsUnix()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128)) return true;
            else return false;

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
