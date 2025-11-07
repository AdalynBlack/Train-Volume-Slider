/* Train Volume Slider Mod for Forsaken Frontiers
 * Copyright (C) 2025 Adalyn
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using HarmonyLib;
using Il2Cppmadeinfairyland.forsakenfrontiers.ui.PauseMenu.OptionsMenu.Audio;

namespace TrainVolumeSlider.Patches;
class FFMasterVolumeSliderPatches
{
    [HarmonyPatch(typeof(FFMasterVolumeSlider), "Start")]
    [HarmonyPrefix]
    static void FFMasterVolumeSliderStartPrefix(FFMasterVolumeSlider __instance) {
        TrainVolumeSlider.CreateSlider(__instance.gameObject);
    }
}
