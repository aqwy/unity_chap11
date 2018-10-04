using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectibleItem : MonoBehaviour {
    [SerializeField] private string itemName;

    void OnTriggerEnter(Collider other)
    {
        /*Debug.Log("item collected: " + itemName);*/
        Managers.inventory.addItem(itemName);
        Destroy(this.gameObject);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
