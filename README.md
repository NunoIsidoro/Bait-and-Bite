# Bait and Bite
Este projeto é sobre um jogo de simulação Topdown 2D desenvolvido em Unity com C#. 
No jogo, o jogador pode controlar o personagem principal, mover-se pelo mapa com o objetivo capturar o máximo de peixes. 
A mecânica central do jogo combina inteligencia artificial e gestão de recursos, onde o jogador deve balancear a eficiência da pesca com a sustentabilidade da zona de pesca. 

## Grupo
- 20115 [Andre Cerqueira](https://github.com/AndreCerqueira)
- 20116 [Nuno Fernandes](https://github.com/NunoIsidoro)

## Funcionalidades
- Movimentação do jogador com NavMesh e PathFinding; 
- Sistema de captura de peixes;
- Exploração do mapa;
- Algoritmo de genética para reprodução dos peixes;
- Algoritmo de máquina de estados para o comportamento dos peixes.
- Inventário dos peixes.

## Como Jogar
- Use o butão esquerdo do rato para mover o personagem principal pelo cenário.
- Entre numa zona de pesca para capturar peixes.
- Desenvolva estratégias de pesca para que cada geração criada seja melhor que a anterior.

## Estrutura do Código
O projeto é dividido em vários scripts, cada uma responsável por um aspecto específico do jogo. Alguns dos mais importantes:

1. <b>Player_Controller:</b> Este script, é criado para controlar o personagem principal. Utiliza o NavMeshAgent da Unity para navegação e um Animator para animações.
2. <b>GeneticAlgorithm:</b> Simula um algoritmo genético aplicado a uma população de peixes.
3. <b>Fish:</b> É responsável por controlar o comportamento individual de peixes em um ambiente de jogo.
4. <b>FishData:</b> É uma classe que define os dados para um peixe individual. 

## Análise do Código
### Player_Controller
O script é responsável pelo controle de movimento e animação do personagem principal, utilizando o sistema de navegação da Unity (NavMesh).

**Geração da NavMesh**
A NavMesh no Unity é criada através do processo de "Triangulação de Polígonos", que envolve três etapas principais:

**Análise do Terreno e Obstáculos:** O Unity analisa a geometria do terreno e obstáculos presentes na cena para determinar áreas navegáveis.
**Decomposição do Espaço Navegável:** O espaço é dividido em polígonos (geralmente triângulos) para formar a NavMesh, baseando-se na geometria do ambiente e nas definições fornecidas.
**Otimização:** Os polígonos são otimizados para reduzir a complexidade computacional sem comprometer a precisão da navegação.

**Pathfinding com o Algoritmo A***
Para o movimento dos agentes, o Unity utiliza predominantemente o algoritmo A* (A estrela), conhecido pela sua eficiência e eficácia. O processo inclui:

**Nós e Custo:** O ambiente é dividido em "nós" com custos associados para movimento.
**Cálculo de Custo:** O A* calcula os custos de caminho considerando o custo real de movimento ('G') e uma estimativa heurística até o destino ('H').
**Seleção do Caminho:** O algoritmo seleciona o nó com o menor valor de 'F' (F = G + H), otimizando a distância e a estimativa até o objetivo.
**Evitando Obstáculos:** Obstáculos são contornados naturalmente, já que seus custos associados são altos.
**Encontrando o Melhor Caminho**: O algoritmo continua até achar o caminho mais eficiente até o destino.

**Considerações Adicionais**
**Performance:** Algoritmos como o A* podem ser intensivos em termos de computação, especialmente em grandes espaços de busca. O Unity otimiza isso com estruturas de dados eficientes e configurações ajustáveis da NavMesh.
**Variações do A:*** Dependendo das necessidades, variações do A* ou outros algoritmos podem ser utilizados.
Adaptações: Em jogos complexos, esses algoritmos são adaptados ou estendidos para lidar com requisitos específicos.

### GeneticAlgorithm
O script implementa um algoritmo genético simples para simular a evolução de uma população de peixes em um ambiente virtual.

### Código
```cs

    private void EvolvePopulation()
    {
        List<FishData> newPopulation = new List<FishData>(population); // Inicia com a população atual

        // Define um fator de proporção para reprodução
        int reproductionFactor = 5; // Uma reprodução para cada 10 peixes
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
            }
        }

        Debug.Log("New Population: " + newPopulation.Count);

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

private void ManageFoodSuply()
    {
        // Restore 100% of food at the beginning
        foodSuply = maxFoodSupply;

        float totalFoodConsumed = 0f;
        foreach (FishData fish in population)
        {
            // Define o consumo de comida baseado no tamanho do peixe, por exemplo
            float foodConsumed = fish.size * 3.25f; // Ajuste este fator conforme necessário
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
        for (int i = population.Count - 1; i >= 0; i--)
        {
            FishData fish = population[i];
            float chanceToSurvive = fish.resistence; // Resistência como percentagem de chance de sobreviver
            float randomChance = Random.Range(0f, 100f); // Gera um número aleatório entre 0 e 100

            if (randomChance > chanceToSurvive * 100f)
            {
                GameObject fishGameObject = GameObject.Find(fish.uid);
                if (fishGameObject != null)
                {
                    fishGameObject.GetComponent<Fish>().currentState = FishState.Dead;
                    Debug.Log($"Fish {fish.uid} died of starvation");
                }
                else
                {
                    Debug.Log($"Fish GameObject with UID {fish.uid} not found");
                }

                // Se o peixe não sobreviver, remove-o da população
                population.RemoveAt(i);
            }
        }
    }
```
**Definição de Variáveis e Estruturas de Dados:**
population: Uma lista estática pública de FishData, representando a população de peixes.
generation: Um contador estático público para rastrear as gerações da população.
maxFoodSupply e foodSupply: Variáveis para gerenciar a disponibilidade de comida no ambiente.
mutationRate: A taxa de mutação usada no algoritmo genético.
reproductionInterval e timeUntilNextReproduction: Controlam o intervalo de tempo para eventos de reprodução.
reproductionTimerText: Um elemento de UI para exibir o temporizador de reprodução.

**Inicialização (Start):**
Inicializa a população, define a taxa de mutação e o número de geração.
Carrega dados de população salvos ou inicia uma nova população.
Configura a reprodução periódica e inicia um contador regressivo para o próximo evento de reprodução.

**Evolução da População (EvolvePopulation):**
Cria uma nova população a partir da existente, aplicando reprodução e mutação.
Agrupa peixes por tipo para seleção de pais.
Implementa a função Crossover para misturar genes dos pais e Mutate para introduzir variações genéticas.
Atualiza a população e incrementa a geração.
Gerencia o suprimento de comida após a reprodução.

**Reprodução e Mutação:**
Crossover: Combinando características dos pais (velocidade, resistência, tamanho) para criar um novo peixe.
Mutate: Aplica mutações aleatórias às características do peixe.

**Gerenciamento de Comida (ManageFoodSuply):**
Calcula e subtrai o consumo total de comida pela população.
Se o suprimento de comida chega a zero, tenta eliminar peixes com base em sua resistência.

**Salvamento e Carregamento da População:**
Usa PlayerPrefs e JsonUtility para salvar e carregar dados da população.

**Atualização do Temporizador de Reprodução:**
Um Coroutine que atualiza um contador regressivo para o próximo evento de reprodução.

### FishData
É uma classe que representa os dados de um peixe individual em um ambiente de simulação.

### Código
```cs
[System.Serializable]
public class FishData
{
    public string uid;
    public FishType fishType;
    public float vel;
    public float resistence; 
    public int age; 
    public float size; 
    public bool isSwimming;

    public FishData(FishType fishType, float vel, float resistence, int age, float size)
    {
        uid = System.Guid.NewGuid().ToString();
        this.fishType = fishType;
        this.vel = vel;
        this.resistence = resistence;
        this.age = age;
        this.size = size;
    }


    public FishData DeepCopy()
    {
        return new FishData(this.fishType, this.vel, this.resistence, this.age, this.size);
    }

}

public enum FishType
{
    Tuna = 0,
    PufferFish = 1,
    Sardine = 2
}
```
**Variáveis de Instância:**
uid: Identificador único para cada peixe, gerado usando System.Guid.
fishType: Tipo do peixe, representado por um enum FishType.
vel: Velocidade do peixe.
resistence: Resistência do peixe, que influencia sua chance de sobreviver a causas naturais.
age: Idade do peixe, com um impacto na sua probabilidade de morrer conforme envelhece.
size: Tamanho do peixe, afetando a quantidade de comida que ele consome.
isSwimming: Um booleano que pode ser usado para indicar se o peixe está nadando atualmente.

**Construtor:**
A classe tem um construtor que inicializa as variáveis de instância com valores fornecidos e gera um uid único.

**Método DeepCopy:**
O método DeepCopy cria e retorna uma cópia profunda do objeto FishData. Isso é útil em situações onde uma cópia independente de um peixe é necessária, evitando alterações indesejadas no objeto original.

**Enumeração FishType**
Define três tipos de peixes: Tuna, PufferFish e Sardine. Cada tipo de peixe é representado por um valor numérico único (0, 1 e 2, respectivamente).

### Fish
é uma classe de controlo para um peixe individual em uma simulação ou jogo. 

### Código
```cs

public enum FishState
{
    Swimming,
    Hiding,
    Dead,
    Hooked
}

public class Fish : MonoBehaviour
{
    public FishState currentState;

    private bool fishCreated;
    private bool leftSided;
    private int dir => leftSided ? 1 : -1;

    public GameObject fishHookPrefab;
    public FishData fishData;
    private FishGenerator fishGenerator;


    void Start()
    {
        // Get data
        fishGenerator = FindAnyObjectByType<FishGenerator>();
    }


    public void SetData(FishData fishData, bool leftSided)
    {
        currentState = FishState.Swimming;
        this.leftSided = leftSided;
        this.fishData = fishData;
        transform.localScale = new Vector2(fishData.size, fishData.size);
        fishCreated = true;
    }


    void Update()
    {
        if (!fishCreated) return;

        switch (currentState)
        {
            case FishState.Swimming:
                Swim();
                break;
            case FishState.Hooked:
                Hooked();
                break;
            case FishState.Dead:
                Die();
                break;
        }

        CheckForGetFished();
    }


    // if mouse left clicked above fish destroy it
    void CheckForGetFished()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                currentState = FishState.Hooked;

                // Create fish hook
                var fishHook = Instantiate(fishHookPrefab, transform.position, Quaternion.identity);
                fishHook.transform.SetParent(transform);
            }
        }
    }


    void Swim()
    {
        var x = transform.position.x + fishData.vel * Time.deltaTime * dir;
        transform.position = new Vector2(x, transform.position.y);

        if (transform.position.x > fishGenerator.maxX && leftSided || transform.position.x < fishGenerator.minX && !leftSided)
        {
            currentState = FishState.Hiding;
            fishData.isSwimming = false;
            Destroy(gameObject);
        }
    }


    void Hooked()
    {
        var y = transform.position.y + 4 * Time.deltaTime;
        transform.position = new Vector2(transform.position.x, y);

        if (transform.position.y > fishGenerator.maxX)
        {
            int currentAmount = PlayerPrefs.GetInt(fishData.fishType.ToString());
            PlayerPrefs.SetInt(fishData.fishType.ToString(), currentAmount + 1);

            Destroy(gameObject);
            
            Debug.Log("Fish removed from population: " + fishData.fishType);
            GeneticAlgorithm.population.Remove(fishData);
        }
    }


    void Die()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        if (transform.position.y < fishGenerator.minY)
        {
            Destroy(gameObject);
            Debug.Log("Fish removed from population: " + fishData.fishType);
            GeneticAlgorithm.population.Remove(fishData);
        }
    }
}
```

**Enumeração FishState**
Define estados possíveis para o peixe: Swimming, Hiding, Dead, Hooked.

**Classe Fish**

Variáveis e Propriedades:
currentState: O estado atual do peixe.
fishCreated: Booleano para verificar se o peixe foi criado corretamente.
leftSided: Booleano que indica se o peixe está voltado para a esquerda.
dir: Direção do movimento do peixe, baseada em leftSided.
fishHookPrefab: Prefab do anzol que pode ser associado ao peixe.
fishData: Dados do peixe, possivelmente utilizando a classe FishData analisada anteriormente.
fishGenerator: Referência a um FishGenerator, que provavelmente é usado para gerenciar aspectos da simulação de peixes.

Métodos:
Start: Inicializa o fishGenerator.
SetData: Configura os dados e propriedades iniciais do peixe.
Update: Verifica o estado atual do peixe e executa ações correspondentes. Também verifica se o peixe foi "pescado" com um clique do mouse.
CheckForGetFished: Detecta se o peixe foi clicado e muda seu estado para Hooked.
Swim: Controla o movimento do peixe enquanto nada.
Hooked: Gerencia o comportamento do peixe quando fisgado.
Die: Define o comportamento do peixe quando morre.

**Funcionalidade do Script**
Ciclo de Vida do Peixe: O peixe inicia no estado Swimming e pode mudar para Hiding, Dead, ou Hooked dependendo de interações no jogo.
Interação do Usuário: O peixe pode ser "pescado" (estado Hooked) através de um clique do mouse.
Movimento e Comportamento: O peixe se move horizontalmente na tela quando está nadando. Se alcançar um limite definido (provavelmente os limites da área de jogo), ele se esconde e é destruído.
Captura e Morte: Quando fisgado ou morto, o peixe é removido da simulação e destruído.
