using UnityEngine;
using System.Collections;

public abstract class Actor : MonoBehaviour {

    public bool choosen = false;
    public abstract void DoAction(GameObject requester);
}
