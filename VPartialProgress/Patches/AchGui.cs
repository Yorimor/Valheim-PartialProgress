using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace VPartialProgress.Patches
{
    [HarmonyPatch(typeof(AchievementsGui))]
    [HarmonyPatch("CreateStatRow")]
    public class AchievementsPatches
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                // Main.logger.LogInfo(codes[i].opcode + " :: " + codes[i].operand);

                if (codes[i].opcode.Equals(OpCodes.Blt_Un) && i > 1)
                {
                    if (codes[i - 1].opcode.Equals(OpCodes.Ldarg_3))
                    {
                        codes[i - 1] = new CodeInstruction(OpCodes.Ldc_R4, 1f);
                    }
                }
            }

            return codes;
        }
    }
}