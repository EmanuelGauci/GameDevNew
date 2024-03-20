using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    public void RunPayload() {
        Destroy(this.gameObject);
    }
}
