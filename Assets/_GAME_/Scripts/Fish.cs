using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private bool fishCreated;
    private bool leftSided;
    private int dir => leftSided ? 1 : -1;

    private FishData fishData;
    private FishGenerator fishGenerator;


    void Start()
    {
        // Get data
        fishGenerator = FindAnyObjectByType<FishGenerator>();
    }


    public void SetData(FishData fishData, bool leftSided)
    {
        this.leftSided = leftSided;
        this.fishData = fishData;
        transform.localScale = new Vector2(fishData.size, fishData.size);
        fishCreated = true;
    }


    void Update()
    {
        if (!fishCreated) return;


        var x = transform.position.x + fishData.vel * Time.deltaTime * dir;
        transform.position = new Vector2(x, transform.position.y);


        if (transform.position.x > fishGenerator.maxX && leftSided || transform.position.x < fishGenerator.minX && !leftSided)
        {
            fishData.isSwimming = false;
            Destroy(gameObject);
        }

        // if mouse left clicked above fish destroy it
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Destroy(gameObject);

                // Remove fish from population

                Debug.Log("Fish removed from population: " + fishData.fishType);

                GeneticAlgorithm.population.Remove(fishData);
            }
        }
    }
}
