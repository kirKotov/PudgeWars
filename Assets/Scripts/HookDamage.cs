using UnityEngine;

public class HookDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag != other.gameObject.tag && !other.gameObject.CompareTag("Map"))
        {
            Destroy(other.gameObject);
        }
    }
}