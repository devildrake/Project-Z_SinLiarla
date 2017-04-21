/*Este código está pensado para manejar la lógica de selección y movimiento de los zombies, así como el listado de estos 
 y de los villagers en partida*/
/*Se hace una referencia general al InputHandler
*/
using UnityEngine;
using System.Collections;

public class MusicPlaylist : MonoBehaviour
{
    public bool ActivateOnAwake = true;
    public AudioClip[] MusicList;

    void Awake()
    {
        if (ActivateOnAwake && MusicManager.Instance)
            MusicManager.Instance.ChangePlaylist(this);
    }

    void Start()
    {
        // Have playlist persist across scenes.
        DontDestroyOnLoad(gameObject); // Don't destroy this object

        // When a new scene is loaded, destroy the other playlists.
        foreach (MusicPlaylist playlist in GameObject.FindObjectsOfType<MusicPlaylist>())
        {
            if (playlist.name != this.name)
            {
                Destroy(playlist.gameObject);
            }
        }
    }

}