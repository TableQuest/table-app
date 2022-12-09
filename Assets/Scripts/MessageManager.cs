using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 25 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high

    private GridManager grid;

    public GameState gameState;
    public OSC osc;
    public TextMeshPro text;
    public GameObject menuPrefab;
    public GameObject playerPrefab;
    public GameObject zoneMenuInitPrefab;

    List<TuioEntity> tuioEvents = new List<TuioEntity>();
    List<TuioEntity> deadTouches = new List<TuioEntity>();
    List<GameObject> zonesMenuActives = new List<GameObject>();

    private const string cursor = "/tuio/2Dcur";
    private const string obj = "/tuio/2Dobj";

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        osc.SetAddressHandler(cursor, Generate2DTUIOEvent);
        osc.SetAddressHandler(obj, Generate2DTUIOEvent);

        grid = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    private void Generate2DTUIOEvent(OscMessage oscM)
    {
        string message = GetMessage(oscM);
        ManageTuioObjectEvent(message, oscM.address);
    }

    private void ManageTuioObjectEvent(string message, string adress)
    {
        string[] messageTab = message.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        List<string> tmp = new List<string>(messageTab);
    //    Debug.Log(string.Join(",", tmp));
        switch (tmp[0])
        {
            case "alive":
                tmp.RemoveAt(0);
                UpdateCollection(tmp, adress);
                break;
            case "set":
                //5 et 11
              //  Debug.Log(tmp[5]);
               // Debug.Log(Int32.Parse(tmp[5])*(360/6.283175));
                tmp.RemoveAt(0);
                if (adress == obj)
                {
                    CheckObjectObj(tmp);
                }
                else
                    CheckObject(tmp);
                break;
            case "fseq":
                string str = "Voici les detections:\n";
                foreach (TuioEntity t in tuioEvents)
                {
                    str += t;
                    //cast an invisible ray that will collide with the first object
                    Camera cam = Camera.main;
                    float height = 2f * cam.orthographicSize;
                    float width = height * cam.aspect;

                    float xCoord = t.position.TUIOPosition.x / WIDTH_GRID_UNIT;
                    float yCoord = (t.position.TUIOPosition.y / HEIGHT_GRID_UNIT) ;
                    
                    
                    var vec = new Vector2(grid.GetTileAtPosition(0, 0).GetWidth() * xCoord, grid.GetTileAtPosition(0, 0).GetHeight() * yCoord);
                    
                    // Debug.Log("Position of vec : "+ vec.ToString());
                    
                    RaycastHit2D hitinfo = Physics2D.Raycast(vec, Vector2.zero);
                    if (hitinfo.collider != null)
                    {
                        if (hitinfo.transform.GetComponent<clickMenu>() != null)
                        {
                            hitinfo.transform.GetComponent<OSCEvent>().RunFunction(t); //will run specific function based on the state of the TUIOEvent
                        }
                    }
                }
                text.SetText(str);
             //   tuioEvents = tuioEvents.Except(deadTouches).ToList();
                break;
        }
    }

    private void CheckObject(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        // float xCoord = (int)(float.Parse(tmp[1]) / WIDTH_GRID_UNIT);
        // float yCoord = -(int)(float.Parse(tmp[2]) / HEIGHT_GRID_UNIT) + 14;
        float xCoord = float.Parse(tmp[1]);
        float yCoord = float.Parse(tmp[2]);
        TuioCursor tuioEvent = (TuioCursor)tuioEvents.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioCursor(id, xCoord, 1.0f - yCoord);
            tuioEvents.Add(tuioEvent);
            StartCoroutine(InstantiateType(tuioEvent));
        }
        else
        {
            Vector2 p = new Vector2(xCoord, 1.0f - yCoord);
            tuioEvent.UpdateCoordinates(p);
        }
    }

    private void CheckObjectObj(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        string value = tmp[1];
        float xCoord = float.Parse(tmp[2]);
        float yCoord = float.Parse(tmp[3]);
        float deg = 0f;

        if (tmp.Count > 4)
        {
            deg = float.Parse(tmp[4]) * Mathf.Rad2Deg;
        }

        TuioObject tuioEvent = (TuioObject)tuioEvents.Find(e => e.value == value);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioObject(id, xCoord, 1.0f - yCoord, value);
            tuioEvents.Add(tuioEvent);
            StartCoroutine(InstantiateType(tuioEvent));
        }
        else
        {
            Vector2 p = new Vector2(xCoord, 1.0f - yCoord);
            tuioEvent.UpdateCoordinates(p);
        }
        gameState.HandleTangibleEvents(value, new Vector2(xCoord, yCoord), deg);
    }

    private IEnumerator InstantiateType(TuioEntity tuioEvent)
    {
        yield return new WaitForSeconds(1.0f);
        if (tuioEvents.Contains(tuioEvent) && tuioEvent.State == TuioState.MAINTAIN_DOWN)
            tuioEvent.State = TuioState.LONG_CLICK;
    }

    private void UpdateCollection(List<string> idAlive, string adress)
    {
        if (adress == obj)
            deadTouches = tuioEvents.FindAll(e => !(e is TuioCursor || idAlive.Contains(e.Id.ToString())));
        else
            deadTouches = tuioEvents.FindAll(e => !(e is TuioObject || idAlive.Contains(e.Id.ToString())));
        foreach (TuioEntity t in deadTouches)
            t.State = TuioState.CLICK_UP;
    }

    private string GetMessage(OscMessage message)
    {
        string res = "";
        foreach (object obj in message.values)
        {
            res += obj + " ";
        }
        return res;
    }

    private void Update()
    {
        foreach (TuioEntity t in tuioEvents)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(t.position.TUIOPosition.x * Screen.width, t.position.TUIOPosition.y * Screen.height, 0));
        }
    }

    private string isInZone(Vector2 positionTangible)
    {
        foreach (GameObject zone in zonesMenuActives)
        {
            if (Vector2.Distance(positionTangible, zone.GetComponent<SpriteRenderer>().bounds.center) <= zone.GetComponent<SpriteRenderer>().bounds.extents.x)
            {
                Debug.Log("DANS ZONE");
                Destroy(GameObject.Find(zone.name));;
                string value = zone.name.Replace("zone", "");
                zonesMenuActives.Remove(zone);
                return value;
            }
        }
        return "";
    }

    private void createNewZoneMenu(float x, float y, string value)
    {

        GameObject tempZone = Instantiate(zoneMenuInitPrefab, new Vector3(100, 100, -10), Quaternion.identity);
        tempZone.name = "zone" + value;
        zonesMenuActives.Add(tempZone);
    }
}