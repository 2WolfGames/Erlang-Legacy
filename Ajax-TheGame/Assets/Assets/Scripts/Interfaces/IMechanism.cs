using System.Collections;
using UnityEngine;

public interface IMechanism
{
    void Activate(Lever l);

    bool CanActivate(Lever l);
}
