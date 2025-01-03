// JotunnModExample
// A Valheim mod using Jötunn
// Used to demonstrate the libraries capabilities
// 
// File:    JotunnModExample.cs
// Project: JotunnModExample

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.GUI;
using Jotunn.Managers;
using Jotunn.Utils;
using JotunnModExample.ConsoleCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Logger = Jotunn.Logger;

namespace JotunnModExample
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    [BepInDependency("cinnabun.backpacks-v1.0.0", BepInDependency.DependencyFlags.SoftDependency)]
    internal class JotunnModExample : BaseUnityPlugin
    {
        // BepInEx' plugin metadata
        public const string PluginGUID = "com.jotunn.JotunnModExample";
        public const string PluginName = "JotunnModExample";
        public const string PluginVersion = "2.11.0";

        // Your mod's custom localization
        private CustomLocalization Localization;

        // Asset and prefab loading
        private GameObject BackpackPrefab;

        // Test assets
        private Texture2D TestTex;
        private Sprite TestSprite;
        private GameObject TestPanel;

        // Fixed buttons
        private ButtonConfig ShowGUIButton;
        private ButtonConfig RaiseSkillButton;
        private ButtonConfig CreateColorPickerButton;
        private ButtonConfig CreateGradientPickerButton;

        // Variable button backed by a KeyCode and a GamepadButton config
        private ConfigEntry<KeyCode> EvilSwordSpecialConfig;
        private ConfigEntry<InputManager.GamepadButton> EvilSwordGamepadConfig;
        private ButtonConfig EvilSwordSpecialButton;

        // Variable BepInEx Shortcut backed by a config
        private ConfigEntry<KeyboardShortcut> ShortcutConfig;
        private ButtonConfig ShortcutButton;

        // Configuration values
        private ConfigEntry<string> StringConfig;
        private ConfigEntry<float> FloatConfig;
        private ConfigEntry<int> IntegerConfig;
        private ConfigEntry<bool> BoolConfig;

        // Custom skill
        private Skills.SkillType TestSkill = 0;

        // Custom status effect
        private CustomStatusEffect EvilSwordEffect;

        // Custom RPC
        public static CustomRPC UselessRPC;

        // Jötunn's undo queues are identified by name. Every mod that uses
        // the same queue name shares that queue.
        private const string QueueName = "TestUndo";

        private void Awake()
        {
            AddLocalizations();
            Harmony.CreateAndPatchAll(typeof(JotunnPatches), null);
        }

        private void AddLocalizations()
        {
            // Create a custom Localization instance and add it to the Manager
            Localization = new CustomLocalization();
            LocalizationManager.Instance.AddLocalization(Localization);

            // Add translations for our custom skill
            Localization.AddTranslation("English", new Dictionary<string, string>
            {
                {"menu_mod_notice", "." }
            });
        }
    }
}