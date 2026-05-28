using UnityEngine;

public class CrystalCollect : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Cristal recogido");

            CaveController.instance.AddCrystal(value);

            Destroy(gameObject);
        }
    }
}