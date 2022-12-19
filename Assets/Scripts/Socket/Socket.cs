using System.Collections;
using UnityEngine;
using SocketIOClient;
using System.Threading;
using System.Collections.Concurrent;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class Socket : MonoBehaviour
{
    public readonly ConcurrentQueue<Action> _mainThreadhActions = new ConcurrentQueue<Action>();
    public SocketIO client;
    public string requestURI;

    public string clientId;
    // Start is called before the first frame update
    private void StartConnection()
    {
        DontDestroyOnLoad(gameObject);
        // Create a new thread in order to run the InitSocketThread method
        var thread = new Thread(InitSocketThread);
        // start the thread
        thread.Start();
        
        if (requestURI == null)
        {
            requestURI = "http://localhost:3000";
        }
        Debug.Log("Connection to : " + requestURI);
        
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil((() => _mainThreadhActions.Count > 0));
            if (!_mainThreadhActions.TryDequeue(out var action))
            {
                Debug.LogError("Something Went Wrong ! ", this);
                yield break;
            }
            
            action?.Invoke();
        }
    }

    async void InitSocketThread()
    {
        if (client == null)
        {
            client = new SocketIO(requestURI);
            await client.ConnectAsync();
            if (clientId != null)
            {
                await client.EmitAsync("tableConnection", "");
                _mainThreadhActions.Enqueue(() =>
                {
                    SceneManager.LoadScene("MainScene");
                });
            }
        }
    }

    public async void sendDebug(string message)
    {
        await client.EmitAsync("debugMessage", message);
    }
    
    public void ClickConnect()
    {
        var input = GameObject.Find("Input").GetComponent<TMP_InputField>();
        requestURI = "http://" + input.text + ":3000";
        StartConnection();
    }

}
