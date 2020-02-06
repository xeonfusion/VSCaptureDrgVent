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

namespace VSCaptureDrgVent
{
    static class DataConstants
    {
		/*
         Request current Alarms (Codepage 3) 23H
         Request current measured Data (Codepage 1) 24H 
         Request current low Alarm Limits (Codepage 1) 25H 
         Request current high Alarm Limits (Codepage 1) 26H 
         Request current Alarms (Codepage 1) 27H 
         Request current Date and Time 28H 
         Request current Device Setting 29H 
         Request current Text Messages 2AH 
         Request current measured Data (Codepage 2) 2BH 
         Request current low Alarm Limits (Codepage 2) 2CH 
         Request current high Alarm Limits (Codepage 2) 2DH 
         Request current Alarms (Codepage 2) 2EH 
         Request Device Identification 52H 
         Request Trend Data Status 6CH 
         Request Trend Data 6DH
        
		 Configure Data response
         24H for current Data, low Alarm Limits and high Alarm Limits (codepage 1)
         27H for current Alarms (codepage 1)
         29H for current Device Settings
         2AH for current Textmessages
         2BH for current Data, low Alarm Limits and high Alarm Limits (codepage 2)
         2EH for current Alarms (codepage 2)

        */

		public const byte BOFCOMCHAR = 0x1B;
        public const byte BOFRESPCHAR = 0x01;
        public const byte EOFCHAR = 0x0D;
        public const byte SOH = 0x01;
        public const byte ENDOFTEXT = 0x03;
        
        public const byte SYNC_BYTE = 0xD0;
        public const byte SYNC_MASK = 0xF0;
        public const byte SYNC_CMD_BYTE = 0xC0;
        public const byte RT_BYTE = 0x80;
        public const byte RT_BYTE_MASK = 0xC0;
        public const byte SC_END_OF_SEQUENCE = 0xC0;
        public const byte SC_DATASTREAM_1_4 = 0xC1;
        public const byte SC_DATASTREAM_5_8 = 0xC2;
        public const byte SC_DATASTREAM_9_12 = 0xC3;
        public const byte SC_TX_DATASTREAM_5_8 = 0xC4;
        public const byte SC_TX_DATASTREAM_9_12 = 0xC5;
        public const byte SC_START_CYCLE = 0xC6;
        public const byte SC_CORRUPT_DATA = 0xCF;

        public static byte[] poll_request_icc_msg = {
            0x51
        };
        public static byte[] poll_request_deviceid = {
            0x52
        };

		public static byte[] poll_request_no_operation = {
			0x30
		};

		public static byte[] poll_request_stop_com = {
			0x55
		};

		public static byte[] poll_request_config_measured_data_codepage1 = {
			0x24
		};

		public static byte[] poll_request_config_measured_data_codepage2 = {
			0x2B
		};

        public static byte[] poll_request_current_date_time =
        {
            0x28
        };

		public static byte[] poll_request_device_settings =
		{
			0x29
		};

		public static byte[] poll_request_text_messages =
		{
			0x2A
		};

        public static byte[] poll_request_real_time_data_config =
        {
            0x53
        };

        public static byte[] poll_configure_real_time_transmission =
        {
            0x54
        };
        
        public static byte[] poll_request_real_time_config_changed =
        {
            0x56
        };



        public enum MeasurementCP1 : byte
        {
			BreathingPressure = 0x05,   //mbar
			ComplianceFrac = 0x06,  //mlPerMBar
			Compliance = 0x07,  //LPerBar
			Resistance = 0x08, //mbarPerLPerSec
			CarbonDioxideProduction = 0x09, //mLPerMin
			ResistanceFrac = 0x0B,
		    rSquared = 0x2A,    //None
			InspHalothanekPa = 0x50,   //kPa
			ExpHalothanekPa = 0x51, //kPa
			InspEnfluranekPa = 0x52,   //kPa
			ExpEnfluranekPa = 0x53, //kPa
			InspIsofluranekPa = 0x54,  //kPa
			ExpIsofluranekPa = 0x55,    //kPa
			InspDesfluranekPa = 0x56,  //kPa
			ExpDesfluranekPa = 0x57,    //kPa
			InspSevofluranekPa = 0x58, //kPa
			ExpSevofluranekPa = 0x59,  //kPa
			InspAgentkPa = 0x5A,  //kPa
			ExpAgentkPa = 0x5B, //kPa
			InspAgent2kPa = 0x5C,  //kPa
			ExpAgent2kPa = 0x5D,  //kPa
			O2Uptake = 0x64,   //TenMlPerMin
			AmbientPressure = 0x6B, //mbar
			MinimalAirwayPressure = 0x71,
		    OcclusionPressure = 0x72,
		    MeanBreathingPressure = 0x73,  //mbar
			PlateauPressure = 0x74, //mbar
			PEEPBreathingPressure = 0x78,  //mbar
		    IntrinsicPEEPBreathingPressure = 0x79,
		    SpontaneousMinuteVolumeFrac = 0x7A,
		    RRmand = 0x7B, //OnePerMin
			PeakBreathingPressure = 0x7D,   //mbar
			VTmand = 0x7E, //L
			VTspon = 0x7F, //L
			TrappedVolume = 0x81,
		    TidalVolumeFrac = 0x82,
		    VTemand = 0x83, //mL
		    VTespon = 0x84, //mL
			InspmandatoryTidalVolumeVTimand = 0x85, //mL
			TidalVolume = 0x88, //mL
			DeadSpace = 0x89,
		    RelativeDeadSpace = 0x8A,
		    InspiratorySpontaneousSupportVolume = 0x8B,
		    InspMAC = 0xAC, //None
			ExpMAC = 0xAD, // None
			InspDesfluranePct = 0xAE,  //pct
			ExpDesfluranePct = 0xAF,    //pct
			InspSevofluranePct = 0xB0,  //pct
			ExpSevofluranePct = 0xB1,   //pct
			Leakage = 0xB2, //mLPerMin
			LeakageRelPctleak = 0xB3,  //pct
			RespiratoryRatePressure = 0xB4, //OnePerMin
			SpontaneousRespiratoryRate = 0XB5,
		    SpontaneousFractionMinVoPctMVsponMVtotal = 0xB6,    //pct
			SpontaneousMinuteVolume = 0xB7,
		    RespiratoryMinuteVolume = 0xB8,
		    RespiratoryMinuteVolumeFrac = 0xB9, //L
			ApneaDuration = 0xBD,  //sec
			AirwayTemperature = 0xC1,
		    DeltaO2 = 0xC4, //pct
			RapidShallowBreathingIndex = 0xC9,
		    RespiratoryRateCO2 = 0xD5, //OnePerMin
			RespiratoryRate = 0xD6,
		    RespiratoryRateVolumePerFlow = 0xD7,    //OnePerMin
			RespiratoryRateDerived = 0xD9,  //OnePerMin
			InspCO2Pct = 0xDA, //pct
			EndTidalCO2Percent = 0xDB,  //pct
			N2OFlow = 0xDD, //mLPerMin
			AirFlow = 0xDE, //mLPerMin
			PulseRateDerived = 0xDF,   //OnePerMin
			PulseRateOximeter = 0xE1,  //OnePerMin
			O2Flow = 0xE2, //mLPerMin
			EndTidalCO2kPa = 0xE3, //kPa
			InspCO2mmHg = 0xE5, //mmHg
			EndTidalCO2mmHg = 0xE6, //mmHg
			ItoE_Ipart = 0xE7, //None
			ItoE_Epart = 0xE8, //None
			InspAgentPct = 0xE9,   //pct
			ExpAgentPct = 0xEA, //pct
		    OxygenSaturation = 0xEB,   //pct
			InspAgent2Pct = 0xED,  //pct
			ExpAgent2Pct = 0xEE,   //pct
			ExpO2 = 0xEF,  //pct
			InspO2 = 0xF0, //pct
            InspHalothanePct = 0xF4,    //pct
			ExpHalothanePct = 0xF5, //pct
			InspEnfluranePct = 0xF6,  //pct
			ExpEnfluranePct = 0xF7, //pct
			InspIsofluranePct = 0xF8,   //pct
			ExpIsofluranePct = 0xF9,   //pct
			InspN2OPct = 0xFB,  //pct
			ExpN2OPct = 0xFC,  //pct
			InspCO2kPa = 0xFF, //kPa


        };

        public enum MeasurementCP2 : byte
        {
			VTspon = 0x00,  //mL
			ElastanceE = 0x06, //mbarPerL
			Tau = 0x07, //sec
			ExpiratoryTidalVolumeVTe = 0x21,   //mL
			InspiratoryTidalVolumeVTi = 0x22,   //mL
			EIP = 0x23, //mbar
			Tlowmax = 0x7D, //sec
			PressureVariability = 0x7B, //pct

		};

        public enum DeviceSettings : byte
        {
			Oxygen = 0x01,  //pct
			MaxInpirationFlow = 0x02,  //LPerMin
			InspTidalVolume = 0x04, //L
			InspiratoryTime = 0x05, //sec
			IPart = 0x07, //None
			EPart = 0x08,  //None
			FrequencyIMV = 0x09,   //OnePerMin
			FrequencyIPPV = 0x0A,  //OnePerMin
			PEEP = 0x0B,   //mbar
			IntermittentPEEP = 0x0C,   //mbar
			BIPAPLowPressure = 0x0D,   //mbar
			BIPAPHighPressure = 0x0E,  //mbar
			BIPAPLowTime = 0x0F,   //sec
			BIPAPHighTime = 0x10,  //sec
			ApneaTime = 0x11,  //sec
			PressureSupportPressure = 0x12, //mbar
			MaxInspirationAirwayPressure = 0x13,    //mbar
			TriggerPressure = 0x15, //mbar
			TachyapneaFrequency = 0x16, //OnePerMin
			TachyapneaDuration = 0x17, //sec
			InspPause_InspTime = 0x27, //pct
			FlowTrigger = 0x29, //LPerMin
			ASBRamp = 0x2E, //sec
			FreshgasFlow = 0x2F,   //mLPerMin
			VT = 0x40, //mL
			MinimalFrequency = 0x42,   //OnePerMin
			InspiratoryPressure = 0x45, //mbar
			Age = 0x4A, //yr
			Weight = 0x4B, //kg
            InspiratoryFlow = 0x4C, //L/sec

        };

        public enum TextMessages : byte
        {
			VentModeIPPV = 0x01,
			VentModeIPPVAssist = 0x02,
			VentModeCPPV = 0x04,
			VentModeCPPVAssist = 0x05,
			VentModeSIMV = 0x06,
			VentModeSIMVASB = 0x07,
			SB = 0x8,
			ASB = 0x09,
			CPAP = 0x0A,
			CPAP_ASB = 0x0B,
			MMV = 0x0C,
			MMV_ASB = 0x0D,
			BIPAP = 0x0E,
			SYNCHRON_MASTER = 0x0F,
			SYNCHRON_SLAVE = 0x10,
			APNEA_VENTILATION = 0x11,
			DS = 0x12,
			BIPAP_SMV = 0x18,
			BIPAP_SMV_ASB = 0x19,
			BIPAP_APRV = 0x1A,
			VentStandby = 0x1E,
			Adults = 0x20,
			Neonates = 0x21,
			CO2InmmHg = 0x22,
			CO2InkPa = 0x23,
			CO2InPercent = 0x24,
			AnesGasHalothane = 0x25,
			AnesGasEnflurane = 0x26,
			AnesGasIsoflurane = 0x27,
			AnesGasDesflurane = 0x28,
			AnesGasSevoflurane = 0x29,
			NoAnesGas = 0x2A,
			VentModeManualSpont = 0x2B,
			SelectedLanguage = 0x2C,
			VentModePCV = 0x34,
			VentModeFreshGasExt = 0x36,
			CarrierGasAir = 0x37,
			CarrierGasN2O = 0x38,
            VentPtMode = 0x3A,
            VenType = 0x48,
			AnesGas2Halothane = 0x4A,
			AnesGas2Enflurane = 0x4B,
			AnesGas2Isoflurane = 0x4C,
			AnesGas2Desflurane = 0x4D,
			AnesGas2Sevoflurane = 0x4E,
			NoAnesGas2 = 0x4F,
			PerformingLeakageTest = 0x53,
			DeviceInStandby = 0x54,
			AgentUnitkPa = 0x56,
			AgentUnitPct = 0x57,
			HLMModeActive = 0x58,
			VolumeMode = 0x59,
			PressureMode = 0x5A,
			PressureSupportMode = 0x5B,
			PressureSupportAdded = 0x5C,
			SyncIntermittentVent = 0x5D,

		};

