using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLife : MonoBehaviour {
    public float duration = 1.0f;
	void Start () {
        Destroy(this.gameObject, 1.0f);
	}
}
