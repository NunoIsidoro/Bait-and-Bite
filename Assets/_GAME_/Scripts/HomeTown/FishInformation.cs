using UnityEngine;

public class FishInformation : MonoBehaviour
{
    public GameObject fishInfoPanel;
    public Player_Controller playerController;

    void Start()
    {
        if (fishInfoPanel != null)
            fishInfoPanel.SetActive(false);
        else
            Debug.LogError("fishInfoPanel n�o est� definido no Inspector!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFishInfo();
        }
    }

    public void ToggleFishInfo()
    {
        if (fishInfoPanel != null)
        {
            bool isActive = !fishInfoPanel.activeSelf;
            fishInfoPanel.SetActive(isActive);

            if (playerController != null)
                playerController.canMove = !isActive;
            else
                Debug.LogError("playerController n�o est� definido.");
        }
        else
        {
            Debug.LogError("fishInfoPanel n�o est� definido.");
        }
    }

    public void CloseFishInfo()
    {
        if (fishInfoPanel != null)
        {
            fishInfoPanel.SetActive(false);

            if (playerController != null)
                playerController.canMove = true;
            else
                Debug.LogError("playerController n�o est� definido.");
        }
    }
}




