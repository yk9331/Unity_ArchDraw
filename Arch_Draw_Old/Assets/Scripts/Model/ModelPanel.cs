using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelPanel : MonoBehaviour {
    [SerializeField]
    ModelBunch modelBunchPrefab;


    private void Start() {
        ScrollRect scrollrect = GetComponent<ScrollRect>();
        RectTransform content = GetComponent<ScrollRect>().content;

        var group = from modelData in ModelList.instance.models group modelData by modelData.type;
        print(group.Count());

        foreach (ModelType type in ModelList.instance.myModelType) {
            //print(type.ToString());
            var models = group.SingleOrDefault(g => g.Key == type)?.ToList() ?? new List<MyModel>(0);
            //print(materials.Count);
            if (models.Count > 0) {
                var bunch = Instantiate(modelBunchPrefab, content);
                bunch.Setup(type, models, scrollrect);
            }
        }
    }
}
