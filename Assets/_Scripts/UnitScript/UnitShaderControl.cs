using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShaderControl : MonoBehaviour
{
   Material  mat;
    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
                
        mat.SetFloat("_HitValue", 0f);
        mat.SetFloat("_OutlineValue", 0f);
      
    
        
    }
    private void Start()
    {
        SetUnitColor();     
    }

    public void SetUnitColor()
    {
        mat.SetColor("_PartColor", GetComponentInParent<Player>().PlayerColor);
    }
    public void HolyShieldON()
    {
        mat.SetFloat("_OutlineValue", 1f);

    }
    public void HolyShieldOff()
    {
        mat.SetFloat("_OutlineValue", 0f);

    }

    public void PlayUnitHit()
    {
        StartCoroutine(Hit());

        IEnumerator Hit()
        {
            mat.SetFloat("_HitValue", 1f);
            yield return Utilis.GetWait(0.1f);
            mat.SetFloat("_HitValue", 0f);
        }
    }
    public void FireDoTOn()
    {
        mat.SetFloat("_FlameValue", 1f);
    }

    public void FireDoTOff()
    {
        mat.SetFloat("_FlameValue", 0f);
    }




}