        public enum AlarmsCP1 : byte
        {
			ApneaCombinedSource = 0x00,
			NoSpO2Pulse10Seconds = 0x01,
			SpO2PulseBelowLowLimit = 0x02,
			O2SatBelowLowLimit = 0x07,
			InpiredOxygenBelowLowLimit = 0x08,
			InspHalothaneExceedsHighLimit = 0x09,
			InspEnfluraneExceedsHighLimit = 0x0B,
			InspIsofluraneExceedsHighLimit = 0x0C,
			ApneaNoCO2Fluct30Seconds = 0x0D,
			ApneaNoVolumeExhaled30Seconds = 0x0E,
			ApneaPressureAbsent15Seconds = 0x0F,
			AirwayPressureExceedsHighLimit = 0x10,
			CheckGasSupply = 0x11,
			CheckAirSupply = 0x12,
			CheckO2Supply = 0x13,
			MinuteVolumeBelowLowLimit = 0x19,
			SpO2PulseExceedsHighLimit = 0x1E,
			InspSevofluraneExceedsHighLimit = 0x1F,
			O2SatExceedsHighLimit = 0x22,
			InspDesfluraneExceedsHighLimit = 0x24,
			EndTidalCO2BelowLowLimit = 0x27,
			EndTidalCO2ExceedsHighLimit = 0x28,
			InspHalothaneBelowLowLimit = 0x29,
			InspEnfluraneBelowLowLimit = 0x2A,
			InspIsofluraneBelowLowLimit = 0x2B,
			InspDesfluraneBelowLowLimit = 0x2C,
			InspSevofluraneBelowLowLimit = 0x32,
			VolumeNotConstant = 0x33,
			SpO2SensorDisconnectedOrFault = 0x35,
			PercentOxygenExceedsHighLimit = 0x37,
			AssistedSpontaneousBreathingExceeds4Seconds = 0x3A,   
			InspCO2ExceedsHighLimit = 0x3C,
			CO2PatientSensorLineBlocked = 0x3D,
			TachyapenaAlarmDisabled = 0x40,
			CheckFlowSensor = 0x42,
			BatteryLow = 0x4B,
			CO2AlarmDisabled = 0x57,
			OximeterAlarmDisabled = 0x5B,
			VolumeAlarmDisabled = 0x5E,
			CO2MonInLowAccMode = 0x63,
			CO2WindowOccluded = 0x64,
			PrimarySpeakerFailure = 0x65,
			MixedAgentDetected = 0x66,
			MultigasMonitorDeviceFailure = 0x67,
			OximeterDeviceFailure = 0x68,
			N2OMeasurementInoperable = 0x69,
			CO2DeviceFailure = 0x6A,
			AgentMeasurementInoperable = 0x6E,
			CommunicationErrorRS232Port2 = 0x78,
			CommunicationErrorRS232Port1 = 0x79,
			InternalCommunicationError = 0x7C,
			RespiratoryRateExceedsHighLimit = 0x90,
			ApnoeDetectedByEvita = 0x98,
			DisconnectionVentilator = 0x9A,
			MinuteVolumeExceedsHighLimit = 0x9B,
			ProblemsWithRespirator = 0x9F,
			VentCommunicationLost = 0xA0,
			InspiredOxygenMeasurementInoperable = 0xA1,
			MeanAirwayPressureBelowMinus2mbar = 0xA3,
			VolumeMeasurementInoperable = 0xA4,
			MinVolAlarmDisabled = 0xAC,
			PressureMeasurementInoperable = 0xAD,
			CheckExpirationValve = 0xB0,
			TooHighRespiratorDeviceTemp = 0xB7,
			AirwayTemperatureMeasurementInoperable = 0xB8,
			CheckAirwayTemperatureSensor = 0xB9,
			AirwayTemperatureExceedsHighLimit = 0xBA,
			SighModeActive = 0xBB,
			BreathingSystemVented = 0xBC,
			CheckOxygenSupply = 0xBD,
			OxygenMeasurementInoperable = 0xBE,
			InpiredOxygenExceedsHighLimit = 0xBF,
			InspiredOxygenBelowLowLimit = 0xC0,
			FlowMeasurementInoperable = 0xC1,
			GasMixerInoperableAdvisory = 0xC2,
			TimeLimitedRespiratoryVolume = 0xC3,
			PressureLimitedRespiratoryVolume = 0xC4,
			HighRespiratorDeviceTemp = 0xC5,
			RespiratorSynchronizationInoperable = 0xC6,
			FailToCycle = 0xC7,
			FreshGasDeliveryFailure = 0xC8,
			InternalTemperatureHigh = 0xC9,
			FanFailure = 0xCA,
			CO2SensorDisconnectedOrFault = 0xD9,
			PEEPHighPressureLimit = 0xDA,
			TidalVolumeExceedsHighLimit = 0xE8,
			NeoVolumeMeasurementInoperable = 0xEA,
			CheckN2OSupply = 0xED,
			TwoAgentsDetected = 0xEE,
			PowerFail = 0xEF,
			CheckSettingOfPmax = 0xF1,
			O2SafetyFlowOpenDuringNormalOperation = 0xF4,
			InspCO2AlarmsOff = 0xF7,
			PEEPExceedsPressureThreshold15Seconds = 0xF8,

		};

        public enum AlarmsCP2 : byte
        {
			O2CylinderPressureLowWithoutWallSupply = 0x31,
			O2CylinderEmptyWithoutWallSupply = 0x32,
			O2CylinderNotConnected = 0x33,
			N2OCylinderEmpty = 0x34,
			N2ODeliveryFailure = 0x35,
			O2DeliveryFailure = 0x36,
			AIRDeliveryFailure = 0x37,
			SetFreshGasFlowNotAttained = 0x38,
			InternalExternalSwitchoverValveError = 0x39,
			InspN2OHigh = 0x3A,
			CircleOccluded = 0x6A,
			BreathingSystemDisconnected = 0x8D,
			LossOfData = 0x91,
			ApneaVentilation = 0x93,
			CircleLeakage = 0x9C,
			VentNotInLockedPosition = 0xA0,
			PowerSupplyError = 0xA1,
			ExpHalothaneExceedsHighLimit = 0xA2,
			ExpEnfluraneExceedsHighLimit = 0xA3,
			ExpIsofluraneExceedsHighLimit = 0xA4,
			ExpDesfluraneExceedsHighLimit = 0xA5,
			ExpSevofluraneExceedsHighLimit = 0xA6,
			SetTidalVolumeNotAttained = 0xA7,
			InspFlowSensorInoperable = 0xA8,
			SettingCanceled = 0xA9,
			FreshGasFlowTooHigh = 0xAC,
			FreshGasFlowActive = 0xAD,
			OxygenCylinderOpen = 0xAF,
			N2OCylinderOpen = 0xB0,
			AirCylinderOpen = 0xB1,
			N2OCylinderSensorNotConnected = 0xB2,
			AirCylinderSensorNotConnected = 0xB3,
			O2CylinderSensorNotConnected = 0xB4,
			AirCylinderPressureLow = 0xB5,
			AirFreshGasFlowMeasurementInoperable = 0xC7,
			O2FreshGasFlowMeasurementInoperable = 0xC8,
			N2OFreshGasFlowMeasurementInoperable = 0xC9,
			NoAirSupply = 0xCA,
			NoN2OSupply = 0xCB,

		};


        public enum MedibusXRealTimeData : byte
        {
            Airway_pressure = 0x00, //mbar
            Flow_inspiratory_expiratory = 0x01, //L/min
            O2_saturation_pulse_Pleth = 0x02,
            Respiratory_volume_since_start_of_inspiration = 0x03, //mL
            O2_concentration_inspiratory_expiratory = 0x05,  //%
            CO2_concentration_mmHg= 0x06, //mmHg
            CO2_concentration_kPa = 0x07, //kPa
            CO2_concentration_Percent = 0x08, //%
            Concentration_of_primary_agent_inspiratory_expiratory_Percent  = 0x0A, //%
            Halothane_concentration_inspiratory_expiratory_Percent = 0x0B, //%
            Enflurane_concentration_inspiratory_expiratory_Percent = 0x0C, //%
            Isoflurane_concentration_inspiratory_expiratory_Percent = 0x0D, //%
            Desflurane_concentration_inspiratory_expiratory_Percent = 0x0E, //%
            Sevoflurane_concentration_inspiratory_expiratory_Percent = 0x0F, //%
            Tracheal_pressure = 0x1C, //mbar
            Inspiratory_device_flow = 0x1E, //L/min
            Concentration_of_primary_agent_inspiratory_expiratory_kPa  = 0x2A, //kPa
            Halothane_concentration_inspiratory_expiratory_kPa = 0x2B, //kPa
            Enflurane_concentration_inspiratory_expiratory_kPa = 0x2C, //kPa
            Isoflurane_concentration_inspiratory_expiratory_kPa = 0x2D, //kPa
            Desflurane_concentration_inspiratory_expiratory_kPa = 0x2E, //kPa
            Sevoflurane_concentration_inspiratory_expiratory_kPa = 0x2F, //kPa

            /*Enable_Disable_datastream1_to_4_command_code  = 0xC1,
            Enable_Disable_datastream5_to_8_command_code = 0xC2,
            Enable_Disable_datastream9_to_12_command_code = 0xC3,
            Enable_all_datastream_argument = 0xCF,
            Disable_all_datastream_argument = 0xC0,
            End_of_Sync_Command_Sequence_command_code = 0xC0,
            End_of_Sync_Command_Sequence_argument = 0xC0,
            Sync_byte_start_code = 0xD0,
            Sync_signal_start_of_ventilator_inspiratory_cycle_command_code = 0xC6,
            Sync_signal_start_of_ventilator_inspiratory_cycle_argument = 0xC0,
            Sync_signal_start_of_ventilator_expiratory_cycle_command_code = 0xC6,
            Sync_signal_start_of_ventilator_expiratory_cycle_argument = 0xC1,*/

        };

