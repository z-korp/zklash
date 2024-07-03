using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadScene : MonoBehaviour
{

    // On passe les objets que l'on veut mettre dans la scene don't destroy on load
    public GameObject[] objects;

    // On fait un singleton pour que ce soit plus facile à acceder
    public static DontDestroyOnLoadScene instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            //Debug.LogWarning("More than one instance of DontDestroyOnLoadScene");
            return;
        }
        instance = this;

        // On lui passe des objets et il va passer dessus pour ne pas les détruire à chaque fois que la scène change
        foreach (var element in objects)
        {
            // En fait ce que fait la fonction DontDestroyOnLoad c'est de mettre l'objet dans une autre scene, mais unity les joue quand même dans la scène où ils sont créés.
            DontDestroyOnLoad(element);
        }
    }

    public void RemoveFromDontDestroyOnLoad()
    {
        foreach (var element in objects)
        {
            // On prend l'element qui est dans la scène don't destroy on load et on le replace dans la scene actuelle
            SceneManager.MoveGameObjectToScene(element, SceneManager.GetActiveScene());
        }

    }
}
