using UnityEngine;
using System.Collections;

public abstract class Actor : MonoBehaviour {

    public abstract void DoAction(GameObject requester);
}
