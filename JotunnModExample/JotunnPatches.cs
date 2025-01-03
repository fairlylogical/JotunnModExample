﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JotunnModExample
{
    internal class JotunnPatches
    {
        [HarmonyPatch(typeof(Skills), "LowerAllSkills")]
        [HarmonyPrefix]
        private static bool Prefix()
        {
            return false;
        }

        [HarmonyPatch(typeof(Player), "CreateTombStone")]
        [HarmonyPrefix]
        //keep equipmt on death:w
        private static bool Prefix2()
        {
            return false;
        }
        [HarmonyPatch(typeof(CharacterDrop), "GenerateDropList")]
        [HarmonyPrefix]
        private static bool Prefix2(ref CharacterDrop __instance, ref List<KeyValuePair<GameObject, int>> __result)
        {
            foreach (CharacterDrop.Drop drop in __instance.m_drops)
            {
                drop.m_chance = 100;
                if (drop.m_amountMax > 1)
                {
                    drop.m_amountMax *= 2;
                }
                drop.m_amountMin = drop.m_amountMax;
            }
            return true;
        }

        [HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { })]
        [HarmonyPrefix]
        private static bool Prefix3(ref DropTable __instance)
        {
            EditDrop(ref __instance);
            return true;
        }
        [HarmonyPatch(typeof(DropTable), "GetDropListItems")]
        [HarmonyPrefix]
        private static bool Prefix4(ref DropTable __instance)
        {
            EditDrop(ref __instance);
            return true;
        }

        [HarmonyPatch(typeof(DropTable), "GetDropListItems")]
        [HarmonyPostfix]
        private static void Postfix(ref List<ItemDrop.ItemData> __result)
        {
            if (__result != null && __result.Count > 0)
            {
                foreach(var res in __result)
                {
                    if (res.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Material)
                    {
                        res.m_stack *= 4;
                    }
                }
            }
        }

        //  List<ItemDrop.ItemData>
        private static void EditDrop(ref DropTable __instance)
        {
            __instance.m_dropChance = 100;
            if (__instance.m_dropMax > 1)
            {
                __instance.m_dropMax *= 2;
            }
            for (int i = 0; i < __instance.m_drops.Count; i++)
            {
                var drop = __instance.m_drops[i];
                if (drop.m_stackMax > 1)
                {
                    drop.m_stackMax *= 2;
                }
                drop.m_stackMin = drop.m_stackMax;
            }
            __instance.m_dropMin = __instance.m_dropMax;
            __instance.m_oneOfEach = false;
        }
    }
}
