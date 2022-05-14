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
        GameEvents.cardPlayed.AddListener(TryToPlayCard);

        // also set the energy to max at start
        energyCount = energyMax;
        UpdateEnergyText();
    }

    private void TryToPlayCard(CardManager card)
    {
        // if the card can be played, use the energy and discard it
        if (energyCount - card.getEnergyCost() >= 0)
        {
            UseEnergy(card.getEnergyCost());
            GameEvents.cardDiscarded.Invoke(card);
        }
        // else return it to the hand
        GameEvents.cardCannotPlay.Invoke(card);
    }

    private void UseEnergy(int energyUsed)
    {
        // expend the energy
        energyCount -= energyUsed;
        UpdateEnergyText();
        // let everything know the remaining energy
        GameEvents.energyRemaining.Invoke(energyCount);
    }

    private void ResetEnergy()
    {
        // reset the energy
        energyCount = energyMax;
        UpdateEnergyText();
        // let everything know the remaining energy
        GameEvents.energyRemaining.Invoke(energyCount);
    }

    private void UpdateEnergyText()
    {
        energyText.text = energyCount + "/" + energyMax;
    }
}