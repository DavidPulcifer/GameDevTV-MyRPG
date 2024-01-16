using Cinemachine;
using RPG.Attributes;
using RPG.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform repawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = 0.2f;
        [SerializeField] float healthRegenPercentage = 20;
        [SerializeField] float enemyHealthRegenPercentage = 20;

        private void Awake()
        {
            GetComponent<Health>().onDie.AddListener(Respawn);
        }

        private void Start()
        {
            if(GetComponent<Health>().IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }        

        IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            RespawnPlayer();
            ResetEnemies();
            yield return fader.FadeIn(fadeTime);

        }

        private void ResetEnemies()
        {
            foreach (AIController enemyController in FindObjectsOfType<AIController>())
            {
                Health health = enemyController.GetComponent<Health>();
                if(health && !health.IsDead())
                {
                    enemyController.Reset();
                    health.Heal(health.GetMaxHealthPoints() * enemyHealthRegenPercentage / 100);
                }
            }
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = repawnLocation.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(repawnLocation.position);
            Health health = GetComponent<Health>();
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if(activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }
    }

}

