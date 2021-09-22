using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public SlingShooter SlingShooter;
    public TrailController trailController;
    public List<Bird> Birds;
    public List<Enemy> enemies;
    private Bird _shotBird;
    public BoxCollider2D TapColider;

    private bool _isGameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<Birds.Count;i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapColider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    public void AssignTrail(Bird bird)
    {
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());
        TapColider.enabled = true;
    }

    void OnMouseUp()
    {
        if(_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }


    public void ChangeBird()
    {
        TapColider.enabled = false;
        if(_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if(Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for(int i = 0;i<enemies.Count; i++)
        {
            if(enemies[i].gameObject == destroyedEnemy)
            {
                enemies.RemoveAt(i);
                break;
            }
        }

        if(enemies.Count == 0)
        {
            _isGameEnded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
