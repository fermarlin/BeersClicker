using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    public int[] habilitiesLvl = new int[10];
    public event Action<int, int> OnHabilityChanged;

    public void UpgradeHab(int habilityIndex)
    {
        habilitiesLvl[habilityIndex]++;
        OnHabilityChanged?.Invoke(habilityIndex, habilitiesLvl[habilityIndex]);
    }
}
