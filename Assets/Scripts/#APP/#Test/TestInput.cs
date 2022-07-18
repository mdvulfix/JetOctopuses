using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log($"Key {KeyCode.C} was pushed down. Scene Core must be loaded.");
            //LoadRequired?.Invoke(m_Core);
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {   
            Debug.Log($"Key {KeyCode.L} was pushed down. Scene Login must be loaded.");
            //LoadRequired?.Invoke(m_Login);
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            Debug.Log($"Key {KeyCode.M} was pushed down. Scene Menu must be loaded.");
            //LoadRequired?.Invoke(m_Menu);
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            Debug.Log($"Key {KeyCode.G} was pushed down. Scene Level must be loaded.");
            //LoadRequired?.Invoke(m_Level);
        }
    }
}
