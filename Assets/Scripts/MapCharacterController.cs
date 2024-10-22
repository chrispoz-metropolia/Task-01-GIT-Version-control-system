using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCharacterController : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.manager.currentLevel == Levels.NO_LEVEL) return;

        Transform level = GameObject.Find("Level" + ((int)GameManager.manager.currentLevel)).transform;
        transform.position = level.GetChild(2).transform.position;
        level.GetComponent<LoadLevel>().Cleared(true, GameManager.manager.currentStars);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(new Vector2(horizontalMovement, verticalMovement));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("LevelTrigger")) return;

        Levels sceneToLoad = collision.GetComponent<LoadLevel>().sceneToLoad;
        GameManager.manager.currentLevel = sceneToLoad;
        GameManager.manager.currentStars = 0;
        SceneManager.LoadScene("Level" + ((int)sceneToLoad));
    }
}
