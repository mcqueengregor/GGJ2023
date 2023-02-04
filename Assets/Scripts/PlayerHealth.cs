using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health Setup")]
    public int CurrentHea;
    public int MaxHea;
    

    

    [Header("Player Invincibility")]
    private SpriteRenderer SpRend;
    public int InvinvibilityTimer = 4;
    private bool PlayerHurt = false;
    private float SpriteFlicker = 0f;




    // Start is called before the first frame update
    void Start()
    {
        CurrentHea = MaxHea;
        SpRend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHurt == true)
        {
            SpriteFlicker += Time.deltaTime / 1f;

            if(SpriteFlicker >= 0.25f)
            {
                if (SpRend.enabled == true)
                {
                    SpRend.enabled = false;
                    SpriteFlicker = 0f;

                }
                else if (SpRend.enabled == false)
                {
                    SpRend.enabled = true;
                    SpriteFlicker = 0f;

                }
            }
        }
    }

    public void DamagePlayer(int Hit)
    {
        if (PlayerHurt == false)
        {
            PlayerHurt = true;
            StartCoroutine(Invulnerable());
            CurrentHea = CurrentHea - Hit;
        }
    }



    IEnumerator Invulnerable()
    {
        
        SpriteFlicker = 0f;
        yield return new WaitForSeconds(InvinvibilityTimer);
        PlayerHurt = false;
        SpRend.enabled = true;
    }

}
