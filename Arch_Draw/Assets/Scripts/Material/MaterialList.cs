using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Hybrid.Generic;
using UnityEngine;



public enum MaterialType{
    Concrete ,
    Wood,
    Metal,
    Glass,
    Stone,
    Textiles,
    Brick,
    Paint,
    Fabric,
    Plaster
}

[System.Serializable]
public struct MyMaterial{
    [HideInInspector]
    public int Key { get; set; }
    public MaterialType type;
    public string materialName;
    public Sprite img;
    public Material pcMaterial;
    public Material mobileMaterial;
}

public class MaterialList : Singleton<MaterialList> {

    public MaterialType[] myMaterialsType = {MaterialType.Concrete,
        MaterialType.Wood,MaterialType.Metal,MaterialType.Glass,
        MaterialType.Stone,MaterialType.Textiles,MaterialType.Brick,
        MaterialType.Paint,MaterialType.Fabric,MaterialType.Plaster};
   
    public MyMaterial[] materials;
}