        public enum MedibusXDeviceSettings : byte
        {
            Inspiratory_oxygen_fraction = 0x01, // FiO2 % 
            Inspiratory_flow = 0x02,    //L/min
            Time_interval_between_sighs = 0x03, // Interval sighs
            Inspiratory_tidal_volume_L = 0x04,    // VTi L
            Inspiratory_time = 0x05,    // Ti s
            Expiratory_time = 0x06, // Te s
            IE_I_part = 0x07,    //
            IE_E_part = 0x08,    //
            Respiratory_rate = 0x09,    // RR 1/min
            Minimal_provided_respiratory_rate = 0x0A,   // RRmin 1/min
            Positive_end_expiratory_pressure = 0x0B,    // PEEP mbar
            Additional_intermittent_PEEP_for_sighs = 0x0C,  // intPEEPsigh
            Lower_pressure_level_during_APRV = 0x0D,    // Plow mbar
            Upper_pressure_level_during_APRV = 0x0E,    // Phigh
            Time_of_lower_pressure_level_during_APRV = 0x0F,    // Tlow s
            Time_of_upper_pressure_level_during_APRV = 0x10,    // Thigh
            Apnea_alarm_time = 0x11,    // Tapn s
            Pressure_amplitude_above_PEEP_in_Pressure_Support = 0x12,   // Psupp mbar
            Pressure_limitation = 0x13, // Pmax mbar
            Number_of_respiratory_cycles_per_sigh = 0x14,   // phase
            P01_repetition_interval = 0x17, // Interval P0.1 min
            Delay_for_minute_volume_higH = 0x18,    // alarm
            Delay_for_minute_volume_low_alarm = 0x19,   // MVlow delay s
            Air_temperature = 0x1A, // T Air °C
            Skin_temperature = 0x1B,    // T Skin °C
            Relative_humidity = 0x1C,   // Humidity %
            Oxygen_concentration = 0x24,    // O2 %
            Plateau_time_in_percent_of_inspiratory_time = 0x27,   // % Tplat %
            Flow_trigger_threshold = 0x29,  // Trigger  L/min
            Frequency_during_HFO = 0x2A,    // fhf Hz
            Pressure_rise_time = 0x2E,  // Slope s
            Fresh_gas_flow = 0x2F,  // FG flow mL/min
            Flow_assist_during_PPS_ventilation = 0x3C,  // Flow Assist (mbar*s/L)/10
            Volume_assist_during_PPS_ventilation = 0x3D,    // Vol. Assist (mbar/L)/10
            Radiant_warmer_power = 0x3E,    // Heater rad %
            Mattress_temperature = 0x3F,    // T Matt °C
            Inspiratory_tidal_volume_mL = 0x40,    // VTi mL
            Respiratory_rate_for_apnea_ventilation = 0x42,  // RRapn 1/min
            Tidal_volume_for_apnea_ventilation = 0x44,  // VTapn L
            Inspiratory_pressure = 0x45,    // Pinsp mbar
            Compensation_rate_during_ATC = 0x46,    // % comp %
            Inner_diameter_of_the_endotracheal_tracheostoma_tube = 0x47,    //tube mm
            Ratio_of_inspiration_to_expiration_IE_percent	 = 0x49,    //
            Age = 0x4A, //
            Weight = 0x4B,  // Wt kg
            Disconnection_alarm_time = 0x4E,    // Tdiscon
            Flow_Acceleration = 0x4F,   // Flow Acc mbar/s
            Volatile_anesthetic_agent_concentration_in_the_fresh = 0x55,    // gas
            Endtidal_volatile_agent_concentration = 0x56,  // etVA kPa
            Isoflurane_concentration_in_the_fresh = 0x5B,   // gas
            Endtidal_isoflurane_concentration = 0x5C,  // etIso kPa
            Desflurane_concentration_in_the_fresh = 0x5D,   // gas
            Endtidal_desflurane_concentration = 0x5E,  // etDes kPa
            Sevoflurane_concentration_in_the_fresh = 0x5F,  // gas
            Endtidal_sevoflurane_concentration = 0x60, // etSev kPa
            Oxygen_concentration_in_the_fresh = 0x61,   // gas
            Minimum_freshgas_flow = 0x62,  // MinFG flow L/min
            Maximal_inspiratory_time_for_flow_cycled_strokes = 0x63,    // Timax s
            Constant_flow_during_O2_therapy = 0x65, // Const flow L/min
            Mean_airway_pressure_during_HFO = 0x66, // MAPhf mbar
            Inspiratory_part_of_IE_ratio_during_HFO = 0x67,    // I (I:Ehf) no unit
            Expiratory_part_of_IE_ratio_during_HFO = 0x68, // E (I:Ehf) no unit
            Pressure_amplitude_during_HFO = 0x69,   // Ampl hf mbar
            Tidal_volume_during_HFO = 0x6A, // VThf mL
            Respiratory_rate_of_sighs_during_HFO = 0x6B,    // Rrsigh
            Inspiratory_time_of_sighs_during_HFO = 0x6C,    // Tisigh
            Inspiratory_pressure_of_sighs_during_HFO = 0x6D,    // Psigh
            Tidal_volume_during_apnea_ventilation = 0x6E,   // VTapn mL
            Ideal_body_weight = 0x6F,   // IBW kg
            Inspiratory_termination_criterion_based_on_peak_inspiratory_flow = 0x72,    // Insp. term. %PIF
            Expiratory_termination_criterion_during_APRV_AutoRelease = 0x73,    // Exp. term. %PEF
            Ideal_body_weight_in_SmartCare = 0x74,  // SC-Weight  kg
            Start_of_night_rest_in_SmartCare = 0x75,    // SC-start of rest h
            End_of_night_rest_in_SmartCare = 0x76,  // SC-end of rest h
            Pressure_rise_time_of_sighs_during_HFO = 0x78,  // Slopesigh
            FiO2_in_SmartCare_upper_limit = 0x79, // SC-FiO2 high
            PEEP_in_SmartCare_upper_limit = 0x7A, // SC-PEEPmax mbar
            Pressure_variability = 0x7B,    // Press. var. %
            Maximum_time_of_low_pressure_level_during_APRV_AutoRelease = 0x7D,  // Tlow max s
            Spontaneous_respiratory_rate_in_SmartCare_lower_limit = 0x7E, // SC-RRspon low 1/min
            Spontaneous_respiratory_rate_in_SmartCare_upper_limit = 0x7F, // SC-RRspon high
            Tidal_volume_in_SmartCare_lower_limit = 0x80, // SC-VT low mL/kg
            Endtidal_CO2_in_SmartCare_upper_limit = 0x81,    // SC-etCO2higH
            Patient_height_in_SmartCare = 0x82, // SC-Height cm
            Patient_height = 0x83,  // Ht cm
            Maximal_peak_to_peak_pressure_during_HFO = 0x84,    // Ampl hf max mbar
            Volume_limitation = 0x85,   // VTmax mL
            Manual_inspiratory_pressure_for_Neonates_during_NIV = 0x86, // PmanInsp mbar
            Maximum_time_of_pressure_for_manual_inspiration_pressure_for_Neonates_during_NIV = 0x87,    // TmanInsp s
            Maximum_airway_pressure_in_Smart_Ventilation_Control = 0x88,    // SVC-Pmax mbar
            Airway_access = 0x8A, // 0: Tube, 1: Laryngeal Mask
            Lung_mechanics_in_Smart_Ventilation_Control = 0x8B, // 0: Normal, 1: Restrictive, 2: Obstructive
            Ventilation_goal_of_Smart_Ventilation_Control = 0x8C, // 0: Controlled Ventilation, 1: Augmented Ventilation, 2: Encourage SB, 3: Recovery

        };

