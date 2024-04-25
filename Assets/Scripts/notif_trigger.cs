using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class notif_trigger : MonoBehaviour
{
    [Header("Notif Removal")]
    [SerializeField] private bool removeAfterExit = false;
    [SerializeField] private bool disableAfterTimer = false;
    [SerializeField] float disableTimer = 1.0f;

    [Header("Notif Animation")]
    [SerializeField] private Animator notifanim;


    IEnumerator EnableNotif()
    {
        notifanim.Play("notif anim");

        if (disableAfterTimer)
        {
            yield return new WaitForSeconds(disableTimer);
            RemoveNotif();
        }
    }

    void RemoveNotif()
    {
        notifanim.Play("notif anim out");
    }
}
