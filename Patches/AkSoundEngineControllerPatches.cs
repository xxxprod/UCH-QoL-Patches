using HarmonyLib;

namespace UCHQoLPatches.Patches;

[HarmonyPatch(typeof(AkSoundEngineController))]
public static class AkSoundEngineControllerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("OnApplicationFocus", MethodType.Normal)]
    public static bool OnApplicationFocus(bool focus)
    {
        // disable AkSoundEngineController.OnApplicationFocus to keep audio running when game is not focused
        return false;
    }
}