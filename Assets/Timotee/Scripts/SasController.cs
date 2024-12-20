using System;
using DefaultNamespace;
using UnityEngine;

public class SasController : MonoBehaviour
{
    [Header("Sas Settings")] 
    [SerializeField] private bool _sasEntry = true;


    [NonSerialized] public int mapId;
    
    private static Action OnSasEntry;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        if (_sasEntry)
        {
            if (MapManager.CurrentId == mapId)
                MapManager.LoadNextMap();
        }
        else
            MapManager.UnloadPreviousMap(mapId);

    }
}