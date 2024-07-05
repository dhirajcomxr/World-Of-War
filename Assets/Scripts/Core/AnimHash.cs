using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHash 
{
    public const string None = "None";

    public const string Horizontal = "Horizontal";
    public const string Virtical = "Vertical";

    public const string jump = "jump in";
    public const string land = "land";

    public const string takeDamage = "TakeDamage";

    public const string Interacting = "isInteracting";
    public const string CanDoCombo = "canDoCombo";

    public const string BackStab = "BackStab";

    public const string death = "death";
    public const string block = "Block";
    public AnimHash()
    {        
        Animator.StringToHash(None);
        Animator.StringToHash(Horizontal);
        Animator.StringToHash(Virtical);
        Animator.StringToHash(jump);
        Animator.StringToHash(land);
        Animator.StringToHash(takeDamage);
        Animator.StringToHash(Interacting);
        Animator.StringToHash(CanDoCombo);
        Animator.StringToHash(BackStab);
        Animator.StringToHash(death);        
        Animator.StringToHash(block);        

    }
}
public class TagHash
{
    public const string PLAYER = "Player";
    public const string ENEMY = "Enemy";
    public const string GROUND = "ground";

    public TagHash()
    {
        Animator.StringToHash(PLAYER);
        Animator.StringToHash(ENEMY);
        Animator.StringToHash(GROUND);
    }
}
