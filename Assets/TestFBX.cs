using UnityEngine;
using System.Collections;

public class TestFBX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnAttackStart()
    {
        Debug.Log("1111111111111111111111111111111111111");
    }

    public void OnAttackOn(int i)
    {
        Debug.Log(i+"-----------------------------");
    }
}
