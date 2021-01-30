using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsManager : MonoBehaviour
{
    public bool hasHeliHat;
    public bool hasSuperShoes;

    // Start is called before the first frame update
    void Start()
    {
        hasHeliHat = false;
        hasSuperShoes = true;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHeliHat()
    {
        hasHeliHat = true;
    }

    public void LoseHeliHat()
    {
        hasHeliHat = false;
    }

    public void GetSuperShoes()
    {
        hasSuperShoes = true;
    }

    public void LoseSuperShoes()
    {
        hasSuperShoes = false;
    }
}
