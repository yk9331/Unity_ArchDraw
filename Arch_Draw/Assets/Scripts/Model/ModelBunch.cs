using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBunch : MonoBehaviour {

    [SerializeField]
    ModelPost modelPostPrefab;
    [SerializeField]
    Text modelType;



    public void Setup(ModelType type, List<MyModel> models, ScrollRect parent) {
        RectTransform content = GetComponent<RectTransform>();
        modelType.text = type.ToString();
        if (models != null) {
            foreach (MyModel m_Model in models) {
                var modelPost = Instantiate(modelPostPrefab, content);
                modelPost.Setup(m_Model, parent);
            }
        }
    }
}
