using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int moveDistance = 1;
    public int movesLeft = 3;
    public float Range = 1;
    public int Health = 1;
    public int Damage = 1;
    public bool belongsToPlayer = false;


    public void pathFound(Vector3[] waypoints, bool successful)
    {
        if(successful)
            StartCoroutine(followPath(waypoints));
        movesLeft -= waypoints.Length - 1;
        if (movesLeft <= 0)
        {
            GameManager.instance.unitsLeftInTurn--;
            GameManager.instance.updateTurn();
        }
    }

    IEnumerator followPath(Vector3[] waypoints)
    {
        if(waypoints.Length == 0)
        {
            yield break;
        }
        bool followingPath = true;
        int pathIndex = 0;
        float maxMoveTime = 1f;
        while (followingPath)
        {
            if (pathIndex == waypoints.Length - 1)
            {
                followingPath = false;
                break;
            }
            else
            {
                
                pathIndex++;
            }
            if (followingPath)
            {
                float currentMoveTime = 0;
                while (Vector3.Distance(transform.localPosition, waypoints[pathIndex]) > 0)
                {
                    currentMoveTime += Time.deltaTime;
                    transform.localPosition = Vector3.Lerp(transform.position, waypoints[pathIndex], currentMoveTime / maxMoveTime);
                    yield return null;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GameManager.instance.loseUnit(belongsToPlayer, this);
            Destroy(this.gameObject);
        }

    }
}
