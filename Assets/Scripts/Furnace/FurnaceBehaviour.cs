using System.Collections;
using TMPro;
using UnityEngine;

public class FurnaceBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshPro InUseSignage;
    [SerializeField] string InUseMessage = "Melting Ice...";
    [SerializeField] OxygenComponent oxygen;
    [SerializeField] float burnTime = 4;
    [SerializeField] float oxygenGain;

    //can fuel not be entered
    [HideInInspector] public bool InUse;

    //player has entered fuel, give oxygen after a time
    IEnumerator BurnSequence()
    {
        InUse = true;
        InUseSignage.text = InUseMessage;

        yield return new WaitForSeconds(burnTime);

        oxygen.ChangeOxygen(oxygenGain);
        InUseSignage.text = "";
        InUse = false;
    }

    public float Burn()
    {
        StartCoroutine(BurnSequence());
        return oxygenGain;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