        public enum MedibusXMeasurementCP1 : byte
        {
            Accumulated_air_consumption_of_the_device_per_case = 0x00,  // Air cons  L
            Accumulated_N2O_consumption_of_the_device_per_case = 0x01,  // N2O Cons  L
            Expiratory_minute_volume = 0x03,    // MVe L/min
            Accumulated_O2_consumption_of_the_device_per_case = 0x04,   // O2 cons  L
            Airway_pressure = 0x05, // Paw mbar
            Inspiratory_minute_volume = 0x06,   // MVi L/min Carina
            Dynamic_compliance = 0x07,  // Cdyn L/bar
            Resistance = 0x08,  // R mbar/(L/s)
            CO2_production = 0x09,  // V'CO2 mL/min
            Spontaneous_expiratory_minute_volume = 0x0A,    // MVespon L/min
            Patient_airway_resistance = 0x0B,   // Rpat mbar/(L/s)
            Mandatory_expiratory_minute_volume = 0x0D,  // MVemand L/min
            Increase_of_measured_CO2_value_in_phase_III_of_capnogram = 0x0E,    // CO2 slope mmHg/L
            O2_central_gas_supply_pressure = 0x0F,  // O2-CGS mbar
            Increase_of_measured_CO2_value_in_phase_III_of_capnogram_KPa_L = 0x10,    // CO2 slope kPa/L
            Increase_of_measured_CO2_value_in_phase_III_of_capnogram_Vol_Percent_L = 0x11,    // CO2 slope Vol%/L
            Average_device_flow = 0x12, // Flow device L/min
            Mandatory_tidal_volume = 0x16,  // VTmand mL
            ST_deviation_in_lead_I = 0x19,  // STI  mm
            ST_deviation_in_lead_II = 0x1A, // STII mm
            Accumulated_halothane_consumption_of_device = 0x1B, // Hal cons mL
            Accumulated_enflurane_consumption_of_device = 0x1C, // Enf cons mL
            Accumulated_isoflurane_consumption_of_device = 0x1D,    // Iso cons mL
            Accumulated_desflurane_consumption_of_device = 0x1E,    // Des cons mL
            Accumulated_sevoflurane_consumption_of_device = 0x1F,   // Sev cons mL
            Cardiac_index = 0x21,   // CI L/min/m^2
            Stroke_volume = 0x22,   // SV mL
            Stroke_index = 0x23,    // SVI mL/m^2
            Left_ventricular_stroke_work_index = 0x24,  // LVSWI g*m/m^2
            Right_ventricular_stroke_work_index = 0x25, // RVSWI g*m/m^2
            Systemic_vascular_resistance = 0x26,    // SVR dyn*s/cm5
            Accumulated_gas_consumption_of_the_device = 0x27,   // Gas cons L
            Therapy_case_duration = 0x28,   // Tcase min
            Correlation_factor = 0x2A,  // r2 no unit
            Left_ventricular_pressure_diastolic = 0x2B,   // LV D mmHg
            Left_ventricular_pressure_systolic = 0x2C,    // LV S mmHg
            Left_ventricular_pressure_mean = 0x2D,    // LV M mmHg
            Right_ventricular_pressure_diastolic = 0x2E,  // RV D mmHg
            Right_ventricular_pressure_systolic = 0x2F,   // RV S mmHg
            Right_ventricular_pressure_mean = 0x30,   // RV M mmHg
            General_diastolic_pressure_channel_1	 = 0x31,    // GP1 D mmHg
            General_systolic_pressure_channel_1	 = 0x32,    // GP1 S mmHg
            General_mean_pressure_channel_1	 = 0x33,    // GP1 M mmHg
            General_diastolic_pressure_channel_2	 = 0x34,    // GP2 D mmHg
            General_systolic_pressure_channel_2	 = 0x35,    // GP2 S mmHg
            General_mean_pressure_channel_2	 = 0x36,    // GP2 M mmHg
            Intracranial_pressure = 0x37,   // ICP mmHg
            Cerebral_perfusion_pressure = 0x38, // CPP mmHg
            Left_atrial_pressure = 0x39,    // LA mmHg
            Mean_dynamic_compliance = 0x3C, // Cdyn mean L/bar
            Inspiratory_halothane_concentration = 0x50, // inHal kPa
            Endtidal_halothane_concentration = 0x51,   // etHal kPa
            Inspiratory_enflurane_concentration = 0x52, // inEnf kPa
            Endtidal_enflurane_concentration = 0x53,   // etEnf kPa
            Inspiratory_isoflurane_concentration = 0x54,    // inIso kPa
            Endtidal_isoflurane_concentration = 0x55,  // etIso kPa
            Inspiratory_desflurane_concentration = 0x56,    // inDes kPa
            Endtidal_desflurane_concentration = 0x57,  // etDes kPa
            Inspiratory_sevoflurane_concentration = 0x58,   // inSev kPa
            Endtidal_sevoflurane_concentration = 0x59, // etSev kPa
            Inspiratory_concentration_of_primary_agent = 0x5A,  // kPa
            Endtidal_concentration_of_primary_agent = 0x5B,    // kPa
            Inspiratory_concentration_of_secondary_agent = 0x5C,    // kPa
            Endtidal_concentration_of_secondary_agent = 0x5D,  // kPa
            ST_deviation_in_lead_III = 0x5E,    // STIII mm
            ST_deviation_in_lead_aVL = 0x5F,    // STaVL mm
            ST_deviation_in_lead_aVF = 0x60,    // STaVF mm
            ST_deviation_in_lead_aVR = 0x61,    // STaVR mm
            ST_deviation_in_lead_V = 0x62,  // STV mm
            ST_deviation_in_lead_V_plus	 = 0x63,    // STV+ mm
            O2_uptake = 0x64,   // V'O2 10 mL/min
            Peripheral_skin_temperature = 0x65, // T Peri °C
            Central_skin_temperature = 0x66,    // T Centr °C
            Spontaneous_inspiratory_tidal_volume = 0x68,    // VTispon mL
            Plateau_time = 0x69,    // Tplat s
            Mattress_temperature = 0x6A,    // T Matt °C
            Ambient_pressure = 0x6B,    // Pamb mbar
            Relative_humidity = 0x6C,   // Humidity %
            Air_temperature = 0x6D, // T Air °C
            Radiant_warmer_power = 0x6E,    // Heater rad %
            Spontaneous_inspiratory_time = 0x6F,    // Tispon s
            Minimum_pressure = 0x71,    // Pmin mbar
            Occlusion_pressure_in_P01_maneuver = 0x72, // P0.1 mbar
            Mean_airway_pressure = 0x73,    // Pmean mbar
            Plateau_pressure = 0x74,    // Pplat mbar
            Inspiratory_peak_flow = 0x76,   // Flowipeak mL/s
            Positive_endexpiratory_pressure = 0x78,    // PEEP mbar
            Intrinsic_PEEP = 0x79,  // PEEPi mbar
            Mandatory_respiratory_rate = 0x7B,  // RRmand 1/min
            Mandatory_minute_volume = 0x7C, // MVmand L/min
            Peak_inspiratory_pressure = 0x7D,   // PIP mbar
            Mandatory_tidal_volume_L = 0x7E,  // VTmand L
            Spontaneous_tidal_volume = 0x7F,    // VTspon L
            Volume_trapped_in_the_lung_by_intrinsic_PEEP = 0x81,    // Vtrap mL
            Mandatory_expiratory_tidal_volume = 0x83,   // VTemand mL
            Spontaneous_expiratory_tidal_volume = 0x84, // VTespon mL
            Mandatory_inspiratory_tidal_volume = 0x85,  // VTimand mL
            Tidal_volume_during_HFO = 0x86, // VThf mL
            Tidal_volume = 0x88,    // VT mL
            Serial_dead_space_volume = 0x89,    // Vds mL
            Ratio_of_serial_dead_space_volume_to_expiratory_tidal_volume = 0x8A,    // Vds/VTe %
            Inspiratory_tidal_volume = 0x8B,    // VTi mL
            Noninvasive_blood_pressure_diastolic = 0x8C, // NIBP D mmHg
            Negative_inspiratory_force = 0x8D,  // NIF mbar
            Noninvasive_blood_pressure_mean = 0x91,  // NIBP M mmHg
            Time_since_last_noninvasive_blood_pressure_measurement = 0x92, // Tnibp min
            Noninvasive_blood_pressure_systolic = 0x96,  // NIBP S mmHg
            Cardiac_output = 0x9B,  // C.O. L/min 01.01
            Total_halothane_uptake = 0x9C,  // Hal tot mL
            Total_desflurane_uptake = 0x9D, // Des tot mL
            Total_isoflurane_uptake = 0x9E, // Iso tot mL
            Total_sevoflurane_uptake = 0x9F,    // Sev tot mL
            Total_enflurane_uptake = 0xA0,  // Enf tot mL
            Pulmonary_arterial_pressure_diastolic = 0xA1, // PA D mmHg
            Duration_of_phototherapy = 0xA2,    // Dur PT h:min
            Irradiance_during_phototherapy = 0xA3,  // Irradiance PT µW/cm2/nm
            Central_venous_pressure = 0xA5, // CVP mmHg
            Pulmonary_arterial_pressure_mean = 0xA6,  // PA M mmHg
            Right_atrial_pressure = 0xA7,   // RA mmHg
            Pulmonary_arterial_pressure_systolic = 0xAB,  // PA S mmHg
            MAC_multiple_derived_from_inspiratory_concentrations = 0xAC,    // inxMAC no unit
            MAC_multiple_derived_from_expiratory_concentrations = 0xAD, // xMAC no unit
            Inspiratory_desflurane_concentration_percent = 0xAE,    // inDes %
            Endtidal_desflurane_concentration_percent = 0xAF,  // etDes %
            Inspiratory_sevoflurane_concentration_percent = 0xB0,   // inSev %
            Endtidal_sevoflurane_concentration_percent = 0xB1, // etSev %
            Leakage_of_the_breathing_system_mL_min = 0xB2,  //
            Leakage_minute_volume_in_percent_of_inspiratory_minute_volume = 0xB3, // % leak %
            Respiratory_rate_based_on_pressure_measurement = 0xB4,  // RRp 1/min
            Spontaneous_respiratory_rate = 0xB5,    // RRspon 1/min
            Spontaneous_breathing_portion_of_minute_volume = 0xB6,  // % MVspon %
            Spontaneous_minute_volume = 0xB7,   // MVspon L/min
            Minute_volume = 0xB9,   // MV L/min
            Apnea_duration = 0xBD,  // s
            Temperature_2 = 0xBE,   // T2 °C
            Oxygen_concentration = 0xC0,    // O2 %
            Airway_temperature = 0xC1,  // T Airw °C
            Skin_temperature_difference = 0xC2, // ?T Skin °C
            Temperature_1 = 0xC3,   // T1 °C
            Inspiratory_expiratory_oxygen_concentration_difference = 0xC4,  // ?O2 %
            Pulmonary_wedge_pressure = 0xC5,    // PWP mmHg
            Battery_capacity = 0xC6,    // %
            Insp_O2_accuracy = 0xC7,   // O2bias %
            Arterial_diastolic_pressure = 0xC8, // ART D mmHg
            Rapid_shallow_breathing_index = 0xC9,   // RSBI 1/min/L
            Arterial_mean_pressure = 0xCD,  // ART M mmHg
            Respiratory_rate_of_triggered = 0xD0,   // mandatory breaths
            Arterial_systolic_pressure = 0xD2,  // ART S mmHg
            Respiratory_rate_based_on_CO2_measurement = 0xD5,   // RRc 1/min
            Respiratory_rate = 0xD6,    // RR 1/min
            Respiratory_rate_based_on_volume_flow_measurement = 0xD7,   // RRf 1/min
            Respiratory_rate_min = 0xD9,    // RR 1/min
            Inspiratory_CO2_concentration = 0xDA,   // inCO2 %
            Endtidal_CO2_concentration = 0xDB, // etCO2 %
            Heart_rate = 0xDC,  // HR 1/min
            N2O_freshgas_flow = 0xDD,  // FG V'N2O mL/min
            Air_freshgas_flow = 0xDE,  // FG V'Air mL/min
            Pulse_Rate = 0xE1,  // PLSm1/min
            Oxygen_freshgas_flow = 0xE2,   // FG V'O2 mL/min
            Endtidal_CO2_concentration_kPa = 0xE3, // etCO2 kPa
            Light_level = 0xE4, // Light lux
            Inspiratory_CO2_concentration_mmHg = 0xE5,   // inCO2 mmHg
            Endtidal_CO2_concentration_mmHg = 0xE6, // etCO2 mmHg
            Measured_IE_I_part = 0xE7,   // no unit
            Measured_IE_E_part = 0xE8,   // no unit
            Inspiratory_concentration_of_primary_agent_percent = 0xE9,  // %
            Endtidal_concentration_of_primary_agent_percent = 0xEA,    // %
            Oxygen_saturation_from_pulse_oximetry = 0xEB,   // SpO2 %
            Noise_level = 0xEC, // Noise dB(A)
            Inspiratory_concentration_of_secondary_agent_percent = 0xED,    // %
            Endtidal_concentration_of_secondary_agent_percent = 0xEE,  // %
            Endtidal_oxygen_concentration_percent = 0xEF,  // etO2 %
            Inspiratory_oxygen_fraction = 0xF0, // FiO2 %
            Convective_heater_power = 0xF1, // Heater conv %
            Body_weight = 0xF2, // BW g
            Weight_date = 0xF3, // DDMM
            Inspiratory_halothane_concentration_percent = 0xF4, // inHal %
            Endtidal_halothane_concentration_percent = 0xF5,   // etHal %
            Inspiratory_enflurane_concentration_percent = 0xF6, // inEnf %
            Endtidal_enflurane_concentration_percent = 0xF7,   // etEnf %
            Inspiratory_isoflurane_concentration_percent = 0xF8,    // inIso %
            Endtidal_isoflurane_concentration_percent = 0xF9,  // etIso %
            Weight_sample_age = 0xFA,   // h
            Inspiratory_N2O_concentration = 0xFB,   // inN2O %
            Endtidal_N2O_concentration_percent = 0xFC, // etN2O %
            Inspiratory_CO2_concentration_kPa = 0xFF,	// inCO2 kPa

        };

