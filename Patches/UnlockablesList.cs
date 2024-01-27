using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZetasTweaks.Patches
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Unlockables", order = 2)]
    public class UnlockablesList : ScriptableObject
    {
        public List<UnlockableItem> unlockables = new List<UnlockableItem>();
    }
}
