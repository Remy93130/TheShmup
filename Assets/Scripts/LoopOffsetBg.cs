﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopOffsetBg : MonoBehaviour
{
  
    void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;

        

        

        
        offset.y = GameObject.FindGameObjectWithTag("Player").transform.position.y / 100;

        offset.x += Time.deltaTime / 10f;

        mat.mainTextureOffset = offset;


    }
}
