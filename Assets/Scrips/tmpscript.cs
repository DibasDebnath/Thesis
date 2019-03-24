using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpscript : MonoBehaviour
{
    public CarAgent CA;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CA.m_horizontalInput == 1 && CA.m_verticalInput == 1)
        {
            Debug.Log("AAA");
        }
        if (CA.m_horizontalInput == -1 && CA.m_verticalInput == -1)
        {
            Debug.Log("AAA");
        }
        if (CA.m_horizontalInput == 1 && CA.m_verticalInput == -1)
        {
            Debug.Log("AAA");
        }
        if (CA.m_horizontalInput == -1 && CA.m_verticalInput == 1)
        {
            Debug.Log("AAA");
        }
    }
}
