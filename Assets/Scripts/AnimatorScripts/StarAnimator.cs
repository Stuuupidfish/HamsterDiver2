using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StarAnimator : MonoBehaviour
{
	[SerializeField, Range(0.01f, 1f)] private float startScaleMultiplier = 0.15f;
	[SerializeField, Min(0.01f)] private float animationDuration = 0.35f;
	[SerializeField] private AudioClip jingle;
	private AudioSource audioSource;
	private Vector3 targetScale;
	public float CompletionDuration
	{
		get
		{
			return Mathf.Max(animationDuration, jingle.length);
		}
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		targetScale = transform.localScale;
		transform.localScale = targetScale * startScaleMultiplier;
	}

	private void Start()
	{
		audioSource.PlayOneShot(jingle);

		StartCoroutine(AnimateIn());
	}

	private IEnumerator AnimateIn()
	{
		Vector3 startScale = targetScale * startScaleMultiplier;
		float elapsed = 0f;

		while (elapsed < animationDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			float progress = Mathf.Clamp01(elapsed / animationDuration);
			transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
			yield return null;
		}

		transform.localScale = targetScale;
	}
}
