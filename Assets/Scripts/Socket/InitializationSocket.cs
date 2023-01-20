using UnityEngine;
using SocketIOClient;
using System.Threading;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;

public class InitializationSocket : MonoBehaviour
{
    public SocketIO _client;
    private Socket socket;
    private GameState _gameState;
    bool firstSwitch = true;

    void Start()
    {
        socket = GameObject.Find("SocketClient").GetComponent<Socket>();
        _client = socket.client;
        _gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        DefineCoroutines();
    }

    public void DefineCoroutines()
    {
        var thread = new Thread(RouteThread);
        thread.Start();

    }

    private void RouteThread()
    {
        while (_client == null)
        {
            _client = socket.client;
            Thread.Sleep(300);
        }

        _client.On("playerConnection", (data) =>
        {
            string str = data.GetValue<string>(0);

            socket._mainThreadhActions.Enqueue(() =>
            {
                _gameState._entityManager.RemoveHelper(str);
            });
        });


        _client.On("switchState", (data) =>
        {
            string str = data.GetValue<string>(0);
            switch (str)
            {
                case "FREE":
                    socket._mainThreadhActions.Enqueue(() =>
                    {
                        _gameState._state = STATE.PLAYING;
                        Debug.Log("changing to: " + _gameState._state);
                        if (firstSwitch)
                        {
                            _gameState._menuManager.populateMenu();
                            firstSwitch = false;
                        }
                    });
                    break;

                case "RESTRICTED":
                    socket._mainThreadhActions.Enqueue(() =>
                    {
                        _gameState._state = STATE.CONSTRAINT;
                        Debug.Log("changing to: " + _gameState._state);
                    });
                    break;

                default:
                    Debug.Log("State " + str + " is wrong or not implemented yet.");
                    break;
            }
        });


        _client.On("newNpc", (data) =>
        {
            socket._mainThreadhActions.Enqueue(() =>
            {
                string str = data.GetValue<string>(0);
                TempNpc npcData = JsonConvert.DeserializeObject<TempNpc>(str);
                _gameState._entityManager.CreateNewNpc(npcData.id, npcData.name); //normalement cet ID c'est celui du monstre (10: Goblin, 11: Ogre)
            });
        });

        _client.On("updateInfoCharacter", (data) =>
        {
            string str = data.GetValue<string>(0);
            Debug.Log("DATA EST :" + data);
            socket._mainThreadhActions.Enqueue(() =>
            {
                CharacterUpdateInfo cui = JsonConvert.DeserializeObject<CharacterUpdateInfo>(str);
                updateInfoCharacter(cui.playerId, cui.variable, cui.value);
                //GameObject.Find("Canvas").GetComponent<CharacterSceneManager>().PrintCharacterPanel(character);
            });
        });

        _client.On("resumeGame", (data) =>
        {
            socket._mainThreadhActions.Enqueue(() =>
            {
                Debug.Log("Received resumeGame");
                _gameState._state = _gameState._previousState;
                _gameState.WrongMove.SetActive(false);
            });
        }); 
        
        _client.On("pauseGame", (data) =>
        {
            string msg = data.GetValue<string>(0);

            if (_gameState._state != STATE.PAUSE)
            {
                socket._mainThreadhActions.Enqueue(() =>
                {
                    _gameState._previousState = _gameState._state;
                    _gameState._state = STATE.PAUSE;
                    _gameState.WrongMove.SetActive(true);
                    Debug.Log("GameState changed to PAUSE");
                    _gameState.WrongMove.transform.Find("ErrorMessage").GetComponent<TextMeshPro>().text = msg;
                });
            }
            else
            {
                socket._mainThreadhActions.Enqueue(() =>
                {
                    _gameState.WrongMove.transform.Find("ErrorMessage").GetComponent<TextMeshPro>().text += "\n" + msg;
                });

            }


        });
    }

    public void updateInfoCharacter(string playerId, string variable, string value)
    {
        Player character = _gameState._entityManager.GetPlayerWithGlobalId(playerId);
        switch (variable)
        {
            case "life":
                try
                {
                    character.life = Int32.Parse(value);
                }
                catch (Exception e)
                {
                    Debug.Log("Life value is not numerical: " + e);
                }
                break;
            case "lifeMax":
                try
                {
                    character.lifeMax = Int32.Parse(value);
                }
                catch (Exception e)
                {
                    Debug.Log("LifeMax value is not numerical: " + e);
                }
                break;
            case "mana":
                try
                {
                    Debug.Log(value);
                    character.mana = Int32.Parse(value);
                }
                catch (Exception e)
                {
                    Debug.Log("Mana value is not numerical: " + e);
                }
                break;
            case "manaMax":
                try
                {
                    character.manaMax = Int32.Parse(value);
                }
                catch (Exception e)
                {
                    Debug.Log("ManaMax value is not numerical: " + e);
                }
                break;
        }

        _client.On("pauseGame", (data) =>
        {
            string msg = data.GetValue<string>(0);

            if (_gameState._state != STATE.PAUSE)
            {
                socket._mainThreadhActions.Enqueue(() =>
                {
                    _gameState._previousState = _gameState._state;
                    _gameState._state = STATE.PAUSE;
                    _gameState.WrongMove.SetActive(true);
                    Debug.Log("GameState changed to PAUSE");
                    _gameState.WrongMove.transform.Find("ErrorMessage").GetComponent<TextMeshPro>().text = msg;
                });
            }
            else
            {
                socket._mainThreadhActions.Enqueue(() =>
                {
                    _gameState.WrongMove.transform.Find("ErrorMessage").GetComponent<TextMeshPro>().text += "\n" + msg;
                });

            }


        });
    }


    [Serializable]
    public class CharacterUpdateInfo
    {
        public CharacterUpdateInfo(string playerId, string variable, string value)
        {
            this.playerId = playerId;
            this.variable = variable;
            this.value = value;
        }

        public string playerId;
        public string variable;
        public string value;
    }


    public class TempNpc
    {
        public string description;
        public int id;
        public int life;
        public int lifeMax;
        public string name;
    }
}
