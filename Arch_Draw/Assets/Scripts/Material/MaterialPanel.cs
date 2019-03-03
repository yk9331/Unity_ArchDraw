using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialPanel : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    MaterialBunch materialBunchPrefab;


    private void Start() {
        ScrollRect scrollrect = GetComponent<ScrollRect>();
        RectTransform content = GetComponent<ScrollRect>().content;
        var group = from materialData in MaterialList.instance.materials group materialData by materialData.type;
        print(group.Count());

        foreach (MaterialType type in MaterialList.instance.myMaterialsType) {
            //print(type.ToString());
            var materials = group.SingleOrDefault(g => g.Key == type)?.ToList() ?? new List<MyMaterial>(0) ;
            //print(materials.Count);
            if(materials.Count>0){
                var bunch = Instantiate(materialBunchPrefab, content);
                bunch.Setup(type, materials,scrollrect);
            }
        }
    }
}
