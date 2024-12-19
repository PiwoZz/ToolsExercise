using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject Cube1M, Slope1M, CuttedSlope2M_P1, CuttedSlope2M_P2;
    [SerializeField] 
    private string _mapPath;
    
    Dictionary<string, GameObject> modelDico = new Dictionary<string, GameObject>();
    Dictionary<string, Quaternion> quaternionDico = new Dictionary<string, Quaternion>();

    // Start is called before the first frame update
    void Start()
    {
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

        using (StreamReader reader = new StreamReader(_mapPath))
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
                            GameObject obj = Cube1M;
                            modelDico.TryGetValue(content[currentColumn], out obj);

                            Quaternion quaternion = new Quaternion();
                            quaternionDico.TryGetValue(content[currentColumn], out quaternion);

                            if (obj == null) continue;
                            var newObj = Instantiate<GameObject>(obj, new Vector3(-currentColumn, i, currentLine), quaternion);
                            newObj.transform.SetParent(transform);
                        }
                    }
                }
                currentLine++;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
