﻿using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class PlayerInputHandler : ITickable, IDisposable
    {
        private bool _tick;

        private GameInfo Info { get; set; }
        private BadgeSpawnPoint.Registry SpawnRegistry { get; set; }
        private BadgeSpawner BadgeSpawner { get; set; }
        private GameEvents GameEvents { get; set; }

        public PlayerInputHandler(GameInfo info,
            BadgeSpawnPoint.Registry spawnRegistry,
            BadgeSpawner badgeSpawner,
            GameEvents gameEvents)
        {
            Info = info;
            SpawnRegistry = spawnRegistry;
            BadgeSpawner = badgeSpawner;
            GameEvents = gameEvents;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
            GameEvents.BadgeSpawned += OnBadgeSpawned;
        }

        public void Tick()
        {
            if (_tick == false)
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (SpawnRegistry.Spawns.Any(spawn => spawn.gameObject.GetInstanceID() == hit.collider.gameObject.GetInstanceID()))
                {
                    var spawnPoint = hit.collider.GetComponent<BadgeSpawnPoint>();
                    if (spawnPoint.Badge != null)
                    {
                        return;
                    }

                    if (Input.GetMouseButton(1))
                    {
                        spawnPoint.OnMouseExit();
                        return;
                    }

                    spawnPoint.MakeVisible();

                    if (Input.GetMouseButtonDown(0))
                    {
                        BadgeSpawner.MakeStep(spawnPoint.Coordinates);
                        spawnPoint.OnMouseExit();
                    }
                }
            }
        }

        public void Dispose()
        {
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
            GameEvents.BadgeSpawned -= OnBadgeSpawned;
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState" || e.PropertyName == "ActivePlayer" || e.PropertyName == "ActivePlayerMadeSteps")
            {
                ValidateTick();
            }
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            if (isVictorious)
            {
                _tick = false;
            }
            else
            {
                ValidateTick();
            }
        }

        private void ValidateTick()
        {
            if (Info.GameState != GameStates.Started)
            {
                _tick = false;
                return;
            }
            if (Info.ActivePlayer.Type == PlayerTypes.AI)
            {
                _tick = false;
                return;
            }
            if (Info.ActivePlayerMadeSteps == Info.StepSize)
            {
                _tick = false;
                return;
            }
            _tick = true;
        }
    }
}