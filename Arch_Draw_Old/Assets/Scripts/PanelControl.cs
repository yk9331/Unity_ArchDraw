using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour {

    [SerializeField]
    GameObject[] panels;



    public void OnMatBtnClick() {

        if (panels[0].gameObject.activeInHierarchy == false) {
            DisablAllPanels();
            panels[0].gameObject.SetActive(true);
        } else
            panels[0].gameObject.SetActive(false);
    }
    public void OnSaveBtnClick() {

        if (panels[1].gameObject.activeInHierarchy == false) {
            DisablAllPanels();
            panels[1].gameObject.SetActive(true);
        } else
            panels[1].gameObject.SetActive(false);
    }
    public void OnLoadBtnClick() {

        if (panels[2].gameObject.activeInHierarchy == false) {
            DisablAllPanels();
            panels[2].gameObject.SetActive(true);
            WorldMapManager.instance.GetFiles();
        } else
            panels[2].gameObject.SetActive(false);

    }

    public void OnModelBtnClick() {

        if (panels[3].gameObject.activeInHierarchy == false) {
            DisablAllPanels();
            panels[3].gameObject.SetActive(true);
        } else
            panels[3].gameObject.SetActive(false);
    }

    private void DisablAllPanels() {
        foreach (GameObject p in panels) {
            p.gameObject.SetActive(false);
        }
    }

    private void Start() {
        DisablAllPanels();
    }
}
