using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeZone : MonoBehaviour
{
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(CaveController.instance.currentCrystals >=
               CaveController.instance.crystalsNeeded)
            {
                Debug.Log("Escapaste");

                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("Aun faltan cristales");
            }
        }
    }
}
