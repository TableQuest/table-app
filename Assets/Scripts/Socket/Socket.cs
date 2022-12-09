using System.Collections;
using UnityEngine;
using SocketIOClient;
using System.Threading;
using System.Collections.Concurrent;
using System;

public class Socket : MonoBehaviour
{
    public readonly ConcurrentQueue<Action> _mainThreadhActions = new ConcurrentQueue<Action>();
    public SocketIO client;

    public string clientId;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);
        // Create a new thread in order to run the InitSocketThread method
        var thread = new Thread(InitSocketThread);
        // start the thread
        thread.Start();


        // Wait until a callback action is added to the queue
        yield return new WaitUntil(() => _mainThreadhActions.Count > 0);

        // If this fails something is wrong ^^
        // simply get the first added callback
        if (!_mainThreadhActions.TryDequeue(out var action))
        {
            Debug.LogError("Something Went Wrong ! ", this);
            yield break;
        }

        // Execute the code of the added callback
        action?.Invoke();
        
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
            client = new SocketIO("http://localhost:3000/");
            await client.ConnectAsync();
            if (clientId != null)
            {
                await client.EmitAsync("tableConnection", "");
            }
        }
    }

}