        public enum MedibusXMeasurementCP2 : byte
        {
            Spontaneous_tidal_volume = 0x00,    // VTspon mL
            Pressure_amplitude_during_HFO = 0x03,   // Phf mbar
            Inspiratory_part_of_IE_ratio_during_spontaneous_breathing = 0x04,   // I I:Espon  no unit
            Expiratory_part_of_IE_ratio_during_spontaneous_breathing = 0x05,    // E  I:Espon  no unit
            Elastance = 0x06,   // E mbar/L
            Time_constant = 0x07,   // TC s
            Expiratory_time_constant = 0x09,    // TCe s
            Static_compliance = 0x0A,   // Cstat mL/mbar
            Ratio_of_compliance_during_the_last_20_percent_of_inspiration_over_dynamic_compliance = 0x0B,   // C20/Cdyn no unit
            CO2_elimination_coefficient_during_HFO = 0x0C,  // DCO2 10*mL^2/s
            Rapid_shallow_breathing_index = 0x0D,   // RSBI 1/min/mL
            Trainoffour_ratio = 0x0E,   // TOF Ratio %
            Posttetanic_count = 0x0F,   // PTC no unit
            NMT_temperature = 0x10, // T NMT °C
            Single_twitch_mode = 0x11,  // Single %
            Trainoffour_count = 0x12,   // TOF Cnt no unit
            Bispectral_index = 0x13,    // BIS no unit
            Electromyogram_power = 0x14,    // EMG dB
            Signal_quality_index = 0x15,    // SQI %
            Burst_suppression_ratio_BIS = 0x16, // BSR %
            Spectral_edge_frequency_95 = 0x17,  // SEF95 Hz
            Burst_count__BIS = 0x18,    // BCT no unit
            Total_power__BIS = 0x19,    // EEG Pwr dB 
            Goal_for_pressure_support_in_SmartCare = 0x1A,  // SC-Psupp goal mbar
            Rated_pressure_support_by_internal_controller_in_SmartCare = 0x1B,  // SC-Psupp rated mbar
            Duration_of_session__hours__in_SmartCare = 0x1C,    // SC-duration h
            Duration_of_session__minutes__in_SmartCare = 0x1D,  // SC-duration min
            Spontaneous_respiratory_rate_in_SmartCare = 0x1E,   // SC-RRspon 1/min
            Tidal_volume_in_SmartCare = 0x1F,   // SC-VT mL
            Expiratory_tidal_volume = 0x21, // VTe mL
            Inspiratory_tidal_volume = 0x22,    // VTi mL
            Endinspiratory_pressure = 0x23, // EIP mbar
            Effective_expiratory_time_during_APRVAutoRelease = 0x24,    // Tlow s
            CO2_production_volume_per_breath = 0x25,    // VTCO2 mL
            Endtidal_CO2_concentration_in_SmartCare = 0x28, // SC-etCO2 mmHg
            ST_deviation_in_lead_I = 0x29,  // STI mm
            ST_deviation_in_lead_II = 0x2A, // STII mm
            ST_deviation_in_lead_III = 0x2B,    // STIII mm
            ST_deviation_in_lead_aVL = 0x2C,    // STaVL mm
            ST_deviation_in_lead_aVF = 0x2D,    // STaVF mm
            ST_deviation_in_lead_aVR = 0x2E,    // STaVR mm
            ST_deviation_in_lead_V = 0x2F,  // STV mm
            ST_deviation_in_lead_V_plus	 = 0x30,    // STV+ mm
            Upper_pressure_level_during_APRV = 0x31,    // Phigh mbar
            Lower_pressure_level_during_APRV = 0x32,    // Plow mbar
            Spontaneous_mean_tidal_volume = 0x33,   // VTspon mean mL
            Inspiratory_spontaneous_mean_Tidal_volume_ = 0x34,  //VTispon mean mL
            Expiratory_spontaneous_mean_Tidal_Volume = 0x35,    // VTespon mean mL
            Tidal_volume_per_kg_ideal_body_weight = 0x36,   // VT/IBW mL/kg
            Leakage_minute_volume = 0x37,   // MVleak L/min
            Inspiratory_time = 0x38,    // Ti s
            Leakage_of_breathing_system = 0x39, // bag and hoses in Man/Spont mL/min
            Compliance_of_the_breathing_system_including_patient_circuit = 0x3A,    // Cbs+hose mL/hPa
            Compliance_of_the_breathing_hoses = 0x3B,   // Chose mL/hPa
            Adaption_Time = 0x3C,   // Tadapt min
            Applied_upper_limit_for_the_VT_target_range_in_Smart_Ventilation_Control = 0x3D,    // SVC-VT high mL/kg
            Tidal_volume_calculated_by_Smart_Ventilation_Control = 0x3E,    // SVC-VT mL/kg
            Applied_lower_limit_for_the_VT_target_range_in_Smart_Ventilation_Control = 0x3F,    // SVC-VT low mL/kg
            Applied_upper_limit_for_the_etCO2_target_range_in_Smart_Ventilation_Control = 0x40, // SVC-etCO2 high mmHg
            Endtidal_carbon_dioxide_concentration_calculated_by_Smart_Ventilation_Control = 0x41,   // SVC-etCO2 mmHg
            Applied_lower_limit_for_the_etCO2_target_range_in_Smart_Ventilation_Control = 0x42, // SVC-etCO2 low mmHg
            Total_respiratory_rate_in_Smart_Ventilation_Control = 0x43, // SVCRRTotal 1/min
            Spontaneous_respiratory_rate_set_by_Smart_Ventilation_Control = 0x44,   // SVC-RRspon 1/min
            Relative_pressure_support_above_PEEP_set_by_Smart_Ventilation_Control = 0x45,   // SVC-Psupp mbar
            Inspiratory_pressure_set_by_Smart_Ventilation_Control = 0x46,   // SVCPInsp mbar
            Respiratory_rate_set_by_Smart_Ventilation_Control = 0x47,   // SVC-RR 1/min
            Minimum_respiratory_rate_set_by_Smart_Ventilation_Control = 0x48,   // SVC-RRmin 1/min
            Inspiratory_time_set_by_Smart_Ventilation_Control = 0x49,   // SVC-TI s
            Flow_trigger_set_by_Smart_Ventilation_Control = 0x4A,   // SVC-Trigger L/min
            currently_active_ventilation_goal_of_Smart_Ventilation_Control = 0x4B,  // 0: Controlled Ventilation  1: Augmented Ventilation 2: Encourage SB  3: Prep. Extubation
            Current_state_of_Smart_Ventilation_Control = 0x4C,  // SVC state 0:Off 1: On 2: Suspended
            VT_classification_by_Smart_Ventilation_Control = 0x4D,  // SVC VT class 0: Unknown 1: very low 2: low 3: normal 4: high 5: very high
            EtCO2_classification_by_Smart_Ventilation_Control = 0x4E,   // SVC etCO2 class 0: Unknown 1: severe hypoventilated 2: mild hypoventilated 3: normoventilated 4: mild hyperventilated 5: severe hyperventilated
            Therapy_phase_of_Smart_Ventilation_Control = 0x4F,  // SVC phase 0: Adaptation 1: Ventilation Management 2: Recovery
            Ventilation_mode_used_by_Smart_Ventilation_Control = 0x50,  // SVC mode 0: PC-BIPAP/PS 1: SPN-CPAP/ PS
            Spontaneous_breathing_detection_of_Smart_Ventilation_Control = 0x51,    //SVC spont. breathing 0: No 1: Yes
            Resistance_of_the_breathing_hoses = 0x52,   // Rhose mbar/ L/s 
            Difference_between_inspired_and_expired_tidal_volume = 0x53,	// VT mL

        };

