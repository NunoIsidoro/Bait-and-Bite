using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    static public List<FishData> population;
    static public int generation;

    private float maxFoodSupply = 100f;
    static public float foodSuply = 100f;

    private float mutationRate;
    private float reproductionInterval = 10f; // 1 minute
    private float timeUntilNextReproduction;
    public TextMeshProUGUI reproductionTimerText;

    void Start()
    {
        // Inicialização do algoritmo genético
        population = new List<FishData>();
        mutationRate = 0.01f; // Taxa de mutação, ajuste conforme necessário
        generation = 1;

        if (PlayerPrefs.HasKey("PopulationData"))
        {
            LoadPopulation(); // Carrega a população salva
        }
        else
        {
            LoadInitialPopulation();
        }

        InvokeRepeating("StartEvolution", 10f, reproductionInterval);

        timeUntilNextReproduction = reproductionInterval;
        StartCoroutine(UpdateReproductionTimer());
    }


    private void StartEvolution()
    {
        EvolvePopulation();
    }


    static public void SavePopulation()
    {
        string data = JsonUtility.ToJson(population);
        PlayerPrefs.SetString("Population", data);

        PlayerPrefs.SetInt("Generation", generation);
    }


    private void LoadPopulation()
    {
        string data = PlayerPrefs.GetString("Population");
        population = JsonUtility.FromJson<List<FishData>>(data);

        generation = PlayerPrefs.GetInt("Generation");
    }


    private void LoadInitialPopulation()
    {
        population = new List<FishData>
        {
            new FishData(FishType.PufferFish, 1.0f, 0.9f, 1, 1.0f),
            new FishData(FishType.Sardine, 2.0f, 0.8f, 2, 1.8f),
            new FishData(FishType.Tuna, 0.8f, 0.85f, 1, 0.5f),
            new FishData(FishType.PufferFish, 0.95f, 1.1f, 2, 1.2f),
            new FishData(FishType.Sardine, 2.1f, 0.93f, 3, 1.9f),
            new FishData(FishType.Tuna, 0.9f, 0.9f, 2, 0.6f),
            new FishData(FishType.PufferFish, 1.3f, 0.75f, 3, 1.3f),
            new FishData(FishType.Sardine, 2.2f, 0.7f, 4, 2.0f),
            new FishData(FishType.Tuna, 1.0f, 0.87f, 1, 0.7f),
            new FishData(FishType.PufferFish, 1.4f, 0.95f, 5, 1.4f)
        };

        Debug.Log("Initial population created: " + population.Count);
    }


    private void EvolvePopulation()
    {
        List<FishData> newPopulation = new List<FishData>(population); // Inicia com a população atual

        // Define um fator de proporção para reprodução
        int reproductionFactor = 10; // Uma reprodução para cada 10 peixes
        // Determina o número de reproduções com base no tamanho da população
        int numberOfReproductions = Mathf.Max(1, population.Count / reproductionFactor);

        for (int i = 0; i < numberOfReproductions; i++)
        {
            // Agrupa peixes por tipo
            Dictionary<FishType, List<FishData>> groupedByType = new Dictionary<FishType, List<FishData>>();
            foreach (var fish in population)
            {
                if (!groupedByType.ContainsKey(fish.fishType))
                {
                    groupedByType[fish.fishType] = new List<FishData>();
                }
                groupedByType[fish.fishType].Add(fish);
            }

            // Escolhe aleatoriamente um tipo de peixe para reprodução
            try
            {
                FishType selectedType = (FishType)Random.Range(0, groupedByType.Count);
                List<FishData> selectedGroup = groupedByType[selectedType];

                if (selectedGroup.Count >= 2)
                {
                    // Seleciona dois pais aleatoriamente (pode ser o mesmo peixe)
                    FishData parent1 = SelectParent(selectedGroup);
                    FishData parent2 = SelectParent(selectedGroup);

                    if (parent1 != null && parent2 != null)
                    {
                        FishData child = Crossover(parent1, parent2);
                        Mutate(child);
                        newPopulation.Add(child);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        population = newPopulation;
        generation++;
        ManageFoodSuply();
    }

    private FishData SelectParent(List<FishData> fishes)
    {
        int index = Random.Range(0, fishes.Count);
        return fishes[index]; // Retorna um peixe selecionado aleatoriamente
    }


    /*
        A função Crossover é responsável por combinar os genes de dois pais para criar um novo indivíduo (filho). 
        O objetivo é misturar os genes dos pais de maneira que o filho herde características de ambos. 
     */
    private FishData Crossover(FishData parent1, FishData parent2)
    {
        float vel = (parent1.vel + parent2.vel) / 2;
        float resistence = (parent1.resistence + parent2.resistence) / 2;
        float size = (parent1.size + parent2.size) / 2;
        int age = 1;

        return new FishData(parent1.fishType, vel, resistence, age, size);
    }


    /*
        A função Mutate introduz variações aleatórias nos genes de um indivíduo.
        Isso simula mutações biológicas e ajuda a manter a diversidade genética na população, 
        o que é crucial para a eficácia do algoritmo genético.
    */
    private void Mutate(FishData fish)
    {
        if (Random.value < mutationRate)
        {
            fish.vel += Random.Range(-0.1f, 0.1f);
            fish.vel = Mathf.Clamp(fish.vel, 0.5f, 3.0f);

            fish.size += Random.Range(-0.1f, 0.1f);
            fish.size = Mathf.Clamp(fish.size, 0.5f, 3.0f);
        }
    }


    private IEnumerator UpdateReproductionTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // Atualiza a cada segundo
            timeUntilNextReproduction -= 1;
            if (timeUntilNextReproduction <= 0)
            {
                timeUntilNextReproduction = reproductionInterval; // Reinicia o timer
            }

            // Atualiza o texto do timer na tela, se você tiver um elemento de UI para isso
            if (reproductionTimerText != null)
            {
                reproductionTimerText.text = $"Time to Reproduce: {timeUntilNextReproduction}";
            }
        }
    }

    private void ManageFoodSuply()
    {
        // Restore 40% of food at the beginning
        foodSuply = maxFoodSupply * 0.4f;

        float totalFoodConsumed = 0f;
        foreach (FishData fish in population)
        {
            // Define o consumo de comida baseado no tamanho do peixe, por exemplo
            float foodConsumed = fish.size * 1.25f; // Ajuste este fator conforme necessário
            totalFoodConsumed += foodConsumed;
        }

        // Subtrai o consumo total do suprimento de comida
        foodSuply -= totalFoodConsumed;
        if (foodSuply < 0) foodSuply = 0;

        // Verifica se o suprimento de comida está acabando
        if (foodSuply == 0)
        {
            TryToKillFishes();
        }
    }

    private void TryToKillFishes()
    {
        List<FishData> survivingFish = new List<FishData>();

        foreach (FishData fish in population)
        {
            float chanceToSurvive = fish.resistence; // Resistência como porcentagem de chance de sobreviver
            float randomChance = Random.Range(0f, 100f); // Gera um número aleatório entre 0 e 100

            if (randomChance <= chanceToSurvive)
            {
                survivingFish.Add(fish);
            }
        }

        population = survivingFish; 
    }
}
