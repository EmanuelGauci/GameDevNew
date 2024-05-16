using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    public void RunPayload() {
        SceneManager.LoadScene("BetweenLevels");
    }
}