        public enum MedibusXAlarmsCP1 : byte
        {
            Apnea_by_different_or_undefined_sources = 0x00, // APNEA
            No_SpO2_pulse = 0x01,   // NO SPO2 PULS
            SpO2_pulse_low_limit = 0x02,    // SPO2 PULS LO
            General_systolic_pressure_channel_1_high_limit = 0x03,  // SYS GP1 HIGH
            General_systolic_pressure_channel_1_low_limit = 0x04,   // SYS GP1 LOW
            Heart_rate_pulse_low_limit = 0x05,  // HRT RATE LOW
            Pulmonary_arterial_pressure_systolic_high_limit = 0x06, // SYS PA HI
            SpO2_low_limit = 0x07,  // SPO2 LOW
            Inspiratory_oxygen_concentration_low_limit = 0x08,  //  O2 LOW
            Inspiratory_halothane_concentration_high_limit = 0x09,  //  HAL HIGH
            Pulmonary_arterial_pressure_diastolic_high_limit = 0x0A,    // DIAS PA HI
            Inspiratory_enflurane_concentration_high_limit = 0x0B,  //  ENF HIGH
            Inspiratory_isoflurane_high_limit = 0x0C,   //  ISO HIGH
            Apnea_detected_by_CO2_monitoring = 0x0D,    // APNEA CO2
            Apnea_detected_by_volume_monitoring = 0x0E, // APNEA VOL
            Apnea_detected_by_pressure_monitoring = 0x0F,   // APNEA PRES
            Airway_pressure_high_limit = 0x10,  // PAW HIGH
            Check_freshgas_setting = 0x11,  // FRESH GAS
            Check_air_supply = 0x12,    // AIR SUPPLY
            Check_O2_supply = 0x13, // O2 SUPPLY
            Pulmonary_arterial_pressure_mean_high_limit = 0x14, // MEAN PA HI
            Pulmonary_arterial_pressure_systolic_low_limit = 0x15,  // SYS PA LOW
            Pulmonary_arterial_pressure_diastolic_low_limit = 0x16, // DIAS PA LO
            Pulmonary_arterial_pressure_mean_low_limit = 0x17,  // MEAN PA LO
            Expiratory_pressure_high_limit = 0x18,  // PRESS EXP HI
            Minute_volume_low_limit = 0x19, // MIN VOL LOW
            NIBP_systolic_low_limit = 0x1A, // NBP SYS LOW
            NIBP_diastolic_low_limit_NBP_DIA_LOW = 0x1B,    //
            Arterial_systolic_pressure_low_limit = 0x1C,    // SYS ART LOW
            General_diastolic_pressure_channel_1_high_limit = 0x1D, // DIAS GP1 HI
            SpO2_pulse_high_limit = 0x1E,   // SPO2 PULS HI
            Inspiratory_sevoflurane_concentration_high_limit = 0x1F,    //  SEV HIGH
            General_diastolic_pressure_channel_1_low_limit = 0x20,  // DIAS GP1
            Heart_rate_high_limit = 0x21,   // HRT RATE HI
            SpO2_high_limit = 0x22, // SPO2 HIGH
            NIBP_systolic_high_limit = 0x23,    // NBP SYS HI
            Inspiratory_desflurane_concentration_high_limit = 0x24, //  DES HIGH
            Arterial_systolic_pressure_high_limit = 0x25,   // SYS ART HI
            General_mean_pressure_channel_1_high_limit = 0x26,  // MEAN GP1 HI
            Endtidal_CO2_low_limit = 0x27,  // ET CO2 LOW
            Endtidal_CO2_high_limit = 0x28, // ET CO2 HIGH
            Inspiratory_halothane_concentration_low_limit = 0x29,   //  HAL LOW
            Inspiratory_enflurane_concentration_low_limit = 0x2A,   //  ENF LOW
            Inspiratory_isoflurane_low_limit = 0x2B,    //  ISO LOW
            Inspiratory_desflurane_low_limit = 0x2C,    //  DES LOW
            NIBP_diastolic_high_limit = 0x2D,   // NBP DIA HI
            Temperature_1_low_limit = 0x2E, // TEMP1 LOW
            Temperature_2_low_limit = 0x2F, // TEMP2 LOW
            Air_temperature_high = 0x30,    // AIR TEMP HI
            General_mean_pressure_channel_1_low_limit = 0x31,   // MEAN GP1 LOW
            Inspiratory_sevoflurane_low_limit = 0x32,   //  SEV LOW
            Volume_not_constant = 0x33, // VOL INCONST
            SpO2_sensor_disconnected_or_fault = 0x35,   //SPO2SEN DISC
            CO2_Filter_check_failed = 0x36, // CO2 FIL. ERR
            Inspiratory_oxygen_concentration_high_limit = 0x37, // O2 HIGH
            General_systolic_pressure_channel_2_high_limit = 0x38,  // SYS GP2 HIGH
            General_systolic_pressure_channel_2_low_limit = 0x39,   // SYS GP2 LOW
            Pressure_support_terminated_by_time = 0x3A, // PS LIMITED
            Oxygen_analyzer_not_calibrated = 0x3B,  // CAL O2 
            Inspiratory_CO2_high_limit = 0x3C,  // INSP CO2 HI
            CO2_patient_sensor_line_blocked = 0x3D, // CO2 LINE BLK
            CO2_zerocalibration_requested = 0x3E,   // CO2 ZERO CAL
            NIBP_mean_low_limit = 0x3F, // NBP MEAN LO
            General_diastolic_pressure_channel_2_high_limit = 0x40, // DIAS GP2 HI
            NIBP_mean_high_limit = 0x41,    // NBP MEAN HI
            Check_flow_sensor = 0x42,   // FLOW SENSOR
            O2_sensor_inoperable = 0x43,    // O2 SENS ERR
            General_diastolic_pressure_channel_2_low_limit = 0x44,  // DIAS GP2 LOW
            General_mean_pressure_channel_2_high_limit = 0x45,  // MEAN GP2 HI
            Temperature_1_failure = 0x46,   // TEMP1 ERR
            Temperature_2_failure = 0x47,   // TEMP2 ERR
            Air_temperature_measurement_failure = 0x48, // AIR TEMP ERR
            Check_NIBP_cuff_size = 0x49,    // BP CUFF ERR
            Unable_to_take_NiBP_measurement = 0x4A, // BP CUFF DISC
            Battery_capacity_low = 0x4B,    // BATTERY LOW
            General_mean_pressure_channel_2_low_limit = 0x4C,   // MEAN GP2 LOW
            Left_ventricular_pressure_systolic_high_limit = 0x4D,   // SYS LV HIGH 
            Left_ventricular_pressure_systolic_low_limit = 0x4E,    // SYS LV LOW 
            LL_electrode_failure = 0x4F,    // ECG ELEC. LL 
            LA_electrode_failure = 0x50,    // ECG ELEC. LA 
            RA_electrode_failure = 0x51,    // ECG ELEC. RA
            Left_ventricular_pressure_diastolic_high_limit = 0x52,  // DIAS LV HIGH
            ECG_ST_analysis_impossible = 0x53,  // NO S-T 
            Central_venous_pressure_low_limit = 0x54,   // CVP LO
            Central_venous_pressure_high_limit = 0x55,  // CVP HI
            Left_ventricular_pressure_diastolic_low_limit = 0x56,   // DIAS LV LOW
            CO2_alarm_disabled = 0x57,  // CO2 ALRM OFF
            Cardiovasc_alarms_disabled_alarms_off = 0x5A,  // HEMO ALRM OF
            Pulse_oximeter_alarm_disabled = 0x5B,   // SPO2 ALRM OF
            Left_ventricular_pressure_mean_high_limit = 0x5C,   // MEAN LV HIGH
            Volume_alarm_disabled = 0x5E,   // VOL ALRM OFF
            NIBP_hose_unplugged = 0x60, // NBP STANDBY
            Left_ventricular_pressure_mean_low_limit = 0x61,    // MEAN LV LOW
            Left_atrial_pressure_high_limit = 0x62, // LA HIGH
            Left_atrial_pressure_low_limit = 0x63,  // LA LOW
            Clean_CO2_sensor_window_occluded = 0x64,    // CLEAN CO2
            Speaker_failure = 0x65, // SPEAKER FAIL
            Mixed_agent_detected = 0x66,    // MIXED AGENT
            Multigas_monitor_device_failure = 0x67, // GAS MON ERR
            SpO2_device_failure = 0x68, // SPO2 ERR
            N2Omeasurement_inoperable = 0x69,   // N2O ERR
            CO2_device_failure = 0x6A,  // CO2 ERR
            Air_temperature_low = 0x6B, // AIR TEMP LOW
            NiBP_device_failure = 0x6C, // NBP ERR
            Agentmeasurement_inoperable = 0x6E, // AGT ERR
            Right_ventricular_pressure_systolic_high_limit = 0x6F,  // SYS RV HIGH
            Oxygen_concentration_low = 0x70,    // OXYGEN LOW
            Right_ventricular_pressure_systolic_low_limit = 0x71,   // SYS RV LOW
            Right_ventricular_pressure_diastolic_high_limit = 0x72, // DIAS RV HIGH
            Right_ventricular_pressure_diastolic_low_limit = 0x73,  // DIAS RV LOW
            Right_ventricular_pressure_mean_high_limit = 0x74,  // MEAN RV HIGH
            Right_ventricular_pressure_mean_low_limit = 0x75,   // MEAN RV LOW
            Intracranial_pressure_high_limit = 0x76,    // ICP HIGH
            Intracranial_pressure_low_limit = 0x77, // ICP LOW
            Communication_error_RS232_port = 0x78,  // RS232COM ERR
            Cerebral_perfusion_pressure_high_limit = 0x79,  // CPP HIGH
            Cerebral_perfusion_pressure_low_limit = 0x7A,   // CPP LOW 
            Hemomed_Pod_1_failure = 0x7B,   // HEMO POD ERR 
            Internal_system_communication_error = 0x7C, // INT COM ERR 
            Arterial_pressure_no_pulse_detected = 0x7D, // NO ART PULS 
            Problems_with_intracranial_pressure_measurement = 0x7F, // ICP ERR 
            Problems_with_left_atrial_pressure_measurement = 0x80,  // LA ERR 
            Problems_with_right_ventricular_pressure_measurement = 0x81,    // RV ERR 
            Problems_with_left_ventricular_pressure_measurement = 0x82, // LV ERR 
            Problems_with_general_blood_pressure_measurement_channel_1 = 0x83,  // GP1 ERR 
            Problems_with_general_blood_pressure_measurement_channel_2 = 0x84,  // GP2 ERR 
            No_pulse_detected_for_right_ventricular_pressure_measurement = 0x85,    // NO RV PULS 
            Arterial_mean_pressure_low_limit = 0x86,    // MEAN ART LO 
            Arterial_mean_pressure_high_limit = 0x87,   // MEAN ART HI 
            Arterial_diastolic_pressure_low_limit = 0x88,   // DIAS ART LO 
            No_pulse_detected_for_left_ventricular_pressure_measurement = 0x89, // NO LV PULS 
            Arterial_diastolic_pressure_high_limit = 0x8A,  // DIAS ART HI 
            No_pulse_detected_for_general_blood_pressure_measurement_channel_1 = 0x8B,  // NO GP1 PULS 
            No_pulse_detected_for_general_blood_pressure_measurement_channel_2 = 0x8C,  // NO GP2 PULS
            Respiratory_rate_high_limit = 0x90, // RESP RATE HI
            ECG_leads_failure = 0x92,   // ECG ELECTROD 
            ST_segment_high_alarm = 0x94,   // S-T HIGH 
            Rescue_ventilation_active = 0x95,   // RESCUE VENT
            Asystole = 0x96,    // ASYSTOLE 
            Apnea_detected_by_respiratory_monitoring = 0x98,    // APNEA RESP
            ST_segment_low_alarm = 0x99,    // S-T LOW 
            Disconnection_ventilator_airway_pressure_low_limit = 0x9A,  // PAW LOW
            Minute_volume_high_limit = 0x9B,    // MIN VOL HIGH
            Water_level_low = 0x9E, // WATER LOW
            Problems_with_respirator = 0x9F,    // VENT ERR
            Ventilator_Communication_lost = 0xA0,   // COM VENT ERR
            Inspiratory_O2_measurement_inoperable = 0xA1,   // O2 ERR
            Flow_calibration_necessary = 0xA2,  // VOL CAL
            Airway_pressure_negative = 0xA3,    // PAW NEGATIVE
            Volume_measurement_inoperable = 0xA4,   // VOL ERR
            Check_water_supply = 0xA6,  // CHECK WATER
            AutoAdapt_active = 0xA7,    // AUTO ADAPT
            Oxygen_concentration_high = 0xA8,   // OXYGEN HIGH
            Leakage_valve_blocked = 0xA9,   // LEAK VALVE
            Convective_heater_failure = 0xAB,   // AIR HTR INOP
            Minute_volume_alarm_disabled = 0xAC,    // MV ALARM OFF
            Pressure_measurement_inoperable = 0xAD, // PRESS ERR
            Continuous_nebulization_active = 0xAE,  // CONT. NEBUL
            Check_watertrap = 0xAF, // WATER TRAP
            Check_expiratory_valve = 0xB0,  // EXP-VALVE
            Hood_open_check_hood = 0xB1,    // CHECK HOOD
            Weaning_temperature_not_reached = 0xB2, // CHECK WEAN
            Gas_calibration_in_progress = 0xB3, // GAS MON CAL
            Heated_mattress_faulty = 0xB4,  // MAT HTR INOP
            Sensor_module_failure = 0xB5,   // SENS MOD ERR
            Warmup_procedure_completed = 0xB6,  // WARM-UP DONE
            Temperature_of_ventilator_blower_too_high = 0xB7,   // VENT TEMP HI
            Airway_temperature_measurement_inop = 0xB8, // AW-TEMP INOP
            Check_airway_temperature_sensor = 0xB9, // AW-TEMP SENS
            Airway_temperature_high_limit = 0xBA,   // AW-TEMP HIGH
            Session_cannot_be_continued = 0xBB, // SCAVNOTADAPT
            Breathing_System_not_locked = 0xBC, // SYSTEM OPEN
            Check_O2_supply_low = 0xBD, // LO O2 SUPPLY
            O2_measurement_inoperable = 0xBE,   //  O2 ERR
            Weaning_procedure_failed = 0xBF,    // WEANING FAIL
            Inspiratory_O2_low_limit = 0xC0,    // O2 LOW
            Flow_measurement_inoperable = 0xC1, // VOL ERR
            Gas_mixing_andor_delivery_failure = 0xC2,   // MIXER INOP
            Pressure_limited_respiratory_volume = 0xC4, // PRESSURE LIM
            Weaning_procedure_completed = 0xC5, // WEANING DONE 
            Fail_to_cycle = 0xC7,   // CYCLE FAILED
            Gas_mixer_inoperable = 0xC8,    // MIXER INOP
            Internal_device_temperature_too_high = 0xC9,    // INT.TMP.HIGH
            Problems_with_fan = 0xCA,   // FAN ERR
            Temperature_1_high_limit = 0xCB,    // TEMP1 HIGH
            Temperature_2_high_limit = 0xCC,    // TEMP2 HIGH
            O2_measurement_temporarily_unavailable = 0xCD,  // O2 UNAVAIL
            N2O_measurement_temporarily_unavailable = 0xCE, // N2O UNAVAIL
            Agent_measurement_temporarily_unavailable = 0xCF,   // AGT UNAVAIL
            Velectrode_failure = 0xD0,  // ECG ELEC. V
            Patient_gas_measurement_overflow = 0xD1,    // GASMON
            VFIB_VTACH_arrhythmia_detected = 0xD2,  // VENTR. FIBR.
            Multiple_gas_measurement_values_inaccurate = 0xD5,  // GASMON.INACC
            Problems_with_arterial_pressure_measurement = 0xD6, // ART ERR
            Problems_with_central_venous_pressure_measurement = 0xD7,   // CVP ERR
            Agent_measurement_values_inaccurate = 0xD8, // AGENT INACC
            CO2_sensor_disconnected_or_fault = 0xD9,    // CO2 SENSOR
            PEEP_high_limit = 0xDA, // PEEP HIGH
            Cardiac_output_device_failure = 0xDD,   // CO ERR
            Cardiac_output_check_injectate_probe = 0xDE,    // INJ. PROBE
            Cardiac_output_transducer_fail = 0xDF,  // CATHETER
            Problems_with_pulmonary_arterial_pressure_measurement = 0xE2,   // PA ERR
            Pulmonary_arterial_pressure_no_pulse_detected = 0xE3,   // NO PA PULS
            N2O_measurement_values_inaccurate = 0xE4,   // N2O INACC
            O2_measurement_values_inaccurate = 0xE5,    // O2 INACC
            Air_supply_pressure_high_limit = 0xE6,  // AIR PRESS HI
            High_O2_supply_pressure = 0xE7, // HI O2 SUPPLY
            Tidal_volume_high_limit = 0xE8, // TIDAL VOL HI
            O2_alarm_disabled = 0xE9,   // O2 ALARM OFF
            CO2_measurement_values_inaccurate = 0xEB,   // CO2 INACC
            Gas_supply_failure = 0xEC,  // GAS FAILURE
            Check_N2O_supply = 0xED,    // N2O SUPPLY
            Two_agents_detected = 0xEE, // 2nd AGENT
            Power_fail = 0xEF,  // POWER FAIL
            Skin_temperature_difference_high = 0xF0,    // DTEMP HIGH
            Check_setting_of_Pmax = 0xF1,   // P MAX
            Internal_system_fault = 0xF2,   // SYSTEM FAULT
            No_fresh_gas = 0xF3,    // NO FRESHGAS
            O2_safety_flow_activated = 0xF4,    // SAFETY O2 ON
            Inspiratory_MAC_high_limit = 0xF5,  // INSP MAC HI
            Skin_temperature_difference_low = 0xF6, // DTEMP LOW
            Inspiratory_CO2_alarm_disabled = 0xF7,  // FICO2 ALARM OFF
            Continuous_high_pressure = 0xF8,    // CONT PRES
            Pressure_threshold_low = 0xFA,  // THRESHOLD LO
            Check_APL_valve = 0xFB, // APL VALVE
            CO2_not_calibrated = 0xFC,  // CO2 NOT CAL
            Battery_inoperable = 0xFD,  // BATTERY ERR
            Check_cooling = 0xFE,   // COOLING
            Disconnection_ventilator = 0xFF,	// DISCONNECT

        };

