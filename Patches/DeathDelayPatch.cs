using BepInEx.Configuration;
using GameEvent;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace UCHQoLPatches.Patches {
    // Some code based on https://github.com/cekco/UCH-BetterRespawn
    // Optimized to not get called every phase change. Other mods may overwrite this value later.

    public class DeathDelayPatch {

        private static ConfigEntry<float> freeplayDeathTimer;
        private static ConfigEntry<float> challengeDeathTimer;

        public static void InitializeConfig(ConfigFile config) {
            freeplayDeathTimer = config.Bind(
                "QoL - Respawn",
                "Freeplay Timer",
                0.3f,
                new ConfigDescription("Sets the time between dying and respawning in Freeplay mode.", new AcceptableValueRange<float>(0f,10f))
            );
            challengeDeathTimer = config.Bind(
                "QoL - Respawn",
                "Challenge Timer",
                3f,
                new ConfigDescription("Sets the time between dying and respawning in Challenge mode.\nValues over 5 seconds are banned as they give a postmortem advantage.", 
                new AcceptableValueRange<float>(0f,5f))
            );
        }

        [HarmonyPatch(typeof(GameControl), "NotifySetupStartDone")]
        [HarmonyPostfix]
        public static void PatchSetupStartFinish(ref Queue<GamePlayer> ___PlayerQueue) {
            foreach (GamePlayer player in ___PlayerQueue) {
                player.CharacterInstance.maxDeathDelay = GameSettings.GetInstance().GameMode switch
                {
                    GameState.GameMode.CHALLENGE => challengeDeathTimer.Value,
                    GameState.GameMode.FREEPLAY => freeplayDeathTimer.Value,
                    _ => player.CharacterPrefab.maxDeathDelay // default value, steals from the parent prefab used to construct.
                };
            }
        }
    }
}
