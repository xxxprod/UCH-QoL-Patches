using BepInEx;
using BepInEx.Configuration;
using GameEvent;
using HarmonyLib;
using UCHQoLPatches.Patches;
using UnityEngine;

namespace UCHQoLPatches;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        AkSoundEngineControllerPatches.InitializeConfig(Config);
        DeathDelayPatch.InitializeConfig(Config);
        HideChat.InitializeConfig(Config);

        new Harmony("uch.patch.qol.xxxprod.com").PatchAll();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}