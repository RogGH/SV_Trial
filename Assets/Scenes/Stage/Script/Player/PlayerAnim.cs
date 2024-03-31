using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    Animator animator;

    enum AnimNo {
        Idle, Move,
    };
    AnimNo animNo;
    string[] nameTbl = {
        "Idle", "Move",
    };

    // Šî–{‚Í¶Œü‚«
    float defaultDir = 1;

    void AnimInit()
    {
        animator = GetComponent<Animator>();

        // Œ»ó‚Í‚O”Ô‚Ì‚İ‰EŒü‚«‚É
        defaultDir = 1;
    }
}
