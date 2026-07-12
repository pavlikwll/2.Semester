//Volodymyr Pavlik
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    // Der Ort, an den der Spieler teleportiert wird.
    // Im Inspector wird hier ein leeres Objekt hineingezogen, z. B. Spawn_InsideHouse.
    [SerializeField] private Transform teleportPlace;
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineCamera cinemachine;
    
    // CanvasGroup des schwarzen UI-Bildschirms.
    // Über alpha erzeugen wir einen Fade-Effekt: 0 = transparent, 1 = schwarzer Bildschirm.
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private float fadeDuration = 0.3f;

    // Schutz vor einem doppelten Start des Übergangs.
    // Ohne das könnte der Spieler den Trigger mehrmals hintereinander auslösen.
    private bool _isTeleporting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTeleporting)
        {
            // Wenn der Übergang bereits läuft, machen wir nichts.
            return;
        }

        if (other.CompareTag("Player"))
        {
            // Wir starten die Übergangs-Coroutine.
            // Eine Coroutine ist notwendig, weil der Fade-Effekt nicht sofort, sondern schrittweise ablaufen soll.
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        _isTeleporting = true;

        // Bildschirm bis auf Schwarz abdunkeln.
        // yield return bedeutet: Warte, bis Fade(1) vollständig abgeschlossen ist.
        yield return Fade(1);

        // Alte Position des Spielers vor der Teleportation speichern.
        // Das benötigt Cinemachine, um zu erkennen, wie weit der Spieler tatsächlich "gesprungen" ist.
        Vector3 oldPos = player.position;

        // Spieler an die neue Position teleportieren.
        player.position = teleportPlace.position;
        
        // Differenz zwischen neuer und alter Position berechnen.
        // Beispiel: vorher x = 0, danach x = 20, also delta = 20.
        Vector3 delta = player.position - oldPos;

        // Cinemachine mitteilen, dass der Spieler diese Strecke nicht gelaufen, sondern sofort teleportiert wurde.
        // Ohne diesen Aufruf würde die Kamera dem Spieler möglicherweise langsam über die gesamte Karte folgen.
        cinemachine.OnTargetObjectWarped(player, delta);

        // Vorherigen Kamerazustand zurücksetzen.
        // Dadurch werden Ruckler, seltsame Verschiebungen und unerwünschte Glättung nach dem Teleport verhindert.
        cinemachine.PreviousStateIsValid = false;

        // Spiel wieder sichtbar machen:
        // Der schwarze Bildschirm verschwindet schrittweise.
        yield return Fade(0);

        _isTeleporting = false;
    }

    IEnumerator Fade(float target)
    {
        // Aktuelle Transparenz des Fade-Bildschirms beim Start.
        float start = fade.alpha;

        // Zeit-Zähler für den sanften Übergang.
        float time = 0;
        
        // Solange fadeDuration noch nicht erreicht ist, wird alpha schrittweise verändert.
        while (time < fadeDuration)
        {
            // Zeit hinzufügen, die seit dem letzten Frame vergangen ist.
            time += Time.deltaTime;

            // Mathf.Lerp verändert den Wert sanft von start zu target.
            // time / fadeDuration liefert einen Fortschritt von 0 bis 1.
            fade.alpha = Mathf.Lerp(start, target, time / fadeDuration);

            // Einen Frame warten und dann die Schleife fortsetzen.
            yield return null;
        }

        // Am Ende den exakten Zielwert setzen.
        // Das verhindert kleine Ungenauigkeiten wie 0.659 statt 1.
        fade.alpha = target;
    }
}