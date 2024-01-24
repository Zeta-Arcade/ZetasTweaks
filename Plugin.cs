using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using ZetasTweaks.Patches;
using UnityEngine;

namespace ZetasTweaks
{
    public static class PluginInfo
    {
        public const string modGUID = "Zeta.ZetasTweaks";
        public const string modName = "Zetas Tweaks";
        public const string modVersion = "1.0.0.0";
    }
    [BepInPlugin(PluginInfo.modGUID, PluginInfo.modName, PluginInfo.modVersion)]
    public class ZetasTweaksBase : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.modGUID);

        public static ZetasTweaksBase Instance;

        public enum LifeIncrementType
        {
            Addition,
            Multiplier
        }

        public static ConfigEntry<LifeIncrementType> Life_Increment_Type;

        public static ConfigEntry<bool> Respawn_Enabled;

        public static ConfigEntry<int> Respawn_Cost;

        public static ConfigEntry<bool> Respawn_Orbit_Only;

        public static ConfigEntry<int> Life_Increment_Amount;

        public static ConfigEntry<float> Life_Increment_Multiplier;

        ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.modGUID);

            mls.LogInfo("Zetas Tweaks has awaken");
            ConfigSettings.BindConfigSettings();
            harmony.PatchAll(typeof(ZetasTweaksBase));
            harmony.PatchAll(typeof(ConfigSync));
            harmony.PatchAll(typeof(PlayerControllerB));
            harmony.PatchAll(typeof(ShopPricesBPatch));
            harmony.PatchAll(typeof(ShipTeleporterPatch));
            mls.LogInfo("Zetas Tweaks loaded");
        }
        public static void Log(string message)
        {
            Instance.PassLog(message, false);
        }

        public static void LogError(string message)
        {
            Instance.PassLog(message, true);
        }
        void PassLog(string message, bool isError)
        {
            if (isError)
            {
                mls.LogError(message);
            }
            else
            {
                mls.LogInfo(message);
            }
        }
    }
    [Serializable]
    public static class ConfigSettings
    {
        public static ConfigEntry<int> WalkieTalkieCost;
        public static ConfigEntry<int> FlashlightCost;
        public static ConfigEntry<int> ShovelCost;
        public static ConfigEntry<int> LockpickerCost;
        public static ConfigEntry<int> ProFlashlightCost;
        public static ConfigEntry<int> StunGrenadeCost;
        public static ConfigEntry<int> BoomboxCost;
        public static ConfigEntry<int> TZPCost;
        public static ConfigEntry<int> ZapGunCost;
        public static ConfigEntry<int> JetpackCost;
        public static ConfigEntry<int> ExtensionLadderCost;
        public static ConfigEntry<int> RadarBoosterCost;
        public static ConfigEntry<int> SprayPaintCost;
        public static ConfigEntry<bool> RespawnEnabled;
        public static ConfigEntry<bool> RespawnOrbitOnly;
        public static ConfigEntry<int> RespawnCost;
        public static ConfigEntry<bool> LifeIncrementEnabled;
        public static ConfigEntry<bool> LifeIncrementResets;
        public static ConfigEntry<ZetasTweaksBase.LifeIncrementType> LifeIncrementType;
        public static ConfigEntry<int> LifeIncrementAmount;
        public static ConfigEntry<float> LifeIncrementMultiplier;
        public static ConfigEntry<int> TPCooldown;
        public static ConfigEntry<int> ITPCooldown;
        public static void BindConfigSettings()
        {
            //Shop prices
            WalkieTalkieCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "WalkieTalkie", 12, "How much the walkie talkie should cost by default. Default value is 12.");
            ZetasTweaksBase.Log("Debug: Walkie Talkie cost is set to: " + WalkieTalkieCost);
            FlashlightCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Flashlight", 15, "How much the Flashlight should cost by default. Default value is 15.");
            ShovelCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Shovel", 30, "How much the Shovel should cost by default. Default value is 30.");
            LockpickerCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Lockpicker", 20, "How much the Lockpicker should cost by default. Default value is 20.");
            ProFlashlightCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "ProFlashlight", 25, "How much the Pro-Flashlight should cost by default. Default value is 25.");
            StunGrenadeCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Stun granade", 40, "How much the Stun grenade should cost by default. Default value is 40.");
            BoomboxCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Boombox", 60, "How much the Boombox should cost by default. Default value is 60.");
            TZPCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "TZP", 120, "How much the TZP Inhalant should cost by default. Default value is 120.");
            ZapGunCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Zap gun", 400, "How much the Zap gun should cost by default. Default value is 400.");
            JetpackCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Jetpack", 700, "How much the Jetpack should cost by default. Default value is 700.");
            ExtensionLadderCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Extension ladder", 60, "How much the Extension Ladder should cost by default. Default value is 60.");
            RadarBoosterCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Radar booster", 50, "How much the Radar Booster should cost by default. Default value is 50.");
            SprayPaintCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings", "Spray paint", 50, "How much the Spray paint should cost by default. Default value is 50.");

            //Respawn system
            RespawnEnabled = ZetasTweaksBase.Instance.Config.Bind<bool>("Settings2", "Respawn Enabled", true, "Whether you can pay to respawn employees. All following respawn-related settings only work with this on. Default value is True.");
            RespawnOrbitOnly = ZetasTweaksBase.Instance.Config.Bind<bool>("Settings2", "Respawn Orbit Only", true, "Whether you can only respawn employees in orbit, intended for use with the Permadeath mod. If playing without the mod, false is recommended if you want to be able to respawn employees mid-mission as orbit is pointless. Default value is True.");
            RespawnCost = ZetasTweaksBase.Instance.Config.Bind<int>("Settings2", "Respawn Cost", 100, "The starting amount of the respawn cost. Default is 100.");
            LifeIncrementEnabled = ZetasTweaksBase.Instance.Config.Bind<bool>("Settings2", "Life Increment Enabled", true, "Whether the respawn cost of an employee increases each time. Default value is True.");
            LifeIncrementResets = ZetasTweaksBase.Instance.Config.Bind<bool>("Settings2", "Life Increment Resets", true, "Whether the respawn cost an employee resets each quota. Default value is True.");
            LifeIncrementType = ZetasTweaksBase.Instance.Config.Bind<ZetasTweaksBase.LifeIncrementType>("Settings2", "Life Increment Type", ZetasTweaksBase.LifeIncrementType.Addition, "How to increase the employee respawn cost. If 'Addition', it will use the 'Life Increment Amount', and ADD that amount each respawn. If 'Multiplier', it will use the 'Life Increment Multiplier', and MULTIPLY by that amount each respawn. Default value is 'Addition'.");
            LifeIncrementAmount = ZetasTweaksBase.Instance.Config.Bind<int>("Settings2", "Life Increment Amount", 100, "How much to add to the respawn cost each time, if 'Life Increment Type' is set to 'Addition'. Default value is 100.");
            LifeIncrementMultiplier = ZetasTweaksBase.Instance.Config.Bind<float>("Settings2", "Life Increment Multiplier", 2.0f, "How much to multiply the respawn cost each time, if 'Life Increment Type' is set to 'Multiply'. Default value is 2.0.");

            //Misc Tweaks
            TPCooldown = ZetasTweaksBase.Instance.Config.Bind<int>("Settings3", "TPCooldown", 10, "How long the cooldown is on the teleporter, in seconds. Default value is 10.");
            ITPCooldown = ZetasTweaksBase.Instance.Config.Bind<int>("Settings3", "ITPCooldown", 210, "How long the cooldown is on the inverse teleporter, in seconds. Default value is 210 (3 minutes 30 seconds)");
        }
    }
    [Serializable]
    [HarmonyPatch]
    public class ConfigSync
    {
        public static ConfigSync defaultConfig;

        public static ConfigSync instance;

        public static PlayerControllerB localPlayerController;

        public static bool isSynced = false;

        public float staminaRegenMultiplier = 1f;

        public int WalkieTalkieCost = 12;
        public int FlashlightCost = 15;
        public int ShovelCost = 30;
        public int LockpickerCost = 20;
        public int ProFlashlightCost = 25;
        public int StunGrenadeCost = 40;
        public int BoomboxCost = 60;
        public int TZPCost = 120;
        public int ZapGunCost = 400;
        public int JetpackCost = 700;
        public int ExtensionLadderCost = 60;
        public int RadarBoosterCost = 50;
        public int SprayPaintCost = 50;
        public bool RespawnEnabled = true;
        public bool RespawnOrbitOnly = true;
        public int RespawnCost = 100;
        public bool LifeIncrementEnabled = true;
        public bool LifeIncrementResets = true;
        public ZetasTweaksBase.LifeIncrementType LifeIncrementType = ZetasTweaksBase.LifeIncrementType.Addition;
        public int LifeIncremenentAmount = 100;
        public float LifeIncrementMultiplier = 2.0f;
        public int TPCooldown = 10;
        public int ITPCooldown = 210;

        public static void BuildDefaultConfigSync()
        {
            instance = new ConfigSync();
        }

        public static void BuildServerConfigSync()
        {
            if (defaultConfig == null)
            {
                defaultConfig = new ConfigSync();
                defaultConfig.WalkieTalkieCost = Mathf.Clamp(ConfigSettings.WalkieTalkieCost.Value, 1, 9999);
                defaultConfig.FlashlightCost = Mathf.Clamp(ConfigSettings.FlashlightCost.Value, 1, 999);
                defaultConfig.ShovelCost = Mathf.Clamp(ConfigSettings.ShovelCost.Value, 1, 999);
                defaultConfig.LockpickerCost = Mathf.Clamp(ConfigSettings.LockpickerCost.Value, 1, 999);
                defaultConfig.ProFlashlightCost = Mathf.Clamp(ConfigSettings.ProFlashlightCost.Value, 1, 999);
                defaultConfig.StunGrenadeCost = Mathf.Clamp(ConfigSettings.StunGrenadeCost.Value, 1, 999);
                defaultConfig.BoomboxCost = Mathf.Clamp(ConfigSettings.BoomboxCost.Value, 1, 999);
                defaultConfig.TZPCost = Mathf.Clamp(ConfigSettings.TZPCost.Value, 1, 999);
                defaultConfig.ZapGunCost = Mathf.Clamp(ConfigSettings.ZapGunCost.Value, 1, 999);
                defaultConfig.JetpackCost = Mathf.Clamp(ConfigSettings.JetpackCost.Value, 1, 999);
                defaultConfig.ExtensionLadderCost = Mathf.Clamp(ConfigSettings.ExtensionLadderCost.Value, 1, 999);
                defaultConfig.RadarBoosterCost = Mathf.Clamp(ConfigSettings.RadarBoosterCost.Value, 1, 999);
                defaultConfig.SprayPaintCost = Mathf.Clamp(ConfigSettings.SprayPaintCost.Value, 1, 999);
                defaultConfig.RespawnEnabled = ConfigSettings.RespawnEnabled.Value;
                defaultConfig.RespawnOrbitOnly = ConfigSettings.RespawnOrbitOnly.Value;
                defaultConfig.RespawnCost = Mathf.Clamp(ConfigSettings.RespawnCost.Value, 1, 9999);
                defaultConfig.LifeIncrementEnabled = ConfigSettings.LifeIncrementEnabled.Value;
                defaultConfig.LifeIncrementResets = ConfigSettings.LifeIncrementResets.Value;
                defaultConfig.LifeIncrementType = ConfigSettings.LifeIncrementType.Value;
                defaultConfig.LifeIncremenentAmount = Mathf.Clamp(ConfigSettings.LifeIncrementAmount.Value, 1, 999);
                defaultConfig.LifeIncrementMultiplier = Mathf.Clamp(ConfigSettings.LifeIncrementMultiplier.Value, 0.1f, 100f);
                defaultConfig.TPCooldown = Mathf.Clamp(ConfigSettings.TPCooldown.Value, 0, 999);
                defaultConfig.ITPCooldown = Mathf.Clamp(ConfigSettings.ITPCooldown.Value, 0, 999);
                instance = defaultConfig;
                ZetasTweaksBase.Log("Default config overwritten");
            }
            ZetasTweaksBase.Log("Default config was set correctly");
        }

        [HarmonyPatch(typeof(PlayerControllerB), "ConnectClientToPlayerObject")]
        [HarmonyPostfix]
        public static void InitializeLocalPlayer(PlayerControllerB __instance)
        {
            ZetasTweaksBase.Log("Controller initialised");
            localPlayerController = __instance;
            if (NetworkManager.Singleton.IsServer)
            {
                BuildServerConfigSync();
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ZetasTweaks-OnRequestConfigSync", OnReceiveConfigSyncRequest);
                OnLocalClientConfigSync();
            }
            else
            {
                ZetasTweaksBase.Log("Player is set to not synced");
                isSynced = false;
                BuildDefaultConfigSync();
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ZetasTweaks-OnReceiveConfigSync", OnReceiveConfigSync);
                RequestConfigSync();
            }
        }

        public static void RequestConfigSync()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                ZetasTweaksBase.Log("Sending config sync request to server.");
                FastBufferWriter messageStream = new FastBufferWriter(4, Allocator.Temp);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ZetasTweaks-OnRequestConfigSync", 0uL, messageStream);
            }
            else
            {
                ZetasTweaksBase.LogError("Failed to send config sync request.");
            }
        }

        public static void OnReceiveConfigSyncRequest(ulong clientId, FastBufferReader reader)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                ZetasTweaksBase.Log("Receiving config sync request from client with id: " + clientId + ". Sending config sync to client.");
                byte[] array = SerializeConfigToByteArray(instance);
                FastBufferWriter messageStream = new FastBufferWriter(array.Length + 4, Allocator.Temp);
                int value = array.Length;
                messageStream.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
                messageStream.WriteBytesSafe(array);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ZetasTweaks-OnReceiveConfigSync", clientId, messageStream);
            }
        }

        public static void OnReceiveConfigSync(ulong clientId, FastBufferReader reader)
        {
            if (reader.TryBeginRead(4))
            {
                reader.ReadValueSafe(out int value, default(FastBufferWriter.ForPrimitives));
                if (reader.TryBeginRead(value))
                {
                    ZetasTweaksBase.Log("Receiving config sync from server.");
                    byte[] value2 = new byte[value];
                    reader.ReadBytesSafe(ref value2, value);
                    instance = DeserializeFromByteArray(value2);
                    OnLocalClientConfigSync();
                }
                else
                {
                    ZetasTweaksBase.LogError("Error receiving sync from server.");
                }
            }
            else
            {
                ZetasTweaksBase.LogError("Error receiving bytes length.");
            }
        }

        public static void OnLocalClientConfigSync()
        {
            //localPlayerController.movementSpeed = defaultMovementSpeed * instance.movementSpeedMultiplier;
            isSynced = true;
        }

        public static byte[] SerializeConfigToByteArray(ConfigSync config)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, config);
            return memoryStream.ToArray();
        }

        public static ConfigSync DeserializeFromByteArray(byte[] data)
        {
            MemoryStream serializationStream = new MemoryStream(data);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (ConfigSync)binaryFormatter.Deserialize(serializationStream);
        }
    }
}
