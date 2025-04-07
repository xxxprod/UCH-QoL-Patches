using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UCHQoLPatches.Patches {
    [HarmonyPatch(typeof(ChatDisplay))]
    public static class HideChat {
        private static ConfigEntry<KeyCode> ToggleVisibility;
        private static ConfigEntry<float> MinimumHideLength;

        private static float InvisibilityTimer;

        public static void InitializeConfig(ConfigFile config) {
            ToggleVisibility = config.Bind(
                "QoL - Chat",
                "Toggle With Key",
                KeyCode.Alpha0,
                "Key to press to automatically open/close ChatDisplay"
                );
            MinimumHideLength = config.Bind(
                "QoL - Chat",
                "Min Delay from Keypress",
                0.5f,
                "The minimum amount of time between pressing the toggle key and the chat reappearing (in seconds).\nMessages sent in the next [hidelength] seconds will not reopen the chat, and you will not be able to open the chat for this amount of time."
                );
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void ToggleOnKeyBefore(ChatDisplay __instance, ref float ___VisibilityTimer) {
            if (Input.GetKeyUp(ToggleVisibility.Value) && !__instance.currentChatInputField.inputField.isFocused) {
                InvisibilityTimer = MinimumHideLength.Value;
            }
            if (InvisibilityTimer > 0f) {
                __instance.currentChatInputField.CancelChatMessage();
                __instance.currentChatInputField.gameObject.SetActive(false);
                __instance.ChatMode = false;
                ___VisibilityTimer = 0;
                __instance.ChatCanvasGroup.alpha = 0f;
            }
            InvisibilityTimer -= Time.unscaledDeltaTime;
        }
    }
}
