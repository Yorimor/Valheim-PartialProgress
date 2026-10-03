using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace VPartialProgress.Patches
{
    [HarmonyPatch(typeof(AchievementsGui))]
    [HarmonyPatch("CreateStatRow")]
    public class AchievementsGuiPatches
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var branchCode = -1;

            var myTargetNum = 1f;
            if (Main.showNoProgress.Value)
            {
                myTargetNum = 0f;
            }
            
            MethodInfo myGetColor = AccessTools.Method(typeof(AchievementsGuiPatches), nameof(AchievementsGuiPatches.MyGetColor));
            MethodInfo methodToFind = AccessTools.Method(typeof(Color), "get_green");
            
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                // Main.logger.LogInfo(codes[i].opcode + " :: " + codes[i].operand);

                if (codes[i].opcode.Equals(OpCodes.Blt_Un) && i > 1)
                {
                    branchCode = i;
                    if (codes[i - 1].opcode.Equals(OpCodes.Ldarg_3))
                    {
                        codes[i - 1] = new CodeInstruction(OpCodes.Ldc_R4, myTargetNum);
                    }
                }

                if (branchCode == -1) continue;
                
                if (codes[i].opcode.Equals(OpCodes.Call))
                {
                    if (codes[i].operand is MethodInfo method && method == methodToFind)
                    {
                        // Main.logger.LogInfo("   " + codes[i].opcode + " :: " + codes[i].operand);
                        codes[i] = new CodeInstruction(OpCodes.Call, myGetColor);
                        codes.InsertRange(i, new CodeInstruction[]
                        {
                            new CodeInstruction(OpCodes.Ldarg_2),
                            new CodeInstruction(OpCodes.Ldarg_3),
                        });
                    }
                }
            }
            
            // Main.logger.LogInfo("");
            // Main.logger.LogInfo("--- NEW ---");
            // Main.logger.LogInfo("");
            //
            // for (var i = 0; i < codes.Count; i++)
            // {
            //     Main.logger.LogInfo(codes[i].opcode + " :: " + codes[i].operand);
            // }

            return codes;
        }

        public static Color MyGetColor(float current, float total)
        {
            var color = Color.gray;
            
            if (current >= total)
            {
                color = Color.green;
            }
            else if (current >= 1)
            {
                color =  Color.red;
            }
            else if (current <= 0)
            {
                color =  Color.grey;
            }

            return color;
        }
    }
    
    [HarmonyPatch(typeof(Achievements))]
    [HarmonyPatch("Initialize")]
    public class AchievementsPatches
    {
        [HarmonyPrefix]
        public static void InitPatch(Achievements __instance)
        {
            if (Main.showSecrets.Value)
            {
                foreach (var achievementList in __instance.m_achievementLists)
                {
                    foreach (var achievement in achievementList.m_achievements)
                    {
                        achievement.m_clickable = true;
                        achievement.m_isSecret = false;
                    }
                }
            }
        }
    }
}