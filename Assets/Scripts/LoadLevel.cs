using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public Levels sceneToLoad;

    private void Start()
    {
        LevelData levelData;
        if (GameManager.manager.levelCleared.TryGetValue(sceneToLoad, out levelData))
        {
            Cleared(levelData.cleared, levelData.stars);
        }
    }

    public void Cleared(bool isCleared, int stars)
    {
        if (!isCleared) return;

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        GameObject starsText = transform.GetChild(1).gameObject;
        starsText.GetComponent<MeshRenderer>().enabled = true;
        starsText.GetComponent<TextMesh>().text = stars + "/3 ★";
        GetComponent<CircleCollider2D>().enabled = false;

        if (!GameManager.manager.levelCleared.ContainsKey(sceneToLoad))
        {
            GameManager.manager.levelCleared.Add(sceneToLoad, new LevelData());
        }

        LevelData levelData = GameManager.manager.levelCleared[sceneToLoad];
        levelData.cleared = isCleared;
        if (stars > levelData.stars)
        {
            levelData.stars = stars;
        }
    }
}