        public enum MedibusXAlarmsCP2 : byte
        {
            NIBP_did_not_measure = 0x01,    // NBP NO RSLT
            NIBP_max_inflation_press_exceeded = 0x03,   // OVERPRESS
            NIBP_measurement_max_time_exceeded = 0x07,  // NBP TIMEOUT
            NIBP_tube_blocked = 0x0B,   // NBP TUBE
            Humidifier_inoperable = 0x0C,   // HUM INOP
            Humidity_low = 0x0D,    // HUMIDITY LOW
            N2O_supply_pressure_high_limit = 0x10,  // N2O PRESS HI
            Volatile_agent_dosage_error = 0x11, // VA ERR
            Volatile_agent_dosage_error_and_mixer_error = 0x12, // VA MIX ERR
            CPAP_pressure_not_established_due_to_leak_setting_disabled = 0x14,  // CPAP DEACT
            Central_skin_temperature_low_limit = 0x15,  // CTEMP LOW
            Central_skin_temperature_high_limit = 0x16, // CTEMP HIGH
            Central_skin_temperature_failure = 0x17,    // CTEMP ERR
            Peripheral_skin_temperature_low_limit = 0x18,   // PTEMP LOW
            Peripheral_skin_temperature_high_limit = 0x19,  // PTEMP HIGH
            Peripheral_skin_temperature_failure = 0x1A, // PTEMP ERR
            Ambient_temperature_measurement_failure = 0x1B, // AMB TEMP ERR
            O2_cylinder_pressure_low = 0x31,    // O2CYLINDLOW
            O2_cylinder_empty_or_cylinder_pressure = 0x32,  // low O2 CYL
            O2_cylinder_not_connected = 0x33,   // O2CYLDISCON
            N2O_cylinder_empty_or_cylinder_pressure_low = 0x34, // N2O CYL
            N2O_delivery_failure = 0x35,    // NO N2O
            O2_delivery_failure = 0x36, // NO OXYGEN
            AIR_delivery_failure = 0x37,    // NO AIR
            Set_freshgas_flow_not_attained = 0x38,  // FG LIMITED
            Internalexternal_switch_valve_inoperable = 0x39,    // FG EXTERN
            Insp_N2O_high_limit = 0x3A, // N2O HIGH
            Ambient_pressure_measurement_disturbed_or_inoperable = 0x3B,    // AMB PRESS
            Agent_dosage_inoperable = 0x40, // VOLAT ERR
            Agent_reservoir_low = 0x41, // VOLAT SUPPLY
            CO2_sample_gas_line_disconnected = 0x42,    // CO2LINE
            Check_setting_flow = 0x43,  // CHECK FLOW
            Check_setting_FiO2 = 0x44,  // CHECK FIO2
            Highflow_switch_to_fresh_gas_control_mode = 0x45,   // FGFLOW HIGH
            Oxygen_module_inoperable_O2 = 0x50, // MOD INOP
            No_internal_battery_detected = 0x59,    // INTBATTERY
            Power_supply_by_battery = 0x5A, // BATTERY ON
            No_nebuliser = 0x5B,    // NO NEBULIS
            Battery_less_than_2_min_left = 0x5C,    // BATT  2MIN
            Battery_less_than_5_min_left = 0x5D,    // BATT  5MIN
            Apnea_alarm_disabled = 0x66,    // APN ALRM OFF
            Minute_volume_low_alarm_disabled = 0x67,    // MV LOW OFF
            Tidal_volume_high_alarm_disabled = 0x68,    // VT HIGH OFF
            Tube_obstructed = 0x6A, // TUBE OBSTRUC
            Check_microfilter = 0x6C,   // MICROFILTER
            Power_supply_by_external = 0x6D,    // DC EXTERN DC ON
            Ambient_pressure_high = 0x6E,   // AMB PRESS HI
            Ambient_pressure_low = 0x6F,    // AMB PRESS LO
            Right_atrial_pressure_low_limit = 0x74, // RA LO
            Right_atrial_pressure_high_limit = 0x75,    // RA HI
            Problems_with_right_atrial_pressure_measurement = 0x78, // RA ERR
            Mattress_temperature_high = 0x7C,   // MAT TEMP HI
            Check_patient = 0x7F,   // CHK PATIENT
            Breathing_system_disconnected = 0x8D,   // VENT ASSEMBL
            Check_neonatal_flow_sensor = 0x90,  // NEO FLOW
            Loss_of_data = 0x91,    // LOSS OF DATA
            Apnea_ventilation = 0x93,   // APNEA VENT
            Check_ventilator = 0x94,    // CHECK VENT
            Ventilator_standby = 0x95,  // VENT STANDBY
            Problems_with_PEEP_control = 0x96,  // PEEP ERR
            Nebulizer_is_activated = 0x98,  // NEBULIZER ON
            Inspiration_hold_aborted = 0x99,    // INSPHOLD END
            Leakage_detected = 0x9C,    // LEAKAGE
            Backup_ventilation = 0x9D,  // BACKUP VENT
            Expiration_hold_aborted = 0x9E, // EXPHOLD END
            Nebulization_terminated = 0x9F, // NEBULIZ OFF
            Ventilator_not_in_locked_Position = 0xA0,   // VENTUNLOCKD
            Problems_with_power_supply = 0xA1,  // PWR SPLY ERR
            Expiratory_halothan_high_limit = 0xA2,  // EXPHAL HIGH
            Expiratory_enflurane_high_limit = 0xA3, // EXPENF HIGH
            Expiratory_isoflurane_high_limit = 0xA4,    // EXPISO HIGH
            Expiratory_desflurane_high_limit = 0xA5,    // EXPDES HIGH
            Expiratory_sevolurane_high_limit = 0xA6,    // EXPSEV HIGH
            Set_tidal_volume_not_attained = 0xA7,   // TIDAL VOL
            Inspiratory_flow_measurement_inoperable = 0xA8, // INSP VOL ERR
            Setting_could_not_be_performed = 0xA9,  // SETCANCELED
            Confirm_settings = 0xAB,    // CONFIRM SET
            Freshgas_flow_too_high = 0xAC,  // FG TOO HIGH
            Freshgas_flow_active = 0xAD,    // FG ACTIVE
            Air_trapping = 0xAE,    // AIR TRAPPING
            Oxygen_cylinder_open = 0xAF,    // O2 CYL OPEN
            N2O_cylinder_open = 0xB0,   // N2O CYL OPEN
            Air_cylinder_open = 0xB1,   // AIR CYL OPEN
            N2O_cylinder_sensor_not_connected = 0xB2,   // N2OCYLSENS
            Air_cylinder_sensor_not_connected = 0xB3,   // AIRCYLSENS
            O2_cylinder_sensor_not_connected = 0xB4,    // O2 CYLSENS
            Air_cylinder_empty_or_cylinder_pressure_low = 0xB5, // AIR CYL
            Inspiratory_resistance_resistance_of_test = 0xB6,   // INRES HIGH
            Expiratory_resistance_resistance_of_test = 0xB7,    // EXPRESHIGH
            Setting_alarm_limit_or_ventilation_mode_was_changed_but_not_confirmed = 0xB8,  //NO CONFIRM
            Automatic_uptake_control_not_operable = 0xB9,   // PBAG INOP
            Delivered_volume_greater_set_tidal_volume_due_to_minimum_required = 0xBA,   // PIP PMIN REACHED
            Insp_N2O_low_limit = 0xBB,  //  N2O LOW 
            Detected_agent_differs_from_selected_type = 0xBC,   // WRONG AGENT 
            Insp_vol_agent_high_limit = 0xBD,   // INSP AGTHI 
            DIVA_module_I_or_II_is_warming_up = 0xBE,   // DIVA WARMUP 
            Relearning_ECG_pattern = 0xBF,  // ECG RELEARN 
            Unable_to_relearn_ECG_pattern = 0xC0,   // ECG RLRNERR 
            ECG_inoperable = 0xC1,  // ECG INOP 
            RLelectrode_failure_ECG_ELEC = 0xC2,    // RL 
            Vplus_electrode_failure_ECG_ELEC = 0xC3,    // V+ 
            Cardiac_output_blood_temperature_high_limit = 0xC4, // BLOOD TEMP 
            Cardiac_output_blood_temperature_low_limit = 0xC5,  // BLOOD TEMP 
            All_alarms_disabled = 0xC6, // ALL ALRM SUS 
            Air_freshgas_flow_measurement_inoperable = 0xC7,    // FG AIR SENS
            O2_freshgas_flow_measurement_inoperable = 0xC8, // FG O2 SENS
            N2O_freshgas_flow_measurement_inoperable = 0xC9,    // FG N2O SENS
            No_air_supply = 0xCA,   // NO AIR
            No_N2O_supply = 0xCB,   // NO N2O
            PEEP_low_limit = 0xCC,  // PEEP LOW
            Set_inspiratory_pressure_not_attained = 0xCD,   // PINSP
            Hose_kinked = 0xCE, // HOSE KINKED
            Set_tidal_volume_too_high_for_connected_hose = 0xCF,    // VT HIGH HOSE
            Pressure_relief_valve_opened = 0xD0,    // PRESS RELIEF
            Hose_system_defect = 0xD1,  // HOSE ERROR
            Check_MAC_value = 0xD2, // MAC LOW
            Water_trap_expired = 0xD3,  //  WATERTROLD
            CO2_absorber_depleted = 0xD4,   // ABSORB OLD
            CO2_absorber_present = 0xD5,    // ABSPRESENT
            Hoses_interchanged = 0xD6,  // HOSES MIXED
            Hoses_incompatible = 0xD7,  // WRONG HOSES
            Accessory_ID_detection_functions_inoperable = 0xD8, // IDFUNCINOP
            Breathing_circuit_expired = 0xD9,   // HOSE OLD
            Set_expiration_time_not_attainable = 0xDA,  // EXP TIME ERR
            NMT_TOF_high_limit = 0xDB,  // NMT TOFHIGH 
            NMT_TOF_low_limit = 0xDC,   // NMT TOFLOW 
            NMT_initializingNMT_single_out_of_range = 0xDD, // NMT INOP 
            NMT_check_sensors_andor_electrodes = 0xDE,  // NMT SENS 
            NMT_temperature_out_of_range = 0xDF,    // NMT TEMP 
            Bispectral_index_high = 0xE0,   // BISHIGH 
            Bispectral_index_low = 0xE1,    // BISLOW
            Bispectral_calculation_not_possible = 0xE2, // BIS INOP 
            Check_BIS_sensor = 0xE3,    // BIS SENS 
            BISX_out_of_range = 0xE4,   // BIS 
            SmartCare_patient_session_aborted = 0xE5,   // SC ABORTED
            SmartCare_inoperable_patient_session_terminated = 0xE6, // SC INOP
            SmartCare_Central_Hypoventilation = 0xE7,   // CENTRAL HYPO
            SmartCare_Persistent_Tachypnea = 0xE8,  // PERS TACHYP
            SmartCare_Unexplained_Hyperventilation = 0xE9,  // UNEXPL HYPER
            SmartCare_Reduce_PEEP_if_possible = 0xEA,   // PEEP REDUCIB
            SmartCare_SBT_successful = 0xEB,    // CONS SEPARAT
            Ejector_inoperable = 0xEC,  // EJECTOR INOP
            Constant_CO2_offset = 0xED, // CONSTANT CO2
            Autoflow_VT_too_low = 0xEF, // AF VT LOW
            Pilot_lines_switched = 0xF1,    // PILOTLINE SW
            Breathing_hoses_missing = 0xF2, // HOSE MISSING
            SmartCare_Reduce_FiO2_if_possible = 0xF3,   // FIO2 REDUCIB
            Mean_Airway_Pressure_low_limit = 0xF4,  // MAP LOW
            Airway_pressure_high_HFO = 0xF7,    // PAW HIGH HF
            Tidal_volume_low_Limit = 0xF8,  // TIDAL VOL LO
            Plow_high_limit = 0xF9, // PLOW HIGH
            Plow_low_limit = 0xFA,  // PLOW LOW
            Delivered_volume_limited_by_VTmax = 0xFB,   // VT LIMITED
            Air_entrainment = 0xFC, // AIR ENTRAIN
            Ventilation_pause_exceeded = 0xFD,  // VENT PAUSE
            Power_supply_by_internal_battery = 0xFE,    // INT BATT ON
            In_O2_therapy_the_set_flow_cannot_be_reached_due_to_high_resistance = 0xFF,	// FLOW LOW

        };

