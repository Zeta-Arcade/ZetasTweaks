using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Unity.Netcode;
using ZetasTweaks;
using ZetasTweaks.Patches;
namespace ZetasTweaks.Patches
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal class ShipTeleporterPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void customCooldown(ref float ___cooldownAmount, ref bool ___isInverseTeleporter)
        {
            if (___isInverseTeleporter)
            {
                ___cooldownAmount = ConfigSync.instance.ITPCooldown;
            }
            else
            {
                ___cooldownAmount = ConfigSync.instance.TPCooldown;
            }
        }
    }
}
