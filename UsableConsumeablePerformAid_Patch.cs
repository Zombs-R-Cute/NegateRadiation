using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Rocket.Core.Logging;
using SDG.Unturned;

namespace Zombs_R_Cute_NegateRadiation
{
    [HarmonyPatch(typeof(UseableConsumeable), "performAid")]
    public class UsableConsumeablePerformAid_Patch
    {

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var search = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Callvirt,
                    AccessTools.PropertyGetter(typeof(SDG.Unturned.ItemConsumeableAsset), "virus")),
                new CodeInstruction(OpCodes.Conv_R4),
            };

            var codes = new List<CodeInstruction>(instructions);
            var newCodes = new List<CodeInstruction>();
            
            var offset = Search();
            
            for (int i = 0; i < codes.Count; i++)
            {
                if (i == offset)
                {
                    newCodes.Add(new CodeInstruction(OpCodes.Ldc_R4, 0.0f));
                    i += search.Count;
                }

                newCodes.Add(codes[i]);
            }

            foreach (var instruction in newCodes)
            {
                yield return instruction;
            }

            Logger.Log("UsableConsumeablePerformAid_Patch: Patch applied");


            int Search()
            {
                int sOffset = 0;
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode != search[0].opcode)
                        continue;

                    sOffset = i;
                    for (int j = 1; j < search.Count; j++)
                    {
                        if (i + j == codes.Count
                            || codes[i + j].opcode != search[j].opcode ||
                            (codes[i + j].opcode == OpCodes.Callvirt && codes[i + j].operand != search[j].operand))
                        {
                            sOffset = 0;
                            break;
                        }
                    }

                    if (sOffset != 0)
                        break;
                }

                return sOffset;
            }
        }
    }
}