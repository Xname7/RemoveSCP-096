using HarmonyLib;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Mistaken.RemoveSCP_096
{
    [HarmonyPatch(typeof(ScpSpawner), nameof(ScpSpawner.SpawnableScps), MethodType.Getter)]
    internal static class ScpSpawnerSpawnableScpsPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = NorthwoodLib.Pools.ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Brfalse_S);
            var label = generator.DefineLabel();
            newInstructions[index + 5].WithLabels(label); // Ldloca_S V_1

            newInstructions.InsertRange(index + 1, new CodeInstruction[]
            {
                new(OpCodes.Ldloca_S, 2),
                new(OpCodes.Call, AccessTools.Method(typeof(KeyValuePair<RoleTypeId, PlayerRoleBase>), "get_Key")),
                new(OpCodes.Conv_I4),
                new(OpCodes.Ldc_I4, 9), // RoleTypeId.Scp096
                new(OpCodes.Beq_S, label),
            });

            foreach (var instruction in newInstructions)
                yield return instruction;

            NorthwoodLib.Pools.ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
