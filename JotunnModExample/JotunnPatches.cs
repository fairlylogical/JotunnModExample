﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MonoMod.InlineRT.MonoModRule;

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
                    res.m_shared.m_teleportable = true;
                    if (res.m_dropPrefab?.name == "Stone" || res.m_shared.m_name == "Stone")
                    {
                        continue;
                    }

                    if (res.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Material || res.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo)
                    {
                        res.m_stack *= 4;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(ZoneSystem), "GetGlobalKey", new Type[] {typeof(GlobalKeys)})]
        [HarmonyPostfix]
        private static void PostfixTeleport(ref bool __result, GlobalKeys key)
        {
            if (key == GlobalKeys.TeleportAll)
            {
                __result = true;
            }   
        }
        [HarmonyPatch(typeof(SEMan), "ModifyEitrRegen")]
        [HarmonyPostfix]
        private static void ManaRegen(ref float eitrMultiplier)
        {
            eitrMultiplier = 12f;
        }

        [HarmonyPatch(typeof(Player), "ActivateGuardianPower")]
        [HarmonyPostfix]
        private static void ModerCooldown(ref Player __instance)
        {
            if (__instance.GetGuardianPowerName() == "GP_Moder")
            {
                __instance.m_guardianPowerCooldown = 0.1f;
            }
        }

        //  List<ItemDrop.ItemData>
        private static void EditDrop(ref DropTable __instance)
        {
            __instance.m_dropChance = 100;
            bool hasStone = false;
            if (__instance.m_dropMax > 1)
            {
                __instance.m_dropMax *= 2;
            }
            for (int i = 0; i < __instance.m_drops.Count; i++)
            {
                var drop = __instance.m_drops[i];
                GameObject item = drop.m_item;
                ItemDrop.ItemData itemData = item.GetComponent<ItemDrop>()?.m_itemData;
                /*
                if (itemData != null)
                {
                    itemData.m_shared.m_teleportable = true;
                    if (
    isNameMatch(itemData, item, "Stone")
    || isNameMatch(itemData, item, "CopperOre")
    || isNameMatch(itemData, item, "BlackMetalScrap")
    || isNameMatch(itemData, item, "BlackMarble")
    )
                    {
                        hasStone = true;
                        continue;
                    }
                }
                */
                if (drop.m_stackMax > 1 || itemData?.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Material  || itemData?.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo)
                {
                    drop.m_stackMax *= 2;
                }
                drop.m_stackMin = drop.m_stackMax;
            }
            //__instance.m_oneOfEach = hasStone;
            __instance.m_dropMin = __instance.m_dropMax;
        }
        private static bool isNameMatch(ItemDrop.ItemData itemData, GameObject item, string name)
        {
            return itemData.m_shared.m_name == name || item?.name == name || itemData.m_dropPrefab?.name == name;
        }

        [HarmonyPatch(typeof(Plant), "GetGrowTime")]
        public class MakePlantsGrowFaster
        {
            private static void Postfix(ref float __result)
            {
                __result /= 4;
            }
        }

    }
}
