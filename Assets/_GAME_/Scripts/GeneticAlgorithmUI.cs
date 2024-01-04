using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GeneticAlgorithmUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static int generation => GeneticAlgorithm.generation;
    public static float foodSuply => GeneticAlgorithm.foodSuply;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int tunaQuantity = GetTunaQuantity();
        int pufferFishQuantity = GetPufferFishQuantity();
        int sardineQuantity = GetSardineQuantity();

        text.text = $"Tuna: {tunaQuantity}\r\nPuffer Fish: {pufferFishQuantity}\r\nSardine: {sardineQuantity}\r\n---\r\nFood Supply: {Math.Round(foodSuply, 2)}%\r\nGeneration: {generation}";
    }


    private int GetPufferFishQuantity()
    {
        return GeneticAlgorithm.population.Where(x => x.fishType == FishType.PufferFish).Count();
    }

    private int GetSardineQuantity()
    {
        return GeneticAlgorithm.population.Where(x => x.fishType == FishType.Sardine).Count();
    }

    private int GetTunaQuantity()
    {
        return GeneticAlgorithm.population.Where(x => x.fishType == FishType.Tuna).Count();
    }
}
