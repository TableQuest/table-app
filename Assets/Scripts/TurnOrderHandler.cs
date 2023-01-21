using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderHandler : MonoBehaviour
{
    string turnGlobalID;
    int currentIndex;
    public List<Entity> turnOrderListEntity;
    SocketIO _client;
    // Start is called before the first frame update
    void Start()
    { 
        _client = GameObject.Find("SocketClient").GetComponent<Socket>().client;
    }

    public void TurnOrderOn(List<Entity> turnOrderListEntity)
    {
        turnGlobalID = turnOrderListEntity[0].globalId;
        this.turnOrderListEntity = turnOrderListEntity;
        this.currentIndex = 0;
    }

    public void TurnOrderNext()
    {
        currentIndex = currentIndex + 1 == turnOrderListEntity.Count ? 0 : currentIndex + 1;
        turnGlobalID = turnOrderListEntity[currentIndex].globalId;
    }

    public Entity getEntityTurn()
    {
        return turnOrderListEntity[currentIndex];
    }
    
    public void removeEntity(Entity entity)
    {
        int indexRemove = turnOrderListEntity.FindIndex(a => a == entity);
        if (indexRemove != -1)
        {
            currentIndex = currentIndex >= indexRemove && currentIndex != 0 ? currentIndex - 1 : currentIndex; 
            turnOrderListEntity.Remove(entity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
