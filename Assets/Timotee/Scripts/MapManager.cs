using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;
        
        private Dictionary<string, List<Map>> maps = new();
        private Dictionary<string, List<Sas>> sasList = new();

        [SerializeField] private string _currentMap;

        public static int CurrentId => instance._currentId;
        private int _currentId = 0;
        private int _lastUnloadedId = 0;

        public static int GetMapId(string mapPath)
        {
            return instance.maps[instance._currentMap].Find(obj => obj.filePath == mapPath).id;
        }
        
        private void Awake()
        {
            instance = this;
            
            foreach (var filePath in Directory.EnumerateFiles("Assets/Map"))
            {
                if (!filePath.EndsWith(".csv"))
                    continue;

                if (filePath.Split('_').Length == 1)
                    continue;

                string mapName = filePath.Split('_')[0].Split('\\').Last();
                string chunkName = filePath.Split('_')[1].Split('.')[0];
            
                bool isMap = chunkName.Split(' ')[0] == "Map";
            
                if (isMap && !maps.ContainsKey(mapName))
                    maps.Add(mapName, new List<Map>());
                else if (!isMap && !sasList.ContainsKey(mapName))
                    sasList.Add(mapName, new List<Sas>());
            
                if (isMap)
                {
                    int id = int.Parse(chunkName.Split(' ')[1]);
                    maps[mapName].Add(new Map { filePath = filePath, id = id });
                }
                else
                {
                    int firstMap = int.Parse(chunkName.Split(' ')[1].Split('-')[0]);
                    int secondMap = int.Parse(chunkName.Split(' ')[1].Split('-')[1]);
                    sasList[mapName].Add(new Sas { firstMap = firstMap, secondMap = secondMap, filePath = filePath });
                }
            }
        }

        public static void LoadNextMap()
        {
            instance.LocalLoadNextMap();
            
        }

        [ContextMenu("LoadNextMap")]
        private void LocalLoadNextMap()
        {
            _currentId++;

            Map map = maps[_currentMap].Find(obj => obj.id == _currentId);

            if (map == null)
                return;

            List<Sas> sas = sasList[_currentMap].FindAll(obj => obj.firstMap == _currentId || obj.secondMap == _currentId);

            List<string> names = new() {map.filePath};
            names.AddRange(sas.Select(obj => obj.filePath).ToList());
            
            SceneGenerator.instance.StartLoading(names.ToArray());
        }

        public static void UnloadPreviousMap(int id) => instance.LocalUnloadPreviousMap(id);
        
        [ContextMenu("UnloadMap")]
        private void LocalUnloadPreviousMap(int id)
        {
            if (_lastUnloadedId >= id) return;
            
            List<Map> previousMaps = maps[_currentMap].FindAll(obj => obj.id == id);
            List<Sas> previousSas = sasList[_currentMap].FindAll(obj => obj.firstMap == id);
            
            List<string> names  = previousMaps.Select(obj => obj.filePath).ToList();
            names.AddRange(previousSas.Select(obj => obj.filePath).ToList());
            SceneGenerator.instance.StartUnloading(names.ToArray());
        }
    }
}