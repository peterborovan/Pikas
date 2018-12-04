using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FaceCard : MonoBehaviour {
    public string FaceFileName;
    public void ClickOnCard() {
        
        Debug.LogFormat("face={0}", FaceFileName);
        FacePickerManager.setSelectedFace(FaceFileName);
    }
}
