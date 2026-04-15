using Unity.VisualScripting;
using UnityEngine;

public class G_agarrao : C_golpes
{
    public G_agarrao()
    {
        nome = "agarrao";

        JcustoStamina = 10;
        IcustoStamina = 30;

        conectorImaoEsq = "Ad";
        conectorImaoDir = "Ae";

        JdirEqui = "d";
        IdirEqui = "t";

        pernaAberta = true;
    }
}
