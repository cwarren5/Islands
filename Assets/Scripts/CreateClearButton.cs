using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateClearButton : MonoBehaviour
{
    private MeshRenderer thisRenderer;
    public Terrain terrain;
    private TerrainData tData;
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<MeshRenderer>();
        thisRenderer.enabled = false;
        tData = terrain.terrainData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {       
        int resolution = 129;
        float[,] heightArray = new float[resolution, resolution];
        tData.SetHeights(0, 0, heightArray);
    }

    private void OnMouseEnter()
    {
        thisRenderer.enabled = true;
    }

    private void OnMouseExit()
    {
        thisRenderer.enabled = false;
    }
}
