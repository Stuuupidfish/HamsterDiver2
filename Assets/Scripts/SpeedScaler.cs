using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpeedScaler
{
    // Per-axis multipliers for movement. Adjust `VerticalScale` to speed up/slow down
    // the game's vertical scroll without changing horizontal behaviours.
    public static float VerticalScale = 85f; // preserves previous Scale value
    public static float HorizontalScale = 3f;
}
