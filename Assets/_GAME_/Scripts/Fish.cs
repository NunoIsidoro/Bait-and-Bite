using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
