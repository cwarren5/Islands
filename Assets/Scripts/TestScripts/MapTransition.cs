﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private MeshRenderer waterMaterial = default;
    [SerializeField] private float transitionSpeed = 1;
    private float startingWaterTans = default;
    private float startingYScale = default;
    private float currentScale = default;
    private float lerpProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        startingYScale = transform.localScale.y;
        startingWaterTans = waterMaterial.material.color.a;
        currentScale = startingYScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && lerpProgress < 1)
        {
            LerpTerrainScale();
            LerpWaterTransparency();

            lerpProgress += Time.deltaTime * transitionSpeed;
        }
        else if (!Input.GetMouseButton(1) && lerpProgress > 0)
        {
            LerpTerrainScale();
            LerpWaterTransparency();

            lerpProgress -= Time.deltaTime * transitionSpeed;
        }
    }

    private void LerpWaterTransparency()
    {
        Color currentWaterColor = waterMaterial.material.color;
        currentWaterColor.a = Mathf.Lerp(startingWaterTans, 0, lerpProgress);
        waterMaterial.material.color = currentWaterColor;
    }

    private void LerpTerrainScale()
    {
        currentScale = Mathf.Lerp(startingYScale, .1f, lerpProgress);
        transform.localScale = new Vector3(transform.localScale.x, currentScale, transform.localScale.z);
    }
}
