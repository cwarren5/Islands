using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeTerrainTool : MonoBehaviour
{

    public enum DeformMode { RaiseLower, Flatten, Smooth }
    DeformMode deformMode = DeformMode.RaiseLower;
    string[] deformModeNames = new string[] { "Raise Lower", "Flatten", "Smooth" };

    public Terrain terrain;
    public Texture2D deformTexture;
    public float strength = 1;
    public float area = 1;
    public bool showHelp;

    Transform buildTarget;
    Vector3 buildTargPos;
    Light spotLight;

    //GUI
    Rect windowRect = new Rect(10, 10, 400, 185);
    bool onWindow = false;
    bool onTerrain;
    Texture2D newTex;
    float strengthSave;

    //Raycast
    private RaycastHit hit;

    //Deformation variables
    private int xRes;
    private int yRes;
    private float[,] saved;
    float flattenTarget = 0;
    Color[] craterData;

    TerrainData tData;

    float strengthNormalized
    {
        get
        {
            return (strength) / 9.0f;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Create build target object
        GameObject tmpObj = new GameObject("BuildTarget");
        buildTarget = tmpObj.transform;

        tData = terrain.terrainData;
        if (tData)
        {
            //Save original height data
            xRes = tData.heightmapResolution;
            yRes = tData.heightmapResolution;
            saved = tData.GetHeights(0, 0, xRes, yRes);
        }

        //Change terrain layer to UI
        terrain.gameObject.layer = 5;
        brushScaling();
    }

    // Update is called once per frame
    void Update()
    {
        raycastHit();

        if (onTerrain && !onWindow)
        {
            terrainDeform();
        }
    }

    void raycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = new RaycastHit();
        //Do Raycast hit only against UI layer
        if (Physics.Raycast(ray, out hit, 300, 1 << 5))
        {
            onTerrain = true;
            if (buildTarget)
            {
                buildTarget.position = Vector3.Lerp(buildTarget.position, hit.point + new Vector3(0, 1, 0), Time.time);
            }
        }
        else
        {
            if (buildTarget)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
                buildTarget.position = curPosition;
                onTerrain = false;
            }
        }
    }

    //TerrainDeformation
    //___________________________________________________________________________________________________________________
    void terrainDeform()
    {
        if (Input.GetMouseButtonDown(0))
        {
            buildTargPos = buildTarget.position - terrain.GetPosition();
            float x = Mathf.Clamp01(buildTargPos.x / tData.size.x);
            float y = Mathf.Clamp01(buildTargPos.z / tData.size.z);
            flattenTarget = tData.GetInterpolatedHeight(x, y) / tData.heightmapScale.y;
        }

        //Terrain deform
        if (Input.GetMouseButton(0))
        {

            buildTargPos = buildTarget.position - terrain.GetPosition();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                strengthSave = strength;
            }
            else
            {
                strengthSave = -strength;
            }

            if (newTex && tData && craterData != null)
            {
                int x = (int)Mathf.Lerp(0, xRes, Mathf.InverseLerp(0, tData.size.x, buildTargPos.x));
                int z = (int)Mathf.Lerp(0, yRes, Mathf.InverseLerp(0, tData.size.z, buildTargPos.z));
                x = Mathf.Clamp(x, newTex.width / 2, xRes - newTex.width / 2);
                z = Mathf.Clamp(z, newTex.height / 2, yRes - newTex.height / 2);
                int startX = x - newTex.width / 2;
                int startY = z - newTex.height / 2;
                float[,] areaT = tData.GetHeights(startX, startY, newTex.width, newTex.height);
                for (int i = 0; i < newTex.height; i++)
                {
                    for (int j = 0; j < newTex.width; j++)
                    {
                        if (deformMode == DeformMode.RaiseLower)
                        {
                            areaT[i, j] = areaT[i, j] - craterData[i * newTex.width + j].a * strengthSave / 15000;
                        }
                        else if (deformMode == DeformMode.Flatten)
                        {
                            areaT[i, j] = Mathf.Lerp(areaT[i, j], flattenTarget, craterData[i * newTex.width + j].a * strengthNormalized);
                        }
                        else if (deformMode == DeformMode.Smooth)
                        {
                            if (i == 0 || i == newTex.height - 1 || j == 0 || j == newTex.width - 1)
                                continue;

                            float heightSum = 0;
                            for (int ySub = -1; ySub <= 1; ySub++)
                            {
                                for (int xSub = -1; xSub <= 1; xSub++)
                                {
                                    heightSum += areaT[i + ySub, j + xSub];
                                }
                            }

                            areaT[i, j] = Mathf.Lerp(areaT[i, j], (heightSum / 9), craterData[i * newTex.width + j].a * strengthNormalized);
                        }
                    }
                }
                tData.SetHeights(x - newTex.width / 2, z - newTex.height / 2, areaT);
            }
        }
    }

    void brushScaling()
    {
        //Apply current deform texture resolution 
        newTex = Instantiate(deformTexture) as Texture2D;
        TextureScale.Point(newTex, deformTexture.width * (int)area / 20, deformTexture.height * (int)area / 10);
        newTex.Apply();
        craterData = newTex.GetPixels();
    }
}
