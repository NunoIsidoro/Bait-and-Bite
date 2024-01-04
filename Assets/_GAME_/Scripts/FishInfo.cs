using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishInfo : MonoBehaviour
{
    public TextMeshProUGUI sardine;
    public TextMeshProUGUI puffer;
    public TextMeshProUGUI tuna;

    // Update is called once per frame
    void Update()
    {
        sardine.text = "<color=#B3B5C7><size=65%>Sardine</size></color>\r\n" + PlayerPrefs.GetInt(FishType.Sardine.ToString()).ToString();
        puffer.text = "<color=#B3B5C7><size=65%>Puffer Fish</size></color>\r\n" + PlayerPrefs.GetInt(FishType.PufferFish.ToString()).ToString();
        tuna.text = "<color=#B3B5C7><size=65%>Tuna</size></color>\r\n" + PlayerPrefs.GetInt(FishType.Tuna.ToString()).ToString();
    }
}
