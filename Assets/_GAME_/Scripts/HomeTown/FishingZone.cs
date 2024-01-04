using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Importe este namespace

public class FishingZone : MonoBehaviour
{
    public TextMeshProUGUI fishingZoneMessage;
    public string sceneToLoad; // Nome da cena a ser carregada
    private bool isPlayerInZone = false; // Flag para verificar se o jogador está na zona

    private void Awake()
    {
        if (fishingZoneMessage != null)
        {
            fishingZoneMessage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fishingZoneMessage.gameObject.SetActive(true);
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fishingZoneMessage.gameObject.SetActive(false);
            isPlayerInZone = false;
        }
    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad); // Carrega a nova cena
        }
    }
}
