// JotunnModStub
// a Valheim mod skeleton using Jötunn
// 
// File:    JotunnModStub.cs
// Project: JotunnModStub

using BepInEx;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace GuardianStones
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class GuardianStones : BaseUnityPlugin
    {
        public const string PluginGuid = "polyphonyrequiem.valheim.guardianstones";
        public const string PluginName = "GuardianStones";
        public const string PluginVersion = "0.0.1";

        private Harmony harmony;

        public GuardianStones()
        {
            string gsmbasm = Path.Combine(new FileInfo(Assembly.GetAssembly(typeof(GuardianStones)).Location).DirectoryName, "GuardianStones.MonoBehaviors.dll");
            Assembly.LoadFrom(gsmbasm);
        }
                
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private static string menhirLocationName = "GuardianStone_Location_Test";

        private void Awake()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(GuardianStones).Assembly, PluginGuid);

            // Jotunn comes with MonoMod Detours enabled for hooking Valheim's code
            // https://github.com/MonoMod/MonoMod
            On.FejdStartup.Awake += FejdStartup_Awake;
            
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html

            ZoneManager.OnVanillaLocationsAvailable += ZoneManager_OnVanillaLocationsAvailable;
        }

        private void ZoneManager_OnVanillaLocationsAvailable()
        {
            // Load asset bundle from the filesystem
            var guardianstoneAssetBundle = AssetUtils.LoadAssetBundleFromResources("guardianstones", typeof(GuardianStones).Assembly);

            var locationAsset = guardianstoneAssetBundle.LoadAsset<GameObject>(menhirLocationName);

            if (locationAsset == null)
            {
                Jotunn.Logger.LogError("Asset failed to load");
            }

            var menhirLocation = ZoneManager.Instance.CreateLocationContainer(locationAsset);
            ZoneManager.Instance.AddCustomLocation(
                new CustomLocation(
                    menhirLocation, 
                    fixReference: true,
                    new LocationConfig
            {
                Biome = Heightmap.Biome.Meadows,
                Quantity = 30,
                Priotized = true,
                ExteriorRadius = 3f,
                MinAltitude = 1f,
                ClearArea = true,
                MaxDistance = 150
            }));

            ZoneManager.OnVanillaLocationsAvailable -= ZoneManager_OnVanillaLocationsAvailable;

            guardianstoneAssetBundle.Unload(false);
        }

        private void FejdStartup_Awake(On.FejdStartup.orig_Awake orig, FejdStartup self)
        {
            // This code runs before Valheim's FejdStartup.Awake
            Jotunn.Logger.LogInfo("FejdStartup is going to awake");

            // Call this method so the original game method is invoked
            orig(self);

            // This code runs after Valheim's FejdStartup.Awake
            Jotunn.Logger.LogInfo("FejdStartup has awoken");
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }
}