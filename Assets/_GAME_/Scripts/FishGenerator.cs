using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public GameObject pufferFishPrefab;
    public GameObject sardinePrefab;
    public GameObject tunaPrefab;

    public float maxX;
    public float minX;
    float minY;
    float maxY;
    private GameObject[] fishes;

    private List<FishData> population => GeneticAlgorithm.population;


    void Start()
    {
        // Get data
        maxX = GameObject.Find("maxX").transform.position.x;
        minX = GameObject.Find("minX").transform.position.x;
        minY = GameObject.Find("minY").transform.position.y;
        maxY = GameObject.Find("maxY").transform.position.y;

        fishes = new GameObject[3] { pufferFishPrefab, sardinePrefab, tunaPrefab};
    }


    void Update()
    {
        if (population.Count > 0)
        {
            bool hasFishNotSwimming = population.Any(fishData => !fishData.isSwimming);
            if (!hasFishNotSwimming)
                return;

            // Seleciona um peixe aleatório que não esteja a nadar
            FishData selectedFishData;
            do
            {
                int index = Random.Range(0, population.Count);
                selectedFishData = population[index];
            } while (selectedFishData.isSwimming);
            selectedFishData.isSwimming = true;

            bool leftSided = Random.Range(0, 2) == 0;
            Vector2 pos = new Vector2(leftSided ? minX : maxX, Random.Range(minY, maxY));

            // Escolhe o prefab baseado no tipo de peixe
            GameObject fishPrefab = GetFishPrefab(selectedFishData.fishType);
            GameObject newFish = Instantiate(fishPrefab, pos, Quaternion.identity);
            newFish.GetComponent<Fish>().SetData(selectedFishData, leftSided);

            // Rotaciona o peixe se necessário
            if (!leftSided)
            {
                newFish.GetComponent<SpriteRenderer>().flipX = true;
            }

            newFish.transform.parent = GameObject.Find("Fishes").transform;
        }
    }


    GameObject GetFishPrefab(FishType fishType)
    {
        return fishType switch
        {
            FishType.PufferFish => fishes[0],
            FishType.Tuna => fishes[2],
            FishType.Sardine => fishes[1],
            _ => null,
        };
    }

}
