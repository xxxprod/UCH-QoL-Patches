using BepInEx;
using BepInEx.Configuration;
using GameEvent;
using HarmonyLib;
using UnityEngine;

namespace UCHQoLPatches;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin, IGameEventListener
{
    private ConfigEntry<float> _maxDeathDelay;

    private void Awake()
    {
        new Harmony("uch.patch.qol.xxxprod.com").PatchAll();

        _maxDeathDelay = Config.Bind("FreePlay", "On Death Respawn Delay", 0.3f, "The delay in seconds until the character is respawned after death.");
        _maxDeathDelay.SettingChanged += MaxDeathDelay_SettingChanged;

        GameEventManager.ChangeListener<StartPhaseEvent>(this, true);

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void MaxDeathDelay_SettingChanged(object sender, System.EventArgs e)
    {
        UpdateMaxDeathDelay();
    }

    public void handleEvent(GameEvent.GameEvent e)
    {
        if (e is StartPhaseEvent)
            UpdateMaxDeathDelay();
    }

    private void UpdateMaxDeathDelay()
    {
        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            character.maxDeathDelay = _maxDeathDelay.Value;
        }

        Debug.Log("Updated MaxDeathDelay on all characters.");
    }
}