using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas GameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameOverCanvas.enabled = false; // when game starts the death canvas should be turned off.
                                        // it will latrer be triggered on when the player dies.
    }

    // This function handles the death canvas for the game. 
    public void HandleDeath()
    {
        GameOverCanvas.enabled = true;
        Time.timeScale = 0; // stops time so that the game does not continue going on while player is dead
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        Cursor.lockState = CursorLockMode.None; // unlocks the cursor from its frozen form during gameplay so that it can be used here.
        Cursor.visible = true; // turns the cursor visible.
    }
}
