using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Animation : MonoBehaviour
{
    public Renderer meshRenderer;
    public float speed = 0.5f;

    // truquear una mmda para que vaya más rápido

    void Update()
    {
        // Vector2 offset = meshRenderer.material.mainTextureOffset;
        // offset = offset + new Vector2(0, speed * Time.deltaTime);
        // meshRenderer.material.mainTextureOffset = offset;


        meshRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
