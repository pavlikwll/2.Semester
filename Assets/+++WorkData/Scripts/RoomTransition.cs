using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    // Місце, куди буде телепортовано гравця.
    // У Inspector сюди перетягується порожній об’єкт, наприклад Spawn_InsideHouse.
    [SerializeField] private Transform teleportPlace;
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    // CanvasGroup чорного UI-екрана.
    // Через alpha робимо fade: 0 = прозоро, 1 = чорний екран.
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private float fadeDuration = 0.3f;

    // Захист від подвійного запуску переходу.
    // Без цього гравець може зачепити тригер кілька разів поспіль.
    private bool _isTeleporting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTeleporting)
        {
            // Якщо перехід уже триває — нічого не робимо.
            return;
        }

        if (other.CompareTag("Player"))
        {
            // Запускаємо Coroutine переходу.
            // Coroutine потрібна, бо fade має тривати не миттєво, а поступово.
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        _isTeleporting = true;

        // Затемнюємо екран до чорного.
        // yield return означає: зачекай, поки Fade(1) повністю завершиться.
        yield return Fade(1);

        // Запам’ятовуємо стару позицію гравця перед телепортом.
        // Це потрібно для Cinemachine, щоб зрозуміти, наскільки далеко стрибнув Player.
        Vector3 oldPos = player.position;
        // Телепортуємо гравця в нову точку.
        player.position = teleportPlace.position;
        
        // Рахуємо різницю між новою і старою позицією.
        // Наприклад: був у x=0, став у x=20, отже delta = 20.
        Vector3 delta = player.position - oldPos;

        // Повідомляємо Cinemachine, що Player не пробіг цю відстань, а був телепортований.
        // Без цього камера може плавно “доганяти” гравця через усю карту.
        cinemachineCamera.OnTargetObjectWarped(player, delta);
        // Скидаємо попередній стан камери.
        // Це прибирає ривки, дивні зміщення й зайве згладжування після телепорту.
        cinemachineCamera.PreviousStateIsValid = false;

        // Повертаємо видимість гри: чорний екран поступово зникає.
        yield return Fade(0);

        _isTeleporting = false;
    }

    IEnumerator Fade(float target)
    {
        // Поточна прозорість fade-екрана на момент старту.
        float start = fade.alpha;
        // Лічильник часу для плавного переходу.
        float time = 0;
        
        // Поки не минув увесь час fadeDuration, поступово змінюємо alpha.
        while (time < fadeDuration)
        {
            // Додаємо час, який минув від попереднього кадру.
            time += Time.deltaTime;
            // Mathf.Lerp плавно змінює значення від start до target.
            // time / fadeDuration дає прогрес від 0 до 1.
            fade.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            // Чекаємо один кадр і продовжуємо цикл.
            yield return null;
        }
        // Наприкінці примусово ставимо точне значення.
        // Це прибирає можливу похибку типу 0.99997 замість 1.
        fade.alpha = target;
    }
}