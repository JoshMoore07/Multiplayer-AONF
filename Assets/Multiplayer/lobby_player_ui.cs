using TMPro;
using UnityEngine;

public class lobby_player_ui : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    
    public void SetPlayer(string name)
    {
        playerNameText.text = name;
    }

    public void SetEmpty()
    {
        playerNameText.text = "WAITING FOR PLAYER...";
    }
}
