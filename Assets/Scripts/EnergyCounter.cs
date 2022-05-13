using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCounter : MonoBehaviour
{
    public Text energyText;
    public int energyMax = 4;
    private int energyCount;

    private void Awake()
    {
        // listen for any events regarding using or resetting energy
        GameEvents.energyUsed.AddListener(UseEnergy);
        GameEvents.energyReset.AddListener(ResetEnergy);
        
        // also set the energy to max at start
        energyCount = energyMax;
        UpdateEnergyText();
    }

    private void UseEnergy(int energyUsed)
    {
        energyCount -= energyUsed;
        UpdateEnergyText();
    }

    private void ResetEnergy()
    {
        energyCount = energyMax;
        UpdateEnergyText();
    }

    private void UpdateEnergyText()
    {
        energyText.text = energyCount + "/" + energyMax;
    }
}