using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMusicPlayer : MonoBehaviour
{
    public GameObject musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject musicPlayerInstance = Instantiate(musicPlayer);
        DontDestroyOnLoad(musicPlayerInstance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