        public enum MedibusXTextMessages : byte
        {
            Mode_VC_CMV_volume_controlled = 0x01,   // continuous mandatory ventilation Mode VC CMV
            Mode_VC_AC_volume_controlled = 0x02,    // assist controlled ventilation Mode VC AC
            Mode_VC_SIMV_volume_controlled = 0x06,  // synchronized intermittent mandatory ventilation Mode VC SIMV
            Mode_SPN_CPAP_spontaneous_ventilation = 0x0A,   // continuous positive airway pressure Mode SPN CPAP
            Mode_VC_MMV_volume_controlled = 0x0C,   // mandatory minute ventilation Mode VC MMV
            Mode_PC_MMV_pressure_controlled = 0x0D, // mandatory minute ventilation Mode PC MMV
            Mode_PC_BIPAP_pressure_controlled = 0x0E,   // bi phasic positive airway pressure Mode PC BIPAP
            Automatic_apnea_ventilation_active = 0x11,  // APNEA VENTILATION
            Device_in_PRODUCT_TEST_mode = 0x12, // Mode DS
            Air_mode_is_active = 0x13,  // Air Mode
            Skin_mode_is_active = 0x14, // Skin Mode
            Oxygen_control_is_active = 0x15,    // Oxygen Control active
            Humidity_control_is_active = 0x16,  // Humidity Control active
            Mode_PC_SIMV_pressure_controlled = 0x18,    // synchronized intermittent mandatory ventilation Mode PC SIMV
            Mode_PC_APRV_pressure_controlled = 0x1A,    // airway pressure release ventilation Mode PC APRV
            Mode_PC_HFO_pressure_controlled = 0x1B, // high frequency oscillation Mode PC HFO
            Suction_maneuver_active = 0x1C, // Suction maneuver active
            Low_flow_maneuver_active = 0x1D,    // Low flow maneuver active
            Ventilator_is_in_standby_mode = 0x1E,   // Ventilator STANDBY
            Device_is_in_adult_mode = 0x20, // Mode Adults
            Device_is_in_neonatal_mode = 0x21,  // Mode Neonates
            Selected_CO2_unit_is_mmHg = 0x22,   // mmHg
            Selected_CO2_unit_is_kPa = 0x23,    // kPa
            Selected_CO2_unit_is_percent = 0x24,    // %
            Halothane_selected_or_detected = 0x25,  // Anesthesia gas HALOTHANE
            Enflurane_selected_or_detected = 0x26,  // Anesthesia gas ENFLURANE
            Isoflurane_selected_or_detected = 0x27, // Anesthesia gas ISOFLURANE
            Desflurane_selected_or_detected_Anesthesia_gas_DESFLURANE = 0x28,   //
            Sevoflurane_selected_or_detected = 0x29,    // Anesthesia gas SEVOFLURANE
            No_anesthesia_agent_selected_nor_detected = 0x2A,   //
            Ventilation_mode_manspont = 0x2B,   // Mode MANSPONT
            Selected_language = 0x2C,   //
            Mode_SPN_PPS_spontaneous = 0x35,    // proportional pressure support Mode SPN PPS
            entilation_mode_FRESH_GAS_EXTERNAL = 0x36,  //Mode FRES GAS EXT
            Selected_carrier_gas_is_air = 0x37, // Carrier Gas AIR
            Selected_carrier_gas_is_N2O = 0x38, // Carrier Gas N2O
            Device_is_in_pediatric_mode = 0x3A, // Mode Pediatrics
            Manual_mode_is_active = 0x3B,   // Manual Mode
            Open_care_therapy_is_active = 0x3C, // Open Care Therapy
            Photo_therapy_is_active = 0x3D, // Photo Therapy
            Mode_PC_PSV_pressure_controlled = 0x3E, // pressure support ventilation Mode PC PSV
            Patient_is_a_female = 0x42, // female
            Patient_is_a_male = 0x43,   // male
            End_expiratory_control_of_anesthesia_gas_active = 0x45, // End exp controlled Agent
            Mode_PC_AC_pressure_controlled = 0x47,  // assisted controlled Mode PC AC
            Device_configured_for_intubated_patient_ventilation = 0x48, // IV   Invasive Ventilation
            Device_configured_for_mask_ventilation = 0x49, // NIV   Non Invasive Ventilation
            Second_agent_halothane_selected_or_detected = 0x4A, // 2nd Anesthesia gas HALOTHANE
            Second_agent_enflurane_selected_or_detected = 0x4B, // 2nd Anesthesia gas ENFLURANE
            Second_agent_isoflurane_selected_or_detected = 0x4C, // 2nd Anesthesia gas ISOFLURANE
            Second_agent_desflurane_selected_or_detected_2nd_Anesthesia_gas_DESFLURANE = 0x4D,	//
            Second_agent_sevoflurane_selected_or_detected = 0x4E,  // 2nd Anesthesia gas SEVOFLURANE
            No_Second_anesthesia_agent_selected_nor_detected = 0x4F,   // No Second anesthesia gas
            Mattress_temperature_control_is_active = 0x50,  // Mattress Temp Control active
            Kangaroo_mode_is_active = 0x51, // Kangaroo Mode
            Device_is_performing_leakage_test_performing = 0x53,    // LEAKAGE TEST
            Device_is_in_standby_mode = 0x54,   // Mode STANDBY
            Low_pressure_oxygenation_active = 0x55, // LPO   Low Pressure Oxygenation
            Selected_agent_unit_is_kPa = 0x56,  // kPa
            Selected_agent_unit_is_percent = 0x57,  // %
            HLM_mode_active = 0x58, // HLM Mode active
            Pressure_support_added_to_current_ventilation_mode = 0x5C,  // PS
            Audio_pause_active = 0x5D,  // Audio pause active
            AutoFlow_added_to_current_ventilation_mode = 0x5E,  // AF
            Fresh_gas_control_of_anaesthesia_gas_active = 0x5F, // Fresh Gas Control
            First_name_of_patient = 0x60,   //
            Last_name_of_patient = 0x61,    //
            Patient_monitoring_is_in_simulation_mode = 0x62,    // SIMULATION ON
            Ventilator_was_switched_off_by_emergence_switch = 0x63, // "Anesthesia ventilator OFF"
            Trigger_level_normal = 0x64,    // Trigger level normal
            Trigger_level_sensitive = 0x65, // Trigger level sensitive
            Apnea_detection_disabled = 0x67,    // Apnea detection disabled
            Tube_type_endotracheal = 0x68,  // Endotracheal
            Volume_guarantee_added_to_current_ventilation_mode = 0x69,  // VG
            Mode_PC_CMV_pressure_controlled = 0x6A, // continuous mandatory ventilation Mode PC CMV
            Mode_O2_therapy = 0x6B, // Mode O2 Therapy
            Abdominal_external_trigger = 0x6C,  // Abdominal External Trigger
            Automatic_tube_compensation_ATC_added_to_current_ventilation_mode = 0x6D,   // ATC
            Expiratory_automatic_tube_compensation_ATC_enabled = 0x6E,  // Expiratory ATC
            Inspiratory_automatic_tube_compensation_ATC_enabled = 0x6F, // Inspiratory ATC
            Tube_type_tracheostoma = 0x70,  // Tracheostoma
            Apnea_ventilation_enabled = 0x71,   // Apnea Ventilation enabled
            Apnea_ventilation_auto_return_enabled = 0x72,   // Apnea Ventilation Auto Return
            Screen_is_locked = 0x73,    // Screen locked
            Volume_support_added_to_current_ventilation_mode = 0x74,    // VS
            AutoRelease_added_to_current_ventilation_mode = 0x75,   // AutoRelease
            Active_humid_unheated = 0x76,   // Active humid unheated
            Active_humid_heated = 0x77, // Active humid heated
            HME_Filter = 0x78,  // HME  Filter
            SmartCare_therapy_phase_adapting = 0x79,    //
            SmartCare_therapy_phase_observing = 0x7A,   //
            SmartCare_therapy_phase_maintaining = 0x7B, //
            SmartCare_diagnosis_normal_ventilation = 0x7C,  //
            SmartCare_diagnosis_hyperventilation = 0x7D,    //
            SmartCare_diagnosis_hypoventilation = 0x7E, //
            SmartCare_diagnosis_insufficient_ventilation = 0x7F,    //
            SmartCare_diagnosis_tachypnea = 0x80,   //
            SmartCare_diagnosis_severe_tachypnea = 0x81,    //
            SmartCare_diagnosis_unexplained_hyperventilation = 0x82,    //
            SmartCare_diagnosis_central_hypoventilation = 0x83, //
            SmartCare_patient_with_COPD = 0x84, //
            SmartCare_patient_with_neurologic_disorder = 0x85,  //
            SmartCare_night_rest_requested_Night_rest = 0x86,   //
            Sigh_added_to_current_ventilation_mode = 0x87,  //
            Cuvette_type_disposable_disposable_cuvette = 0x88,  //
            Cuvette_type_reusable_reusable_cuvette = 0x89,  //
            Hose_type_disposable_disposable_hose = 0x8A,    //
            Hose_type_reusable = 0x8B,  //
            Hose_size_adult = 0x8C, //
            Hose_size_pediatric = 0x8D, //
            Variable_PS_added_to_current_ventilation_mode = 0x8E,   // VariablePS
            SmartCare_zone_of_respiratory_comfort_Customization_active = 0x8F,  // Customizing
            Overall_device_check_result_fully_functional = 0x90,    //
            Overall_device_check_result_conditionally_functional = 0x91,    //
            Overall_device_check_result_non_functional = 0x92,  //
            Tracheal_pressure_calculation_enabled = 0x9F,   //
            Date_and_time_of_last_self_test_execution = 0xA0,   //
            Date_and_time_of_last_leakage_test_execution = 0xA1,    //
            Warm_up_mode_is_active = 0xA2,  // Warm up Mode
            Weaning_mode_is_active = 0xA3,  // Weaning Mode
            Tolerate_cooling_procedure_is_active = 0xA4,    // Cooling Mode
            Closed_care_therapy_is_active = 0xA5,   // Closed Care Therapy
            Multi_step_recruitment_is_active = 0xA6,    // Multi step recruitment
            One_step_recruitment_is_active = 0xA7,  // One step recruitment
            CPR_ventilation_is_paused = 0xA8,   // CPR paused
            Check_the_calibration_status = 0xA9,	//

        };


      }
    }
