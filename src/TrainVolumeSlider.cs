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

using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Il2Cppmadeinfairyland.forsakenfrontiers.actor.player.datadeck;
using Il2Cppmadeinfairyland.forsakenfrontiers.train;
using Il2Cppmadeinfairyland.forsakenfrontiers.ui.PauseMenu.OptionsMenu.Audio;
using Il2Cppmadeinfairyland.fairyengine;
using Il2CppTMPro;
using Il2CppVolumetricAudio;

namespace TrainVolumeSlider;
[RegisterTypeInIl2Cpp]
public class TrainVolumeSlider : MonoBehaviour
{
    VA_AudioSource[] audioSources;
    Slider volumeSlider;
    System.Action<bool> actionOnToggleDataDeck;

    public void OnVolumeChanged(float volume) {
        foreach (var audioSource in this.audioSources)
        {
            audioSource.baseVolume = volume;
        }

        PlayerPrefs.SetFloat("TrainBackgroundVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnToggleDataDeck(bool toggled) {

    }

    public IEnumerator WaitTillHookToPlayer() {
        yield return new WaitUntil((Func<bool>)(() => FairyEngine.LocalPlayer != null));

        FairyEngine.LocalPlayer.GetComponentInChildren<FFDataDeck>().add_EvtOnToggledDataDeck(this.actionOnToggleDataDeck);

        this.audioSources = FFTrain.Instance.GetComponentsInChildren<VA_AudioSource>();
        var volume = PlayerPrefs.GetFloat("TrainBackgroundVolume", 0.37f);
        this.volumeSlider.value = volume;
        this.OnVolumeChanged(volume);
    }
    
    public static TrainVolumeSlider CreateSlider(GameObject reference) {
        var sliderGameObject = Instantiate(reference);

        sliderGameObject.transform.name = "Train Volume Slider";
        sliderGameObject.transform.SetParent(reference.transform.parent, false);
        sliderGameObject.transform.SetAsLastSibling();

        sliderGameObject.GetComponentInChildren<TextMeshProUGUI>().text = "train";

        Destroy(sliderGameObject.GetComponent<FFMasterVolumeSlider>());

        var slider = sliderGameObject.GetComponent<Slider>();
        slider.onValueChanged.RemoveAllListeners();

        var sliderScript = sliderGameObject.AddComponent<TrainVolumeSlider>();
        slider.onValueChanged.AddListener(new System.Action<float>(sliderScript.OnVolumeChanged));

        sliderScript.volumeSlider = slider;
        return sliderScript;
    }

    public void Start() {
        this.actionOnToggleDataDeck = new System.Action<bool>(this.OnToggleDataDeck);
        MelonCoroutines.Start(WaitTillHookToPlayer());
    }

    public void OnDestroy() {
        FairyEngine.LocalPlayer.GetComponentInChildren<FFDataDeck>().remove_EvtOnToggledDataDeck(actionOnToggleDataDeck);
    }

    public TrainVolumeSlider(IntPtr ptr) : base(ptr) {}
    //public TrainVolumeSlider() : base(ClassInjector.DerviedConstructorPointer<TrainVolumeSlider>()) => ClassInjector.DerivedConstructorBody(this);
}
