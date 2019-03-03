using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelList : Singleton<ModelList> {

    public ModelType[] myModelType = {ModelType.Bed,ModelType.Carpet,
        ModelType.Chair,ModelType.Decoration,ModelType.Light,
        ModelType.Shelf,ModelType.Storage,ModelType.Table};

    public MyModel[] models;
}
public enum ModelType {
    Bed,
    Carpet,
    Chair,
    Decoration,
    Light,
    Shelf,
    Storage,
    Table
}

[System.Serializable]
public struct MyModel {
    [HideInInspector]
    public int Key { get; set; }
    public ModelType type;
    public string modelName;
    public Sprite img;
    public GameObject model;

}
