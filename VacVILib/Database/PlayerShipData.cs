﻿using VacVI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VacVI.Database
{
    /// <summary> Contains information about the player ship.</summary>
    public static class PlayerShipData
    {
        #region Constants (Parameter Names)
        const string PARAM_FUEL = "FUEL";
        const string PARAM_ENERGY_LEVEL = "ENERGY LEVEL";
        const string PARAM_ENGINE_DAMAGE = "ENGINE DAMAGE";
        const string PARAM_WEAPON_DAMAGE = "WEAPON DAMAGE";
        const string PARAM_NAV_DAMAGE = "NAV DAMAGE";
        const string PARAM_PARTICLE_CANNON = "PARTICLE CANNON";
        const string PARAM_BEAM_CANNON = "BEAM CANNON";
        const string PARAM_SHIP_TYPE = "SHIP TYPE";
        const string PARAM_ENGINE_CLASS = "ENGINE CLASS";
        const string PARAM_SHIELD_CLASS = "SHIELD CLASS";
        const string PARAM_CARGO_CAPACITY = "CARGO CAPACITY";
        const string PARAM_WING_AND_THRUSTER_CLASS = "WING AND THRUSTER CLASS";
        const string PARAM_CREW_LIMIT = "CREW LIMIT";
        const string PARAM_EQUIPMENT_LIMIT = "EQUIPMENT LIMIT";
        const string PARAM_COUNTERMEASURE_LIMIT = "COUNTERMEASURE LIMIT";
        const string PARAM_HARDPOINT_LIMIT = "HARDPOINT LIMIT";
        const string PARAM_ARMOR_LIMIT = "ARMOR LIMIT";
        const string PARAM_PARTICLE_CANNON_RANGE = "PARTICLE CANNON RANGE";
        const string PARAM_ARMED_MISSILE_RANGE = "ARMED MISSILE RANGE";
        const string PARAM_TARGETED_SUBSYSTEM = "TARGETED SUBSYSTEM";
        const string PARAM_COUNTERMEASURES_REMAINING = "COUNTERMEASURES REMAINING";
        const string PARAM_IDS_MULTIPLIER = "IDS MULTIPLIER";
        const string PARAM_PLAYER_SHIP_VELOCITY = "PLAYER SHIP VELOCITY";
        const string PARAM_PLAYER_SHIP_SET_VELOCITY = "PLAYER SHIP SET VELOCITY";
        const string PARAM_PLAYER_SHIP_ALTITUDE = "PLAYER SHIP ALTITUDE";
        const string PARAM_HEAT_SIGNATURE_LEVEL = "HEAT SIGNATURE LEVEL";
        const string PARAM_PLAYER_POSITION_X = "PLAYER POSITION X";
        const string PARAM_PLAYER_POSITION_Y = "PLAYER POSITION Y";
        const string PARAM_PLAYER_POSITION_Z = "PLAYER POSITION Z";
        const string PARAM_PLAYER_POSITION_SX = "PLAYER POSITION SX";
        const string PARAM_PLAYER_POSITION_SY = "PLAYER POSITION SY";
        const string PARAM_PLAYER_POSITION_SZ = "PLAYER POSITION SZ";
        const string PARAM_FRONT_SHIELD_LEVEL = "FRONT SHIELD LEVEL";
        const string PARAM_RIGHT_SHIELD_LEVEL = "RIGHT SHIELD LEVEL";
        const string PARAM_LEFT_SHIELD_LEVEL = "LEFT SHIELD LEVEL";
        const string PARAM_REAR_SHIELD_LEVEL = "REAR SHIELD LEVEL";
        const string PARAM_SHIELD_LEVEL = "SHIELD LEVEL";
        const string PARAM_ENGINE_THRUSTER_HEAT_INDICATOR = "ENGINE/THRUSTER HEAT INDICATOR";
        const string PARAM_MDTS_STATUS = "MDTS STATUS";
        const string PARAM_MISSILE_LOCK_STATUS = "MISSILE LOCK STATUS";
        const string PARAM_ENERGY_BIAS_SETTING = "ENERGY BIAS SETTING";
        const string PARAM_IDS_STATUS = "IDS STATUS";
        const string PARAM_AFTERBURNER_STATUS = "AFTERBURNER STATUS";
        const string PARAM_AUTOPILOT_STATUS = "AUTOPILOT STATUS";
        const string PARAM_TRACTOR_BEAM_STATUS = "TRACTOR BEAM STATUS";
        const string PARAM_PLAYER_SHIP_TOTAL_VELOCITY_AVL = "PLAYER SHIP TOTAL VELOCITY, AVL";
        const string PARAM_PLAYER_SHIP_HEADING = "PLAYER SHIP HEADING";
        const string PARAM_PLAYER_SHIP_PITCH = "PLAYER SHIP PITCH";

        static readonly string[] PARAM_CARGO_BAY = new string[] { 
            "CARGO BAY 1", "CARGO BAY 2", "CARGO BAY 3", "CARGO BAY 4", "CARGO BAY 5", 
            "CARGO BAY 6", "CARGO BAY 7", "CARGO BAY 8", "CARGO BAY 9", "CARGO BAY 10"
        };
        static readonly string[] PARAM_SECONDARY_WEAPON_SLOT = new string[] { 
            "SECONDARY WEAPON SLOT 1", "SECONDARY WEAPON SLOT 2", "SECONDARY WEAPON SLOT 3", "SECONDARY WEAPON SLOT 4", "SECONDARY WEAPON SLOT 5", 
            "SECONDARY WEAPON SLOT 6", "SECONDARY WEAPON SLOT 7", "SECONDARY WEAPON SLOT 8"
        };
        static readonly string[] PARAM_EQUIPMENT_SLOT = new string[] { 
            "EQUIPMENT SLOT 1", "EQUIPMENT SLOT 2", "EQUIPMENT SLOT 3", "EQUIPMENT SLOT 4", "EQUIPMENT SLOT 5", 
            "EQUIPMENT SLOT 6", "EQUIPMENT SLOT 7", "EQUIPMENT SLOT 8", "EQUIPMENT SLOT 9", "EQUIPMENT SLOT 10"
        };
        #endregion


        #region Enums
        /// <summary> Represents the state of the ship's emitted heat.</summary>
        public enum HeatState { LOW = 0, HIGH = 1 };

        /// <summary> Represents the state of the MTDS (targeting) system.</summary>
        public enum MTDSState { OFF = 0, ON = 1, LOCKED = 2 };

        /// <summary> Represents the missile lock-on state.</summary>
        public enum MissileState { NO_LOCK = 0, LOCKED = 1 };

        /// <summary> Represents the autopilot's state.</summary>
        public enum AutopilotState { OFF = 0, FORM_ON_TARGET = 1, FLY_TO_NAV_POINT = 2 };
        #endregion


        #region Variables - Converted values
        private static int _fuelRemaining = 0;
        private static int _fuelTotal = 0;
        private static double _fuelPercentage = 0;
        private static string[] _cargoBay = new string[10];
        private static Vector3D _position = new Vector3D();
        private static Vector3D _sectorPosition = new Vector3D();
        private static Dictionary<ShieldLevelState, int> _shieldStatus = new Dictionary<ShieldLevelState, int>();
        private static string[] _secWeapon = new string[8];
        private static string[] _equipmentSlot = new string[10];
        private static HeatState _heat;
        private static MTDSState _mtds;
        private static MissileState _missileLock;
        private static int _shieldBias;
        private static int _weaponBias;
        private static OnOffState _ids;
        private static OnOffState _afterburner;
        private static AutopilotState _autopilot;
        private static OnOffState _tractorBeam;
        #endregion


        #region Properties - Converted values
        /// <summary> [EMERC+] Returns the amount of fuel remaining.</summary>
        public static int? FuelRemaining
        {
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)PlayerShipData._fuelRemaining : null; }
        }


        /// <summary> [EMERC+] Returns the total amount of fuel the ship can hold.</summary>
        public static int? FuelTotal
        {
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)PlayerShipData._fuelTotal : null; }
        }


        /// <summary> [EMERC+] Returns the percentage of fuel remaining.</summary>
        public static double? FuelPercentage
        {
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (double?)PlayerShipData._fuelPercentage : null; }
        }


        /// <summary> [EMERC+] Returns the content of the player ship's cargo bay.
        /// <para>The amount of maximum cargo slots differs per game - see <see cref="VacVI.Database.ShipData.MaxCargoSlots"/>.
        /// </para>
        /// </summary>
        public static string[] CargoBay 
		{
			get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? PlayerShipData._cargoBay : null; }
		}


        /// <summary> [EMERC+] Returns the ship's in-sector position as a 3-dimensional vector.</summary>
        public static Vector3D Position 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? PlayerShipData._position : null; }
		}


        /// <summary> [EMERC+] Returns the ship sector position as a 3-dimensional vector.</summary>
        public static Vector3D SectorPosition 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? PlayerShipData._sectorPosition : null; }
		}


        /// <summary> [EMERC+] Returns the ship's shield states (0 to 100).</summary>
        public static Dictionary<ShieldLevelState, int> ShieldStatus
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? PlayerShipData._shieldStatus : null; }
		}


        /// <summary> [EMERC+] Returns an array of currently equipped secondary weapons.</summary>
        public static string[] SecWeapon 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? PlayerShipData._secWeapon : null; }
		}


        /// <summary> [EMERC+] Returns the content of the player ship's equipment slots.
        /// <para>The amount of maximum equipment slots differs per game - see <see cref="VacVI.Database.ShipData.MaxEquipmentSlots"/>.
        /// </para>
        /// </summary>
        public static string[] EquipmentSlot 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? PlayerShipData._equipmentSlot : null; }
		}


        /// <summary> [EMERC+] Returns the ship's current heat state.</summary>
        public static HeatState? Heat 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (HeatState?)PlayerShipData._heat : null; }
		}


        /// <summary> [EMERC+] Returns the MTDS (targeting system) state.</summary>
        public static MTDSState? Mtds 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (MTDSState?)PlayerShipData._mtds : null; }
		}


        /// <summary> [EMERC+] Returns the state of the missile lock.</summary>
        public static MissileState? MissileLock 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (MissileState?)PlayerShipData._missileLock : null; }
		}


        /// <summary> [EMERC+] Returns the power transferred to shields (-5 to +5).</summary>
        public static int? ShieldBias 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)PlayerShipData._shieldBias : null; }
		}


        /// <summary> [EMERC+] Returns the power transferred to the weapon systems (-5 to +5).</summary>
        public static int? WeaponBias 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)PlayerShipData._weaponBias : null; }
		}


        /// <summary> [EMERC+] Returns the IDS (inertia dampening system) state.</summary>
        public static OnOffState? Ids 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (OnOffState?)PlayerShipData._ids : null; }
		}


        /// <summary> [EMERC+] Returns the afterburner's state.</summary>
        public static OnOffState? Afterburner 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (OnOffState?)PlayerShipData._afterburner : null; }
		}


        /// <summary> [EMERC+] Returns the autopilot's state.</summary>
        public static AutopilotState? Autopilot 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (AutopilotState?)PlayerShipData._autopilot : null; }
		}


        /// <summary> [EMERC+] Returns the tractor beam's state.</summary>
        public static OnOffState? TractorBeam 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (OnOffState?)PlayerShipData._tractorBeam : null; }
		}
        #endregion


        #region Properties - Unconverted Values
        /// <summary> [EMERC+] Returns the ship's energy level (0 to 100).</summary>
        public static int? EnergyLevel 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_ENERGY_LEVEL).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's shield level/hull integrity (0 to 100).</summary>
        public static int? ShieldLevel
        {
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_SHIELD_LEVEL).Value : null; }
        }


        /// <summary> [EMERC+] Returns the ship's shield level/hull integrity (0 to 100).
        /// <para>(Same as ShieldLevel)</para>
        /// </summary>
        public static int? HullIntegrity { get { return ShieldLevel; } }


        /// <summary> [EMERC+] Returns the health of the ship's engines (0-100).</summary>
        public static int? EngineHealth 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_ENGINE_DAMAGE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the health of the ship's weapon systems (0-100).</summary>
        public static int? WeaponHealth 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_WEAPON_DAMAGE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the health of the ship's nav-systems (0-100).</summary>
        public static int? NavHealth 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_NAV_DAMAGE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the name of the currently equipped particle cannon.</summary>
        public static string ParticleCannon 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (string)SaveDataReader.GetEntry(PARAM_PARTICLE_CANNON).Value : null; }
		}


        /// <summary> [EMERC+] Returns the name of the currently equipped beam cannon.</summary>
        public static string BeamCannon 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (string)SaveDataReader.GetEntry(PARAM_BEAM_CANNON).Value : null; }
		}


        /// <summary> [EMERC+] Returns the name of the current ship model.</summary>
        public static string ShipType 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (string)SaveDataReader.GetEntry(PARAM_SHIP_TYPE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's engine class.</summary>
        public static int? EngineClass 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_ENGINE_CLASS).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's shield class.</summary>
        public static int? ShieldClass 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_SHIELD_CLASS).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's cargo capacity.</summary>
        public static int? CargoCapacity 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_CARGO_CAPACITY).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's wing class.</summary>
        public static int? WingClass 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_WING_AND_THRUSTER_CLASS).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's thruster class (same as the wing class).</summary>
        public static int? ThrusterClass 
		{
			get { return WingClass; }
		}


        /// <summary> [EMERC+] Returns the ship's crew member limit.</summary>
        public static int? CrewLimit 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_CREW_LIMIT).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's equipment limit.</summary>
        public static int? EquipmentLimit 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_EQUIPMENT_LIMIT).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's equipment limit.</summary>
        public static int? CountermeasureLimit 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_COUNTERMEASURE_LIMIT).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's hardpoint limit.</summary>
        public static int? HardpointLimit 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_HARDPOINT_LIMIT).Value : null; }
		}


        /// <summary> [ELGCY+] Returns the ship's armor limit.</summary>
        public static int? ArmorLimit 
		{
			get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_LEGACY) ? (int?)SaveDataReader.GetEntry(PARAM_ARMOR_LIMIT).Value : null; }
		}


        /// <summary> [EMERC+] Returns the range of the currently equipped particle cannon.</summary>
        public static int? ParticleCannonRange 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_PARTICLE_CANNON_RANGE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the range of the currently equipped missile.</summary>
        public static int? MissileRange 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_ARMED_MISSILE_RANGE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the currently targeted subsystem.</summary>
        public static string TargetedSubsystem 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (string)SaveDataReader.GetEntry(PARAM_TARGETED_SUBSYSTEM).Value : null; }
		}


        /// <summary> [EMERC+] Returns the remaining amount of countermeasures.</summary>
        public static int? CounterMeasures 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_COUNTERMEASURES_REMAINING).Value : null; }
		}


        /// <summary> [EMERC+] Returns the IDS (inertia dampening system) speed multiplier.</summary>
        public static int? IDSMultiplier 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_IDS_MULTIPLIER).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's velocity.</summary>
        public static int? Velocity 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_PLAYER_SHIP_VELOCITY).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's set velocity.</summary>
        public static int? SetVelocity 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_PLAYER_SHIP_SET_VELOCITY).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's altitude.</summary>
        public static int? Altitude 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_PLAYER_SHIP_ALTITUDE).Value : null; }
		}


        /// <summary> [EMERC+] Returns the ship's heat signature level.</summary>
        public static int? HeatSignatureLevel 
		{
            get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_MERCENARY) ? (int?)SaveDataReader.GetEntry(PARAM_HEAT_SIGNATURE_LEVEL).Value : null; }
		}


        /// <summary> [ELGCY+] Returns the ship's total velocity.</summary>
        public static int? TotalVelocity 
		{
			get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_LEGACY) ? (int?)SaveDataReader.GetEntry(PARAM_PLAYER_SHIP_TOTAL_VELOCITY_AVL).Value : null; }
		}


        /// <summary> [ELGCY+] Returns the ship's heading.</summary>
        public static int? Heading 
		{
			get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_LEGACY) ? (int?)SaveDataReader.GetEntry(PARAM_PLAYER_SHIP_HEADING).Value : null; }
		}


        /// <summary> [ELGCY+] Returns the ship's pitch.</summary>
        public static int? Pitch 
		{
			get { return (GameMeta.CurrentGame >= GameMeta.SupportedGame.EVOCHRON_LEGACY) ? (int?)SaveDataReader.GetEntry(PARAM_PLAYER_SHIP_PITCH).Value : null; }
		}
        #endregion


        #region (Static) Constructor
        static PlayerShipData()
        {
            _shieldStatus.Add(ShieldLevelState.FRONT, 0);
            _shieldStatus.Add(ShieldLevelState.RIGHT, 0);
            _shieldStatus.Add(ShieldLevelState.LEFT, 0);
            _shieldStatus.Add(ShieldLevelState.REAR, 0);
            _shieldStatus.Add(ShieldLevelState.TOTAL, 0);
        }
        #endregion


        #region Functions
        /// <summary> Converts some data into a more convenient form.
        /// </summary>
        internal static void Update()
        {
            int maxCount;

            // Get fuel
            string fuel = (string)SaveDataReader.GetEntry(PARAM_FUEL).Value;
            Int32.TryParse(fuel.Substring(0, fuel.IndexOf('/')), out _fuelRemaining);
            Int32.TryParse(fuel.Substring(fuel.IndexOf('/') + 1), out _fuelTotal);
            _fuelPercentage = (double)_fuelRemaining / _fuelTotal;

            // Get cargo bays
            maxCount = ShipData.MaxCargoSlots;
            for (int i = 0; i < maxCount; i++) { _cargoBay[i] = (string)SaveDataReader.GetEntry(PARAM_CARGO_BAY[i]).Value; }

            // Get position data
            _position.X = (int)SaveDataReader.GetEntry(PARAM_PLAYER_POSITION_X).Value;
            _position.Y = (int)SaveDataReader.GetEntry(PARAM_PLAYER_POSITION_Y).Value;
            _position.Z = (int)SaveDataReader.GetEntry(PARAM_PLAYER_POSITION_Z).Value;
            _sectorPosition.X = (int)SaveDataReader.GetEntry(PARAM_PLAYER_POSITION_SX).Value;
            _sectorPosition.Y = (int)SaveDataReader.GetEntry(PARAM_PLAYER_POSITION_SY).Value;
            _sectorPosition.Z = (int)SaveDataReader.GetEntry(PARAM_PLAYER_POSITION_SZ).Value;

            // Get shield levels
            _shieldStatus[ShieldLevelState.FRONT] = (int)SaveDataReader.GetEntry(PARAM_FRONT_SHIELD_LEVEL).Value;
            _shieldStatus[ShieldLevelState.RIGHT] = (int)SaveDataReader.GetEntry(PARAM_RIGHT_SHIELD_LEVEL).Value;
            _shieldStatus[ShieldLevelState.LEFT] = (int)SaveDataReader.GetEntry(PARAM_LEFT_SHIELD_LEVEL).Value;
            _shieldStatus[ShieldLevelState.REAR] = (int)SaveDataReader.GetEntry(PARAM_REAR_SHIELD_LEVEL).Value;
            _shieldStatus[ShieldLevelState.TOTAL] = (        // <-- Total shield strength is not being output by the game, so DIY
                _shieldStatus[ShieldLevelState.FRONT] +
                _shieldStatus[ShieldLevelState.RIGHT] +
                _shieldStatus[ShieldLevelState.LEFT] +
                _shieldStatus[ShieldLevelState.REAR]
            ) / 4;

            // Get secondary weapons
            maxCount = PARAM_SECONDARY_WEAPON_SLOT.Length;
            for (int i = 0; i < maxCount; i++) { _secWeapon[i] = (string)SaveDataReader.GetEntry(PARAM_SECONDARY_WEAPON_SLOT[i]).Value; }

            // Get equipment
            maxCount = ShipData.MaxEquipmentSlots;
            for (int i = 1; i < maxCount; i++) { _equipmentSlot[i] = (string)SaveDataReader.GetEntry(PARAM_EQUIPMENT_SLOT[i]).Value; }

            // Convert heat
            switch ((int)SaveDataReader.GetEntry(PARAM_ENGINE_THRUSTER_HEAT_INDICATOR).Value)
            {
                case 0: _heat = HeatState.LOW; break;
                case 1: _heat = HeatState.HIGH; break;
            }

            // Convert MTDS state
            switch ((int)SaveDataReader.GetEntry(PARAM_MDTS_STATUS).Value)
            {
                case 0: _mtds = MTDSState.OFF; break;
                case 1: _mtds = MTDSState.ON; break;
                case 2: _mtds = MTDSState.LOCKED; break;
            }

            // Convert missile lock state
            switch ((int)SaveDataReader.GetEntry(PARAM_MISSILE_LOCK_STATUS).Value)
            {
                case 0: _missileLock = MissileState.NO_LOCK; break;
                case 1: _missileLock = MissileState.LOCKED; break;
            }

            // Set S/E bias
            string bias;
            Regex biasRegex = new Regex(@"(?<Shields>[-+]?\d+?)S\/(?<Weapons>[-+]?\d+?)W");

            bias = biasRegex.Match((string)SaveDataReader.GetEntry(PARAM_ENERGY_BIAS_SETTING).Value).Groups["Shields"].Value;
            int.TryParse(bias, out _shieldBias);

            bias = biasRegex.Match((string)SaveDataReader.GetEntry(PARAM_ENERGY_BIAS_SETTING).Value).Groups["Weapons"].Value;
            int.TryParse(bias, out _weaponBias);

            // Convert IDS state
            SaveDataReader.ConvertOnOffState((int)SaveDataReader.GetEntry(PARAM_IDS_STATUS).Value, out _ids);

            // Convert afterburner status
            SaveDataReader.ConvertOnOffState((int)SaveDataReader.GetEntry(PARAM_AFTERBURNER_STATUS).Value, out _afterburner);

            // Convert autopilot status
            switch ((int)SaveDataReader.GetEntry(PARAM_AUTOPILOT_STATUS).Value)
            {
                case 0: _autopilot = AutopilotState.OFF; break;
                case 1: _autopilot = AutopilotState.FORM_ON_TARGET; break;
                case 2: _autopilot = AutopilotState.FLY_TO_NAV_POINT; break;
            }

            // Convert tractor beam status
            SaveDataReader.ConvertOnOffState((int)SaveDataReader.GetEntry(PARAM_TRACTOR_BEAM_STATUS).Value, out _tractorBeam);
        }
        #endregion
    }
}
