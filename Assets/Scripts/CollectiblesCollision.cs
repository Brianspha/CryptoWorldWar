using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public Text collected;
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PowerUpGun":
                //Debug.Log("CollectedGun");
                collected.text = "Gun";
                Destroy(collision.gameObject);
                break;
            case "BulletPowerUp":
                //Debug.Log("Collected new Bullet");
                collected.text = "Bullet";
                Destroy(collision.gameObject);
                break;
            case "PowerUpArmor":
                //Debug.Log("Collected new Armor");
                collected.text = "Armor";
                Destroy(collision.gameObject);
                break;
            case "PowerUpStamina":
                //Debug.Log("Collected new Stamina");
                collected.text = "Stamina";
                Destroy(collision.gameObject);
                break;
            case "CollectibeTemplate":
               // collected.text = collision.gameObject.GetComponent<Collectible>().details.Name;
                Destroy(collision.gameObject);
                break;
            default:
                //Debug.Log("Found: "+ collision.gameObject.tag);
                break;
        }
        
    }
}
