using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Map
{
    public string filePath;
    public int id;
}

public class Sas
{
    public string filePath;
    public int firstMap;
    public int secondMap;
}

public class SceneGenerator : MonoBehaviour
{
    public static SceneGenerator instance;
    public bool sceneLoading = false;
    public bool sceneUnloading = false;

    private List<GameObject> _previousMaps = new List<GameObject>();
    
    [SerializeField]
    GameObject Cube1M, Slope1M, CuttedSlope2M_P1, CuttedSlope2M_P2, chest;

    [SerializeField] private GameObject SasTrigger;
    [SerializeField] private GameObject SasUnTrigger;
    [SerializeField] 
    private string _mapPath;
    
    Dictionary<string, GameObject> modelDico = new Dictionary<string, GameObject>();
    Dictionary<string, Quaternion> quaternionDico = new Dictionary<string, Quaternion>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        modelDico.Add("0", Cube1M); quaternionDico.Add("0", Quaternion.Euler(0, 90, 90));//OK

        modelDico.Add("1", Slope1M); quaternionDico.Add("1", Quaternion.Euler(0, 90, 90));//OK
        modelDico.Add("2", Slope1M); quaternionDico.Add("2", Quaternion.Euler(0, 180, 90));//OK
        modelDico.Add("9", Slope1M); quaternionDico.Add("9", Quaternion.Euler(0, 0, 90));//OK
        modelDico.Add("10", Slope1M); quaternionDico.Add("10", Quaternion.Euler(0, -90, 90));//OK

        modelDico.Add("4", CuttedSlope2M_P1); quaternionDico.Add("4", Quaternion.Euler(-180, 0, -270));//OK
        modelDico.Add("5", CuttedSlope2M_P1); quaternionDico.Add("5", Quaternion.Euler(0, 180, 90));//OK
        modelDico.Add("12", CuttedSlope2M_P1); quaternionDico.Add("12", Quaternion.Euler(0, 0, 90));//OK
        modelDico.Add("13", CuttedSlope2M_P1); quaternionDico.Add("13", Quaternion.Euler(-180, 180, 90));//OK

        modelDico.Add("3", CuttedSlope2M_P2); quaternionDico.Add("3", Quaternion.Euler(-180, 0, -270));//OK
        modelDico.Add("6", CuttedSlope2M_P2); quaternionDico.Add("6", Quaternion.Euler(0, 180, 90));//OK
        modelDico.Add("11", CuttedSlope2M_P2); quaternionDico.Add("11", Quaternion.Euler(0, 0, 90));//OK
        modelDico.Add("14", CuttedSlope2M_P2); quaternionDico.Add("14", Quaternion.Euler(-180, 180, 90));//OK
        modelDico.Add("48", SasTrigger); quaternionDico.Add("48", Quaternion.Euler(0, 0, 0));
        modelDico.Add("16", SasUnTrigger); quaternionDico.Add("16", Quaternion.Euler(0, 0, 0));
        
        modelDico.Add("32", chest); quaternionDico.Add("32", Quaternion.Euler(0, 90, 90));
    }

    private bool MapAlreadyLoaded(string mapPath)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == mapPath)
                return true;
        }

        return false;
    }
    
    public IEnumerator GenerateScene(string[] sceneNames, Action _callback = null)
    {
        int mapId = MapManager.CurrentId;
        
        foreach (var sceneName in sceneNames)
        {
            if (MapAlreadyLoaded(sceneName))
                continue;
            
            var currentMap = new GameObject(sceneName);
            currentMap.transform.parent = transform;
        
            using (StreamReader reader = new StreamReader(sceneName))
            {
                int currentLine = 0;
                while (!reader.EndOfStream)
                {
                    string[] content = reader.ReadLine().Split(',');
                    for (int i = 0; i < 4; i++)
                    {
                        for (int currentColumn = 0; currentColumn < content.Length; currentColumn++)
                        {
                            if (content[currentColumn] != null && content[currentColumn] != "" && content[currentColumn] != "-1")
                            {
                                if ((content[currentColumn] == "48" || content[currentColumn] == "16") && i != 0) continue;
                                GameObject obj = Cube1M;
                                modelDico.TryGetValue(content[currentColumn], out obj);

                                Quaternion quaternion = new Quaternion();
                                quaternionDico.TryGetValue(content[currentColumn], out quaternion);

                                if (obj == null) continue;
                                var newObj = Instantiate<GameObject>(obj, new Vector3(-currentColumn, i, currentLine), quaternion);
                                newObj.transform.SetParent(currentMap.transform);

                                if (content[currentColumn] == "48" || content[currentColumn] == "16")
                                {
                                    var sasObj = newObj.GetComponent<SasController>();
                                    sasObj.mapId = mapId;
                                }
                            }
                        }
                    }
                    currentLine++;
                    yield return null;
                }
            }
        }
        
        sceneLoading = false;
        _callback?.Invoke();
    }

    /*public IEnumerator GenerateScene(string sceneNames, Action _callback = null)
    {
        if (_currentMap != null)
            _previousMaps.Add(_currentMap);
        
        _currentMap = new GameObject(sceneNames);
        _currentMap.transform.parent = transform;
        
        using (StreamReader reader = new StreamReader(sceneNames))
        {
            int currentLine = 0;
            while (!reader.EndOfStream)
            {
                string[] content = reader.ReadLine().Split(',');
                for (int i = 0; i < 4; i++)
                {
                    for (int currentColumn = 0; currentColumn < content.Length; currentColumn++)
                    {
                        if (content[currentColumn] != null && content[currentColumn] != "" && content[currentColumn] != "-1")
                        {
                            if (content[currentColumn] == "48" && i != 0) continue;
                            GameObject obj = Cube1M;
                            modelDico.TryGetValue(content[currentColumn], out obj);

                            Quaternion quaternion = new Quaternion();
                            quaternionDico.TryGetValue(content[currentColumn], out quaternion);

                            if (obj == null) continue;
                            var newObj = Instantiate<GameObject>(obj, new Vector3(-currentColumn, i, currentLine), quaternion);
                            newObj.transform.SetParent(_currentMap.transform);
                        }
                    }
                }
                currentLine++;
                yield return null;
            }
        }

        sceneLoading = false;
        _callback?.Invoke();
    }*/

    public void StartLoading(string[] mapNames, Action _callback = null)
    {
        sceneLoading = true;
        StartCoroutine(GenerateScene(mapNames, _callback));
    }

    public void StartUnloading(string[] mapNames)
    {
        
        
        sceneUnloading = true;
        StartCoroutine(UnloadScene(mapNames));
    }
    
    public IEnumerator UnloadScene(string[] mapNames)
    {
        int count = 0;

        foreach (var mapName in mapNames)
        {
            Transform previousMap = null;

            foreach (Transform child in transform)
            {
                if (child.gameObject.name == mapName)
                {
                    previousMap = child;
                    break;
                }
            }
            
            if (previousMap == null) continue;

            while (previousMap.childCount > 0)
            {
                if (count < previousMap.childCount)
                    Destroy(previousMap.GetChild(count).gameObject);
                
                if (previousMap.childCount == 0)
                    break;
                count++;
                if (count < 25) continue;
                
                count = 0;
                yield return null;
            }
        }
        
        sceneUnloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
