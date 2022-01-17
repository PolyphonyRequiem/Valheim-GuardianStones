using GuardianStones.MonoBehaviors;
using HarmonyLib;
using System;

namespace GuardianStones
{
    [HarmonyPatch(typeof(Player), nameof(Player.HaveRequirements), argumentTypes: new Type[]{typeof(Piece), typeof(Player.RequirementMode)})]
    class Patch
    {
        static void Postfix(ref Player __instance, ref bool __result, Piece piece, Player.RequirementMode mode)
        {
            if  (__result == false)
            {
                return;
            }

            if (piece.m_name == "$piece_workbench" && !GuardianStone.IsNearby(__instance.transform.position))
            {
                __instance.Message(MessageHud.MessageType.TopLeft, "A guardian stone needs to be nearby in order to place this workbench.");
                __result = false;
            }
        }
    }
}
