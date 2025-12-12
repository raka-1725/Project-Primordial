# Project Primordial

<img src="Assets\Documents\Project-Primordial_Logo.png" width = 300>


---

* Project Configuration

|Engine|Version|Rendering|Cinemachine|
|------|-------|---------|-----------|
|Unity| 6000.1.17f1|HDRP|3.1.0|




---

# Team BBB

---
- [Overview](#overview)
- [Feature](#features)
- [Core Classes](#core-classes)
- [Features To Implement](#features-to-implement)
- [Issues/Bugs](#issuesbugs)
- [Credits](#credits)
---

# Overview
A puzzle and escape game with magic combat and combat ability system included with Isometric camera view.

---

# Features

- ## WASD Character Controller
    - WASD key binding for character movement
    - Mouse click for attack
    - Number keys & Mouse wheel for selecting magic
- ## Magic Attack
    - Fire, Ice, Area magic attack
    - Customizebale settings
- ## Player Interacting Map Objects
    - Keys to open/Interacting with doors
    - Interactable doors
- ## Procedual Enemy Spawn
    - Player decting spawn
    - Ramdomized Spawn Mechanics

---

# Core Classes
```MovementController.cs``` - Handles character movent with new Unity Input system

```Player.cs``` - Handles Player's health and Win/Lose state

```SProjectileLogic.cs``` - Contorlls magic attack's projectile motion and atttack to the enemy

```SMagicAttackController.cs``` - Controlls all magic attack and cooldown. Controlls Player attack animation and handles each magic's data

```SMagicAttackData.cs``` - `ScriptableObject` - Contains all settings for each magic attack

```Enemy.cs``` - Handles Enemy behaviour made with BehaviourGraph. Various settings for each type of enemy and customizable chase/patrol settings

```EnemySpawner.cs``` - Handles spawning enemy with player priority and weight settings

---

# Captures

<img src="Assets\Documents\PP-GameCap_1.png" width = 400>
<img src="Assets\Documents\PP-GameCap_2.png" width = 400>




---

# Issues/Bugs
    - Known bugs - V1.0
      - Enemy stay still
      - Button Scale
      - Key Rebind
      - Point & Click Attack Sensitivity
      - Elements spawn at same spawner

---


---
