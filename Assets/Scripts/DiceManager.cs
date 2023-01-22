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

    public bool waitingSkill;

    private void Start()
    {
        socket = GameObject.Find("SocketClient").GetComponent<Socket>();
        close.AddComponent<OnClickButton>();
        close.GetComponent<OnClickButton>().call = CancelIfWaiting;
        
        waitingSkill = false;
    }

    public void WaitForSkill(string playerId, string skillName, int targetValue)
    {
        waitingSkill = true;
        OpenPanel(playerId, targetValue);
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

    public async void DiceRoll()
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
        var myData = new
        {
            playerId = _currentPawnCodeId,
            diceId = 4,
            value = diceValue,
            targetValue = _currentTargetValue
        };
        waitingSkill = false;
        await socket.client.EmitAsync("skillDice", JsonConvert.SerializeObject(myData));
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

    public void MovePanel(string id, Vector2 pos)
    {
        var vec = new Vector3(pos.x, pos.y, -10);
        dicePanel.transform.position = new Vector3(pos.x-30, pos.y-55, -10);
    }

    public void CancelIfWaiting()
    {
        dicePanel.SetActive(false);
        successImage.SetActive(false);
        failImage.SetActive(false);
        close.SetActive(false);
        if (waitingSkill)
        {
            socket.client.EmitAsync("cancelPendingSkill", "");
            waitingSkill = false;
        }
    }
}
