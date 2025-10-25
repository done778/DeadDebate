using UnityEditor.SearchService;
using UnityEngine;

//todo : manager (game, ui)
public class GameManager : MonoBehaviour
{
    #region field

    private static GameManager instance;
    private Camera cm;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    public GameObject player;
    public readonly float surviveTime = 600f;

    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        cm = Camera.main; 
        TimeManager.Instance?.Run();
    }

    // Update is called once per frame
    void Update()
    {
        cm.transform.position = new Vector3(player.transform.position.x, cm.transform.position.y, player.transform.position.z-18);
    }
}
