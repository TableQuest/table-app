using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    public GameState gameState;
    // public OnClickButton closeClick;
    public GameObject dicePanel;
    public GameObject successImage;
    public GameObject failImage;
    public Image playerImage;
    public GameObject close;
    public Socket socket;
    
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI diceValueText;
    public TextMeshProUGUI targetValueText;

    public string testName;
    
    private int _currentTargetValue;
    private string _currentPlayerId;
    private string _currentPawnCodeId;

    private void Start()
    {
        close.AddComponent<OnClickButton>();
        close.GetComponent<OnClickButton>().call = delegate
        {
            Debug.Log("Cick CLOSE !");
            dicePanel.SetActive(false);
            successImage.SetActive(false);
            failImage.SetActive(false);
            close.SetActive(false);
        };
        socket = GameObject.Find("SocketClient").GetComponent<Socket>();
    }

    public void OpenPanel(string playerId, int targetValue)
    {
        _currentPlayerId = playerId;
        _currentTargetValue = targetValue;
        
        var player = gameState._entityManager.GetPlayerWithGlobalId(playerId);
        _currentPawnCodeId = player.id;
        
        if (player.name == "Dwarf")
        {
            playerImage.sprite = Resources.Load<Sprite>("Images/dwarf");
        }
        else if (player.name == "Elf")
        {
            playerImage.sprite = Resources.Load<Sprite>("Images/elf");
        }
        playerNameText.text = player.name;
        targetValueText.text = targetValue.ToString();
        diceValueText.text = "?";
        dicePanel.SetActive(true);
        close.SetActive(true);
    }

    public void DiceRoll()
    {
        failImage.SetActive(false);
        successImage.SetActive(false);
        var diceValue = Random.Range(1, 20);
        diceValueText.text = diceValue.ToString();
        if (diceValue >= _currentTargetValue)
        {
            successImage.SetActive(true);
        }
        else
        {
            failImage.SetActive(true);
        }
    }

    public async void DiceRollWithoutTarget()
    {
        failImage.SetActive(false);
        successImage.SetActive(false);
        targetValueText.text = "?";
        var diceValue = Random.Range(1, 20);
        diceValueText.text = diceValue.ToString();
        var myData = new
        {
            playerId = _currentPawnCodeId,
            diceId = 4,
            value = diceValue
        };
        await socket.client.EmitAsync("dice", JsonConvert.SerializeObject(myData));
    }
}
