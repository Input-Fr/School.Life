using UnityEngine;
using System;
using System.Threading.Tasks;
public class waitForTp : MonoBehaviour
{
    bool hasBeenTp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenTp && new Vector3(-73.394f,27.19f,0.52f) == this.transform.position)
        {
            tpAfterDelay();
            hasBeenTp = false;
        }
    }

    async void tpAfterDelay()
    {
        hasBeenTp = true;
        await Task.Delay(30000);
        this.transform.position = new Vector3(-76.32f,1.4312f,-14.56f);
    }
}
