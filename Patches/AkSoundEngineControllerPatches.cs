using BepInEx.Configuration;
using HarmonyLib;

namespace UCHQoLPatches.Patches;

[HarmonyPatch(typeof(AkSoundEngineController))]
public static class AkSoundEngineControllerPatches
{
    private static ConfigEntry<bool> EnableWhenUnfocused;

    public static void InitializeConfig(ConfigFile config) {
        EnableWhenUnfocused = config.Bind(
            "QoL - Sounds", 
            "Play Sounds When Unfocused", 
            true, 
            "Plays audio from game when the application is unfocused, e.g. when\nalt-tabbed or another application is selected."
        );
    }

    [HarmonyPrefix]
    [HarmonyPatch("OnApplicationFocus", MethodType.Normal)]
    public static bool OnApplicationFocus(bool focus)
    {
        // disable AkSoundEngineController.OnApplicationFocus to keep audio running when game is not focused
        // return true = run the original method, return false = do not run the original method
        // returns false when "EnableWhenUnfocused" is true, disabling the controller.
        return !EnableWhenUnfocused.Value;
    }
}