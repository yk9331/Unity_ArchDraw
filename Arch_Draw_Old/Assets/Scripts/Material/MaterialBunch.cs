using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialBunch : MonoBehaviour {

    [SerializeField]
    MaterialPost materialPostPrefab;
    [SerializeField]
    Text materialType;
    


    public void Setup(MaterialType type, List<MyMaterial> materials,ScrollRect parent){
        RectTransform content = GetComponent<RectTransform>();
        materialType.text = type.ToString();
        if(materials!=null){
            foreach(MyMaterial m_Mat in materials){
                var materialPost = Instantiate(materialPostPrefab, content);
                materialPost.Setup(m_Mat,parent);
            }
        }
    }
}
